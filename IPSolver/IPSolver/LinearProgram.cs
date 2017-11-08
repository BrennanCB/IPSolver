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

        private List<int> listOfA;
        private List<int> colOfA;

        private List<int> colY;

        private List<String> canonicalForm;
        private double[,] linearProgramArray;

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
            LinearProgramArray = linearProgramArray;
        }

        public List<String> CanonicalForm
        {
            get => canonicalForm;
            set => canonicalForm = value;
        }

        public bool IsTwoPhase => countA > 0;

        public int StartOfS => countX;
        public int StartOfA => StartOfE + countE;
        public int StartOfE => StartOfS + countS;

        public int RowCount => linearProgramArray.GetLength(0);
        public int ColumnCount => linearProgramArray.GetLength(1);

        public LPType Type
        {
            get => type;
            set => type = value;
        }

        public double[,] LinearProgramArray
        {
            get => linearProgramArray;
            set => linearProgramArray = value;
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

        public int CountX {
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
                    double currentNumber = linearProgramArray[i, j];

                    if (currentNumber != 0 && currentNumber != 1)
                    {
                        bv = false;
                    }
                    else if (linearProgramArray[i, j] == 1)
                    {
                        countOne++;

                        if (countOne > 1)
                            bv = false;
                        else
                            optimalSolution = linearProgramArray[i, colAmount - 1];
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

    }
}
