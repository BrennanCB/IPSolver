using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IPSolver
{
    public class LinearProgram
    {
        private int countS, countE, countA, countX;

        //Removed to see if needed
        //private double[,] arrayA, arrayS, arrayE;

        private LPType type;

        private bool isTwoPhase;

        private List<int> listOfA;
        private List<int> colOfA;

        private List<int> colY;

        private List<String> canonicalForm;
        private double[,] linearProgramMatrix;

        //TODO Temporary Default constructor
        public LinearProgram()
        {

        }

        //TODO, if the above arrays arent needed, ctor needs to be simplified
        public LinearProgram(int countS, int countE, int countA, int countX, List<int> listOfA, List<int> colOfA, List<int> colY, LPType type, List<String> canonicalForm, double[,] linearProgramArray)
        {
            CountS = countS;
            CountE = countE;
            CountA = countA;
            CountX = countX;

            //this.arrayA = arrayA;
            //this.arrayS = arrayS;
            //this.arrayE = arrayE;

            Type = type;

            ListOfA = listOfA;
            ColOfA = colOfA;
            ColY = colY;

            CanonicalForm = canonicalForm;
            LinearProgramMatrix = linearProgramArray;
        }

        public List<String> CanonicalForm
        {
            get => canonicalForm;
            set => canonicalForm = value;
        }

        //TODO Check this
        //public bool IsTwoPhase => countA > 0;

        public int StartOfS => countX;
        public int StartOfA => StartOfE + countE;
        public int StartOfE => StartOfS + countS;

        public int RowCount => linearProgramMatrix.GetLength(0);
        public int ColumnCount => linearProgramMatrix.GetLength(1);

        public bool IsTwoPhase
        {
            get => isTwoPhase;
            set => isTwoPhase = value;
        }

        public LPType Type
        {
            get => type;
            set => type = value;
        }

        public double[,] LinearProgramMatrix
        {
            get => linearProgramMatrix;
            set => linearProgramMatrix = value;
        }

        public int CountA
        {
            get => countA;
            set => countA = value;
        }

        public int CountS
        {
            get => countS;
            set => countS = value;
        }

        public int CountE
        {
            get => countE;
            set => countE = value;
        }

        public int CountX
        {
            get => countX;
            set => countX = value;
        }

        public List<int> ListOfA
        {
            get => listOfA;
            set => listOfA = value;
        }

        public List<int> ColOfA
        {
            get => colOfA;
            set => colOfA = value;
        }

        public List<int> ColY
        {
            get => colY;
            set => colY = value;
        }

        public double[] GetBasicVariables()
        {
            int colAmount = ColumnCount;
            int rowAmount = RowCount;

            double[] basicVariableValues = new double[colAmount - 1];

            for (int j = 0; j < colAmount - 1; j++)
            {
                bool bv = true;
                int countOne = 0;
                double optimalSolution = 0;

                for (int i = 0; i < rowAmount; i++)
                {
                    double currentNumber = linearProgramMatrix[i, j];

                    if (currentNumber != 0 && currentNumber != 1)
                    {
                        bv = false;
                    }
                    else if (linearProgramMatrix[i, j] == 1)
                    {
                        countOne++;

                        if (countOne > 1)
                            bv = false;
                        else
                            optimalSolution = linearProgramMatrix[i, colAmount - 1];
                    }
                }

                if (bv == false)
                    basicVariableValues[j] = 0;
                else if (bv == true && countOne == 1)
                    basicVariableValues[j] = optimalSolution;
            }

            return basicVariableValues;
        }

        public void AddConstraint(String Constraint)
        {
            //TODO Add constraint to the LP
        }

        public void DisplayCanonicalForm()
        {
            Console.WriteLine("\nCanonical Form");
            Console.WriteLine("--------------");

            foreach (var item in CanonicalForm)
            {
                Console.WriteLine(item);
            }

            Console.WriteLine();
        }

        public void DisplaySolution()
        {
            List<double> finalSolution = new List<double>();

            double[] answers = GetBasicVariables();

            int countAnswers = 0;

            //Displays the z-value
            double zValue = Math.Round(answers[countAnswers], 2);
            //Displays the optimal solutions
            Console.WriteLine("Optimal Solutions");
            Console.WriteLine("-----------------");
            Console.WriteLine("Z = " + zValue);

            countAnswers++;

            //TODO Fix the displaying of the answers
            //Displays the X's
            for (int i = 0; i < CountX; i++)
            {
                bool isY = false;
                foreach (var item in ColY)
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
            for (int i = 0; i < CountS; i++)
            {
                Console.WriteLine("S" + (i + 1) + " = " + Math.Round(answers[countAnswers], 2));
                countAnswers++;
            }

            //Displaye the E's
            for (int i = 0; i < CountE; i++)
            {
                Console.WriteLine("E" + (i + 1) + " = " + Math.Round(answers[countAnswers], 2));
                countAnswers++;
            }

            //Displaye the A's
            for (int i = 0; i < CountA; i++)
            {
                Console.WriteLine("A" + (i + 1) + " = " + Math.Round(answers[countAnswers], 2));
                countAnswers++;
            }

            Console.WriteLine();

            //Calls the method that saves the soution
            FileHandler.SaveSolution(zValue, finalSolution);

            Console.WriteLine("The solution has been saved!");

            //Checks if it can draw a graph
            if (CountX == 2)
            {
                //Draws graph
                //Graph();
            }
            else
            {
                Console.WriteLine("This LP has more than two variables, cannot draw this graph");
            }

            Console.ReadKey();
        }
    }
}
