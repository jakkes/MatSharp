using System;
using System.Collections.Generic;
using System.Collections;
using System.Dynamic;
using System.Linq;
using System.Globalization;

namespace MatSharp
{
    public class Matrix
    {
        /// <summary>
        ///     Number of rows
        /// </summary>
        public int Rows
        {
            get
            {
                return _matrix.Length;
            }
        }
        /// <summary>
        ///     Number of columns
        /// </summary>
        public int Columns
        {
            get
            {
                return _matrix.Length == 0 ? 0 : _matrix[0].Length;
            }
        }
        /// <summary>
        ///     Returns the determinant
        /// </summary>
        public double Determinant
        {
            get
            {
                return Matrix.Det(this);
            }
        }
        public double this[int i, int j]
        {
            get
            {
                return _matrix[i][j];
            }
            set
            {
                _matrix[i][j] = value;
            }
        }
        /// <summary>
        ///     Returns the reduced row echolon form
        /// </summary>
        public Matrix Rref
        {
            get
            {
                var mat = Clone();

                int row = 0;

                for(int i = 0; i < Columns; i++){
                    
                    bool oneFixed = false;

                    for(int j = row; j < Rows; j++){
                        if(mat[j,i] != 0){
                            mat.InterchangeRows(row,j);
                            oneFixed = true;
                            break;
                        }
                    }

                    if(!oneFixed) continue;

                    mat.MultiplyRow(row,1 / mat[row,i]);

                    for(int j = 0; j < Rows; j++){
                        if(j == row)
                            continue;
                        if(mat[j,i] == 0)
                            continue;
                        
                        mat.AddRow(row,j,-1 * mat[j,i]);
                    }

                    row++;
                }

                return mat;
            }
        }
        
        private double[][] _matrix;

