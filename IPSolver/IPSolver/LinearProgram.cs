using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IPSolver
{
    public class LinearProgram
    {
        private int countS = 0;
        private int countE = 0;
        private int countA = 0;
        private int countX = 0;

        private double[,] arrayA;
        private double[,] arrayS;
        private double[,] arrayE;

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

        public double[,] LinearProgramArray
        {
            get => linearProgramArray;
            set => linearProgramArray = value;
        }

        public List<String> CanonicalForm => canonicalForm;

        public bool IsTwoPhase() { return countA > 0; }

        public int StartOfS
        {
            get => countX;
        }

        public int StartOfA
        {
            get => GetStartOfE + countE;
        }
        public int GetStartOfE
        {
            get => StartOfS + countS;
        }

        public int GetCountA() { return countA; }

        public void SetCountA(int countA) { this.countA = countA; }

        public int GetCountS() { return countS; }

        public void SetCountS(int countS) { this.countA = countS; }

        public int GetCountE() { return countE; }

        public void SetCountE(int countE) { this.countE = countE; }

        public int CountX {
            get => countX;
            set => countX = value;
        }

        public List<int> GetListOfA() { return listOfA; }

        public void SetListOfA(List<int> listOfA) { this.listOfA = listOfA; }

        public List<int> GetColOfA() { return colOfA; }

        public void SetColOfA(List<int> colOfA) { this.colOfA = colOfA; }

        public int ColumnCount => linearProgramArray.GetLength(1);

        public int RowCount => linearProgramArray.GetLength(0);

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
