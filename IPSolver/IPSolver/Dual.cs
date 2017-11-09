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

        public bool DualRatio(out int winningCol, out int winningRow)
        {
            double winningColAmount = 0;

            double winningRowAmount = 0;


            double winningRatio = double.MaxValue;

            List<double> ratios = new List<double>();

            for (int i = 0; i <= linearProgram.RowCount; i++)
            {
                if (Math.Round(linearProgram.LinearProgramMatrix[])
                {

                }
            }




            //Loops through the rows to choose the winning column
            for (int i = 1; i <= linearProgram.ColumnCount; i++)
            {
                if (linearProgram.Type == LPType.Max)
                {
                    if (Math.Round(linearProgram.LinearProgramMatrix[0, i], 10) < winningColAmount)
                    {
                        winningCol = i;
                        winningColAmount = Math.Round(linearProgram.LinearProgramMatrix[0, i], 10);
                    }
                }
                else
                {
                    if (Math.Round(linearProgram.LinearProgramMatrix[0, i], 10) > winningColAmount)
                    {
                        winningCol = i;
                        winningColAmount = Math.Round(linearProgram.LinearProgramMatrix[0, i], 10);
                    }
                }
            }

            //Makes sure the winning column isnt Z
            if (winningCol == 0)
                return true;

            //Calculates the ratios
            for (int i = 1; i < linearProgram.RowCount; i++)
            {
                //Makes sure that cannot divide by zero
                try
                {
                    double tempRatio = Math.Round(linearProgram.LinearProgramMatrix[i, linearProgram.ColumnCount - 1] / linearProgram.LinearProgramMatrix[i, winningCol], 10);

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
                    if (linearProgram.LinearProgramMatrix[i + 1, winningCol] > 0)
                    {
                        winningRatio = 0;
                        winningRow = i + 1;
                    }
                }
            }

            //Makes sure the winning row isnt the top row
            if (winningRow == 0)
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
                int winningCol = 0;
                int winningRow = 0;

                if (DualRatio(out winningCol, out winningRow))
                {
                    done = true;
                    break;
                }

                double winningNumber = linearProgram.LinearProgramMatrix[winningRow, winningCol];

                //Calculates the new values of winning row
                for (int i = 0; i < colAmount; i++)
                {
                    double newAmount = linearProgram.LinearProgramMatrix[winningRow, i] / winningNumber;

                    linearProgram.LinearProgramMatrix[winningRow, i] = newAmount;
                }

                //Calculates the new amounts of the remaining rows
                for (int i = 0; i < rowAmount; i++)
                {
                    double subtractAmount = linearProgram.LinearProgramMatrix[i, winningCol];
                    for (int j = 0; j < colAmount; j++)
                    {
                        if (i != winningRow)
                            linearProgram.LinearProgramMatrix[i, j] = linearProgram.LinearProgramMatrix[i, j] - subtractAmount * linearProgram.LinearProgramMatrix[winningRow, j];
                    }
                }

                //Displays the table
                Console.WriteLine("\nTable " + tableauNumber);
                UserInterfaceHandler.DisplayTable(linearProgram);

                done = true;

                answerFound = true;

                if (linearProgram.Type == LPType.Max)
                {
                    //Checks if there are any negatives in the top row, to see if it must continue
                    for (int i = 0; i < colAmount; i++)
                    {
                        if (Math.Round(linearProgram.LinearProgramMatrix[0, i], 10) < 0)
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
                        if (Math.Round(linearProgram.LinearProgramMatrix[0, i], 10) > 0)
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

            return linearProgram;
        }
    }
}
