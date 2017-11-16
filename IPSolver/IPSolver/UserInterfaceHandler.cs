
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IPSolver
{
    static class UserInterfaceHandler
    {
        private static double[,] coordindates;
        public static LinearProgram linearProgram;

        //Enum for Sensitivity Analysis
        enum SensitivityMenu
        {
            display1 = 1,
            display2,
            display3,
            display4,
            display5,
            display6,
            display7,
            display8,
            display9,
            display10,
            display11,
            display12
        }

        //New Main Menu with file,Alg& sensitivity ananlysis selection
        public static void Menu()
        {
            GetInputAndOutputFiles();

            //TODO Move this to different place
            #region Stuff to Move
            List<String> unformatedLP = FileHandler.ReadLP();

            foreach (var item in unformatedLP)
            {
                Console.WriteLine(item);
            }

            
            #endregion

            bool done = false;

            do
            {
                try
                {
                    Console.WriteLine(@"
                IP SOLVER
________________________________________________________

                PLEASE SELECT AN ALGORITHM

                1.PRIMAL 
                2.TWO PHASE
                3.DUAL
                4.BRANCH & BOUND
                5.CUTTING PLANE
                ");

                    int userinput = int.Parse(Console.ReadLine());
                    Algorithm menu = (Algorithm)userinput;

                    switch (menu)
                    {
                        case Algorithm.Primal:

                            linearProgram = new LpFormatter(unformatedLP, Algorithm.Primal).GetLinearProgram();

                            linearProgram.DisplayCanonicalForm();

                            PrimalSimplex simplex = new PrimalSimplex(linearProgram);

                            linearProgram = simplex.Solve();
                            break;
                        case Algorithm.TwoPhase:

                            linearProgram = new LpFormatter(unformatedLP, Algorithm.TwoPhase).GetLinearProgram();
                            linearProgram.IsTwoPhase = true;

                            TwoPhase twoPhase = new TwoPhase(linearProgram);

                            linearProgram.DisplayCanonicalForm();

                            linearProgram = twoPhase.Solve();
                            break;
                        case Algorithm.Dual:

                            linearProgram = new LpFormatter(unformatedLP, Algorithm.Dual).GetLinearProgram();

                            linearProgram.DisplayCanonicalForm();

                            Dual dual = new Dual(linearProgram);

                            linearProgram = dual.Solve();
                            break;
                        case Algorithm.BranchAndBound:

                            linearProgram = new LpFormatter(unformatedLP, Algorithm.Dual).GetLinearProgram();

                            linearProgram.DisplayCanonicalForm();

                            Dual bbDual = new Dual(linearProgram);

                            linearProgram = bbDual.Solve();

                            BranchAndBound BB = new BranchAndBound(linearProgram);
                            linearProgram = BB.Solve();
                            break;
                        case Algorithm.CuttingPlane:
                            linearProgram = new LpFormatter(unformatedLP, Algorithm.Dual).GetLinearProgram();

                            linearProgram.DisplayCanonicalForm();

                            Dual cutDual = new Dual(linearProgram);

                            linearProgram = cutDual.Solve();

                            CuttingPlane cutingPlane = new CuttingPlane(linearProgram);
                            linearProgram = cutingPlane.Solve();
                            break;
                        default:
                            break;
                    }

                    //todo check for input errors, set done to false if there arent any
                    done = true;
                }
                catch (FormatException)
                {
                    done = false;
                    Console.WriteLine("Invalid Input");
                }
            } while (!done);

            if (LpTools.CheckIfIPIsSolved(linearProgram))
                linearProgram.DisplaySolution();
            else
            {
                Console.WriteLine("No Solution!");
                Console.ReadKey();
            }

            Console.Clear();

            if (LpTools.CheckIfIPIsSolved(linearProgram))
            {
                do
                {
                    SensitivityAnalysisMenu();
                } while (true);
            }
                
        }

        //Loop this?
        public static void SensitivityAnalysisMenu()
        {
            try
            {
                //Sensitivity Analysis Menu
                Console.WriteLine(@"
                                  IP SOLVER
________________________________________________________________________________________
                                                                                        
                           SENSITIVITY ANALYSIS

                       
             1. Display the range of a selected Non-Basic Variable.
             2. Apply and display a change of a selected Non-Basic Variable.
             3. Display the range of a selected Basic Variable.
             4. Apply and display a change of a selected Basic Variable.
             5. Display the range of a selected constraint right-hand-side value.
             6. Apply and display a change of a selected constraint right-hand-side value.
             7. Display the range of a selected variable in a Non-Basic Variable column.
             8. Apply and display a change of a selected variable in a Non-Basic Variable column.
             9. Add a new activity to an optimal solution.
             10. Add a new constraint to an optimal solution.
             11. Display the shadow prices.
             12. Duality
");

                Console.WriteLine("\nSolved LP\n");
                linearProgram.DisplayCurrentTable();
                Console.WriteLine();

                int userInputSensitivityAnalysis = int.Parse(Console.ReadLine());

                SensitivityMenu smenu = (SensitivityMenu)userInputSensitivityAnalysis;
                switch (smenu)
                {
                    case SensitivityMenu.display1:
                        //TODO Display the range of a selected Non-Basic Variable.
                        //SensivitityAnalysis.GetNonBasicVariables(linearProgram);

                        Console.WriteLine("Enter the column Number: (Z Column = Column 0)");

                        int rowNumber = int.Parse(Console.ReadLine());

                        Console.WriteLine("Ranges for Non Basic Variables");
                        SensivitityAnalysis.GetNBVRange(SensivitityAnalysis.GetFormatedSensistivityMatrix(linearProgram.LinearProgramMatrix), rowNumber);
                        Console.ReadKey();

                        break;
                    case SensitivityMenu.display2:
                        //TODO Display the range of a selected Non-Basic Variable.
                        Console.WriteLine("Enter the column Number: (Z Column = Column 0)");

                        int columnNumber = int.Parse(Console.ReadLine());

                        Console.WriteLine("Enter the row Number: (Z Column = Column 0)");

                        rowNumber = int.Parse(Console.ReadLine());

                        if (linearProgram.GetBasicVariables()[columnNumber] != 0)
                        {
                            Console.WriteLine("Not NBV");
                        }
                        else
                        {
                            Console.WriteLine("ENter NEw Value:");

                            int valuenew = int.Parse(Console.ReadLine());

                            linearProgram.LinearProgramMatrix[rowNumber, columnNumber] = valuenew;

                            linearProgram.DisplayCurrentTable();

                            if (LpTools.CheckIfIPIsSolved(linearProgram))
                            {
                                Console.WriteLine("No Change");
                                Console.ReadKey();
                            }
                            else
                            {
                                LinearProgram linearProgramNew = (LinearProgram)linearProgram.Clone();

                                Dual dual2 = new Dual(linearProgramNew);

                                dual2.Solve();

                                if (LpTools.CheckIfIPIsSolved(linearProgramNew))
                                {
                                    linearProgramNew.DisplaySolution();
                                }
                                else
                                {
                                    Console.WriteLine("No solution");
                                    Console.ReadKey();
                                }

                            }
                        }



                        break;
                    case SensitivityMenu.display3:
                        //TODO Display the range of a selected Basic Variable.
                        Console.WriteLine("Enter the column Number: (Z Column = Column 0)");

                        rowNumber = int.Parse(Console.ReadLine());

                        

                        Console.WriteLine("Ranges for Basic variables");
                        SensivitityAnalysis.GetRangesForSelectedBV(SensivitityAnalysis.GetFormatedSensistivityMatrix(linearProgram.LinearProgramMatrix), rowNumber);
                        Console.ReadKey();
                        break;
                    case SensitivityMenu.display4:
                        //TODO Apply and display a change of a selected Basic Variable.
                        break;
                    case SensitivityMenu.display5:
                        //TODO Display the range of a selected constraint right-hand-side value.
                        //Console.WriteLine("Enter the row Number: (Z Row = Row 0)");

                        //rowNumber = int.Parse(Console.ReadLine());

                        Console.WriteLine("Ranges for RHS variables");
                        SensivitityAnalysis.GetRangesForRHS(SensivitityAnalysis.GetFormatedSensistivityMatrix(linearProgram.LinearProgramMatrix), linearProgram);
                        Console.ReadKey();
                        break;
                    case SensitivityMenu.display6:
                        //TODO Apply and display a change of a selected constraint right-hand-side value.
                        break;
                    case SensitivityMenu.display7:
                        //TODO Display the range of a selected variable in a Non-Basic Variable column.
                        break;
                    case SensitivityMenu.display8:
                        //TODO Apply and display a change of a selected variable in a Non-Basic Variable column.
                        Console.WriteLine("Under Construction");
                        Console.ReadKey();
                        break;
                    case SensitivityMenu.display9:
                        //TODO Add a new activity to an optimal solution.
                        break;
                    case SensitivityMenu.display10:
                        //TODO Add a new constraint to an optimal solution.
                        Console.WriteLine("Enter the X Number: (Z Row = Row 0)");

                        rowNumber = int.Parse(Console.ReadLine());

                        Console.WriteLine("Type:\n1. <= \n2 . >=");

                        int sign = int.Parse(Console.ReadLine());

                        Console.WriteLine("RHS:");

                        int rhs = int.Parse(Console.ReadLine());

                        linearProgram = LpTools.AddBasicConstraint(linearProgram, rowNumber, sign - 1, rhs);

                        Console.WriteLine("New Table");
                        linearProgram.DisplayCurrentTable();

                        LinearProgram newLP = (LinearProgram) linearProgram.Clone();

                        Dual dual = new Dual(newLP);

                        dual.Solve();

                        if (LpTools.CheckIfIPIsSolved(newLP))
                        {
                            linearProgram.DisplaySolution();
                        }
                        else
                        {
                            Console.WriteLine("No solution");
                            Console.ReadKey();
                        }

                        break;
                    case SensitivityMenu.display11:
                        //TODO Display the shadow prices.
                        Console.WriteLine("shadow prices.");
                        SensivitityAnalysis.GetShadowPrices(SensivitityAnalysis.GetFormatedSensistivityMatrix(linearProgram.LinearProgramMatrix), linearProgram);
                        Console.ReadKey();
                        break;
                    case SensitivityMenu.display12:
                        //TODO Duality

                        Duality duality = new Duality(linearProgram);

                        duality.DeterminDuality();
                        Console.ReadKey();

                        break;
                    default:
                        break;
                }
            }
            catch (FormatException)
            {
                Console.WriteLine("Invalid Input");
            }
        }

        //TODO Check if anything from here can be added into the new menu
        public static void GetInputAndOutputFiles()
        {
            bool isValid = false;

            //Creates the directory if it doesnt exist
            if (!FileHandler.DirectoryExists)
            {
                //Creates the directory
                FileHandler.CreateDirectory();

                Console.WriteLine("A folder called 'Linear Program Solver' has been created in My Documents, please place your files here" +
                    "\n" +
                    "\nPress any key to continue");

                Console.ReadKey();
                Console.Clear();
            }

            do
            {
                //Displys the menu
                Console.WriteLine("Enter the location of your input and output files which need to be processed inside the 'Linear Program Solver' folder the is located in My Documents" +
                    "\nEnter the command in the following format:" +
                    "\n" +
                    "\nSolve <input file> <output file> - Note, the files must be in the format .txt, and the file extension must NOT be added" +
                    "\nE.g. Solve <input> <output>");

                string userCommand = Console.ReadLine();

                string[] splitCommand = userCommand.Split(' ');

                //Makes sure the user enters correct data
                if (splitCommand[0].ToLower() != "solve" || splitCommand.Length != 3)
                {
                    Console.WriteLine("The command you entered is incorrect!" +
                        "\nEnter the command in the following format:" +
                        "\nSolve <input file> <output file>");
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
                    if (!fileFound)
                    {
                        Console.WriteLine("The input file you listed cannot be found. Please ensure that:" +
                            "\n\tThe name is spelt correctly" +
                            "\n\tThe command is in the correct format" +
                            "\n\tThat the file does NOT include the file extension" +
                            "\n\tThat the file is a textfile(.txt)" +
                            "\n\tThat the file is located in the 'Linear Program Solver' folder in My Documents");
                    }
                    else
                    {
                        //Creates the output file
                        FileHandler.SetOutputFile = splitCommand[2];

                        Console.WriteLine("Output file created, this is where you will find the solution saved");

                        isValid = true;
                    }
                }

                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey();

                Console.Clear();
            } while (!isValid);
        }

        public static void Graph()
        {
            List<string> unformatedLP = FileHandler.ReadLP();

            //Sets size of array
            coordindates = new double[unformatedLP.Count(), 2];

            //Loops through all the constraints
            for (int i = 1; i < unformatedLP.Count() - 1; i++)
            {
                //Splits the equation
                string[] tempEquation = unformatedLP[i].Split(' ');

                //Gets the coordinates of the constraints
                if (Convert.ToDouble(tempEquation[0]) != 0 && Convert.ToDouble(tempEquation[1]) != 0)
                {
                    coordindates[i - 1, 0] = Convert.ToDouble(tempEquation[tempEquation.Count() - 1]) / Convert.ToDouble(tempEquation[0]);
                    coordindates[i - 1, 1] = Convert.ToDouble(tempEquation[tempEquation.Count() - 1]) / Convert.ToDouble(tempEquation[1]);
                }
            }

            //double[] bvs = linearProgram.GetBasicVariables();

            //coordindates[coordindates.GetLength(0) - 1, 0] = bvs[1];
            //coordindates[coordindates.GetLength(0) - 1, 1] = bvs[2];

            frmGraph graph = new frmGraph(coordindates);

            //Opens the Form
            graph.ShowDialog();
        }
    }
}