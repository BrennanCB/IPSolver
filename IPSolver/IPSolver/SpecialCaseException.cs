using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IPSolver
{
    class SpecialCaseException : Exception
    {
        public enum Type
        {
            Infeasable,
            Unbounded,
            Degeneracy
        }

        Type caseType;
        private string message;
        private Type CaseType { get => caseType; set => caseType = value; }
        public override string Message { get => message; }

        public SpecialCaseException(Type type)
        {
            caseType = type;

            switch (type)
            {
                case Type.Infeasable:
                    message = "The given problem is infeasable";
                    break;
                case Type.Unbounded:
                    message = "The given problem is unbounded";
                    break;
                case Type.Degeneracy:
                    message = "The given problem is a degenerate";
                    break;
            }
        }
    }
}
