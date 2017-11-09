using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IPSolver
{


    class LpFormatter
    {
        //public static double[,] originalLP, arrayS, arrayE, arrayA, formattedLP, finalLP, twoPhaseLP;
        //public static double[] answers;
        //public static List<string> canonicalForm = new List<string>();

        //public static int countX = 0, countS = 0, countA = 0, countE = 0;
        ////public static List<string> sign = new List<string>();
        //public static bool twoPhase = false, answerFound = false;
        //public static double[,] coordindates;
        //public static List<int> colY = new List<int>();
        //public static string LPType;

        //private int countS = 0;
        //private int countE = 0;
        //private int countA = 0;
        //private int countX = 0;

        LinearProgram linearProgram;

        private double[,] arrayA;
        private double[,] arrayS;
        private double[,] arrayE;
        
        private List<String> unformattedLP;
        //private List<String> canonicalForm;
        //private double[,] formattedLp;
        
        //public double[,] GetFormattedLp()
        //{
        //    return formattedLp;
        //}


        //TODO Check this method, see why its here and if its needed
        public LinearProgram GetLinearProgram()
        {
            if (linearProgram == null)
            {
                //TODO Handle this

                //linearProgram = new LinearProgram();
            }

            return linearProgram;

            //return new LinearProgram(countS, countE, countA, countX, arrayA, arrayS,
            //    arrayE, listOfA, colOfA, canonicalForm, formattedLp);
        }

        public LpFormatter(List<String> unformattedLP, Algorithm algorithm)
        {
            this.unformattedLP = unformattedLP;

            //linearProgram = new LinearProgram(0, 0, 0, 0, new double[0, 0], new double[0, 0], new double[0, 0], new List<int>(), new List<int>(), new List<string>(), new double[0, 0]);
            linearProgram = new LinearProgram
            {
                CountA = 0,
                CountS = 0,
                CountE = 0,
                ListOfA = new List<int>(),
                ColOfA = new List<int>(),
                ColY = new List<int>(),
                CanonicalForm = new List<string>()
            };

            switch (algorithm)
            {
                case Algorithm.Primal: FormatSimplxLP();
                    break;
                case Algorithm.TwoPhase: FormatSimplxLP();
                    break;
                case Algorithm.Dual: FormatDualLP();
                    break;
                case Algorithm.BranchAndBound:
                    break;
                case Algorithm.CuttingPlane:
                    break;
                default:
                    break;
            }
        }

        private void FormatDualLP()
        {
            //Gets z equation
            string zEquation = unformattedLP[0];

            string[] tempZ = zEquation.Split(' ');

            //Gets the type of LP from the first line
            String LPTypeString = tempZ[0].ToUpper();

            if (LPTypeString == "MAX")
                linearProgram.Type = LPType.Max;
            else
                linearProgram.Type = LPType.Min;

            int countEquals = 0;

            foreach (var item in unformattedLP)
            {
                if (item.Contains(" = "))
                    countEquals++;
            }
            
            //Sets the sizes of the arrays that will hold all the variables
            double[,] formatedLp = new double[unformattedLP.Count() - 1 + countEquals, tempZ.Count() + 1];

            //arrayA = new double[0, unformattedLP.Count() - 1];
            arrayS = new double[unformattedLP.Count() - 1 + countEquals, unformattedLP.Count() - 1 + countEquals];
            arrayE = new double[unformattedLP.Count() - 1 + countEquals, unformattedLP.Count() - 1 + countEquals];

            //Sets Z to one
            formatedLp[0, 0] = 1;

            //Gets the number of X's
            linearProgram.CountX = tempZ.Count() - 1;

            try
            {
                //Splits the signs
                string[] tempSign = unformattedLP[unformattedLP.Count() - 1].Split(' ');

                List<String> sign = new List<string>();

                foreach (var item in tempSign)
                {
                    sign.Add(item);
                }

                //Does the canonical form for the z equation
                string tempCanonicalForm = "Z ";

                //Cycles through temp Z equation
                for (int i = 1; i < tempZ.Count(); i++)
                {
                    //Checks if restriction is negative
                    if (sign[i - 1] == "-")
                    {
                        //Saves X's to array
                        formatedLp[0, i] = Convert.ToDouble(tempZ[i]);
                    }
                    else
                    {
                        //Saves X's to array
                        formatedLp[0, i] = Convert.ToDouble(tempZ[i]) * -1;
                    }

                    //Saves X's to the canonical form
                    tempCanonicalForm += formatedLp[0, i] + "X" + i + " ";
                }

                //Adds the Equation to the canonical form list

                linearProgram.CanonicalForm.Add(tempCanonicalForm + "= 0");

                bool changeNeg = false;

                int offset = 0;

                //Fills the array( the X's)
                for (int i = 1; i < unformattedLP.Count() - 1; i++)
                {
                    string[] tempConstraint = unformattedLP[i + offset].Split(' ');

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
                                linearProgram.ColY.Add(j);

                                formatedLp[i + offset, j] = Convert.ToDouble(tempConstraint[j - 1]);

                                tempCanonicalForm += formatedLp[i + offset, j] + "Y" + j + " ";
                            }
                            else
                            {
                                formatedLp[i + offset, j] = Convert.ToDouble(tempConstraint[j - 1]) * -1;

                                tempCanonicalForm += formatedLp[i + offset, j] + "X" + j + " ";
                            }
                        }

                        //Fills in the RHS
                        formatedLp[i + offset, formatedLp.GetLength(1) - 1] = Convert.ToDouble(tempConstraint[tempConstraint.Count() - 1]) * -1;

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
                                linearProgram.ColY.Add(j);

                                formatedLp[i, j] = Convert.ToDouble(tempConstraint[j - 1]) * -1;

                                tempCanonicalForm += formatedLp[i, j] + "Y" + j + " ";
                            }
                            else
                            {
                                formatedLp[i, j] = Convert.ToDouble(tempConstraint[j - 1]);

                                tempCanonicalForm += formatedLp[i, j] + "X" + j + " ";
                            }
                        }
                        //Fills in the RHS
                        formatedLp[i + offset, formatedLp.GetLength(1) - 1] = Convert.ToDouble(tempConstraint[tempConstraint.Count() - 1]);
                    }


                    if (tempConstraint[tempConstraint.Count() - 2] == "<=")
                    {
                        arrayS[i + offset, linearProgram.CountS] = 1;
                        linearProgram.CountS++;

                        tempCanonicalForm += "S" + linearProgram.CountS + " ";
                    }
                    else if (tempConstraint[tempConstraint.Count() - 2] == ">=")
                    {
                        arrayE[i + offset, linearProgram.CountE] = 1;
                        linearProgram.CountE++;
                        
                        for (int j = 0; j < formatedLp.GetLength(1); j++)
                        {
                            formatedLp[i + offset, j] = formatedLp[i + offset, j] * -1;
                        }


                        //arrayA[i, linearProgram.CountA] = 1;
                        //linearProgram.CountA++;

                        tempCanonicalForm += "+E" + linearProgram.CountE + " ";
                    }
                    else //=
                    {
                        arrayS[i + offset, linearProgram.CountS] = 1;
                        linearProgram.CountS++;

                        tempCanonicalForm += "S" + linearProgram.CountS + " ";

                        offset++;

                        arrayE[i + offset, linearProgram.CountE] = 1;
                        linearProgram.CountE++;

                        for (int j = 0; j < formatedLp.GetLength(1); j++)
                        {
                            formatedLp[i + offset, j] = formatedLp[i + offset - 1, j] * -1;
                        }
                        
                        tempCanonicalForm += "+E" + linearProgram.CountE + " ";
                    }

                    //Changes the RHS
                    if (changeNeg == true)
                    {
                        //Adds it to the canonical form list
                        linearProgram.CanonicalForm.Add(tempCanonicalForm + "= " + (formatedLp[i + offset, formatedLp.GetLength(1) - 1]) * -1);
                    }
                    else
                    {
                        //Adds it to the canonical form list
                        linearProgram.CanonicalForm.Add(tempCanonicalForm + "= " + formatedLp[i + offset, formatedLp.GetLength(1) - 1]);
                    }
                }
                linearProgram.LinearProgramMatrix = CreateLPFinalForm(formatedLp);

                //return linearProgram;
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
            catch (ArgumentOutOfRangeException) //Catchs error if not enough signs
            {
                Console.WriteLine("Please ensire that you gave each variable a constraint");
                Console.WriteLine();
                Console.WriteLine("Press any key to continue...");

                Console.ReadKey();
                Environment.Exit(0);
            }


            //TODO handle this
            //return null;
        }



        //Formats the LP, by adding the S, E and A, and creating the canonical form
        private void FormatSimplxLP()
        {
            //Gets z equation
            string zEquation = unformattedLP[0];
            
            string[] tempZ = zEquation.Split(' ');

            //Gets the type of LP from the first line
            String LPTypeString = tempZ[0].ToUpper();

            if (LPTypeString == "MAX")
                linearProgram.Type = LPType.Max;
            else
                linearProgram.Type = LPType.Min;

            //Sets the sizes of the arrays that will hold all the variables
            double[,] formatedLp = new double[unformattedLP.Count() - 1, tempZ.Count() + 1];

            arrayA = new double[unformattedLP.Count() - 1, unformattedLP.Count() - 1];
            arrayS = new double[unformattedLP.Count() - 1, unformattedLP.Count() - 1];
            arrayE = new double[unformattedLP.Count() - 1, unformattedLP.Count() - 1];

            //Sets Z to one
            formatedLp[0, 0] = 1;

            //Gets the number of X's
            linearProgram.CountX = tempZ.Count() - 1;

            try
            {
                //Splits the signs
                string[] tempSign = unformattedLP[unformattedLP.Count() - 1].Split(' ');

                List<String> sign = new List<string>();

                foreach (var item in tempSign)
                {
                    sign.Add(item);
                }

                //Does the canonical form for the z equation
                string tempCanonicalForm = "Z ";

                //Cycles through temp Z equation
                for (int i = 1; i < tempZ.Count(); i++)
                {
                    //Checks if restriction is negative
                    if (sign[i - 1] == "-")
                    {
                        //Saves X's to array
                        formatedLp[0, i] = Convert.ToDouble(tempZ[i]);
                    }
                    else
                    {
                        //Saves X's to array
                        formatedLp[0, i] = Convert.ToDouble(tempZ[i]) * -1;
                    }

                    //Saves X's to the canonical form
                    tempCanonicalForm += formatedLp[0, i] + "X" + i + " ";
                }

                //Adds the Equation to the canonical form list
                
                linearProgram.CanonicalForm.Add(tempCanonicalForm + "= 0");

                bool changeNeg = false;

                //Fills the array( the X's)
                for (int i = 1; i < unformattedLP.Count() - 1; i++)
                {
                    string[] tempConstraint = unformattedLP[i].Split(' ');

                    tempCanonicalForm = null;

                    //Changes RHS to positive
                    //
                    if (Convert.ToDouble(tempConstraint[tempConstraint.Count() - 1]) < 0)
                    {
                        for (int j = 1; j < tempConstraint.Count() - 1; j++)
                        {
                            //Checks if sign restriction is negative
                            if (sign[j - 1] == "-")
                            {
                                //Changes the column to negative
                                linearProgram.ColY.Add(j);

                                formatedLp[i, j] = Convert.ToDouble(tempConstraint[j - 1]);

                                tempCanonicalForm += formatedLp[i, j] + "Y" + j + " ";
                            }
                            else
                            {
                                formatedLp[i, j] = Convert.ToDouble(tempConstraint[j - 1]) * -1;

                                tempCanonicalForm += formatedLp[i, j] + "X" + j + " ";
                            }
                        }

                        //Fills in the RHS
                        formatedLp[i, formatedLp.GetLength(1) - 1] = Convert.ToDouble(tempConstraint[tempConstraint.Count() - 1]) * -1;

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
                                linearProgram.ColY.Add(j);

                                formatedLp[i, j] = Convert.ToDouble(tempConstraint[j - 1]) * -1;

                                tempCanonicalForm += formatedLp[i, j] + "Y" + j + " ";
                            }
                            else
                            {
                                formatedLp[i, j] = Convert.ToDouble(tempConstraint[j - 1]);

                                tempCanonicalForm += formatedLp[i, j] + "X" + j + " ";
                            }
                        }
                        //Fills in the RHS
                        formatedLp[i, formatedLp.GetLength(1) - 1] = Convert.ToDouble(tempConstraint[tempConstraint.Count() - 1]);
                    }

                   
                    if (tempConstraint[tempConstraint.Count() - 2] == "<=")
                    {
                        arrayS[i, linearProgram.CountS] = 1;
                        linearProgram.CountS++;
                        
                        tempCanonicalForm += "S" + linearProgram.CountS + " ";
                    }
                    else if (tempConstraint[tempConstraint.Count() - 2] == ">=")
                    {
                        arrayE[i, linearProgram.CountE] = -1;
                        linearProgram.CountE++;
                        
                        arrayA[i, linearProgram.CountA] = 1;
                        linearProgram.CountA++;
                        
                        tempCanonicalForm += "-E" + linearProgram.CountE + " " + "A" + linearProgram.CountA + " ";
                    }
                    else
                    {
                        arrayA[i, linearProgram.CountA] = 1;
                        linearProgram.CountA++;
                        
                        tempCanonicalForm += "A" + linearProgram.CountA + " ";
                    }

                    //Changes the RHS
                    if (changeNeg == true)
                    {
                        //Adds it to the canonical form list
                        linearProgram.CanonicalForm.Add(tempCanonicalForm + "= " + (formatedLp[i, formatedLp.GetLength(1) - 1]) * -1);
                    }
                    else
                    {
                        //Adds it to the canonical form list
                        linearProgram.CanonicalForm.Add(tempCanonicalForm + "= " + formatedLp[i, formatedLp.GetLength(1) - 1]);
                    }
                }
                linearProgram.LinearProgramMatrix =  CreateLPFinalForm(formatedLp);

                //return linearProgram;
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
            catch (ArgumentOutOfRangeException) //Catchs error if not enough signs
            {
                Console.WriteLine("Please ensire that you gave each variable a constraint");
                Console.WriteLine();
                Console.WriteLine("Press any key to continue...");

                Console.ReadKey();
                Environment.Exit(0);
            }


            //TODO handle this
            //return null;
        }

        //Puts the LP, S, E and A arrays into 1 big array
        private double[,] CreateLPFinalForm(double[,] originalLP)
        {
            //Gets the number of columns of the final array
            int finalLPLength = originalLP.GetLength(1) + linearProgram.CountA + linearProgram.CountE + linearProgram.CountS;

            //Sets the rows and columns for the final array
            double[,] finalLP = new double[originalLP.GetLength(0), finalLPLength];

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
                for (int sCol = 0; sCol < linearProgram.CountS; sCol++)
                {
                    finalLP[i, mainCol] = arrayS[i, sCol];

                    mainCol++;
                }

                //Saves the E's
                for (int eCol = 0; eCol < linearProgram.CountE; eCol++)
                {
                    finalLP[i, mainCol] = arrayE[i, eCol];

                    mainCol++;
                }

                //Saves the A's

                for (int aCol = 0; aCol < linearProgram.CountA; aCol++)
                {
                    finalLP[i, mainCol] = arrayA[i, aCol];

                    if (finalLP[i, mainCol] == 1)
                    {
                        linearProgram.ListOfA.Add(i);
                        linearProgram.ColOfA.Add(mainCol);
                    }

                    mainCol++;
                }

                //Saves the RHS
                finalLP[i, mainCol] = originalLP[i, originalLP.GetLength(1) - 1];
            }
            return finalLP;
        }
    }
}
