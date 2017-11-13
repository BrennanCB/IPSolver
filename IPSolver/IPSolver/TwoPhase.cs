using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IPSolver
{
    class TwoPhase : Simplex
    {
        private double[,] simplexLP;

        public TwoPhase(LinearProgram linearProgram)
            :base(linearProgram)
        {
            LinearProgram = linearProgram;

            FormatTwoPhase();
        }

        //Formats the table for two phase
        public void FormatTwoPhase()
        {
            double[,] twoPhaseLP;

            //Makes the twoPhase array larger
            simplexLP = LinearProgram.LinearProgramMatrix;

            twoPhaseLP = new double[LinearProgram.RowCount + 1, LinearProgram.ColumnCount + 1];

            //Adds the w and z
            twoPhaseLP[0, 0] = 1;
            twoPhaseLP[0, 1] = 0;

            int counterW = 2;

            //Adds 0's to the X, S and E columns
            for (int i = 2; i < (LinearProgram.CountX + LinearProgram.CountS + LinearProgram.CountE) + 2; i++)
            {
                twoPhaseLP[0, i] = 0;
                counterW++;
            }

            //Adds -1 to the A column
            for (int i = 0; i < LinearProgram.CountA; i++)
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
            for (int i = 0; i < LinearProgram.RowCount; i++)
            {
                for (int j = 0; j < LinearProgram.ColumnCount; j++)
                {
                    twoPhaseLP[i + 1, j + 1] = LinearProgram.LinearProgramMatrix[i, j];
                }
            }

            LinearProgram.LinearProgramMatrix = twoPhaseLP;

            //Calculates the New W row
            for (int a = 0; a < LinearProgram.ListOfA.Count; a++)
            {
                for (int i = 0; i < LinearProgram.ColumnCount; i++)
                {
                    LinearProgram.LinearProgramMatrix[0, i] += LinearProgram.LinearProgramMatrix[LinearProgram.ListOfA[a] + 1, i];
                }
            }
        }

        override public bool CheckIfContinue()
        {
            //Checks if there are any positive in the top row, to see if it must continue
            for (int i = 0; i < LinearProgram.ColumnCount; i++)
            {
                if (LinearProgram.LinearProgramMatrix[0, i] > 0)
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
            for (int i = 2; i < LinearProgram.ColumnCount - 1; i++)
            {
                if (LinearProgram.LinearProgramMatrix[0, i] > pivotColAmount)
                {
                    pivotCol = i;
                    pivotColAmount = LinearProgram.LinearProgramMatrix[0, i];
                }
            }

            //Makes sure the winning column isnt Z or W
            if (pivotCol == 0)
                return true;

            //Calculates the ratios
            for (int i = 2; i < LinearProgram.RowCount; i++)
            {
                //Makes sure that cannot divide by zero
                try
                {
                    ratios.Add(LinearProgram.LinearProgramMatrix[i, LinearProgram.ColumnCount - 1] / LinearProgram.LinearProgramMatrix[i, pivotCol]);
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
                    pivotRow = i + 2;
                }
                else if (ratios[i] == 0 && LinearProgram.LinearProgramMatrix[i + 2, pivotCol] > 0)
                {
                    winningRatio = 0;
                    pivotRow = i + 2;
                }
            }

            //Makes sure the winning row isnt the top two rows
            if (pivotRow == 0)
                return true;

            return false;
        }

        override public LinearProgram Solve()
        {
            int tableauNumber = 1;

            bool done = false;
            bool answerFound = false;

            int pivotRow = 0;
            
            Console.WriteLine("\nPhase 1 - Table 1");
            LinearProgram.DisplayCurrentTable();

            //Loops till final table
            do
            {
                tableauNumber++;
                
                pivotRow = 0;

                if (RatioTest(out pivotRow, out int pivotCol))
                {
                    done = true;
                    break;
                }

                CalculateNewCellValues(pivotRow, pivotCol);

                Console.WriteLine("\nPhase 1 - Table " + tableauNumber);
                LinearProgram.DisplayCurrentTable();

                done = true;
                answerFound = true;

                if (CheckIfContinue())
                {
                    done = false;
                    answerFound = false;
                }

                double wRHS = Math.Round(LinearProgram.LinearProgramMatrix[0, LinearProgram.ColumnCount - 1], 10);

                //Checks if the W rhs amount is 0
                if (wRHS == 0)
                {
                    done = true;
                    answerFound = true;
                }
            } while (!done);

            //Checks if an answer is found
            if (answerFound)
            {
                //Finds BVs
                double[] bvs = LinearProgram.GetBasicVariables();
                List<int> bvCols = new List<int>();

                for (int i = 0; i < bvs.Length; i++)
                {
                    if (bvs[i] != 0)
                        bvCols.Add(i);
                }

                bool deleteNegatives = false;

                //If A is BV, delete Negatives
                for (int aCol = 0; aCol < LinearProgram.ColOfA.Count; aCol++)
                {
                    for (int bvCol = 0; bvCol < bvCols.Count; bvCol++)
                    {
                        if (LinearProgram.ColOfA[aCol] + 1 == bvCols[bvCol])
                        {
                            //Breaks out of outer loop
                            aCol = LinearProgram.ColOfA.Count;

                            deleteNegatives = true;
                            break;
                        }
                    }
                }

                //Deletes the A's
                if (Math.Round(LinearProgram.LinearProgramMatrix[0, LinearProgram.ColumnCount - 1], 10) == 0)
                {
                    if (deleteNegatives)
                    {
                        for (int i = 0; i < LinearProgram.ColumnCount; i++)
                        {
                            if (LinearProgram.LinearProgramMatrix[0, i] < 0)
                            {
                                for (int j = 0; j < LinearProgram.RowCount; j++)
                                {
                                    LinearProgram.LinearProgramMatrix[j, i] = 0;
                                }
                            }
                        }
                    }

                    for (int i = 0; i < simplexLP.GetLength(0); i++)
                    {
                        for (int j = 0; j < simplexLP.GetLength(1); j++)
                        {
                            simplexLP[i, j] = LinearProgram.LinearProgramMatrix[i + 1, j + 1];
                        }
                    }

                    //Sets the Amounts in the A coulmns to 0
                    foreach (var item in LinearProgram.ColOfA)
                    {
                        if (!bvCols.Contains(item + 1))
                        {
                            for (int i = 0; i < simplexLP.GetLength(0); i++)
                            {
                                simplexLP[i, item] = 0;
                            }
                        }
                    }
                    
                    LinearProgram.IsTwoPhase = false;
                    
                    Console.WriteLine("\nPhase 2 - Initial Table");

                    LinearProgram.LinearProgramMatrix = simplexLP;

                    LinearProgram.DisplayCurrentTable();

                    PrimalSimplex simplex = new PrimalSimplex(LinearProgram);

                    //Calls the appropriate simplex method
                    LinearProgram = simplex.Solve();

                    ///TODO handle what happens if theres no solution
                }
                else
                {
                    Console.WriteLine("No Solution");
                }
            }
            else
            {
                if (pivotRow == 0)
                {
                    Console.WriteLine("This LP is unbounded and has no solution");
                }

                Console.WriteLine("No Solution");

                Console.WriteLine("");
            }

            return LinearProgram;
        }
    }
}