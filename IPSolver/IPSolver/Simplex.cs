using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IPSolver
{
    public class Simplex
    {
        LinearProgram lp;

        public Simplex(LinearProgram lp)
        {
            this.lp = lp;
        }


        //Returns the Basic Variables
        public LinearProgram Solve(LPType type)
        {
            int tableauNumber = 0;
            int colAmount = lp.getColumnCount();
            int rowAmount = lp.getRowCount();

            //Lists to hold BV and Ratios
            List<int> posFinalBV = new List<int>();

            bool done = false;
            bool answerFound = true; 

            //Loops till final table
            do
            {
                tableauNumber++;

                //Resets the variables
                int winningCol = 0;
                double winningColAmount = 0;

                int winningRow = 0;

                double winningRatio = double.MaxValue;

                List<double> ratios = new List<double>();

                //Loops through the rows to choose the winning column
                for (int i = 1; i < lp.GetLinearProgram().GetLength(1) - 1; i++)
                {
                    if (type == LPType.Max)
                    {
                        if (Math.Round(lp.GetLinearProgram()[0, i], 10) < winningColAmount)
                        {
                            winningCol = i;
                            winningColAmount = Math.Round(lp.GetLinearProgram()[0, i], 10);
                        }
                    }
                    else
                    {
                        if (Math.Round(lp.GetLinearProgram()[0, i], 10) > winningColAmount)
                        {
                            winningCol = i;
                            winningColAmount = Math.Round(lp.GetLinearProgram()[0, i], 10);
                        }
                    }
                }

                //Makes sure the winning column isnt Z
                if (winningCol == 0)
                {
                    done = true;
                    break;
                }

                    //Calculates the ratios
                    for (int i = 1; i < lp.GetLinearProgram().GetLength(0); i++)
                    {
                        //Makes sure that cannot divide by zero
                        try
                        {
                            double tempRatio = Math.Round(lp.GetLinearProgram()[i, lp.GetLinearProgram().GetLength(1) - 1] / lp.GetLinearProgram()[i, winningCol], 10);

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
                            if (lp.GetLinearProgram()[i + 1, winningCol] > 0)
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
                        break;
                    }
                    
                        double winningNumber = lp.GetLinearProgram()[winningRow, winningCol];

                        //Calculates the new values of winning row
                        for (int i = 0; i < colAmount; i++)
                        {
                            double newAmount = lp.GetLinearProgram()[winningRow, i] / winningNumber;

                            lp.GetLinearProgram()[winningRow, i] = newAmount;
                        }

                        //Calculates the new amounts of the remaining rows
                        for (int i = 0; i < rowAmount; i++)
                        {
                            double subtractAmount = lp.GetLinearProgram()[i, winningCol];
                            for (int j = 0; j < colAmount; j++)
                            {
                                if (i != winningRow)
                                    lp.GetLinearProgram()[i, j] = lp.GetLinearProgram()[i, j] - subtractAmount * lp.GetLinearProgram()[winningRow, j];  
                            }
                        }

                        //Displays the table
                        Console.WriteLine("\nTable " + tableauNumber);
                        UserInterfaceHandler.DisplayTable(lp);

                        done = true;

                        answerFound = true;

                        if (type == LPType.Max)
                        {
                            //Checks if there are any negatives in the top row, to see if it must continue
                            for (int i = 0; i < colAmount; i++)
                            {
                                if (Math.Round(lp.GetLinearProgram()[0, i], 10) < 0)
                                {
                                    done = false;
                                    answerFound = false;
                                    break;
                                }
                            }
                        } 
                        else
                        {
                            //Checks if there are any positives in the top row, to see if it must continue
                            for (int i = 0; i < colAmount; i++)
                            {
                                if (Math.Round(lp.GetLinearProgram()[0, i], 10) > 0)
                                {
                                    done = false;
                                    answerFound = false;
                                    break;
                                }
                            }
                        }   
            } while (done == false);

            //Checks if there is an answer
            //TODO Handle the case when there is no solution found, as currently it will display no solution but still return the lp
            if (answerFound == true)
            {
                
            }
            else
            {
                Console.WriteLine("No Solution");
            }

            return lp;
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
