﻿using System;
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

        public static bool CheckIfIPIsSolved(LinearProgram LinearProgram)
        {
          
            double[,] problemMatrix = LinearProgram.LinearProgramMatrix;

            for (int i = 1; i < LinearProgram.ColumnCount-1; i++)
            {
                double tempValue = Math.Round(problemMatrix[0, i],5);
                if (tempValue < 0 && LinearProgram.Type == LPType.Max)
                    return false;
                if (tempValue> 0 && (LinearProgram.Type == LPType.Min || LinearProgram.IsTwoPhase))
                    return false;
            }


            for (int j = 0; j < LinearProgram.RowCount; j++)
            {
                double tempValue = Math.Round(problemMatrix[j, LinearProgram.ColumnCount - 1], 5);
                if (tempValue < 0)
                    return false;
            }
            return true;
        }

        public static bool CheckIfIPIsSolved(LinearProgram LinearProgram, bool Integer)
        {
            if (!CheckIfIPIsSolved(LinearProgram))
                return false;
            if (!Integer)
                return true;

            double[] xValues = findXValues(LinearProgram);
            //TODO: OPTIMIZE THIS
            //for (int i = 0; i < xValues.Length; i++)
            //    xValues[i] = Math.Round(LinearProgram.LinearProgramMatrix[i + 1, LinearProgram.ColumnCount - 1], 5);

            for (int i = 0; i < xValues.Length; i++)
            {
                if (xValues[i] % 1 != 0)
                    return false;
            }
           
            return true;
        }

        public static bool IsSpecialCase(LinearProgram LinearProgram)
        {
            if (IsInfeasable(LinearProgram))
                return true;
            if (IsUnbounded(LinearProgram))
                return true;
            return false;
        }

        private static bool IsInfeasable(LinearProgram LinearProgram)
        {
            //Check infeasable
            double minValue = double.MaxValue;
            int minLocation = 0;

            for (int j = 1; j < LinearProgram.RowCount; j++)
            {
                if (LinearProgram.LinearProgramMatrix[j, LinearProgram.ColumnCount - 1] < minValue)
                {
                    minValue = Math.Round(LinearProgram.LinearProgramMatrix[j, LinearProgram.ColumnCount - 1],5);
                    minLocation = j;
                }

            }

            if (minValue >= 0)
                return false;

            bool valid = false;
            for (int i = 1; i < LinearProgram.ColumnCount-1; i++)
            {
                if (Math.Round(LinearProgram.LinearProgramMatrix[minLocation, i], 5) < 0)
                {
                    valid = true;
                    break;
                }
            }

            if (!valid)
            {
                Console.WriteLine("===== Lp is infeasable =====");
                return true;
            }
          
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
            {
                Console.WriteLine("Lp is unbounded");
                return true;
            }
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

            LinearProgram tempLp = (LinearProgram)linearProgram.Clone(); 
            int constraintRow = linearProgram.RowCount;

            double[,] newArray = new double[linearProgram.RowCount +1, linearProgram.ColumnCount +1];
            for (int c = 0; c < linearProgram.ColumnCount - 1; c++)
            {
                for (int r = 0; r < linearProgram.RowCount; r++)
                {
                    newArray[r, c] = linearProgram.LinearProgramMatrix[r, c];
                }
            }
            for (int i = 0; i < linearProgram.RowCount; i++)
                newArray[i, linearProgram.ColumnCount] = linearProgram.LinearProgramMatrix[i, linearProgram.ColumnCount-1];
            for (int i = 0; i < linearProgram.RowCount; i++)
                newArray[i, linearProgram.ColumnCount-1] = 0;
            
            newArray[constraintRow, column] = 1;
            newArray[constraintRow, linearProgram.ColumnCount] = rhs;

            newArray[constraintRow, linearProgram.ColumnCount - 1] = 1;

            
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
            if (ConstraintType == GREATER_THAN)
            {
                tempLp.CountE++;
                for (int i = 0; i < linearProgram.ColumnCount + 1; i++)
                {
                    newArray[constraintRow, i] = newArray[constraintRow, i] - newArray[conflictingRow, i];
                }
                    newArray[constraintRow, linearProgram.ColumnCount - 1] *= -1;
            }
            else
            {
                tempLp.CountS++;
                for (int i = 0; i < linearProgram.ColumnCount + 1; i++)
                {
                    newArray[constraintRow, i] = newArray[constraintRow, i] - newArray[conflictingRow, i];
                }
            }
           
            if(newArray[constraintRow, linearProgram.ColumnCount-1] < 0)
                for (int i = 0; i < linearProgram.ColumnCount + 1; i++)
                {
                    newArray[constraintRow, i] *= -1;
                }
            tempLp.LinearProgramMatrix = newArray;
            
            tempLp.DisplayCurrentTable();
            
            return tempLp;
        }

        //Only works when one new slack variable is added
        public static double[,] AddRow(double[] row, LinearProgram linearProgram)
        {
            double[,] newArray = new double[linearProgram.RowCount + 1, linearProgram.ColumnCount + 1];
            for (int c = 0; c < linearProgram.ColumnCount - 1; c++)
            {
                for (int r = 0; r < linearProgram.RowCount; r++)
                {
                    newArray[r, c] = linearProgram.LinearProgramMatrix[r, c];
                }
            }
            for (int i = 0; i < linearProgram.RowCount; i++)
            {
                newArray[i, linearProgram.ColumnCount] = linearProgram.LinearProgramMatrix[i, linearProgram.ColumnCount - 1];
            }
            for (int i = 0; i < linearProgram.RowCount; i++)
            {
                newArray[i, linearProgram.ColumnCount - 1] = 0;
            }
            for (int i = 0; i < row.Length; i++)
            {
                newArray[linearProgram.RowCount , i] = row[i];
            }
            return newArray;
        }

        public static double[] findXValues(LinearProgram lp)
        {
            double[] xValues = new double[lp.CountX];
            for (int c = 1; c < lp.CountX + 1; c++)
            {
                double x = 0;
                bool basic = true;
                int count1 = 0;
                for (int r = 1; r < lp.RowCount; r++)
                {
                    double currentValue = lp.LinearProgramMatrix[r, c];
                    if (currentValue != 1 && currentValue != 0)
                        basic = false;
                    if (currentValue == 1)
                    {
                        x = Math.Round(lp.LinearProgramMatrix[r, lp.ColumnCount - 1], 5);
                        count1++;
                    }
                }
                if (basic && count1 == 1)
                    xValues[c - 1] = x;
            }
            return xValues;
        }
    }

    
}
