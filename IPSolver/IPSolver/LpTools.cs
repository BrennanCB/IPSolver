using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IPSolver
{
    class LpTools
    {
        public static int GREATER_THAN = 1;
        public static int LESS_THAN= 0;

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

        //TODO: WILL BREAK IF SOLUTION IS NOT YET SOLVED OR IF YOU ADD CONSTRAINT TO A COLUMN WITH NO X SOLUTION
        public static LinearProgram AddBasicConstraint
            (LinearProgram linearProgram, int column, int ConstraintType, int rhs)
        {
            if (ConstraintType != GREATER_THAN && ConstraintType != LESS_THAN)
            {
                throw new ArgumentException
                    ("The argument passed to AddBasicConstraint for ConstraintType does not match the expected value");
            }

            int constraintRow = linearProgram.RowCount;
            double[,] newArray = new double[linearProgram.RowCount +1, linearProgram.ColumnCount +1];
            for (int c = 0; c < linearProgram.ColumnCount-1; c++)
            {
                for (int r = 0; r < linearProgram.RowCount; r++)
                {
                    newArray[r, c] = linearProgram.LinearProgramMatrix[r, c];
                }   
            }
            for (int i = 0; i < linearProgram.RowCount; i++)
            {
                newArray[i, linearProgram.ColumnCount] = linearProgram.LinearProgramMatrix[i, linearProgram.ColumnCount-1];
            }
            for (int i = 0; i < linearProgram.RowCount; i++)
            {
                newArray[i, linearProgram.ColumnCount] = 0;
            }
            newArray[constraintRow, column] = 1;
            newArray[constraintRow, linearProgram.ColumnCount] = rhs;

            newArray[constraintRow, linearProgram.ColumnCount] =
                (ConstraintType == GREATER_THAN) ? -1 : 1;

            //Constraint has been added, now check vilidity
            int conflictingRow = 0;
            for (int i = 0; i < linearProgram.RowCount; i++)
            {
                if(newArray[i, column] == 1)
                {
                    conflictingRow = i;
                    break;
                }
            }
            for (int i = 0; i < linearProgram.ColumnCount +1; i++)
            {   
                newArray[constraintRow, i] = newArray[constraintRow, i] - newArray[conflictingRow, i];
            }
            if(newArray[constraintRow, linearProgram.ColumnCount-1] < 0)
                for (int i = 0; i < linearProgram.ColumnCount + 1; i++)
                {
                    newArray[constraintRow, i] *= -1;
                }
            return linearProgram;
        }
    }
}
