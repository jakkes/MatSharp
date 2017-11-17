using System;

namespace MatSharp {
    public class MatrixMath{
        public static Matrix Round(Matrix matrix, int decimals){
            Matrix mat = new Matrix(matrix.Rows, matrix.Columns);

            for(int i = 0; i < matrix.Rows; i++)
                for(int j = 0; j < matrix.Columns; j++)
                    mat[i,j] = Math.Round(matrix[i,j],decimals);

            return mat;
        }
        public static Matrix Round(Matrix matrix)
            => Round(matrix,0);

        public static Matrix Pow(Matrix x, int y){
            Matrix ret = x;
            for(int i = 0; i < y; i++)
                ret *= x;
            return ret;
        }

        /// <summary>
        ///     Returns an approximation of e^A based on Taylor expansion of degree n
        public static Matrix Exp(Matrix A, int n){
            if(A.Rows != A.Columns)
                throw new ArgumentException("Matrix dimensions do not agree");

            Matrix curr = Matrix.Identity(A.Columns);
            Matrix ret = curr;

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