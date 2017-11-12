using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IPSolver
{
    class CuttingPlane
    {
        LinearProgram linearProgram;
        public CuttingPlane(LinearProgram linearProgram)
        {
            this.linearProgram = linearProgram;
        }
        public LinearProgram Solve()
        {
            int targetRow = 0;
            for (int i = 0; i < linearProgram.RowCount; i++)
            {
                if (linearProgram.LinearProgramMatrix[i, 0] == 1)
                    targetRow = i;
            }
            double[] fractionArray = new double[linearProgram.ColumnCount];
            for (int i = 0; i < linearProgram.ColumnCount; i++)
            {
                fractionArray[i] = linearProgram.LinearProgramMatrix[targetRow, i];
                if (fractionArray[i] % 1 == 0)
                    fractionArray[i] = 0;
                else if (fractionArray[i] > 0)
                    fractionArray[i] -= (int)fractionArray[i];
                else
                    fractionArray[i] += (((int)fractionArray[i] *-1) + 1);

            }



            return linearProgram;
        }

     
    }
}
