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
        }
        
        //TODO Display the Graph
        public static void Graph()
        {
            //Instantiates the graph form
            //GraphForm graph = new GraphForm();

            //Sets size of array
            //coordindates = new double[unformatedLP.Count() - 2, 2];

            //Loops through all the constraints
            //for (int i = 1; i < unformatedLP.Count() - 1; i++)
            //{
            //    //Splits the equation
            //    string[] tempEquation = unformatedLP[i].Split(' ');

            //    //Gets the coordinates of the constraints
            //    if (Convert.ToDouble(tempEquation[0]) == 0)
            //    {

            //    }
            //    else if (Convert.ToDouble(tempEquation[1]) == 0)
            //    {

            //    }
            //    else
            //    {
            //        coordindates[i - 1, 0] = Convert.ToDouble(tempEquation[tempEquation.Count() - 1]) / Convert.ToDouble(tempEquation[0]);
            //        coordindates[i - 1, 1] = Convert.ToDouble(tempEquation[tempEquation.Count() - 1]) / Convert.ToDouble(tempEquation[1]);
            //    }
            //}

            //Opens the Form
            //graph.ShowDialog();
        }
    }
}
