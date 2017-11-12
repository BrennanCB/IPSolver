using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IPSolver
{
    class CuttingPlane
    {
        LinearProgram linearProgram;
        public CuttingPlane(LinearProgram linearProgram)
        {
            this.linearProgram = linearProgram;
        }
        public LinearProgram Solve()
        {
            return linearProgram;
        }
    }
}
