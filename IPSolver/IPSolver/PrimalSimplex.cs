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

        #region redactedCode

        ////Solves Max Simplex
        //public static double[] MaxSimplex()
        //{
        //    int counter = 0;
        //    int colAmount = lp.GetLinearProgram().GetLength(1), rowAmount = lp.GetLinearProgram().GetLength(0);

        //    //Lists to hold BV and Ratios
        //    List<int> posFinalBV = new List<int>();
        //    List<double> ratios = new List<double>();

        //    //Array for final answers
        //    double[] optimalSolutions = new double[colAmount - 1];

        //    int winningCol = 0;
        //    int winningRow = 0;
        //    double winningRatio = double.MaxValue;
        //    double winningColAmount = 0;
        //    bool done = false;

        //    //Loops till final table
        //    do
        //    {
        //        counter++;

        //        //Resets the variables
        //        winningCol = 0;
        //        winningColAmount = 0;
        //        winningRow = 0;
        //        winningRatio = double.MaxValue;
        //        ratios.Clear();

        //        //Loops through the rows to choose the winning column
        //        for (int i = 1; i < lp.GetLinearProgram().GetLength(1) - 1; i++)
        //        {
        //            if (Math.Round(lp.GetLinearProgram()[0, i], 10) < winningColAmount)
        //            {
        //                winningCol = i;
        //                winningColAmount = Math.Round(lp.GetLinearProgram()[0, i], 10);
        //            }
        //        }

        //        //Makes sure the winning column isnt Z
        //        if (winningCol == 0)
        //        {
        //            done = true;
        //        }
        //        else
        //        {
        //            //Calculates the ratios
        //            for (int i = 1; i < lp.GetLinearProgram().GetLength(0); i++)
        //            {
        //                //Makes sure that cannot divide by zero
        //                try
        //                {
        //                    double tempRatio = Math.Round(lp.GetLinearProgram()[i, lp.GetLinearProgram().GetLength(1) - 1] / lp.GetLinearProgram()[i, winningCol], 10);

        //                    ratios.Add(tempRatio);
        //                }
        //                catch (DivideByZeroException)
        //                {
        //                    ratios.Add(-1);
        //                }
        //            }

        //            //Chooses the winning row
        //            for (int i = 0; i < ratios.Count(); i++)
        //            {
        //                if (ratios[i] < winningRatio && ratios[i] > 0)
        //                {
        //                    winningRatio = ratios[i];
        //                    winningRow = i + 1;
        //                }
        //                else if (ratios[i] == 0)
        //                {
        //                    if (lp.GetLinearProgram()[i + 1, winningCol] > 0)
        //                    {
        //                        winningRatio = 0;
        //                        winningRow = i + 1;
        //                    }
        //                }
        //            }

        //            //Makes sure the winning row isnt the top row
        //            if (winningRow == 0)
        //            {
        //                done = true;
        //            }
        //            else
        //            {
        //                double winningNumber = lp.GetLinearProgram()[winningRow, winningCol];

        //                //Calculates the new values of winning row
        //                for (int i = 0; i < colAmount; i++)
        //                {
        //                    double newAmount = lp.GetLinearProgram()[winningRow, i] / winningNumber;

        //                    lp.GetLinearProgram()[winningRow, i] = newAmount;
        //                }

        //                //Calculates the new amounts of the remaining rows
        //                for (int i = 0; i < rowAmount; i++)
        //                {
        //                    double subtractAmount = lp.GetLinearProgram()[i, winningCol];
        //                    for (int j = 0; j < colAmount; j++)
        //                    {
        //                        if (i != winningRow)
        //                        {
        //                            lp.GetLinearProgram()[i, j] = lp.GetLinearProgram()[i, j] - subtractAmount * lp.GetLinearProgram()[winningRow, j];
        //                        }
        //                    }
        //                }

        //                //Displays the table
        //                Console.WriteLine();
        //                Console.WriteLine("Table " + counter);
        //                DisplayTable();

        //                done = true;
        //                answerFound = true;

        //                //Checks if there are any negatives in the top row, to see if it must continue
        //                for (int i = 0; i < colAmount; i++)
        //                {
        //                    if (Math.Round(lp.GetLinearProgram()[0, i], 10) < 0)
        //                    {
        //                        done = false;
        //                        answerFound = false;
        //                    }
        //                }
        //            }
        //        }
        //    } while (done == false);

        //    //Checks if there is an answer
        //    if (answerFound == true)
        //    {
        //        //Finds all the BVs
        //        for (int j = 0; j < colAmount - 1; j++)
        //        {
        //            bool bv = true;
        //            int countOne = 0;
        //            double optimalSolution = 0;

        //            for (int i = 0; i < rowAmount; i++)
        //            {
        //                double currentNumber = lp.GetLinearProgram()[i, j];

        //                if (currentNumber != 0 && currentNumber != 1)
        //                {
        //                    bv = false;
        //                }
        //                else if (lp.GetLinearProgram()[i, j] == 1)
        //                {
        //                    countOne++;

        //                    if (countOne > 1)
        //                    {
        //                        bv = false;
        //                    }
        //                    else
        //                    {
        //                        optimalSolution = lp.GetLinearProgram()[i, colAmount - 1];
        //                    }
        //                }
        //            }

        //            if (bv == false)
        //            {
        //                optimalSolutions[j] = 0;
        //            }
        //            else if (bv == true && countOne == 1)
        //            {
        //                optimalSolutions[j] = optimalSolution;
        //            }
        //        }

        //        //Returns the optimal solutions
        //        return optimalSolutions;
        //    }
        //    else
        //    {
        //        Console.WriteLine("No Solution");
        //    }

        //    foreach (var item in optimalSolutions)
        //    {
        //        Console.WriteLine(item);
        //    }

        //    return optimalSolutions;
        //}

        ////Solves Min Simplex
        //public static double[] MinSimplex()
        //{
        //    int counter = 0;
        //    int colAmount = lp.GetLinearProgram().GetLength(1), rowAmount = lp.GetLinearProgram().GetLength(0);

        //    //Lists to hold BV and Ratios
        //    List<int> posFinalBV = new List<int>();
        //    List<double> ratios = new List<double>();

        //    //Array for final answers
        //    double[] optimalSolutions = new double[colAmount - 1];

        //    int winningCol = 0;
        //    int winningRow = 0;
        //    double winningRatio = double.MaxValue;
        //    double winningColAmount = 0;
        //    bool done = false;

        //    //Loops till final table
        //    do
        //    {
        //        counter++;

        //        //Resets the variables
        //        winningCol = 0;
        //        winningColAmount = 0;
        //        winningRow = 0;
        //        winningRatio = double.MaxValue;
        //        ratios.Clear();

        //        //Loops through the rows t choose the winning column
        //        for (int i = 1; i < lp.GetLinearProgram().GetLength(1) - 1; i++)
        //        {
        //            if (Math.Round(lp.GetLinearProgram()[0, i], 10) > winningColAmount)
        //            {
        //                winningCol = i;
        //                winningColAmount = Math.Round(lp.GetLinearProgram()[0, i], 10);
        //            }
        //        }

        //        //Makes sure the winning column isnt Z
        //        if (winningCol == 0)
        //        {
        //            done = true;
        //            answerFound = true;
        //        }
        //        else
        //        {
        //            //Calculates the ratios
        //            for (int i = 1; i < lp.GetLinearProgram().GetLength(0); i++)
        //            {
        //                //Makes sure that cannot divide by zero
        //                try
        //                {
        //                    ratios.Add(Math.Round(lp.GetLinearProgram()[i, lp.GetLinearProgram().GetLength(1) - 1] / lp.GetLinearProgram()[i, winningCol]));
        //                }
        //                catch (DivideByZeroException)
        //                {
        //                    ratios.Add(-1);
        //                }
        //            }

        //            //Chooses the winning row
        //            for (int i = 0; i < ratios.Count(); i++)
        //            {
        //                if (ratios[i] < winningRatio && ratios[i] > 0)
        //                {
        //                    winningRatio = ratios[i];
        //                    winningRow = i + 1;
        //                }
        //                else if (ratios[i] == 0)
        //                {
        //                    if (lp.GetLinearProgram()[i + 1, winningCol] > 0)
        //                    {
        //                        winningRatio = 0;
        //                        winningRow = i + 1;
        //                    }
        //                }
        //            }

        //            //Makes sure the winning row isnt the top row
        //            if (winningRow == 0)
        //            {
        //                done = true;

        //            }
        //            else
        //            {
        //                double winningNumber = lp.GetLinearProgram()[winningRow, winningCol];

        //                //Calculates the new values of winning row
        //                for (int i = 0; i < colAmount; i++)
        //                {
        //                    double newAmount = lp.GetLinearProgram()[winningRow, i] / winningNumber;

        //                    lp.GetLinearProgram()[winningRow, i] = newAmount;
        //                }

        //                //Calculates the new amounts of the remaining rows
        //                for (int i = 0; i < rowAmount; i++)
        //                {
        //                    double subtractAmount = lp.GetLinearProgram()[i, winningCol];
        //                    for (int j = 0; j < colAmount; j++)
        //                    {
        //                        if (i != winningRow)
        //                        {
        //                            lp.GetLinearProgram()[i, j] = lp.GetLinearProgram()[i, j] - subtractAmount * lp.GetLinearProgram()[winningRow, j];
        //                        }
        //                    }
        //                }

        //                //Displays the table
        //                Console.WriteLine();
        //                Console.WriteLine("Table " + counter);
        //                DisplayTable();

        //                done = true;
        //                answerFound = true;

        //                //Checks if there are any positives in the top row, to see if it must continue
        //                for (int i = 0; i < colAmount; i++)
        //                {
        //                    if (Math.Round(lp.GetLinearProgram()[0, i], 10) > 0)
        //                    {
        //                        done = false;
        //                        answerFound = false;
        //                    }
        //                }
        //            }
        //        }
        //    } while (done == false);

        //    //Checks if answer is found
        //    if (answerFound == true)
        //    {
        //        //Finds BVs
        //        for (int j = 0; j < colAmount - 1; j++)
        //        {
        //            bool bv = true;
        //            int countOne = 0;
        //            double optimalSolution = 0;

        //            for (int i = 0; i < rowAmount; i++)
        //            {
        //                double currentNumber = lp.GetLinearProgram()[i, j];

        //                if (currentNumber != 0 && currentNumber != 1)
        //                {
        //                    bv = false;
        //                }
        //                else if (lp.GetLinearProgram()[i, j] == 1)
        //                {
        //                    countOne++;

        //                    if (countOne > 1)
        //                    {
        //                        bv = false;
        //                    }
        //                    else
        //                    {
        //                        optimalSolution = lp.GetLinearProgram()[i, colAmount - 1];
        //                    }
        //                }
        //            }

        //            if (bv == false)
        //            {
        //                optimalSolutions[j] = 0;
        //            }
        //            else if (bv == true && countOne == 1)
        //            {
        //                optimalSolutions[j] = optimalSolution;
        //            }
        //        }

        //        //Returns the solutions
        //        return optimalSolutions;
        //    }
        //    else
        //    {
        //        Console.WriteLine("No Solution");
        //    }

        //    foreach (var item in optimalSolutions)
        //    {
        //        Console.WriteLine(item);
        //    }

        //    return optimalSolutions;
        //}
        #endregion
    }
}
