using System;

namespace MatSharp {
    public static class MatrixMath{
        public static Matrix<double> Round(this Matrix<double> matrix, int decimals){
            Matrix<double> mat = new Matrix<double>(matrix.Rows, matrix.Columns);

            for(int i = 0; i < matrix.Rows; i++)
                for(int j = 0; j < matrix.Columns; j++)
                    mat[i,j] = Math.Round(matrix[i,j],decimals);

            return mat;
        }
        public static Matrix<double> Round(this Matrix<double> matrix)
            => Round(matrix,0);

        public static Matrix<double> Pow(this Matrix<double> x, int y){
            if(x.Columns != x.Rows)
                throw new ArgumentException("Invalid matrix dimensions");

            if(y == 0)
                return MatrixFactory.Identity(x.Columns);
            else if(y == 1)
                return x;
            else
                return x.Multiply(x.Pow(y - 1));
        }

        /// <summary>
        ///     Returns an approximation of e^A based on Taylor expansion of degree n
        public static Matrix<double> Exp(this Matrix<double> A, int n){
            if(A.Rows != A.Columns)
                throw new ArgumentException("Matrix dimensions do not agree");

            Matrix<double> curr = MatrixFactory.Identity(A.Columns);
            Matrix<double> ret = curr;

            int fac = 1;
            for(int i = 1; i < n; i++){
                fac *= i;
                curr = curr.Multiply(A);
                ret = ret.Add(curr.Multiply(1 / fac));
            }

            return ret;
        }
    }
}