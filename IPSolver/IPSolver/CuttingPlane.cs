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
            double maxValue = -1;
            for (int r = 1; r < linearProgram.RowCount; r++)
            {
                for (int c = 1; c < linearProgram.CountX +1; c++)
                {
                    if (linearProgram.LinearProgramMatrix[r, c] == 1 
                        && linearProgram.LinearProgramMatrix[r, linearProgram.ColumnCount-1] > maxValue)
                    {
                        maxValue = linearProgram.LinearProgramMatrix[r, linearProgram.ColumnCount - 1];
                        targetRow = r;
                    }
                }
               
            }
            double[] fractionArray = new double[linearProgram.ColumnCount+1];
            for (int i = 0; i < linearProgram.ColumnCount; i++)
            {
                fractionArray[i] = linearProgram.LinearProgramMatrix[targetRow, i];
                if (fractionArray[i] % 1 == 0)
                    fractionArray[i] = 0;
                else if (fractionArray[i] > 0)
                    fractionArray[i] -= (int)fractionArray[i];
                else
                    fractionArray[i] += (((int)fractionArray[i] *-1) + 1);
                fractionArray[i] = Math.Round(fractionArray[i], 9);
                fractionArray[i] *= -1;
            }
            
            fractionArray[fractionArray.Length - 1] = fractionArray[fractionArray.Length - 2];
            fractionArray[fractionArray.Length - 2] = 1;

            linearProgram.LinearProgramMatrix = LpTools.AddRow(fractionArray, linearProgram);
            linearProgram.CountS++;

            Dual dual = new Dual(linearProgram);
            return dual.Solve();
        }

     
    }
}
