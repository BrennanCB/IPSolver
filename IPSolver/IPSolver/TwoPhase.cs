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
        private int countA;
        private int countS;
        private int countE;
        private int countX;
        private double[,] formattedLp;
        private double[,] twoPhaseLP;

        public TwoPhase(int countX, int countA, int countS, int countE, double[,] formattedLp)
        {
            this.countA = countA;
            this.countS = countS;
            this.countE = countE;
            this.countX = countX;
            this.formattedLp = formattedLp;
        }


        //Formats the table for two phase
        private void FormatTwoPhase()
        {
            //Makes the twoPhase array larger
            twoPhaseLP = new double[formattedLp.GetLength(0) + 1, formattedLp.GetLength(1) + 1];

            //Adds the w and z
            twoPhaseLP[0, 0] = 1;
            twoPhaseLP[0, 1] = 0;

            int counterW = 2;

            //Adds 0's to the X, S and E columns
            for (int i = 2; i < (countX + countS + countE) + 2; i++)
            {
                twoPhaseLP[0, i] = 0;
                counterW++;
            }

            //Adds -1 to the A column
            for (int i = 0; i < countA; i++)
            {
                twoPhaseLP[0, counterW] = -1;
                counterW++;
            }

            //Sets RHS to 0
            twoPhaseLP[0, twoPhaseLP.GetLength(1) - 1] = 0;

            for (int i = 1; i < twoPhaseLP.GetLength(0); i++)
            {
                twoPhaseLP[i, 0] = 0;
            }

            //FIlls the rest of thw array with the simplex array amounts
            for (int i = 0; i < formattedLp.GetLength(0); i++)
            {
                for (int j = 0; j < formattedLp.GetLength(1); j++)
                {
                    twoPhaseLP[i + 1, j + 1] = formattedLp[i, j];
                }
            }
        }

        //Solves Two phase problems
        public void Solve()
        {
            bool answerFound = false ; //TODO: review change
            int counter = 1;
            int colAmount = twoPhaseLP.GetLength(1), rowAmount = twoPhaseLP.GetLength(0);

            //Lists to hold Ratios
            List<double> ratios = new List<double>();

            int winningCol = 0;
            int winningRow = 0;
            double winningRatio = double.MaxValue;
            double winningColAmount = 0;
            bool done = false;

            //Calculates the New W row
            foreach (var item in listOfA)
            {
                for (int i = 0; i < twoPhaseLP.GetLength(1); i++)
                {
                    twoPhaseLP[0, i] += twoPhaseLP[item + 1, i];
                }
            }

            Console.WriteLine();
            Console.WriteLine("Phase 1 - Table 1");
            UserInterfaceHandler.DisplayTable();

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

                //Loops through the rows t choose the winning column
                for (int i = 2; i < twoPhaseLP.GetLength(1) - 1; i++)
                {
                    if (twoPhaseLP[0, i] > winningColAmount)
                    {
                        winningCol = i;
                        winningColAmount = twoPhaseLP[0, i];
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
                    for (int i = 2; i < twoPhaseLP.GetLength(0); i++)
                    {
                        //Makes sure that cannot divide by zero
                        try
                        {
                            ratios.Add(twoPhaseLP[i, twoPhaseLP.GetLength(1) - 1] / twoPhaseLP[i, winningCol]);
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
                            if (twoPhaseLP[i + 2, winningCol] > 0)
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
                        double winningNumber = twoPhaseLP[winningRow, winningCol];

                        //Calculates the new values of winning row
                        for (int i = 0; i < colAmount; i++)
                        {
                            double newAmount = twoPhaseLP[winningRow, i] / winningNumber;

                            twoPhaseLP[winningRow, i] = newAmount;
                        }

                        //Calculates the new amounts of the remaining rows
                        for (int i = 0; i < rowAmount; i++)
                        {
                            double subtractAmount = twoPhaseLP[i, winningCol];
                            for (int j = 0; j < colAmount; j++)
                            {
                                if (i != winningRow)
                                {
                                    twoPhaseLP[i, j] = twoPhaseLP[i, j] - subtractAmount * twoPhaseLP[winningRow, j];
                                }
                            }
                        }

                        //Displays the table
                        Console.WriteLine();
                        Console.WriteLine("Phase 1 - Table " + counter);
                        UserInterfaceHandler.DisplayTable();

                        done = true;
                        answerFound = true;

                        //Checks if there are any positive in the top row, to see if it must continue
                        for (int i = 0; i < colAmount; i++)
                        {
                            if (twoPhaseLP[0, i] > 0)
                            {
                                done = false;
                                answerFound = false;
                            }
                        }

                        double wRHS = Math.Round(twoPhaseLP[0, twoPhaseLP.GetLength(1) - 1], 10);

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
                List<int> bvCols = new List<int>();

                for (int j = 0; j < colAmount - 1; j++)
                {
                    bool bv = true;
                    int countOne = 0;

                    for (int i = 0; i < rowAmount; i++)
                    {
                        double currentNumber = twoPhaseLP[i, j];

                        if (currentNumber != 0 && currentNumber != 1)
                        {
                            bv = false;
                        }
                        else if (twoPhaseLP[i, j] == 1)
                        {
                            countOne++;

                            if (countOne > 1)
                            {
                                bv = false;
                            }
                        }
                    }

                    if (bv == false)
                    {

                    }
                    else if (bv == true && countOne == 1)
                    {
                        bvCols.Add(j);
                    }
                }

                bool deleteNegatives = false;

                //If A is BV, delete Negatives
                foreach (var item in colOfA)
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
                if (Math.Round(twoPhaseLP[0, twoPhaseLP.GetLength(1) - 1], 10) == 0)
                {
                    if (deleteNegatives == true)
                    {
                        for (int i = 0; i < twoPhaseLP.GetLength(1); i++)
                        {
                            if (twoPhaseLP[0, i] < 0)
                            {
                                for (int j = 0; j < twoPhaseLP.GetLength(0); j++)
                                {
                                    twoPhaseLP[j, i] = 0;
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

                    twoPhase = false;

                    Console.WriteLine();
                    Console.WriteLine("Phase 2 - Initial Table");
                    UserInterfaceHandler.DisplayTable();

                    //Calls the appropriate simplex method
                    answers = Simplex(LPType);
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
