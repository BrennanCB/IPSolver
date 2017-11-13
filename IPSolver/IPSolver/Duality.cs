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

            for (int i = 1; i < originalLP.RowCount; i++)
            {
                for (int k = 0; k <= originalLP.CountX; k++)
                {
                    duality.LinearProgramMatrix[k, i] = originalLP.LinearProgramMatrix[i, k];
                }
            }


            duality.LinearProgramMatrix[0, 0] = 1;

            for (int i = 1; i < originalLP.RowCount; i++)
            {
                duality.LinearProgramMatrix[0, i] = originalLP.LinearProgramMatrix[i, originalLP.ColumnCount - 1];
            }

            for (int i = 1; i < originalLP.CountX; i++)
            {
                duality.LinearProgramMatrix[i + 1, originalLP.RowCount - 1] = originalLP.LinearProgramMatrix[0, i];
            }


            duality.CountX = originalLP.RowCount - 1;
            duality.DisplayCurrentTable();







        }





    }
}
