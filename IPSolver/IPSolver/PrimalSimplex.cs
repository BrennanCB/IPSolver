using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IPSolver
{
    public class PrimalSimplex : Simplex
    {
        public PrimalSimplex(LinearProgram linearProgram)
            :base(linearProgram)
        {
            LinearProgram = linearProgram;
        }

        //TODO rename
        override public bool CheckIfContinue()
        {
            //Checks the top row, to see if it must continue
            for (int i = 1; i < LinearProgram.ColumnCount - 1; i++)
            {
                if ((LinearProgram.Type == LPType.Max && Math.Round(LinearProgram.LinearProgramMatrix[0, i], 10) < 0) ||
                        (LinearProgram.Type == LPType.Min && Math.Round(LinearProgram.LinearProgramMatrix[0, i], 10) > 0))
                    return true;
            }

            return false;
        }

        //Returns true of the ratio test fails
        override public bool RatioTest(out int pivotRow, out int pivotCol)
        {
            pivotCol = 0;
            pivotRow = 0;

            double pivotColAmount = 0;
            double winningRatio = double.MaxValue;

            List<double> ratios = new List<double>();

            //Loops through the rows to choose the winning column
            for (int i = 1; i < LinearProgram.ColumnCount - 1; i++)
            {
                if ((LinearProgram.Type == LPType.Max && Math.Round(LinearProgram.LinearProgramMatrix[0, i], 10) < pivotColAmount) ||
                    (LinearProgram.Type == LPType.Min && Math.Round(LinearProgram.LinearProgramMatrix[0, i], 10) > pivotColAmount))
                {
                    pivotCol = i;
                    pivotColAmount = Math.Round(LinearProgram.LinearProgramMatrix[0, i], 10);
                }
            }

            //Makes sure the winning column isnt Z
            if (pivotCol == 0)
                return true;

            //Calculates the ratios
            for (int i = 1; i < LinearProgram.RowCount; i++)
            {
                //Makes sure that cannot divide by zero
                try
                {
                    double tempRatio = Math.Round(LinearProgram.LinearProgramMatrix[i, LinearProgram.ColumnCount - 1] / LinearProgram.LinearProgramMatrix[i, pivotCol], 10);

                    ratios.Add(tempRatio);
                }
                catch (DivideByZeroException)
                {
                    ratios.Add(-1);
                }
            }

            //Chooses the winning row
            for (int i = 0; i < ratios.Count(); i++)
            {
                if (ratios[i] < winningRatio && ratios[i] > 0)
                {
                    winningRatio = ratios[i];
                    pivotRow = i + 1;
                }
                else if (ratios[i] == 0 && LinearProgram.LinearProgramMatrix[i + 1, pivotCol] > 0)
                {
                    winningRatio = 0;
                    pivotRow = i + 1;
                }
            }

            //Makes sure the winning row isnt the top row
            if (pivotRow == 0)
                return true;

            return false;
        }

        //TODO move this to another place, as ratio test has been taken out and can be used for dual
        override public LinearProgram Solve()
        {
            int tableauNumber = 0;

            bool done = true;
            bool answerFound = true;

            if (CheckIfContinue())
            {
                done = false;
                answerFound = false;
            }

            //Loops till final table
            while (!done)
            {
                tableauNumber++;
                int pivotRow = 0;
                int pivotCol = 0;

                if (RatioTest(out  pivotRow, out  pivotCol))
                {
                    done = true;
                    break;
                }

                CalculateNewCellValues(pivotRow, pivotCol);

                //Displays the table
                Console.WriteLine("\nTable " + tableauNumber);
                LinearProgram.DisplayCurrentTable();

                done = true;
                answerFound = true;

                if (CheckIfContinue())
                {
                    done = false;
                    answerFound = false;
                }
            }

            //Checks if there is an answer
            //TODO Handle the case when there is no solution found, as currently it will display no solution but still return the lp
            if (answerFound == true)
            {

            }
            else
            {
                Console.WriteLine("No Solution");
            }

            return LinearProgram;
        }

        
    }
}
