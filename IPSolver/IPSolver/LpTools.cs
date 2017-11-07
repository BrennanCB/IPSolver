using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IPSolver
{
    class LpTools
    {
        public static bool CheckIfLpIsSolved(LinearProgram LinearProgram)
        {
            if (CheckSpecialCases(LinearProgram))
                return false;

            double[,] problemMatrix = LinearProgram.LinearProgramArray;

            for (int i = 0; i < LinearProgram.ColumnCount; i++)
            {
                if (problemMatrix[0, i] < 0)
                    return false;
            }


            for (int j = 0; j < LinearProgram.RowCount; j++)
            {
                if (problemMatrix[j, LinearProgram.ColumnCount - 1] < 0)
                    return false;
            }

            
        }

        public static bool CheckSpecialCases(LinearProgram LinearProgram)
        {
            return false;
        }
    }
}
