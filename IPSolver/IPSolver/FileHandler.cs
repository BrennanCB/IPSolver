using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IPSolver
{
    public class FileHandler
    {
        public static string docLocation, input, output;

        //Checks if the directory exists
        public static bool DirectoryExists()
        {
            //Sets the location of the folder
            docLocation = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/Linear Program Solver";

            if (Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/Linear Program Solver"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //Creates the directory
        public static void CreateDirectory()
        {
            if (!Directory.Exists(docLocation))
            {
                Directory.CreateDirectory(docLocation);
            }
        }

        //Checks if the input file is valid
        public static bool CheckInputFile(string inputLocal)
        {
            //Formats the input path
            input = docLocation + "/" + inputLocal + ".txt";

            if (File.Exists(input))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //Formats the output file path
        public static void CreateOutputFile(string outputLocal)
        {
            //Formats the output file path
            output = docLocation + "/" + outputLocal + ".txt";
        }

        //Reads the LP from the file
        public static List<string> ReadLP()
        {
            //Checks if the file still exists
            if (!File.Exists(input))
            {
                Console.WriteLine("The input file can no longer be located");
                Console.WriteLine();
                Console.WriteLine("Press any key to exit...");

                Console.ReadKey();

                Environment.Exit(0);
            }

            //Opens the file stream
            FileStream file = new FileStream(input, FileMode.Open, FileAccess.Read);
            StreamReader read = new StreamReader(file);

            List<string> unformattedLP = new List<string>();

            string line = null;

            //Reads the file
            while (!string.IsNullOrWhiteSpace((line = read.ReadLine())))
            {
                unformattedLP.Add(line);
            }

            //Closes the file stream
            read.Close();
            file.Close();

            //Returns the LP
            return unformattedLP;
        }

        //Saves the solution
        public static void SaveSolution(double zValue, List<double> solutions)
        {
            //Opens/Creates the file
            FileStream file = new FileStream(output, FileMode.Create, FileAccess.Write);
            StreamWriter write = new StreamWriter(file);

            //Writes Z
            write.WriteLine("Z - Value: " + zValue);

            string line = null;

            //Formats string of solutions
            foreach (var item in solutions)
            {
                line += item + " ";
            }

            //Writes the solution
            write.WriteLine(line);

            //Closes the file
            write.Close();
            file.Close();
        }

    }
}
