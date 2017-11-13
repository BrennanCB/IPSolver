using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IPSolver
{
    class Dual : Simplex
    {
        public Dual(LinearProgram linearProgram)
            : base(linearProgram)
        {
            LinearProgram = linearProgram;
        }

        override public bool CheckIfContinue()
        {
            for (int i = 1; i < LinearProgram.RowCount; i++)
            {
                if (Math.Round(LinearProgram.LinearProgramMatrix[i, LinearProgram.ColumnCount - 1], 10) < 0)
                    return true;
            }

            return false;
        }

        //Returns true of the ratio test fails
        override public bool RatioTest(out int pivotCol, out int pivotRow)
        {
            double pivotRowAmount = 0;

            pivotRow = 0;
            pivotCol = 0;

            double winningRatio = double.MaxValue;

            List<double> ratios = new List<double>();

            for (int i = 1; i < LinearProgram.RowCount; i++)
            {
                if (Math.Round(LinearProgram.LinearProgramMatrix[i, LinearProgram.ColumnCount - 1]) < pivotRowAmount)
                {
                    pivotRow = i;
                    pivotRowAmount = Math.Round(LinearProgram.LinearProgramMatrix[i, LinearProgram.ColumnCount - 1]);
                }
            }

            if (pivotRow == 0)
                return true;

            for (int i = 1; i < LinearProgram.ColumnCount - 1; i++)
            {
                if (LinearProgram.LinearProgramMatrix[pivotRow, i] < 0)
                {
                    //Makes sure that cannot divide by zero
                    try
                    {
                        double tempRatio = Math.Abs(Math.Round(LinearProgram.LinearProgramMatrix[0, i] / LinearProgram.LinearProgramMatrix[pivotRow, i], 10));

                        ratios.Add(tempRatio);
                    }
                    catch (DivideByZeroException)
                    {
                        ratios.Add(-1);
                    }
                }
                else
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

                    //check
                    pivotCol = i + 1;
                }
                else if (ratios[i] == 0)
                {
                    if (LinearProgram.LinearProgramMatrix[pivotRow, i + 1] > 0)
                    {
                        winningRatio = 0;
                        pivotCol = i + 1;
                    }
                }
            }

            //Makes sure the winning row isnt the top row
            if (pivotRow == 0)
                return true;

            return false;
        }

        //TODO move this to another place, as ratio test has been taken out and can be used for simplex
        override public LinearProgram Solve()
        {
            int tableauNumber = 0;

            bool done = false;
            bool answerFound = true;

            //Loops till final table
            do
            {
                tableauNumber++;

                if (RatioTest(out int pivotCol, out int pivotRow))
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
            } while (!done);

            if (answerFound)
            {
                PrimalSimplex simplex = new PrimalSimplex(LinearProgram);

                LinearProgram = simplex.Solve();
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
