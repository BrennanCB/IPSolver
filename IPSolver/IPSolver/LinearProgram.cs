using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IPSolver
{
    public class LinearProgram
    {
        #region Fields
        private int countS, countE, countA, countX;
        private LPType type;
        private bool isTwoPhase;

        private List<int> listOfA, colOfA, colY;

        private List<String> canonicalForm;

        private double[,] linearProgramMatrix;
        #endregion

        //TODO Temporary Default constructor
        public LinearProgram()
        {

        }
        
        public LinearProgram(int countS, int countE, int countA, int countX, List<int> listOfA, List<int> colOfA, List<int> colY, LPType type, List<String> canonicalForm, double[,] linearProgramArray)
        {
            CountS = countS;
            CountE = countE;
            CountA = countA;
            CountX = countX;
            
            Type = type;

            ListOfA = listOfA;
            ColOfA = colOfA;
            ColY = colY;

            CanonicalForm = canonicalForm;
            LinearProgramMatrix = linearProgramArray;
        }

        #region Properties
        //TODO Check this
        //public bool IsTwoPhase => countA > 0;

        public int StartOfS => CountX;
        public int StartOfA => StartOfE + CountE;
        public int StartOfE => StartOfS + CountS;

        public int RowCount => LinearProgramMatrix.GetLength(0);
        public int ColumnCount => LinearProgramMatrix.GetLength(1);

        public int CountS
        {
            get
            {
                return countS;
            }

            set
            {
                countS = value;
            }
        }

        public int CountE
        {
            get
            {
                return countE;
            }

            set
            {
                countE = value;
            }
        }

        public int CountA
        {
            get
            {
                return countA;
            }

            set
            {
                countA = value;
            }
        }

        public int CountX
        {
            get
            {
                return countX;
            }

            set
            {
                countX = value;
            }
        }

        public LPType Type
        {
            get
            {
                return type;
            }

            set
            {
                type = value;
            }
        }

        public bool IsTwoPhase
        {
            get
            {
                return isTwoPhase;
            }

            set
            {
                isTwoPhase = value;
            }
        }

        public List<int> ListOfA
        {
            get
            {
                return listOfA;
            }

            set
            {
                listOfA = value;
            }
        }

        public List<int> ColOfA
        {
            get
            {
                return colOfA;
            }

            set
            {
                colOfA = value;
            }
        }

        public List<int> ColY
        {
            get
            {
                return colY;
            }

            set
            {
                colY = value;
            }
        }

        public List<string> CanonicalForm
        {
            get
            {
                return canonicalForm;
            }

            set
            {
                canonicalForm = value;
            }
        }

        public double[,] LinearProgramMatrix
        {
            get
            {
                return linearProgramMatrix;
            }

            set
            {
                linearProgramMatrix = value;
            }
        }

        //public bool IsTwoPhase
        //{
        //    get => isTwoPhase;
        //    set => isTwoPhase = value;
        //}

        //public LPType Type
        //{
        //    get => type;
        //    set => type = value;
        //}

        //public List<String> CanonicalForm
        //{
        //    get => canonicalForm;
        //    set => canonicalForm = value;
        //}

        //public double[,] LinearProgramMatrix
        //{
        //    get => linearProgramMatrix;
        //    set => linearProgramMatrix = value;
        //}

        //public int CountA
        //{
        //    get => countA;
        //    set => countA = value;
        //}

        //public int CountS
        //{
        //    get => countS;
        //    set => countS = value;
        //}

        //public int CountE
        //{
        //    get => countE;
        //    set => countE = value;
        //}

        //public int CountX
        //{
        //    get => countX;
        //    set => countX = value;
        //}

        //public List<int> ListOfA
        //{
        //    get => listOfA;
        //    set => listOfA = value;
        //}

        //public List<int> ColOfA
        //{
        //    get => colOfA;
        //    set => colOfA = value;
        //}

        //public List<int> ColY
        //{
        //    get => colY;
        //    set => colY = value;
        //}
        #endregion

        public double[] GetBasicVariables()
        {
            double[] basicVariableValues = new double[ColumnCount - 1];

            for (int j = 0; j < ColumnCount - 1; j++)
            {
                bool bv = true;
                int countOfOnes = 0;
                double optimalSolution = 0;

                for (int i = 0; i < RowCount; i++)
                {
                    double currentNumber = LinearProgramMatrix[i, j];

                    if (currentNumber != 0 && currentNumber != 1)
                    {
                        bv = false;
                    }
                    else if (currentNumber == 1)
                    {
                        countOfOnes++;

                        if (countOfOnes > 1)
                            bv = false;
                        else
                            optimalSolution = LinearProgramMatrix[i, ColumnCount - 1];
                    }
                }

                if (bv == false)
                    basicVariableValues[j] = 0;
                else if (bv == true && countOfOnes == 1)
                    basicVariableValues[j] = Math.Round(optimalSolution, 2);
            }

            return basicVariableValues;
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

        public void DisplayCurrentTable()
        {
            bool isY = false;

            //Checks if two phase

            Console.Write("Row\t");

            if (IsTwoPhase)
                Console.Write("W\t");

            Console.Write("Z\t");

            for (int i = 1; i <= CountX; i++)
            {
                //Checks it the X changes to a Y
                isY = false;
                foreach (var item in ColY)
                {
                    if (item == i)
                        isY = true;
                }

                //Displays Y if true
                if (isY == true)
                    Console.Write("Y" + i + "\t");
                else
                    Console.Write("X" + i + "\t");
            }

            for (int i = 1; i <= CountS; i++)
            {
                Console.Write("S" + i + "\t");
            }

            for (int i = 1; i <= CountE; i++)
            {
                Console.Write("E" + i + "\t");
            }

            for (int i = 1; i <= CountA; i++)
            {
                Console.Write("A" + i + "\t");
            }

            Console.Write("RHS");
            Console.WriteLine();

            //Displays the data
            for (int i = 0; i < RowCount; i++)
            {
                Console.Write(i + "\t");

                for (int j = 0; j < ColumnCount; j++)
                {
                    Console.Write(Math.Round(LinearProgramMatrix[i, j], 2) + "\t");
                }

                Console.WriteLine();
            }
        }

        public void DisplaySolution()
        {
            double[] answers = GetBasicVariables();

            double zValue = answers[0];

            Console.WriteLine("Optimal Solutions\n-----------------\nZ = + " + zValue);

            int countAnswers = 1;
            
            //Displays the X's
            for (int i = 0; i < CountX; i++)
            {
                bool isY = false;

                foreach (var item in ColY)
                {
                    if (item + 1 == i)
                        isY = true;
                }

                if (isY == true)
                    Console.WriteLine("X" + (i + 1) + " = " + answers[countAnswers] * -1);
                else
                    Console.WriteLine("X" + (i + 1) + " = " + answers[countAnswers]);

                countAnswers++;
            }

            //Displaye the S's
            for (int i = 0; i < CountS; i++)
            {
                Console.WriteLine("S" + (i + 1) + " = " + answers[countAnswers]);
                countAnswers++;
            }

            //Displaye the E's
            for (int i = 0; i < CountE; i++)
            {
                Console.WriteLine("E" + (i + 1) + " = " + answers[countAnswers]);
                countAnswers++;
            }

            //Displaye the A's
            for (int i = 0; i < CountA; i++)
            {
                Console.WriteLine("A" + (i + 1) + " = " + answers[countAnswers]);
                countAnswers++;
            }

            Console.WriteLine();

            //Calls the method that saves the soution
            FileHandler.SaveSolution(zValue, answers);

            Console.WriteLine("The solution has been saved!");

            //Checks if it can draw a graph
            if (CountX == 2)
            {
                //Draws graph
                //Graph();
            }
            else
                Console.WriteLine("This LP has more than two variables, cannot draw this graph");

            Console.ReadKey();
        }
    }
}
