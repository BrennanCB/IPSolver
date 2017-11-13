using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IPSolver
{
    class SensivitityAnalysis
    {

        private LinearProgram optimalSoltution;
        private LinearProgram originalLP;

        public LinearProgram OptimalSoltution
        {
            get { return optimalSoltution; }
            set { optimalSoltution = value; }
        }
        
        public LinearProgram OriginalLP
        {
            get {
                if (originalLP == null)
                    originalLP = new LpFormatter(FileHandler.ReadLP(), Algorithm.Dual).GetLinearProgram();

                return originalLP;
            }
            set { originalLP = value; }
        }

        public SensivitityAnalysis(LinearProgram optimalSoltution)
        {
            OptimalSoltution = optimalSoltution;
            originalLP = new LpFormatter(FileHandler.ReadLP(), Algorithm.Dual).GetLinearProgram();
        }


    }
}
