using System;

namespace MatSharp {
    public class MatrixMath{
        public static MatrixObs Round(MatrixObs matrix, int decimals){
            MatrixObs mat = new MatrixObs(matrix.Rows, matrix.Columns);

            for(int i = 0; i < matrix.Rows; i++)
                for(int j = 0; j < matrix.Columns; j++)
                    mat[i,j] = Math.Round(matrix[i,j],decimals);

            return mat;
        }
        public static MatrixObs Round(MatrixObs matrix)
            => Round(matrix,0);

        public static MatrixObs Pow(MatrixObs x, int y){
            MatrixObs ret = x;
            for(int i = 0; i < y; i++)
                ret *= x;
            return ret;
        }

        /// <summary>
        ///     Returns an approximation of e^A based on Taylor expansion of degree n
        public static MatrixObs Exp(MatrixObs A, int n){
            if(A.Rows != A.Columns)
                throw new ArgumentException("Matrix dimensions do not agree");

            MatrixObs curr = MatrixObs.Identity(A.Columns);
            MatrixObs ret = curr;

            int fac = 1;
            for(int i = 1; i < n; i++){
                fac *= i;
                curr *= A;
                ret += 1 / fac * curr;
            }

            return ret;
        }
    }
}