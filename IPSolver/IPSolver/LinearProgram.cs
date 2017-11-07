using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IPSolver
{
    class LinearProgram
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
    }
}
