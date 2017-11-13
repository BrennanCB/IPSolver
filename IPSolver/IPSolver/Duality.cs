using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IPSolver
{
    //todo Temp class, delete after shane done in sensitivity
    class Duality
    {
        private LinearProgram optimalSoltution;
        private LinearProgram originalLP;

        public LinearProgram OptimalSoltution
        {
            get { return optimalSoltution; }
            set { optimalSoltution = value; }
        }

        public LinearProgram OriginalLP
        {
            get
            {
                if (originalLP == null)
                    originalLP = new LpFormatter(FileHandler.ReadLP(), Algorithm.Dual).GetLinearProgram();

                return originalLP;
            }
            set { originalLP = value; }
        }

        public Duality(LinearProgram optimalSoltution)
        {
            OptimalSoltution = optimalSoltution;
            originalLP = new LpFormatter(FileHandler.ReadLP(), Algorithm.Dual).GetLinearProgram();
        }

        public void DeterminDuality()
        {
            originalLP.DisplayCurrentTable();

            LinearProgram duality = (LinearProgram) originalLP.Clone();

            if (originalLP.Type == LPType.Max)
                duality.Type = LPType.Min;
            else
                duality.Type = LPType.Max;

            duality.LinearProgramMatrix = new double[originalLP.CountX + 1, originalLP.RowCount];

            //Fill X Values
            for (int i = 1; i < originalLP.RowCount; i++)
            {
                for (int j = 0; j <= originalLP.CountX; j++)
                {
                    duality.LinearProgramMatrix[j, i] = originalLP.LinearProgramMatrix[i, j];
                }
            }
            
            double[] rhs = new double[originalLP.CountX + 1];

            //Fill RHS
            for (int i = 1; i <= originalLP.CountX; i++)
            {
                rhs[i] = originalLP.LinearProgramMatrix[0, i];
            }
            
            duality.CountA = 0;
            duality.CountS = 0;
            duality.CountE = originalLP.CountX;
            duality.CountX = originalLP.RowCount - 1;

            double[,] eArray = new double[duality.CountE + 1, duality.CountE];

            //Handle URS
            for (int i = 0; i < eArray.GetLength(1); i++)
            {
                eArray[i + 1, i] = 1;
            }

            double[,] finalLP = new double[duality.RowCount, duality.ColumnCount + eArray.GetLength(0)];

            for (int i = 0; i < finalLP.GetLength(0); i++)
            {
                int mainCol = 0;

                //Saves the LP
                for (int orgCol = 0; orgCol < duality.ColumnCount; orgCol++)
                {
                    finalLP[i, orgCol] = duality.LinearProgramMatrix[i, orgCol] * -1;
                      
                    mainCol++;
                }

                //Saves the E's
                for (int eCol = 0; eCol < duality.CountE; eCol++)
                {
                    finalLP[i, mainCol] = eArray[i, eCol];

                    mainCol++;
                }

                //Saves the RHS
                finalLP[i, duality.ColumnCount + duality.CountE] = rhs[i];
            }

            for (int i = 1; i < originalLP.RowCount; i++)
            {
                finalLP[0, i] = originalLP.LinearProgramMatrix[i, originalLP.ColumnCount - 1] * -1;
            }

            duality.LinearProgramMatrix = finalLP;
            duality.LinearProgramMatrix[0, 0] = 1;

            Console.WriteLine("Duality Initial Table");
            duality.DisplayCurrentTable();
            Console.WriteLine();

            Dual dual = new Dual(duality);

            duality = dual.Solve();

            Console.WriteLine();

            if (optimalSoltution.GetBasicVariables()[0] == duality.GetBasicVariables()[0])
                Console.WriteLine("Strong Duality");
            else
                Console.WriteLine("Weak Duality");
        }
    }
}
