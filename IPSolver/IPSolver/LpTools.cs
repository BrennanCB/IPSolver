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

            double[,] problemMatrix = LinearProgram.LinearProgramMatrix;

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

            return true;
        }

        public static bool CheckSpecialCases(LinearProgram LinearProgram)
        {
            if (IsInfeasable(LinearProgram))
                return true;
            if (IsUnbounded(LinearProgram))
                return true;
            //TODO: CHECK DEGENERATES. HOW ARE WE HANDELING NULL VALUES?
            return false;
        }

        private static bool IsInfeasable(LinearProgram LinearProgram)
        {
            //Check infeasable
            double minValue = LinearProgram.LinearProgramMatrix
                [LinearProgram.RowCount - 1, LinearProgram.ColumnCount - 2];
            int minLocation = LinearProgram.RowCount - 1;

            for (int j = 0; j < LinearProgram.RowCount; j++)
            {
                if (LinearProgram.LinearProgramMatrix[j, LinearProgram.ColumnCount - 2] < minValue)
                {
                    minValue = LinearProgram.LinearProgramMatrix[j, LinearProgram.ColumnCount - 2];
                    minLocation = j;
                }

            }

            if (minValue > 0)
                return false;

            bool valid = false;
            for (int i = 0; i < LinearProgram.ColumnCount; i++)
            {
                if (LinearProgram.LinearProgramMatrix[minLocation, i] < 0)
                {
                    valid = true;
                    break;
                }
            }
            if (!valid)
                throw new SpecialCaseException(SpecialCaseException.Type.Infeasable);

            return false;
        }

        private static bool IsUnbounded(LinearProgram LinearProgram)
        {
            double[,] problemMatrix = LinearProgram.LinearProgramMatrix;
            double minValue = problemMatrix[0, 0];
            int minLocation = 0;
            for (int i = 0; i < LinearProgram.ColumnCount - 2; i++)
            {
                if (problemMatrix[0, i] < minValue)
                {
                    minValue = problemMatrix[0, i];
                    minLocation = i;
                }
            }

            if (minValue > 0)
                return false;

            int rhs = LinearProgram.ColumnCount - 1;
            bool valid = false;

            for (int i = 0; i < LinearProgram.RowCount; i++)
            {
                double ratio = problemMatrix[i, rhs] / problemMatrix[i, minLocation];
                if (ratio > 0)
                {
                    valid = true;
                    break;
                }
            }
            if (!valid)
                throw new SpecialCaseException(SpecialCaseException.Type.Unbounded);
            return false;
        }

        
    }
}
