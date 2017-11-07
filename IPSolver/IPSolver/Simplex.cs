using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IPSolver
{
    static class Simplex
    {
        public static double[] Solve(String type)
        {
            int counter = 0;
            int colAmount = finalLP.GetLength(1), rowAmount = finalLP.GetLength(0);

            //Lists to hold BV and Ratios
            List<int> posFinalBV = new List<int>();
            List<double> ratios = new List<double>();

            //Array for final answers
            double[] optimalSolutions = new double[colAmount - 1];

            int winningCol = 0;
            int winningRow = 0;
            double winningRatio = double.MaxValue;
            double winningColAmount = 0;
            bool done = false;

            //Loops till final table
            do
            {
                counter++;

                //Resets the variables
                winningCol = 0;
                winningColAmount = 0;
                winningRow = 0;
                winningRatio = double.MaxValue;
                ratios.Clear();

                //Loops through the rows to choose the winning column
                for (int i = 1; i < finalLP.GetLength(1) - 1; i++)
                {
                    if (type == "MAX")
                    {
                        if (Math.Round(finalLP[0, i], 10) < winningColAmount)
                        {
                            winningCol = i;
                            winningColAmount = Math.Round(finalLP[0, i], 10);
                        }
                    }
                    else
                    {
                        if (Math.Round(finalLP[0, i], 10) > winningColAmount)
                        {
                            winningCol = i;
                            winningColAmount = Math.Round(finalLP[0, i], 10);
                        }
                    }
                }

                //Makes sure the winning column isnt Z
                if (winningCol == 0)
                {
                    done = true;
                }
                else
                {
                    //Calculates the ratios
                    for (int i = 1; i < finalLP.GetLength(0); i++)
                    {
                        //Makes sure that cannot divide by zero
                        try
                        {
                            double tempRatio = Math.Round(finalLP[i, finalLP.GetLength(1) - 1] / finalLP[i, winningCol], 10);

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
                            winningRow = i + 1;
                        }
                        else if (ratios[i] == 0)
                        {
                            if (finalLP[i + 1, winningCol] > 0)
                            {
                                winningRatio = 0;
                                winningRow = i + 1;
                            }
                        }
                    }

                    //Makes sure the winning row isnt the top row
                    if (winningRow == 0)
                    {
                        done = true;
                    }
                    else
                    {
                        double winningNumber = finalLP[winningRow, winningCol];

                        //Calculates the new values of winning row
                        for (int i = 0; i < colAmount; i++)
                        {
                            double newAmount = finalLP[winningRow, i] / winningNumber;

                            finalLP[winningRow, i] = newAmount;
                        }

                        //Calculates the new amounts of the remaining rows
                        for (int i = 0; i < rowAmount; i++)
                        {
                            double subtractAmount = finalLP[i, winningCol];
                            for (int j = 0; j < colAmount; j++)
                            {
                                if (i != winningRow)
                                {
                                    finalLP[i, j] = finalLP[i, j] - subtractAmount * finalLP[winningRow, j];
                                }
                            }
                        }

                        //Displays the table
                        Console.WriteLine();
                        Console.WriteLine("Table " + counter);
                        DisplayTable();

                        done = true;
                        answerFound = true;

                        //Checks if there are any negatives in the top row, to see if it must continue
                        for (int i = 0; i < colAmount; i++)
                        {
                            if (Math.Round(finalLP[0, i], 10) < 0)
                            {
                                done = false;
                                answerFound = false;
                            }
                        }
                    }
                }
            } while (done == false);

            //Checks if there is an answer
            if (answerFound == true)
            {
                //Finds all the BVs
                for (int j = 0; j < colAmount - 1; j++)
                {
                    bool bv = true;
                    int countOne = 0;
                    double optimalSolution = 0;

                    for (int i = 0; i < rowAmount; i++)
                    {
                        double currentNumber = finalLP[i, j];

                        if (currentNumber != 0 && currentNumber != 1)
                        {
                            bv = false;
                        }
                        else if (finalLP[i, j] == 1)
                        {
                            countOne++;

                            if (countOne > 1)
                            {
                                bv = false;
                            }
                            else
                            {
                                optimalSolution = finalLP[i, colAmount - 1];
                            }
                        }
                    }

                    if (bv == false)
                    {
                        optimalSolutions[j] = 0;
                    }
                    else if (bv == true && countOne == 1)
                    {
                        optimalSolutions[j] = optimalSolution;
                    }
                }

                //Returns the optimal solutions
                return optimalSolutions;
            }
            else
            {
                Console.WriteLine("No Solution");
            }

            foreach (var item in optimalSolutions)
            {
                Console.WriteLine(item);
            }

            return optimalSolutions;
        }

        #region redactedCode

        ////Solves Max Simplex
        //public static double[] MaxSimplex()
        //{
        //    int counter = 0;
        //    int colAmount = finalLP.GetLength(1), rowAmount = finalLP.GetLength(0);

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
        //        for (int i = 1; i < finalLP.GetLength(1) - 1; i++)
        //        {
        //            if (Math.Round(finalLP[0, i], 10) < winningColAmount)
        //            {
        //                winningCol = i;
        //                winningColAmount = Math.Round(finalLP[0, i], 10);
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
        //            for (int i = 1; i < finalLP.GetLength(0); i++)
        //            {
        //                //Makes sure that cannot divide by zero
        //                try
        //                {
        //                    double tempRatio = Math.Round(finalLP[i, finalLP.GetLength(1) - 1] / finalLP[i, winningCol], 10);

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
        //                    if (finalLP[i + 1, winningCol] > 0)
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
        //                double winningNumber = finalLP[winningRow, winningCol];

        //                //Calculates the new values of winning row
        //                for (int i = 0; i < colAmount; i++)
        //                {
        //                    double newAmount = finalLP[winningRow, i] / winningNumber;

        //                    finalLP[winningRow, i] = newAmount;
        //                }

        //                //Calculates the new amounts of the remaining rows
        //                for (int i = 0; i < rowAmount; i++)
        //                {
        //                    double subtractAmount = finalLP[i, winningCol];
        //                    for (int j = 0; j < colAmount; j++)
        //                    {
        //                        if (i != winningRow)
        //                        {
        //                            finalLP[i, j] = finalLP[i, j] - subtractAmount * finalLP[winningRow, j];
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
        //                    if (Math.Round(finalLP[0, i], 10) < 0)
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
        //                double currentNumber = finalLP[i, j];

        //                if (currentNumber != 0 && currentNumber != 1)
        //                {
        //                    bv = false;
        //                }
        //                else if (finalLP[i, j] == 1)
        //                {
        //                    countOne++;

        //                    if (countOne > 1)
        //                    {
        //                        bv = false;
        //                    }
        //                    else
        //                    {
        //                        optimalSolution = finalLP[i, colAmount - 1];
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
        //    int colAmount = finalLP.GetLength(1), rowAmount = finalLP.GetLength(0);

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
        //        for (int i = 1; i < finalLP.GetLength(1) - 1; i++)
        //        {
        //            if (Math.Round(finalLP[0, i], 10) > winningColAmount)
        //            {
        //                winningCol = i;
        //                winningColAmount = Math.Round(finalLP[0, i], 10);
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
        //            for (int i = 1; i < finalLP.GetLength(0); i++)
        //            {
        //                //Makes sure that cannot divide by zero
        //                try
        //                {
        //                    ratios.Add(Math.Round(finalLP[i, finalLP.GetLength(1) - 1] / finalLP[i, winningCol]));
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
        //                    if (finalLP[i + 1, winningCol] > 0)
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
        //                double winningNumber = finalLP[winningRow, winningCol];

        //                //Calculates the new values of winning row
        //                for (int i = 0; i < colAmount; i++)
        //                {
        //                    double newAmount = finalLP[winningRow, i] / winningNumber;

        //                    finalLP[winningRow, i] = newAmount;
        //                }

        //                //Calculates the new amounts of the remaining rows
        //                for (int i = 0; i < rowAmount; i++)
        //                {
        //                    double subtractAmount = finalLP[i, winningCol];
        //                    for (int j = 0; j < colAmount; j++)
        //                    {
        //                        if (i != winningRow)
        //                        {
        //                            finalLP[i, j] = finalLP[i, j] - subtractAmount * finalLP[winningRow, j];
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
        //                    if (Math.Round(finalLP[0, i], 10) > 0)
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
        //                double currentNumber = finalLP[i, j];

        //                if (currentNumber != 0 && currentNumber != 1)
        //                {
        //                    bv = false;
        //                }
        //                else if (finalLP[i, j] == 1)
        //                {
        //                    countOne++;

        //                    if (countOne > 1)
        //                    {
        //                        bv = false;
        //                    }
        //                    else
        //                    {
        //                        optimalSolution = finalLP[i, colAmount - 1];
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
