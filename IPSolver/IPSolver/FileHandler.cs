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
        private static string  docLocation = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/Linear Program Solver";
        private static string inputLocation, outputLocation;

        public static bool DirectoryExists => Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/Linear Program Solver");

        public static void CreateDirectory()
        {
            if (!DirectoryExists)
                Directory.CreateDirectory(docLocation);
        }
        
        public static bool CheckInputFile(string inputPath)
        {
            //Formats the input path
            inputLocation = docLocation + "/" + inputPath + ".txt";

            return File.Exists(inputLocation);
        }
       

        public static string SetOutputFile
        {
           
            set { outputLocation = docLocation + "/" + value + ".txt"; }
        }


       

        public static List<string> ReadLP()
        {
            //Checks if the file still exists
            if (!File.Exists(inputLocation))
            {
                Console.WriteLine("The input file can no longer be located");
                Console.WriteLine();
                Console.WriteLine("Press any key to exit...");

                Console.ReadKey();

                Environment.Exit(0);
            }
            
            FileStream file = new FileStream(inputLocation, FileMode.Open, FileAccess.Read);
            StreamReader read = new StreamReader(file);

            List<string> unformattedLP = new List<string>();

            string line = null;
            
            while (!string.IsNullOrWhiteSpace((line = read.ReadLine())))
            {
                line = line.Replace('\t', ' ');

                unformattedLP.Add(line);
            }
            
            read.Close();
            file.Close();
            
            return unformattedLP;
        }
        
        //Saves the basic variables ofr optimal solution
        public static void SaveSolution(double zValue, double[] solutions)
        {
            FileStream file = new FileStream(outputLocation, FileMode.Create, FileAccess.Write);
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
