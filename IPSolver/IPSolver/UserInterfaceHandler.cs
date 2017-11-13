
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IPSolver
{
    static class UserInterfaceHandler
    {
        //Enum for Sensitivity Analysis
        enum SensitivityMenu
        {
            display1 = 0,
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

            LinearProgram linearProgram;
            #endregion

            while (true)
            {
                //Displays Menu For entering File Name
                //                Console.WriteLine(String.Format(@"

                //                                              IP SOLVER
                //________________________________________________________________________________________________________________

                //                                        PLEASE ENTER A FILE "));
                //                string filename = Console.ReadLine();
                //                Console.Clear();

                //Menu For Selecting Algorithm
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

                        LpFormatter lpFormatter = new LpFormatter(unformatedLP, Algorithm.Primal);

                        linearProgram = lpFormatter.GetLinearProgram();

                        linearProgram.DisplayCanonicalForm();

                        Console.WriteLine("Initial Table");

                        linearProgram.DisplayCurrentTable();

                        Simplex simplex = new Simplex(linearProgram);

                        linearProgram = simplex.Solve();
                        linearProgram.DisplaySolution();

                        Console.Clear();

                        SensitivityAnalysisMenu();

                        break;
                    case Algorithm.TwoPhase:
                        
                        lpFormatter = new LpFormatter(unformatedLP, Algorithm.TwoPhase);

                        linearProgram = lpFormatter.GetLinearProgram();
                        linearProgram.IsTwoPhase = true;

                        linearProgram.DisplayCanonicalForm();

                        Console.WriteLine("Initial Table");

                        TwoPhase twoPhase = new TwoPhase(linearProgram);

                        linearProgram.DisplayCurrentTable();

                        //Runs Two Phase
                        linearProgram = twoPhase.Solve();
                        linearProgram.DisplaySolution();

                        Console.Clear();

                        SensitivityAnalysisMenu();

                        break;
                    case Algorithm.Dual:

                        lpFormatter = new LpFormatter(unformatedLP, Algorithm.Dual);

                        linearProgram = lpFormatter.GetLinearProgram();

                        linearProgram.DisplayCanonicalForm();

                        Console.WriteLine("Initial Table");
                        
                        Dual dual = new Dual(linearProgram);

                        linearProgram.DisplayCurrentTable();
                        
                        linearProgram = dual.Solve();

                        linearProgram.DisplaySolution();

                        Console.Clear();

                        SensitivityAnalysisMenu();

                        break;
                    case Algorithm.BranchAndBound:
                        lpFormatter = new LpFormatter(unformatedLP, Algorithm.Dual);

                        linearProgram = lpFormatter.GetLinearProgram();

                        linearProgram.DisplayCanonicalForm();

                        Console.WriteLine("Initial Table");

                        Dual bbDual = new Dual(linearProgram);

                        linearProgram.DisplayCurrentTable();

                        linearProgram = bbDual.Solve();

                        BranchAndBound BB = new BranchAndBound(linearProgram);
                        linearProgram = BB.Sovle();

                        linearProgram.DisplaySolution();

                        //TODO Insert Method to return solved Branch & Bound Simplex

                        Console.Clear();

                        SensitivityAnalysisMenu();

                        break;
                    case Algorithm.CuttingPlane:


                        //TODO Insert Method to Return solved Cutting Plane Simpelex

                        Console.Clear();

                        SensitivityAnalysisMenu();

                        break;
                    default:
                        break;
                }
            }
        }

        //Loop this?
        public static void SensitivityAnalysisMenu()
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
            int userInputSensitivityAnalysis = int.Parse(Console.ReadLine());

            SensitivityMenu smenu = (SensitivityMenu)userInputSensitivityAnalysis;
            switch (smenu)
            {
                case SensitivityMenu.display1:
                    //TODO Display the range of a selected Non-Basic Variable.

                    break;
                case SensitivityMenu.display2:
                    //TODO Display the range of a selected Non-Basic Variable.
                    break;
                case SensitivityMenu.display3:
                    //TODO Display the range of a selected Basic Variable.
                    break;
                case SensitivityMenu.display4:
                    //TODO Apply and display a change of a selected Basic Variable.
                    break;
                case SensitivityMenu.display5:
                    //TODO Display the range of a selected constraint right-hand-side value.
                    break;
                case SensitivityMenu.display6:
                    //TODO Apply and display a change of a selected constraint right-hand-side value.
                    break;
                case SensitivityMenu.display7:
                    //TODO Display the range of a selected variable in a Non-Basic Variable column.
                    break;
                case SensitivityMenu.display8:
                    //TODO Apply and display a change of a selected variable in a Non-Basic Variable column.
                    break;
                case SensitivityMenu.display9:
                    //TODO Add a new activity to an optimal solution.
                    break;
                case SensitivityMenu.display10:
                    //TODO Add a new constraint to an optimal solution.
                    break;
                case SensitivityMenu.display11:
                    //TODO Display the shadow prices.
                    break;
                case SensitivityMenu.display12:
                    //TODO Duality
                    break;
                default:
                    break;
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
                    if (!fileFound)
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
                        FileHandler.SetOutputFile = splitCommand[2];

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
    }
}