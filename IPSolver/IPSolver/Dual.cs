using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IPSolver
{
    class Dual
    {
        LinearProgram linearProgram;

        public Dual(LinearProgram linearProgram)
        {
            this.linearProgram = linearProgram;
        }

        //Returns true of the ratio test fails
        public bool DualRatioTest(out int pivotCol, out int pivotRow)
        {
            double pivotRowAmount = 0;

            pivotRow = 0;
            pivotCol = 0;

            double winningRatio = double.MaxValue;

            List<double> ratios = new List<double>();

            for (int i = 1; i < linearProgram.RowCount; i++)
            {
                if (Math.Round(linearProgram.LinearProgramMatrix[i, linearProgram.ColumnCount - 1]) < pivotRowAmount)
                {
                    pivotRow = i;
                    pivotRowAmount = Math.Round(linearProgram.LinearProgramMatrix[i, linearProgram.ColumnCount - 1]);
                }
            }

            if (pivotRow == 0)
                return true;

            for (int i = 1; i < linearProgram.ColumnCount - 1; i++)
            {
                if (linearProgram.LinearProgramMatrix[pivotRow, i] < 0)
                {
                    //Makes sure that cannot divide by zero
                    try
                    {
                        double tempRatio = Math.Abs(Math.Round(linearProgram.LinearProgramMatrix[0, i] / linearProgram.LinearProgramMatrix[pivotRow, i], 10));

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
                    if (linearProgram.LinearProgramMatrix[pivotRow, i + 1] > 0)
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
        public LinearProgram Solve()
        {
            int tableauNumber = 0;
            int colAmount = linearProgram.ColumnCount;
            int rowAmount = linearProgram.RowCount;

            bool done = false;
            bool answerFound = true;

            //Loops till final table
            do
            {
                tableauNumber++;

                //Resets the variables
                int pivotCol = 0;
                int pivotRow = 0;

                if (DualRatioTest(out pivotCol, out pivotRow))
                {
                    done = true;
                    break;
                }

                double pivotCellValue = linearProgram.LinearProgramMatrix[pivotRow, pivotCol];

                //Calculates the new values of winning row
                for (int i = 0; i < colAmount; i++)
                {
                    double newAmount = linearProgram.LinearProgramMatrix[pivotRow, i] / pivotCellValue;

                    linearProgram.LinearProgramMatrix[pivotRow, i] = newAmount;
                }

                //Calculates the new amounts of the remaining rows
                for (int i = 0; i < rowAmount; i++)
                {
                    double subtractAmount = linearProgram.LinearProgramMatrix[i, pivotCol];
                    for (int j = 0; j < colAmount; j++)
                    {
                        if (i != pivotRow)
                            linearProgram.LinearProgramMatrix[i, j] = linearProgram.LinearProgramMatrix[i, j] - subtractAmount * linearProgram.LinearProgramMatrix[pivotRow, j];
                    }
                }

                //Displays the table
                Console.WriteLine("\nTable " + tableauNumber);
                linearProgram.DisplayCurrentTable();

                done = true;
                answerFound = true;

                for (int i = 1; i < rowAmount; i++)
                {
                    if (Math.Round(linearProgram.LinearProgramMatrix[i, linearProgram.ColumnCount - 1] , 10) < 0)
                    {
                        done = false;
                        answerFound = false;
                        break;
                    }
                }
            } while (!done);

            if (answerFound)
            {
                Simplex simplex = new Simplex(linearProgram);
                
                linearProgram = simplex.Solve();
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

            return linearProgram;
        }
    }
}
