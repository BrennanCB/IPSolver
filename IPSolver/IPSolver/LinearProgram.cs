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
        private double[,] linearProgram;

        
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
            this.linearProgram = linearProgram;
        }

        public double[,] GetLinearProgram()
        {
            return linearProgram;
        }
        public void SetLinearProgram(double[,] linearProgram)
        {
            this.linearProgram = linearProgram;
        }
        public List<String> GetCanonicalForm() { return canonicalForm; }

        public bool IsTwoPhase() { return countA > 0; }

        public int GetStartOfS() { return countX; }

        public int GetStartOfA() { return GetStartOfE() + countE; }

        public int GetStartOfE() { return GetStartOfS() + countS; }

        public int GetCountA() { return countA; }

        public void SetCountA(int countA) { this.countA = countA; }

        public int GetCountS() { return countS; }

        public void SetCountS(int countS) { this.countA = countS; }

        public int GetCountE() { return countE; }

        public void SetCountE(int countE) { this.countE = countE; }

        public int GetCountX() { return countX; }

        public void SetCountX(int countX) { this.countX = countX; }

        public List<int> GetListOfA() { return listOfA; }

        public void SetListOfA(List<int> listOfA) { this.listOfA = listOfA; }

        public List<int> GetColOfA() { return colOfA; }

        public void SetColOfA(List<int> colOfA) { this.colOfA = colOfA; }

        public int getColumnCount()
        {
            return linearProgram.GetLength(1);
        }

        public int getRowCount()
        {
            return linearProgram.GetLength(0);
        }

        public double[] getBasicVariables()
        {
            int colAmount = getColumnCount();
            int rowAmount = getRowCount();

            double[] basicVariableValues = new double[colAmount - 1];

            for (int j = 0; j < colAmount - 1; j++)
            {
                bool bv = true;
                int countOne = 0;
                double optimalSolution = 0;

                for (int i = 0; i < rowAmount; i++)
                {
                    double currentNumber = linearProgram[i, j];

                    if (currentNumber != 0 && currentNumber != 1)
                    {
                        bv = false;
                    }
                    else if (linearProgram[i, j] == 1)
                    {
                        countOne++;

                        if (countOne > 1)
                        {
                            bv = false;
                        }
                        else
                        {
                            optimalSolution = linearProgram[i, colAmount - 1];
                        }
                    }
                }

                if (bv == false)
                {
                    basicVariableValues[j] = 0;
                }
                else if (bv == true && countOne == 1)
                {
                    basicVariableValues[j] = optimalSolution;
                }
            }

            return basicVariableValues;
        }
    }
}
