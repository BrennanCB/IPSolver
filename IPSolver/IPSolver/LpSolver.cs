using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IPSolver
{
   

    static class LpSolver
    {
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
        public static string LPType;

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

    }
}
