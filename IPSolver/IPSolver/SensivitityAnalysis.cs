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
            get
            {
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

        public static double[,] GetFormatedSensistivityMatrix(double[,] LinearProgramMatrix)
        {
            double[,] test = LinearProgramMatrix;
            double[,] formattedMatrix = new double[test.GetLength(0), test.GetLength(1)];

            for (int i = 0; i < formattedMatrix.GetLength(0); i++)
            {
                for (int j = 0; j < formattedMatrix.GetLength(1); j++)
                {
                    formattedMatrix[i, j] = test[i, j];
                }
            }

            return formattedMatrix;

        }

        public static void GetRangesForNBV(double[,] lpFormattedMatrix)
        {
            double[,] zrow = lpFormattedMatrix;
            for (int i = 0; i < 1; i++)
            {
                for (int j = 1; j < zrow.GetLength(1) - 1; j++)
                {
                    if (zrow[i, j] != 0)
                    {
                        Console.Write(string.Format("R<={0}\n", zrow[i, j]));
                    }

                }
                Console.Write(Environment.NewLine + Environment.NewLine);
            }
        }

        public static void GetRangesForBV(double[,] lpFormmattedMatrix)
        {
            double[,] matrix = lpFormmattedMatrix;

            for (int i = 1; i < matrix.GetLength(0); i++)
            {
                for (int j = 1; j < matrix.GetLength(1) - 1; j++)
                {

                    double zrow = matrix[0, j];
                    double range = matrix[i, j];

                    if (range < 0)
                    {
                        Console.Write(string.Format("R <= {0}  \n", zrow / (-1 * range)));

                    }
                    else Console.Write(string.Format("R >= {0}\n", zrow / (-1 * range)));

                }
                Console.Write(Environment.NewLine + Environment.NewLine);
            }
        }

        public static void GetRangesForRHS(double[,] lpformattedMatrix, LinearProgram lp)
        {
            double[,] matrix = lpformattedMatrix;
            for (int i = 1; i < matrix.GetLength(0); i++)
            {
                double rhs = matrix[i, matrix.GetLength(1) - 1];

                for (int j = lp.CountX + 1; j < matrix.GetLength(1) - 1; j++)
                {


                    double bInverse = matrix[i, j];

                    double a = (matrix[i, j] + matrix[i, j] + rhs);

                    Console.Write(string.Format("{0} ", bInverse));
                }
                Console.Write(string.Format("{0} ", rhs));
                Console.Write(Environment.NewLine + Environment.NewLine);
            }

        }

        public static void GetShadowPrices(double[,] lpFormattedMatrix, LinearProgram lp)
        {
            double[,] matrix = lpFormattedMatrix;
            for (int i = 1; i < matrix.GetLength(0); i++)
            {
                for (int j = lp.CountX + 1; j < matrix.GetLength(1) - 1; j++)
                {
                    double sprice = matrix[0, j];
                    Console.Write(string.Format("{0}\n ", sprice));

                }
                Console.Write(Environment.NewLine + Environment.NewLine);
            }
        }




    }
}
