using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace IPSolver
{
    public class FileHandler
    {
        public static string docLocation, input, output;

        public static bool DirectoryExists()
        {
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
        
        public static void CreateDirectory()
        {
            if (!Directory.Exists(docLocation))
            {
                Directory.CreateDirectory(docLocation);
            }
        }
        
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
        
        public static void CreateOutputFile(string outputLocal)
        {
            //Formats the output file path
            output = docLocation + "/" + outputLocal + ".txt";
        }
        
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
            
            FileStream file = new FileStream(input, FileMode.Open, FileAccess.Read);
            StreamReader read = new StreamReader(file);

            List<string> unformattedLP = new List<string>();

            string line = null;
            
            while (!string.IsNullOrWhiteSpace((line = read.ReadLine())))
            {
                unformattedLP.Add(line);
            }
            
            read.Close();
            file.Close();
            
            return unformattedLP;
        }
        
        public static void SaveSolution(double zValue, List<double> solutions)
        {
            FileStream file = new FileStream(output, FileMode.Create, FileAccess.Write);
            StreamWriter write = new StreamWriter(file);
            
            write.WriteLine("Z - Value: " + zValue);

            string line = null;

            //Formats string of solutions
            foreach (var item in solutions)
            {
                line += item + " ";
            }
            
            write.WriteLine(line);
            
            write.Close();
            file.Close();
        }

    }
}
