using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IPSolver
{
    class BranchAndBound
    {

        private class ProblemNode
        {
            LinearProgram problem;
            bool solved;
            double[] xValues;
            double zValue;

            public ProblemNode(LinearProgram problem, bool solved, double[] xValues, double zValue)
            {
                this.Problem = problem;
                this.Solved = solved;
                this.XValues = xValues;
                this.ZValue = zValue;
            }

            public LinearProgram Problem { get => problem; set => problem = value; }
            public bool Solved { get => solved; set => solved = value; }
            public double[] XValues { get => xValues; set => xValues = value; }
            public double ZValue { get => zValue; set => zValue = value; }
        }

        private List<ProblemNode> problems;

        public BranchAndBound(LinearProgram problem)
        {
            problems = new List<ProblemNode>();
            problems.Add(new ProblemNode(problem, false, null, 0));
        }

        public LinearProgram Sovle(LinearProgram problem, LPType type)
        {
            bool solved = false;
            ProblemNode currentOptimal = problems.ElementAt(0);

            //loop runs until all problems are solved, exit via break
            while (true)
            {
                ProblemNode currentProblem = problems.ElementAt(0);
                //this loop checks to see if all problems are solved
                for (int i = 0; i < problems.Count; i++)
                {
                    if (!problems.ElementAt(i).Solved)
                        break;
                    if (problems.Count == i - 1)
                        solved = true;
                }

                if (solved)
                    break;

                Dual dual = new Dual(currentProblem.Problem);
                currentProblem.Problem =  dual.Solve();

                double[] xValues = new double[currentProblem.Problem.CountX];

                currentProblem.XValues = xValues;

                currentProblem.ZValue = 
                    currentProblem.Problem.LinearProgramMatrix[0, currentProblem.Problem.ColumnCount];

                currentProblem.Solved = true;

                for (int i = 0; i < xValues.Length; i++)
                    xValues[i] = currentProblem.Problem.LinearProgramMatrix[0, i];

                for (int i = 0; i < xValues.Length; i++)
                {
                    if((xValues[i] % 1) != 0)
                    {

                    }
                        
                }
                //todo: check for infinte loop when initial problem cannot be solved

                if ((type == LPType.Max && currentProblem.ZValue > currentOptimal.ZValue)
                    || (type == LPType.Min && currentOptimal.ZValue > currentProblem.ZValue))
                { 
                    currentOptimal = currentProblem;
                }
            }
            return currentOptimal.Problem;
        }


    }
}
