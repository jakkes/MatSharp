using System;
using System.Collections.Generic;
using System.Collections;
using System.Dynamic;
using System.Linq;

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
        public double Determinant {
            get{
                return Matrix.Det(this);
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

        public static double Det(Matrix a) => Det(a, Enumerable.Range(0, a.Columns), Enumerable.Range(0, a.Rows));
        private static double Det(Matrix a, IEnumerable<int> cols, IEnumerable<int> rows){
            int colCount = cols.Count();
            int rowCount = rows.Count();
            if(colCount != rowCount)
                throw new ArgumentException("Matrix does not have same number of rows as columns.");
            if(colCount == 1)
                return a[cols.First(),rows.First()];
            else{
                
                double det = 0;

                foreach(var col in cols)
                    det += Det(a, cols.Where(x => x != col), rows.Skip(1)) * a[col,rows.First()];

                return det;
            }
        }
        public static Matrix Parse(string text) => Parse(text,' ',';');
        public static Matrix Parse(string text, char colSep, char rowSep){

            var rows = text.Split(rowSep);

            Matrix mat = new Matrix();
            mat._matrix = new double[rows.Length][];

            for(int i = 0; i < rows.Length; i++){
                var cols = rows[i].Split(colSep);
                
                if(i > 0 && cols.Length != mat._matrix[i-1].Length)
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
        public static Matrix operator*(Matrix a, Matrix b){
            Matrix mat = new Matrix(b.Rows,b.Columns);

            for(int i = 0; i < mat.Rows; i++){
                for(int j = 0; j < mat.Columns; j++){
                    mat[i,j] = 0;
                    for(int k = 0; k < a.Columns; k++){
                        mat[i,j] += a[i,k] * b[k,j];
                    }
                }
            }

            return mat;
        }
    }
}
