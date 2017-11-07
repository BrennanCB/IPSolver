using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IPSolver
{
    static class UserInterfaceHandler
    {

        //     *COMMENT*
        //kkkkkkk
        //_____________________
        //    ...........

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


    }
}
