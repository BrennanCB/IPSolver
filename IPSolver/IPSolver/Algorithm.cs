using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IPSolver
{
    public enum Algorithm
    {
        Primal = 1,
        TwoPhase,
        Dual,
        BranchAndBound,
        CuttingPlane
    }
}
