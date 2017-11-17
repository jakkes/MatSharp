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
            
        }
    }
}