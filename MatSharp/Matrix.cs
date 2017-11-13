using System;

namespace MatSharp
{
    public class Matrix
    {
        public int Rows {
            get {
                return _matrix.Length;
            }
        }
        public int Columns {
            get {
                return _matrix.Length == 0 ? 0 : _matrix[0].Length;
            }
        }
        public double this [int i,int j]{
            get{
                return _matrix[i][j];
            }
            set {
                _matrix[i][j] = value;
            }
        }
        private double[][] _matrix;

        private Matrix(){ }
        public Matrix(int rows, int cols) {
            _matrix = new double[rows][];
            for (int i = 0; i < cols; i++)
                _matrix[i] = new double[cols];
        }

        
        public static Matrix Parse(string text) => Parse(text,' ',';');
        public static Matrix Parse(string text, char colSep, char rowSep){

            var rows = text.Split(rowSep);

            Matrix mat = new Matrix();
            mat._matrix = new double[rows.Length][];

            for(int i = 0; i < rows.Length; i++){
                var cols = rows[i].Split(colSep);
                
                if(i > 0 && cols.Length != mat._matrix[i].Length)
                    throw new FormatException("Inconsistent matrix dimensions supplied");

                mat._matrix[i] = new double[cols.Length];

                for(int j = 0; j < cols.Length; j++)
                    mat._matrix[i][j] = double.Parse(cols[j]);
            }

            return mat;

        }
        public static Matrix Zeroes(int size) => Zeroes(size, size);
        public static Matrix Zeroes(int rows, int cols){
            Matrix mat = new Matrix(rows,cols);
            for(int i = 0; i < rows; i++)
                for(int j = 0; j < cols; j++)
                    mat[i,j] = 0;

            return mat;
        }
        public static Matrix Ones(int size) => Ones(size,size);
        public static Matrix Ones(int rows, int cols){
            Matrix mat = new Matrix(rows,cols);
            for(int i = 0; i < rows; i++)
                for(int j = 0; j < cols; j++)
                    mat[i,j] = 1;

            return mat;
        }
        public static Matrix operator+(Matrix a, Matrix b){
            if(a.Rows != b.Rows || a.Columns != b.Columns)
                throw new ArgumentException("Invalid matrix dimensions.");

            Matrix mat = new Matrix(a.Rows,a.Columns);
            
            for(int i = 0; i < mat.Rows; i++)
                for(int j = 0; j < mat.Columns; j++)
                    mat[i,j] = a[i,j] + b[i,j];

            return mat;
        }
        public static Matrix operator*(double a, Matrix b){
            Matrix mat = new Matrix(b.Rows,b.Columns);

            for(int i = 0; i < mat.Rows; i++)
                for(int j = 0; j < mat.Columns; j++)
                    mat[i,j] = a * b[i,j];

            return mat;
        }
        public static Matrix operator*(int a, Matrix b){
            Matrix mat = new Matrix(b.Rows,b.Columns);

            for(int i = 0; i < mat.Rows; i++)
                for(int j = 0; j < mat.Columns; j++)
                    mat[i,j] = a * b[i,j];

            return mat;
        }
        public static Matrix operator*(Matrix a, double b) => b * a;
        public static Matrix operator*(Matrix a, int b) => b * a;
        public static Matrix operator-(Matrix a, Matrix b){
            if(a.Rows != b.Rows || a.Columns != b.Columns)
                throw new ArgumentException("Invalid matrix dimensions.");

            Matrix mat = new Matrix(a.Rows,a.Columns);
            
            for(int i = 0; i < mat.Rows; i++)
                for(int j = 0; j < mat.Columns; j++)
                    mat[i,j] = a[i,j] - b[i,j];

            return mat;
        }
        
    }
}
