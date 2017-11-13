using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IPSolver
{
    public abstract class Simplex
    {
        LinearProgram linearProgram;

        public LinearProgram LinearProgram
        {
            get
            {
                return linearProgram;
            }

            set
            {
                linearProgram = value;
            }
        }

        public Simplex(LinearProgram linearProgram)
        {
            LinearProgram = linearProgram;
        }

        public abstract bool CheckIfContinue();
        public abstract bool RatioTest(out int pivotRow, out int pivotCol);
        public abstract LinearProgram Solve();

        public void CalculateNewCellValues(int pivotRow, int pivotCol)
        {
            double pivotCellValue = LinearProgram.LinearProgramMatrix[pivotRow, pivotCol];

            //Calculates the new values of winning row
            for (int i = 0; i < LinearProgram.ColumnCount; i++)
            {
                double newAmount = LinearProgram.LinearProgramMatrix[pivotRow, i] / pivotCellValue;

                LinearProgram.LinearProgramMatrix[pivotRow, i] = newAmount;
            }

            //Calculates the new amounts of the remaining rows
            for (int i = 0; i < LinearProgram.RowCount; i++)
            {
                double subtractAmount = LinearProgram.LinearProgramMatrix[i, pivotCol];
                for (int j = 0; j < LinearProgram.ColumnCount; j++)
                {
                    if (i != pivotRow)
                        LinearProgram.LinearProgramMatrix[i, j] = LinearProgram.LinearProgramMatrix[i, j] - subtractAmount * LinearProgram.LinearProgramMatrix[pivotRow, j];
                }
            }
        }
    }
}
