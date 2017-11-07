using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IPSolver
{
    class Program
    {
        
        static void Main(string[] args)
        {
            UserInterfaceHandler.Menu();
            
            List<String> unformatedLP = FileHandler.ReadLP();
            LpFormatter LpFormatter = new LpFormatter(unformatedLP);
            
            foreach (var item in unformatedLP)
            {
                Console.WriteLine(item);
            }
            
            double[,] formattedLp = LpFormatter.GetFormattedLp();

            //Displays the canonical form
            Console.WriteLine();
            Console.WriteLine("Canonical Form");
            Console.WriteLine("--------------");

            foreach (var item in LpFormatter.GetCanonicalForm())
            {
                Console.WriteLine(item);
            }

            Console.WriteLine();

            //Sets where it is two pahse or not
            if (LpFormatter.IsTwoPhase())
            {
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
                    answers = Solve("MAX");
                }
                else
                {
                    DisplayTable();

                    //Runs Min Method
                    answers = Solve("MIN");
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
