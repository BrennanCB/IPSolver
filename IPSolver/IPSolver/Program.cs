using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IPSolver
{
    class Program
    {
        public static string LPType;
        public static double[,] originalLP, arrayS, arrayE, arrayA, formattedLP, finalLP, twoPhaseLP;
        public static double[] answers;
        public static List<string> unformatedLP = new List<string>();
        public static List<string> canonicalForm = new List<string>();
        public static List<int> listOfA = new List<int>();
        public static List<int> colOfA = new List<int>();
        public static int countX = 0, countS = 0, countA = 0, countE = 0;
        public static List<string> sign = new List<string>();
        public static bool twoPhase = false, answerFound = false;
        public static double[,] coordindates;
        public static List<int> colY = new List<int>();

        static void Main(string[] args)
        {
            //Calls the menu
            Menu();

            //Gets data from the textfile
            unformatedLP = FileHandler.ReadLP();

            //Writes the data from the text file
            foreach (var item in unformatedLP)
            {
                Console.WriteLine(item);
            }

            //Formates the LP
            FormatSimplxLP(unformatedLP);

            //Puts all variables into one array
            CreateLPFinalForm();

            //Displays the canonical form
            Console.WriteLine();
            Console.WriteLine("Canonical Form");
            Console.WriteLine("--------------");

            foreach (var item in canonicalForm)
            {
                Console.WriteLine(item);
            }

            Console.WriteLine();

            //Sets where it is two pahse or not
            if (countA > 0)
            {
                twoPhase = true;

                FormatTwoPhase();
            }

            Console.WriteLine("Initial Table");

            //Initials the answers array
            answers = new double[finalLP.GetLength(1) - 1];

            //Checks if twophase or not
            if (twoPhase == false)
            {
                //Runs Max Method
                if (LPType == "MAX")
                {
                    DisplayTable();

                    //Runs Max Method
                    answers = MaxSimplex();
                }
                else
                {
                    DisplayTable();

                    //Runs Min Method
                    answers = MinSimplex();
                }
            }
            else
            {
                DisplayTable();

                //Runs Two Phase
                TwoPhase();
            }

            Console.WriteLine();

            double zValue = 0;
            List<double> finalSolution = new List<double>();

            int countAnswers = 0;

            //Displays the optimal solutions
            Console.WriteLine("Optimal Solutions");
            Console.WriteLine("-----------------");
            Console.WriteLine("Z = " + Math.Round(answers[countAnswers], 2));

            //Displays the z-value
            zValue = Math.Round(answers[countAnswers], 2);

            countAnswers++;

            //Displays the X's
            for (int i = 0; i < countX; i++)
            {
                bool isY = false;
                foreach (var item in colY)
                {
                    if (item + 1 == i)
                    {
                        isY = true;
                    }
                }

                if (isY == true)
                {
                    Console.WriteLine("X" + (i + 1) + " = " + Math.Round(answers[countAnswers], 2) * -1);
                }
                else
                {
                    Console.WriteLine("X" + (i + 1) + " = " + Math.Round(answers[countAnswers], 2));
                }

                finalSolution.Add(Math.Round(answers[countAnswers], 2));
                countAnswers++;
            }

            //Displaye the S's
            for (int i = 0; i < countS; i++)
            {
                Console.WriteLine("S" + (i + 1) + " = " + Math.Round(answers[countAnswers], 2));
                countAnswers++;
            }

            //Displaye the E's
            for (int i = 0; i < countE; i++)
            {
                Console.WriteLine("E" + (i + 1) + " = " + Math.Round(answers[countAnswers], 2));
                countAnswers++;
            }

            //Displaye the A's
            for (int i = 0; i < countA; i++)
            {
                Console.WriteLine("A" + (i + 1) + " = " + Math.Round(answers[countAnswers], 2));
                countAnswers++;
            }

            Console.WriteLine();

            //Calls the method that saves the soution
            FileHandler.SaveSolution(zValue, finalSolution);

            Console.WriteLine("The solution has been saved!");

            //Checks if it can draw a graph
            if (countX == 2)
            {
                //Draws graph
                Graph();
            }
            else
            {
                Console.WriteLine("This LP has more than two variables, cannot draw this graph");
            }

            Console.ReadLine();
        }

        //Main Menu
        public static void Menu()
        {
            bool directoryExists = false, isValid = false;

            //Checks if the directory exists
            directoryExists = FileHandler.DirectoryExists();

            //Creates the directory if it doesnt exist
            if (directoryExists != true)
            {
                //Creates the directory
                FileHandler.CreateDirectory();

                Console.WriteLine("A folder called 'Linear Program Solver' has been created in My Documents, please place your files here");

                Console.WriteLine();
                Console.WriteLine("Press any key to continue");

                Console.ReadKey();

                Console.Clear();
            }

            do
            {
                //Displys the menu
                Console.WriteLine("Enter the location of your input and output files which need to be processed inside the 'Linear Program Solver' folder the is located in My Documents");
                Console.WriteLine("Enter the command in the following format:");
                Console.WriteLine();
                Console.WriteLine("Solve <input file> <output file> - Note, the files must be in the format .txt, and the file extension must NOT be added");
                Console.WriteLine("E.g. Solve <input> <output>");

                string userCommand = Console.ReadLine();

                string[] splitCommand = userCommand.Split(' ');

                //Makes sure the user enters correct data
                if (splitCommand[0].ToLower() != "solve" || splitCommand.Length != 3)
                {
                    Console.WriteLine("The command you entered is incorrect!");
                    Console.WriteLine("Enter the command in the following format:");
                    Console.WriteLine("Solve <input file> <output file>");
                }
                else
                {
                    char[] notAllowed = { '<', '>', '/', '\\', ':', '*', '?', '"', '|' };

                    //Removes invalid characters from the file names
                    splitCommand[1] = splitCommand[1].Trim(notAllowed);
                    splitCommand[2] = splitCommand[2].Trim(notAllowed);

                    //Checks if the input file is found
                    bool fileFound = FileHandler.CheckInputFile(splitCommand[1]);

                    //Gives error if not found
                    if (fileFound == false)
                    {
                        Console.WriteLine("The input file you listed cannot be found. Please ensure that:");
                        Console.WriteLine("\tThe name is spelt correctly");
                        Console.WriteLine("\tThe command is in the correct format");
                        Console.WriteLine("\tThat the file does NOT include the file extension");
                        Console.WriteLine("\tThat the file is a textfile(.txt)");
                        Console.WriteLine("\tThat the file is located in the 'Linear Program Solver' folder in My Documents");
                    }
                    else
                    {
                        //Creates the output file
                        FileHandler.CreateOutputFile(splitCommand[2]);

                        Console.WriteLine("Output file created, this is where you will find the solution saved");

                        isValid = true;
                    }
                }

                Console.WriteLine();
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();

                Console.Clear();
            } while (isValid == false);
        }

        //Formats the LP, by adding the S, E and A, and creating the canonical form
        public static void FormatSimplxLP(List<string> unformattedLP)
        {
            //Gets z equation
            string zEquation = unformattedLP[0];

            //Separates the Z's
            string[] tempZ = zEquation.Split(' ');

            //Gets the type of LP from the first line
            LPType = tempZ[0].ToUpper();

            //Sets the sizes of the arrays that will hold all the variables
            originalLP = new double[unformattedLP.Count() - 1, tempZ.Count() + 1];

            arrayA = new double[unformattedLP.Count() - 1, unformattedLP.Count() - 1];
            arrayS = new double[unformattedLP.Count() - 1, unformattedLP.Count() - 1];
            arrayE = new double[unformattedLP.Count() - 1, unformattedLP.Count() - 1];

            //Sets Z to one
            originalLP[0, 0] = 1;

            //Gets the number of X's
            countX = tempZ.Count() - 1;

            try
            {
                //Splits the signs
                string[] tempSign = unformattedLP[unformattedLP.Count() - 1].Split(' ');

                //Adds to list
                foreach (var item in tempSign)
                {
                    sign.Add(item);
                }

                //Does the canonical form for the z equation
                string tempCanonicalForm = "Z ";

                //Cycles through temp Z equation
                for (int i = 1; i < tempZ.Count(); i++)
                {
                    try
                    {
                        //Checks if restriction is negative
                        if (sign[i - 1] == "-")
                        {
                            //Saves X's to array
                            originalLP[0, i] = Convert.ToDouble(tempZ[i]);
                        }
                        else
                        {
                            //Saves X's to array
                            originalLP[0, i] = Convert.ToDouble(tempZ[i]) * -1;
                        }
                    }
                    catch (ArgumentOutOfRangeException) //Catchs error if not enough signs
                    {
                        Console.WriteLine("Please ensire that you gave each variable a constraint");
                        Console.WriteLine();
                        Console.WriteLine("Press any key to continue...");

                        Console.ReadKey();
                        Environment.Exit(0);
                    }
                    //Saves X's to the canonical form
                    tempCanonicalForm += originalLP[0, i] + "X" + i + " ";
                }

                //Adds the Equation to the canonical form list
                canonicalForm.Add(tempCanonicalForm + "= 0");

                bool changeNeg = false;

                //Fills the array( the X's)
                for (int i = 1; i < unformattedLP.Count() - 1; i++)
                {
                    bool changeY = false;

                    string[] tempConstraint = unformattedLP[i].Split(' ');

                    tempCanonicalForm = null;

                    //Changes RHS to positive
                    if (Convert.ToDouble(tempConstraint[tempConstraint.Count() - 1]) < 0)
                    {
                        for (int j = 1; j < tempConstraint.Count() - 1; j++)
                        {
                            //Checks if sign restriction is negative
                            if (sign[j - 1] == "-")
                            {
                                //Changes the column to negative
                                colY.Add(j);

                                originalLP[i, j] = Convert.ToDouble(tempConstraint[j - 1]);

                                tempCanonicalForm += originalLP[i, j] + "Y" + j + " ";
                            }
                            else
                            {
                                originalLP[i, j] = Convert.ToDouble(tempConstraint[j - 1]) * -1;

                                tempCanonicalForm += originalLP[i, j] + "X" + j + " ";
                            }
                        }

                        //Fills in the RHS
                        originalLP[i, originalLP.GetLength(1) - 1] = Convert.ToDouble(tempConstraint[tempConstraint.Count() - 1]) * -1;

                        //Changes the sign
                        if (tempConstraint[tempConstraint.Count() - 2] == "<=")
                        {
                            tempConstraint[tempConstraint.Count() - 2] = ">=";
                        }
                        else if (tempConstraint[tempConstraint.Count() - 2] == ">=")
                        {
                            tempConstraint[tempConstraint.Count() - 2] = "<=";
                        }

                        changeNeg = true;
                    }
                    else
                    {
                        for (int j = 1; j < tempConstraint.Count() - 1; j++)
                        {
                            //Checks if sign restriction is negative
                            if (sign[j - 1] == "-")
                            {
                                //Changes the column to negative
                                colY.Add(j);

                                originalLP[i, j] = Convert.ToDouble(tempConstraint[j - 1]) * -1;

                                tempCanonicalForm += originalLP[i, j] + "Y" + j + " ";
                            }
                            else
                            {
                                originalLP[i, j] = Convert.ToDouble(tempConstraint[j - 1]);

                                tempCanonicalForm += originalLP[i, j] + "X" + j + " ";
                            }
                        }
                        //Fills in the RHS
                        originalLP[i, originalLP.GetLength(1) - 1] = Convert.ToDouble(tempConstraint[tempConstraint.Count() - 1]);
                    }

                    if (tempConstraint[tempConstraint.Count() - 2] == "<=")
                    {
                        //Adds the S's
                        arrayS[i, countS] = 1;
                        countS++;

                        //Adds the S to the canonical form
                        tempCanonicalForm += "S" + countS + " ";
                    }
                    else if (tempConstraint[tempConstraint.Count() - 2] == ">=")
                    {
                        //Adds the E's
                        arrayE[i, countE] = -1;
                        countE++;

                        //Adds The A's
                        arrayA[i, countA] = 1;
                        countA++;

                        //Adds the E and A to the canonical form
                        tempCanonicalForm += "-E" + countE + " " + "A" + countA + " ";
                    }
                    else
                    {
                        //Adds The A's
                        arrayA[i, countA] = 1;
                        countA++;

                        //Adds the A to the canonical form
                        tempCanonicalForm += "A" + countA + " ";
                    }

                    //Changes the RHS
                    if (changeNeg == true)
                    {
                        //Adds it to the canonical form list
                        canonicalForm.Add(tempCanonicalForm + "= " + (originalLP[i, originalLP.GetLength(1) - 1]) * -1);
                    }
                    else
                    {
                        //Adds it to the canonical form list
                        canonicalForm.Add(tempCanonicalForm + "= " + originalLP[i, originalLP.GetLength(1) - 1]);
                    }
                }
            }
            catch (FormatException) //Checks if data is valid
            {
                //Gives error and exits if invalid
                Console.WriteLine("Some of the input data is incorrect, plese check the data and run the program again");
                Console.WriteLine();
                Console.WriteLine("Press any key to exit...");

                Console.ReadKey();

                Environment.Exit(0);
            }
            catch (IndexOutOfRangeException) //Checks if data is valid
            {
                //Gives error and exits if invalid
                Console.WriteLine("Please check your input, ensure that:");
                Console.WriteLine("\tYou entered the correct amount of sign variables");
                Console.WriteLine("\tYou entered every variable and gave each a value in every constraint");

                Console.WriteLine();
                Console.WriteLine("Press any key to exit...");

                Console.ReadKey();

                Environment.Exit(0);
            }
        }

        //Puts the LP, S, E and A arrays into 1 big array
        public static void CreateLPFinalForm()
        {
            //Gets the number of columns of the final array
            int finalLPLength = originalLP.GetLength(1) + countA + countE + countS;

            //Sets the rows and columns for the final array
            finalLP = new double[originalLP.GetLength(0), finalLPLength];

            //Chooses the row
            for (int i = 0; i < finalLP.GetLength(0); i++)
            {
                int mainCol = 0;

                //Saves the LP
                for (int orgCol = 0; orgCol < originalLP.GetLength(1) - 1; orgCol++)
                {
                    finalLP[i, mainCol] = originalLP[i, orgCol];

                    mainCol++;
                }

                //Saves the S's
                for (int sCol = 0; sCol < countS; sCol++)
                {
                    finalLP[i, mainCol] = arrayS[i, sCol];

                    mainCol++;
                }

                //Saves the E's
                for (int eCol = 0; eCol < countE; eCol++)
                {
                    finalLP[i, mainCol] = arrayE[i, eCol];

                    mainCol++;
                }

                //Saves the A's
                for (int aCol = 0; aCol < countA; aCol++)
                {
                    finalLP[i, mainCol] = arrayA[i, aCol];

                    if (finalLP[i, mainCol] == 1)
                    {
                        listOfA.Add(i);
                        colOfA.Add(mainCol);
                    }

                    mainCol++;
                }

                //Saves the RHS
                finalLP[i, mainCol] = originalLP[i, originalLP.GetLength(1) - 1];
            }
        }

        //Displays the table
        public static void DisplayTable()
        {
            bool isY = false;

            //Checks if two phase
            if (twoPhase == false)
            {
                //Adds the top row
                Console.Write("Row\tZ\t");

                for (int i = 1; i < countX + 1; i++)
                {
                    //Checks it the X changes to a Y
                    isY = false;
                    foreach (var item in colY)
                    {
                        if (item == i)
                        {
                            isY = true;
                        }
                    }

                    //Displays Y if true
                    if (isY == true)
                    {
                        Console.Write("Y" + i + "\t");
                    }
                    else
                    {
                        Console.Write("X" + i + "\t");
                    }

                }

                for (int i = 1; i < countS + 1; i++)
                {
                    Console.Write("S" + i + "\t");
                }

                for (int i = 1; i < countE + 1; i++)
                {
                    Console.Write("E" + i + "\t");
                }

                for (int i = 1; i < countA + 1; i++)
                {
                    Console.Write("A" + i + "\t");
                }
                Console.Write("RHS");
                Console.WriteLine();

                //Displays the data
                for (int i = 0; i < finalLP.GetLength(0); i++)
                {
                    Console.Write(i + "\t");

                    for (int j = 0; j < finalLP.GetLength(1); j++)
                    {
                        Console.Write(Math.Round(finalLP[i, j], 2) + "\t");
                    }

                    Console.WriteLine();
                }
            }
            else
            {
                //Adds the top row
                Console.Write("Row\tW\tZ\t");

                for (int i = 1; i < countX + 1; i++)
                {
                    //Checks it the X changes to a Y
                    isY = false;
                    foreach (var item in colY)
                    {
                        if (item == i)
                        {
                            isY = true;
                        }

                    }

                    //Displays Y if true
                    if (isY == true)
                    {
                        Console.Write("Y" + i + "\t");
                    }
                    else
                    {
                        Console.Write("X" + i + "\t");
                    }
                }

                for (int i = 1; i < countS + 1; i++)
                {
                    Console.Write("S" + i + "\t");
                }

                for (int i = 1; i < countE + 1; i++)
                {
                    Console.Write("E" + i + "\t");
                }

                for (int i = 1; i < countA + 1; i++)
                {
                    Console.Write("A" + i + "\t");
                }
                Console.Write("RHS");
                Console.WriteLine();

                //Displays the data
                for (int i = 0; i < twoPhaseLP.GetLength(0); i++)
                {
                    Console.Write(i + " \t");

                    for (int j = 0; j < twoPhaseLP.GetLength(1); j++)
                    {
                        Console.Write(Math.Round(twoPhaseLP[i, j], 2) + "\t");
                    }

                    Console.WriteLine();
                }
            }
        }

        //Solves Max Simplex
        public static double[] MaxSimplex()
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
                    if (Math.Round(finalLP[0, i], 10) < winningColAmount)
                    {
                        winningCol = i;
                        winningColAmount = Math.Round(finalLP[0, i], 10);
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

        //Solves Min Simplex
        public static double[] MinSimplex()
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

                //Loops through the rows t choose the winning column
                for (int i = 1; i < finalLP.GetLength(1) - 1; i++)
                {
                    if (Math.Round(finalLP[0, i], 10) > winningColAmount)
                    {
                        winningCol = i;
                        winningColAmount = Math.Round(finalLP[0, i], 10);
                    }
                }

                //Makes sure the winning column isnt Z
                if (winningCol == 0)
                {
                    done = true;
                    answerFound = true;
                }
                else
                {
                    //Calculates the ratios
                    for (int i = 1; i < finalLP.GetLength(0); i++)
                    {
                        //Makes sure that cannot divide by zero
                        try
                        {
                            ratios.Add(Math.Round(finalLP[i, finalLP.GetLength(1) - 1] / finalLP[i, winningCol]));
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

                        //Checks if there are any positives in the top row, to see if it must continue
                        for (int i = 0; i < colAmount; i++)
                        {
                            if (Math.Round(finalLP[0, i], 10) > 0)
                            {
                                done = false;
                                answerFound = false;
                            }
                        }
                    }
                }
            } while (done == false);

            //Checks if answer is found
            if (answerFound == true)
            {
                //Finds BVs
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

                //Returns the solutions
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

        //Formats the table for two phase
        public static void FormatTwoPhase()
        {
            //Makes the twoPhase array larger
            twoPhaseLP = new double[finalLP.GetLength(0) + 1, finalLP.GetLength(1) + 1];

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
            for (int i = 0; i < finalLP.GetLength(0); i++)
            {
                for (int j = 0; j < finalLP.GetLength(1); j++)
                {
                    twoPhaseLP[i + 1, j + 1] = finalLP[i, j];
                }
            }
        }

        //Solves Two phase problems
        public static void TwoPhase()
        {
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
            DisplayTable();

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
                        DisplayTable();

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
                    DisplayTable();

                    //Calls the appropriate simplex method
                    if (LPType == "MAX")
                    {
                        answers = MaxSimplex();
                    }
                    else
                    {
                        answers = MinSimplex();
                    }

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

        public static void Graph()
        {
            //Instantiates the graph form
            //GraphForm graph = new GraphForm();

            //Sets size of array
            coordindates = new double[unformatedLP.Count() - 2, 2];

            //Loops through all the constraints
            for (int i = 1; i < unformatedLP.Count() - 1; i++)
            {
                //Splits the equation
                string[] tempEquation = unformatedLP[i].Split(' ');

                //Gets the coordinates of the constraints
                if (Convert.ToDouble(tempEquation[0]) == 0)
                {

                }
                else if (Convert.ToDouble(tempEquation[1]) == 0)
                {

                }
                else
                {
                    coordindates[i - 1, 0] = Convert.ToDouble(tempEquation[tempEquation.Count() - 1]) / Convert.ToDouble(tempEquation[0]);
                    coordindates[i - 1, 1] = Convert.ToDouble(tempEquation[tempEquation.Count() - 1]) / Convert.ToDouble(tempEquation[1]);
                }
            }

            //Opens the Form
            //graph.ShowDialog();
        }
    }
}
