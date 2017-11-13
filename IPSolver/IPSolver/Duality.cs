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


        public void RotateLP()
        {
            originalLP.DisplayCurrentTable();

            LinearProgram duality = (LinearProgram) originalLP.Clone();


            if (originalLP.Type == LPType.Max)
            {
                duality.Type = LPType.Min;
            }
            else
            {
                duality.Type = LPType.Max;
            }


            duality.LinearProgramMatrix = new double[originalLP.CountX + 1, originalLP.RowCount];

            //Fill X Values
            for (int i = 1; i < originalLP.RowCount; i++)
            {
                for (int k = 0; k <= originalLP.CountX; k++)
                {
                    duality.LinearProgramMatrix[k, i] = originalLP.LinearProgramMatrix[i, k];
                }
            }


            //duality.LinearProgramMatrix[0, 0] = 1;

            ////Fill Z Row
            //for (int i = 1; i < originalLP.RowCount; i++)
            //{
            //    duality.LinearProgramMatrix[0, i] = originalLP.LinearProgramMatrix[i, originalLP.ColumnCount - 1];
            //}

            double[] rhs = new double[originalLP.CountX + 1];


            //Fill RHS
            for (int i = 1; i <= originalLP.CountX; i++)
            {
                rhs[i] = originalLP.LinearProgramMatrix[0, i];
            }


            foreach (var item in rhs)
            {
                Console.WriteLine(item);

            }

            duality.CountA = 0;
            duality.CountS = 0;
            duality.CountE = originalLP.CountX;

            double[,] eArray = new double[duality.CountE + 2, duality.CountE + 2];

            //Handle URS
            for (int i = 1; i < eArray.GetLength(0); i++)
            {
                eArray[i, i] = 1;
            }


            double[,] finalLP = new double[duality.RowCount, duality.ColumnCount + eArray.GetLength(0) + 1];


            for (int i = 0; i < finalLP.GetLength(0); i++)
            {
                int mainCol = 0;

                //Saves the LP
                for (int orgCol = 0; orgCol < duality.ColumnCount; orgCol++)
                {
                    finalLP[i, mainCol] = originalLP.LinearProgramMatrix[i, orgCol];

                    mainCol++;
                }

                //Saves the E's
                for (int eCol = 0; eCol < duality.CountE; eCol++)
                {
                    finalLP[i, mainCol] = eArray[i, eCol];

                    mainCol++;
                }

                //Saves the RHS
                //finalLP[i, mainCol] = rhs[i];
            }

            duality.LinearProgramMatrix = finalLP;




            duality.CountX = originalLP.RowCount - 1;
            duality.DisplayCurrentTable();







        }





    }
}
