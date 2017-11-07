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

        private double[,] arrayA, arrayS, arrayE;

        private List<int> listOfA;
        private List<int> colOfA;

        private List<String> canonicalForm;
        private double[,] linearProgramArray;
        
        public LinearProgram(int countS, int countE, int countA, int countX, double[,] arrayA, double[,] arrayS,
            double[,] arrayE, List<int> listOfA, List<int> colOfA, List<String> canonicalForm, double[,] linearProgram)
        {
            this.countS = countS;
            this.countE = countE;
            this.countA = countA;
            this.countX = countX;

            this.arrayA = arrayA;
            this.arrayS = arrayS;
            this.arrayE = arrayE;

            this.listOfA = listOfA;
            this.colOfA = colOfA;

            this.canonicalForm = canonicalForm;
            this.linearProgramArray = linearProgram;
        }

        public List<String> CanonicalForm => canonicalForm;

        public bool IsTwoPhase => countA > 0;

        public int StartOfS => countX;
        public int StartOfA => StartOfE + countE;
        public int StartOfE => StartOfS + countS;

        public int RowCount => linearProgramArray.GetLength(0);
        public int ColumnCount => linearProgramArray.GetLength(1);

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
    }
}
