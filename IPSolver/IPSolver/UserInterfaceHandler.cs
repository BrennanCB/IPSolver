using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IPSolver
{
    static class UserInterfaceHandler
    {
        enum AlgorithimType
        {
            Primal = 0,
            TwoPhase,
            Dual,
            BranchAndBound,
            CuttingPlane
        }

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
        public static void Menu()
        {
            while (true)
            {
                Console.WriteLine(String.Format(@"
                                                
                                              IP SOLVER
________________________________________________________________________________________________________________

                                        PLEASE ENTER A FILE "));
                string filename = Console.ReadLine();
                Console.Clear();
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
                AlgorithimType menu = (AlgorithimType)userinput;

                switch (menu)
                {
                    case AlgorithimType.Primal:

                        //TODO Insert method return solved Primal Simplex


                        Console.Clear();
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
                                break;
                            case SensitivityMenu.display2:
                                break;
                            case SensitivityMenu.display3:
                                break;
                            case SensitivityMenu.display4:
                                break;
                            case SensitivityMenu.display5:
                                break;
                            case SensitivityMenu.display6:
                                break;
                            case SensitivityMenu.display7:
                                break;
                            case SensitivityMenu.display8:
                                break;
                            case SensitivityMenu.display9:
                                break;
                            case SensitivityMenu.display10:
                                break;
                            case SensitivityMenu.display11:
                                break;
                            case SensitivityMenu.display12:
                                break;
                            default:
                                break;
                        }

                        break;
                    case AlgorithimType.TwoPhase:


                        //TODO Insert Method to return solved Two Phase Simplex


                        Console.Clear();
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
                        int userInputSensitivityAnalysis1 = int.Parse(Console.ReadLine());

                        SensitivityMenu smenu1 = (SensitivityMenu)userInputSensitivityAnalysis1;
                        switch (smenu1)
                        {
                            case SensitivityMenu.display1:
                                break;
                            case SensitivityMenu.display2:
                                break;
                            case SensitivityMenu.display3:
                                break;
                            case SensitivityMenu.display4:
                                break;
                            case SensitivityMenu.display5:
                                break;
                            case SensitivityMenu.display6:
                                break;
                            case SensitivityMenu.display7:
                                break;
                            case SensitivityMenu.display8:
                                break;
                            case SensitivityMenu.display9:
                                break;
                            case SensitivityMenu.display10:
                                break;
                            case SensitivityMenu.display11:
                                break;
                            case SensitivityMenu.display12:
                                break;
                            default:
                                break;
                        }

                        break;
                    case AlgorithimType.Dual:


                        //TODO Insert Method to Return solved Dual Simplex

                        Console.Clear();
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
                        int userInputSensitivityAnalysis2 = int.Parse(Console.ReadLine());

                        SensitivityMenu smenu2 = (SensitivityMenu)userInputSensitivityAnalysis2;
                        switch (smenu2)
                        {
                            case SensitivityMenu.display1:
                                break;
                            case SensitivityMenu.display2:
                                break;
                            case SensitivityMenu.display3:
                                break;
                            case SensitivityMenu.display4:
                                break;
                            case SensitivityMenu.display5:
                                break;
                            case SensitivityMenu.display6:
                                break;
                            case SensitivityMenu.display7:
                                break;
                            case SensitivityMenu.display8:
                                break;
                            case SensitivityMenu.display9:
                                break;
                            case SensitivityMenu.display10:
                                break;
                            case SensitivityMenu.display11:
                                break;
                            case SensitivityMenu.display12:
                                break;
                            default:
                                break;
                        }

                        break;
                    case AlgorithimType.BranchAndBound:


                        //TODO Insert Method to return solved Branch & Bound Simplex

                        Console.Clear();
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
                        int userInputSensitivityAnalysis3 = int.Parse(Console.ReadLine());

                        SensitivityMenu smenu3 = (SensitivityMenu)userInputSensitivityAnalysis3;
                        switch (smenu3)
                        {
                            case SensitivityMenu.display1:
                                break;
                            case SensitivityMenu.display2:
                                break;
                            case SensitivityMenu.display3:
                                break;
                            case SensitivityMenu.display4:
                                break;
                            case SensitivityMenu.display5:
                                break;
                            case SensitivityMenu.display6:
                                break;
                            case SensitivityMenu.display7:
                                break;
                            case SensitivityMenu.display8:
                                break;
                            case SensitivityMenu.display9:
                                break;
                            case SensitivityMenu.display10:
                                break;
                            case SensitivityMenu.display11:
                                break;
                            case SensitivityMenu.display12:
                                break;
                            default:
                                break;
                        }

                        break;
                    case AlgorithimType.CuttingPlane:


                        //TODO Insert Method to Return solved Cutting Plane Simpelex

                        Console.Clear();
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
                        int userInputSensitivityAnalysis4 = int.Parse(Console.ReadLine());

                        SensitivityMenu smenu4 = (SensitivityMenu)userInputSensitivityAnalysis4;
                        switch (smenu4)
                        {
                            case SensitivityMenu.display1:
                                break;
                            case SensitivityMenu.display2:
                                break;
                            case SensitivityMenu.display3:
                                break;
                            case SensitivityMenu.display4:
                                break;
                            case SensitivityMenu.display5:
                                break;
                            case SensitivityMenu.display6:
                                break;
                            case SensitivityMenu.display7:
                                break;
                            case SensitivityMenu.display8:
                                break;
                            case SensitivityMenu.display9:
                                break;
                            case SensitivityMenu.display10:
                                break;
                            case SensitivityMenu.display11:
                                break;
                            case SensitivityMenu.display12:
                                break;
                            default:
                                break;
                        }

                        break;
                    default:
                        break;
                }

            }
        }

        //Main Menu
        //TODO Check if anything from here can be added into the new menu
        public static void OldMenu()
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


        //Displays the table
        //TODO Rework this to better use the Linear Programming Object instead of parameter
        public static void DisplayTable(LinearProgram lp)
        {
            bool isY = false;

            //Checks if two phase
            if (!lp.IsTwoPhase)
            {
                //Adds the top row
                Console.Write("Row\tZ\t");
            }
            else
            {
                //Adds the top row
                Console.Write("Row\tW\tZ\t");
            }

            for (int i = 1; i <= lp.CountX; i++)
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

            for (int i = 1; i <= lp.CountS; i++)
            {
                Console.Write("S" + i + "\t");
            }

            for (int i = 1; i <= lp.CountE; i++)
            {
                Console.Write("E" + i + "\t");
            }

            for (int i = 1; i <= lp.CountA; i++)
            {
                Console.Write("A" + i + "\t");
            }
            Console.Write("RHS");
            Console.WriteLine();

            //Displays the data
            for (int i = 0; i < lp.RowCount; i++)
            {
                Console.Write(i + "\t");

                for (int j = 0; j < lp.ColumnCount; j++)
                {
                    Console.Write(Math.Round(lp.LinearProgramArray[i, j], 2) + "\t");
                }

                Console.WriteLine();
            }
        }
    }
}