using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IPSolver
{
    class TwoPhase
    {
        //public static double[,] originalLP, arrayS, arrayE, arrayA, formattedLP, finalLP, twoPhaseLP;
        //public static int countX = 0, countS = 0, countA = 0, countE = 0;
        //public static List<int> listOfA = new List<int>();
        //public static List<int> colOfA = new List<int>();
       
        LinearProgram linearProgram;
        //private double[,] twoPhaseLP;

        public TwoPhase(LinearProgram linearProgram)
        {
            this.linearProgram = linearProgram;
        }

        //Formats the table for two phase
        private void FormatTwoPhase()
        {
            //Makes the twoPhase array larger
            //twoPhaseLP = new double[linearProgram.GetLinearProgram().GetLength(0) + 1,
            //    linearProgram.GetLinearProgram().GetLength(1) + 1];

            //Adds the w and z
            linearProgram.LinearProgramArray[0, 0] = 1;
            linearProgram.LinearProgramArray[0, 1] = 0;

            int counterW = 2;

            //Adds 0's to the X, S and E columns
            for (int i = 2; i < (linearProgram.CountX + linearProgram.CountS + linearProgram.CountE) + 2; i++)
            {
                linearProgram.LinearProgramArray[0, i] = 0;
                counterW++;
            }

            //Adds -1 to the A column
            for (int i = 0; i < linearProgram.CountA; i++)
            {
                linearProgram.LinearProgramArray[0, counterW] = -1;
                counterW++;
            }

            //Sets RHS to 0
            linearProgram.LinearProgramArray[0, linearProgram.ColumnCount - 1] = 0;

            for (int i = 1; i < linearProgram.RowCount; i++)
            {
                linearProgram.LinearProgramArray[i, 0] = 0;
            }

            //FIlls the rest of thw array with the simplex array amounts
            for (int i = 0; i < linearProgram.RowCount; i++)
            {
                for (int j = 0; j < linearProgram.ColumnCount; j++)
                {
                    linearProgram.LinearProgramArray[i + 1, j + 1] = linearProgram.LinearProgramArray[i, j];
                }
            }
        }

        //Solves Two phase problems
        public LinearProgram Solve(LPType type)
        {
            bool answerFound = false ; //TODO: review change
            int counter = 1;
            int colAmount = linearProgram.ColumnCount, rowAmount = linearProgram.RowCount;

            //Lists to hold Ratios
            List<double> ratios = new List<double>();
            
            bool done = false;

            //Calculates the New W row

            foreach (var item in listOfA)
            {
                for (int i = 0; i < rowAmount; i++)
                {
                    linearProgram.LinearProgramArray[0, i] += linearProgram.LinearProgramArray[item + 1, i];
                }
            }

            Console.WriteLine();
            Console.WriteLine("Phase 1 - Table 1");
            UserInterfaceHandler.DisplayTable(linearProgram);

            //Loops till final table
            do
            {
                counter++;

                //Resets the variables
                int winningCol = 0;
                double winningColAmount = 0;
                int winningRow = 0;
                double winningRatio = double.MaxValue;
                ratios.Clear();

                //Loops through the rows t choose the winning column
                for (int i = 2; i < linearProgram.ColumnCount - 1; i++)
                {
                    if (linearProgram.LinearProgramArray[0, i] > winningColAmount)
                    {
                        winningCol = i;
                        winningColAmount = linearProgram.LinearProgramArray[0, i];
                    }
                }

                //Makes sure the winning column isnt Z or W
                if (winningCol == 0)
                {
                    done = true;
                    answerFound = true;
                }
                else
                {
                    //Calculates the ratios
                    for (int i = 2; i < linearProgram.RowCount; i++)
                    {
                        //Makes sure that cannot divide by zero
                        try
                        {
                            ratios.Add(linearProgram.LinearProgramArray[i, linearProgram.ColumnCount - 1] / linearProgram.LinearProgramArray[i, winningCol]);
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
                            winningRow = i + 2;
                        }
                        else if (ratios[i] == 0)
                        {
                            if (linearProgram.LinearProgramArray[i + 2, winningCol] > 0)
                            {
                                winningRatio = 0;
                                winningRow = i + 2;
                            }
                        }
                    }

                    //Makes sure the winning row isnt the top two rows
                    if (winningRow == 0)
                    {
                        done = true;
                    }
                    else
                    {
                        double winningNumber = linearProgram.LinearProgramArray[winningRow, winningCol];

                        //Calculates the new values of winning row
                        for (int i = 0; i < colAmount; i++)
                        {
                            double newAmount = linearProgram.LinearProgramArray[winningRow, i] / winningNumber;

                            linearProgram.LinearProgramArray[winningRow, i] = newAmount;
                        }

                        //Calculates the new amounts of the remaining rows
                        for (int i = 0; i < rowAmount; i++)
                        {
                            double subtractAmount = linearProgram.LinearProgramArray[i, winningCol];
                            for (int j = 0; j < colAmount; j++)
                            {
                                if (i != winningRow)
                                {
                                    linearProgram.LinearProgramArray[i, j] = linearProgram.LinearProgramArray[i, j] - subtractAmount * linearProgram.LinearProgramArray[winningRow, j];
                                }
                            }
                        }

                        //Displays the table
                        Console.WriteLine();
                        Console.WriteLine("Phase 1 - Table " + counter);
                        UserInterfaceHandler.DisplayTable(linearProgram);

                        done = true;
                        answerFound = true;

                        //Checks if there are any positive in the top row, to see if it must continue
                        for (int i = 0; i < colAmount; i++)
                        {
                            if (linearProgram.LinearProgramArray[0, i] > 0)
                            {
                                done = false;
                                answerFound = false;
                            }
                        }

                        double wRHS = Math.Round(linearProgram.LinearProgramArray[0, linearProgram.ColumnCount - 1], 10);

                        //Checks if the W rhs amount is 0
                        if (wRHS == 0)
                        {
                            done = true;
                            answerFound = true;
                        }
                    }
                }
            } while (done == false);

            //Checks if an answer is found
            if (answerFound == true)
            {
                //Finds BVs
                double[] bvCols = linearProgram.GetBasicVariables();

                bool deleteNegatives = false;

                //If A is BV, delete Negatives
                foreach (var item in linearProgram.ColOfA)
                {
                    foreach (var item1 in bvCols)
                    {
                        if (item + 1 == item1)
                        {
                            deleteNegatives = true;
                        }
                    }
                }

                //Deletes the A's
                if (Math.Round(linearProgram.LinearProgramArray[0, linearProgram.ColumnCount - 1], 10) == 0)
                {
                    if (deleteNegatives == true)
                    {
                        for (int i = 0; i < linearProgram.ColumnCount; i++)
                        {
                            if (linearProgram.LinearProgramArray[0, i] < 0)
                            {
                                for (int j = 0; j < linearProgram.RowCount; j++)
                                {
                                    linearProgram.LinearProgramArray[j, i] = 0;
                                }
                            }
                        }
                    }

                    for (int i = 0; i < finalLP.GetLength(0); i++)
                    {
                        for (int j = 0; j < finalLP.GetLength(1); j++)
                        {
                            finalLP[i, j] = twoPhaseLP[i + 1, j + 1];
                        }
                    }

                    //Sets the Amounts in the A coulmns to 0
                    foreach (var item in colOfA)
                    {
                        if (!bvCols.Contains(item + 1))
                        {
                            for (int i = 0; i < finalLP.GetLength(0); i++)
                            {
                                finalLP[i, item] = 0;
                            }
                        }
                    }

                    Console.WriteLine();
                    Console.WriteLine("Phase 2 - Initial Table");
                    UserInterfaceHandler.DisplayTable(linearProgram);

                    //Calls the appropriate simplex method

                    Simplex simplex = new Simplex(linearProgram);


                    linearProgram = simplex.Solve(type);

                    return linearProgram;
                }
                else
                {
                    Console.WriteLine("No Solution");
                }
            }
            else
            {
                if (winningRow == 0)
                {
                    Console.WriteLine("This LP is unbounded and has no solution");
                }

                Console.WriteLine("No Solution");

                Console.WriteLine("");
            }
        }
    }
}