        private Matrix() { }
        public Matrix(int rows, int cols)
        {
            _matrix = new double[rows][];
            for (int i = 0; i < rows; i++)
                _matrix[i] = new double[cols];
        }
        /// <summary>
        ///     Copies the matrix into a new reference
        /// </summary>
        public Matrix Clone() => SubMatrix(Enumerable.Range(0, Rows), Enumerable.Range(0, Columns));
        /// <summary>
        ///     Returns a submatrix
        /// </summary>
        public Matrix SubMatrix(int rowFrom, int rowCount, int colFrom, int colCount)
            => SubMatrix(Enumerable.Range(rowFrom, rowCount), Enumerable.Range(colFrom, colCount));
        /// <summary>
        ///     Returns a submatrix containing the rows and columns supplied.
        /// </summary>
        public Matrix SubMatrix(IEnumerable<int> rows, IEnumerable<int> cols)
        {
            int colCount = cols.Count();
            int rowCount = rows.Count();
            Matrix mat = new Matrix(rowCount, colCount);
            int i = 0;
            foreach (var row in rows)
            {
                int j = 0;
                foreach (var col in cols)
                {
                    mat[i, j] = this[row, col];
                    j++;
                }
                i++;
            }
            return mat;
        }
        /// <summary>
        ///     Returns a printable version of the matrix
        /// </summary>
        /// TODO: Make it prettier
        public string toString()
        {
            string str = "";
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                    str += this[i, j] + " ";

                str += "\n";

            }
            return str;
        }
        /// <summary>
        ///     Switches row1 and row2
        /// </summary>
        private void InterchangeRows(int row1, int row2)
        {
            if(row1 == row2) return;

            for (int i = 0; i < Columns; i++)
            {
                double temp = this[row1, i];
                this[row1, i] = this[row2, i];
                this[row2, i] = temp;
            }
        }
        /// <summary>
        ///     Replaces a row with a multiple of same row
        /// </summary>
        private void MultiplyRow(int row, double multiplier)
        {
            for (int i = 0; i < Columns; i++)
                this[row, i] *= multiplier;
        }
        /// <summary>
        ///     Adds one row to another
        /// </summary>
        private void AddRow(int row, int to) => AddRow(row, to, 1);
        /// <summary>
        ///     Adds a multiple of one row to another
        /// </summary>
        private void AddRow(int row, int to, double multiplier)
        {
            for (int i = 0; i < Columns; i++)
                this[to, i] += multiplier * this[row, i];
        }
        public static double Det(Matrix a) => Det(a, Enumerable.Range(0, a.Columns), Enumerable.Range(0, a.Rows));
        private static double Det(Matrix a, IEnumerable<int> cols, IEnumerable<int> rows)
        {
            int colCount = cols.Count();
            int rowCount = rows.Count();
            if (colCount != rowCount)
                throw new ArgumentException("Matrix does not have same number of rows as columns.");
            if (colCount == 1)
                return a[rows.First(), cols.First()];
            else
            {
                double det = 0;

                int colIndex = 0;
                foreach (var col in cols)
                {
                    det += a[rows.First(), col] * Det(a, cols.Where(x => x != col), rows.Skip(1)) * (colIndex % 2 == 0 ? 1 : -1);
                    colIndex++;
                }

                return det;
            }
        }
        public static Matrix Parse(string text) => Parse(text, ' ', ';');
        public static Matrix Parse(string text, char colSep, char rowSep)
        {

            var rows = text.Split(rowSep);

            Matrix mat = new Matrix();
            mat._matrix = new double[rows.Length][];

            var fmt = new NumberFormatInfo(){ NegativeSign = "-" };

            for (int i = 0; i < rows.Length; i++)
            {
                var cols = rows[i].Split(colSep);

                if (i > 0 && cols.Length != mat._matrix[i - 1].Length)
                    throw new FormatException("Inconsistent matrix dimensions supplied");

                mat._matrix[i] = new double[cols.Length];

                for (int j = 0; j < cols.Length; j++)
                    mat._matrix[i][j] = double.Parse(cols[j],fmt);
            }

            return mat;

        }
        public static Matrix Zeroes(int size) => Zeroes(size, size);
        public static Matrix Zeroes(int rows, int cols)
        {
            Matrix mat = new Matrix(rows, cols);
            for (int i = 0; i < rows; i++)
                for (int j = 0; j < cols; j++)
                    mat[i, j] = 0;

            return mat;
        }
        public static Matrix Ones(int size) => Ones(size, size);
        public static Matrix Ones(int rows, int cols)
        {
            Matrix mat = new Matrix(rows, cols);
            for (int i = 0; i < rows; i++)
                for (int j = 0; j < cols; j++)
                    mat[i, j] = 1;

            return mat;
        }
        public static Matrix Identity(int size)
        {
            var mat = new Matrix(size, size);
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    if (i == j)
                        mat[i, j] = 1;
                    else
                        mat[i, j] = 0;
                }
            }
            return mat;
        }
        public static Matrix operator +(Matrix a, Matrix b)
        {
            if (a.Rows != b.Rows || a.Columns != b.Columns)
                throw new ArgumentException("Invalid matrix dimensions.");

            Matrix mat = new Matrix(a.Rows, a.Columns);

            for (int i = 0; i < mat.Rows; i++)
                for (int j = 0; j < mat.Columns; j++)
                    mat[i, j] = a[i, j] + b[i, j];

            return mat;
        }
        public static Matrix operator *(double a, Matrix b)
        {
            Matrix mat = new Matrix(b.Rows, b.Columns);

            for (int i = 0; i < mat.Rows; i++)
                for (int j = 0; j < mat.Columns; j++)
                    mat[i, j] = a * b[i, j];

            return mat;
        }
        public static Matrix operator *(int a, Matrix b)
        {
            Matrix mat = new Matrix(b.Rows, b.Columns);

            for (int i = 0; i < mat.Rows; i++)
                for (int j = 0; j < mat.Columns; j++)
                    mat[i, j] = a * b[i, j];

            return mat;
        }
        public static Matrix operator *(Matrix a, double b) => b * a;
        public static Matrix operator *(Matrix a, int b) => b * a;
        public static Matrix operator -(Matrix a, Matrix b)
        {
            if (a.Rows != b.Rows || a.Columns != b.Columns)
                throw new ArgumentException("Invalid matrix dimensions.");

            Matrix mat = new Matrix(a.Rows, a.Columns);

            for (int i = 0; i < mat.Rows; i++)
                for (int j = 0; j < mat.Columns; j++)
                    mat[i, j] = a[i, j] - b[i, j];

            return mat;
        }
        public static Matrix operator *(Matrix a, Matrix b)
        {

            if (a.Columns != b.Rows)
                throw new ArgumentException("Invalid matrix dimensions.");

            Matrix mat = new Matrix(b.Rows, b.Columns);

            for (int i = 0; i < mat.Rows; i++)
            {
                for (int j = 0; j < mat.Columns; j++)
                {
                    mat[i, j] = 0;
                    for (int k = 0; k < a.Columns; k++)
                    {
                        mat[i, j] += a[i, k] * b[k, j];
                    }
                }
            }

            return mat;
        }
        public static bool operator ==(Matrix a, Matrix b)
        {
            if (a.Rows != b.Rows || a.Columns != b.Columns)
                return false;
            for (int i = 0; i < a.Rows; i++)
                for (int j = 0; j < a.Columns; j++)
                    if (a[i, j] != b[i, j])
                        return false;

            return true;
        }
        public static bool operator !=(Matrix a, Matrix b) => !(a == b);

        public override bool Equals(object obj)
        {
            if (obj is Matrix)
                return this == (Matrix)obj;
            return base.Equals(obj);
        }

        public override int GetHashCode() => base.GetHashCode();
    }
}
