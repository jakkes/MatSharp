using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace MatSharp {
    public class Matrix<T> : IEnumerable<T>
    {
        protected T[][] _matrix;
        
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

        public T this[int i, int j]{
            get{
                return _matrix[i][j];
            } set {
                _matrix[i][j] = value;
            }
        }
        
        /// <summary>
        ///     Returns the transposed matrix
        /// </summary>
        public Matrix<T> Transpose {
            get {
                Matrix<T> mat = new Matrix<T>(Columns,Rows);
                for(int i = 0; i < Rows; i++)
                    for(int j = 0; j < Columns; j++)
                        mat[j,i] = this[i,j];

                return mat;
            }
        }

        /// <summary>
        ///     Copies the matrix into a new reference
        /// </summary>
        public Matrix<T> Clone() => SubMatrix(Enumerable.Range(0, Rows), Enumerable.Range(0, Columns));
        /// <summary>
        ///     Returns a submatrix
        /// </summary>
        public Matrix<T> SubMatrix(int rowFrom, int rowCount, int colFrom, int colCount)
            => SubMatrix(Enumerable.Range(rowFrom, rowCount), Enumerable.Range(colFrom, colCount));
        /// <summary>
        ///     Returns a submatrix containing the rows and columns supplied.
        /// </summary>
        public Matrix<T> SubMatrix(IEnumerable<int> rows, IEnumerable<int> cols)
        {
            int colCount = cols.Count();
            int rowCount = rows.Count();
            Matrix<T> mat = new Matrix<T>(rowCount, colCount);
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
        public Matrix(int rows, int cols)
        {
            _matrix = new T[rows][];
            for (int i = 0; i < rows; i++)
                _matrix[i] = new T[cols];
        }

        /// <summary>
        ///     Creates a matrix filled with data
        /// </summary>
        public Matrix(IEnumerable<T> data, int rows){
            int dataCount = data.Count();
            if(dataCount % rows != 0)
                throw new ArgumentException("Cannot populate a matrix of " + rows + " with the given data.");
            
            int cols = dataCount / rows;
            _matrix = new T[rows][];
            for(int i = 0; i < rows; i++)
                _matrix[i] = new T[cols];
            
            var e = data.GetEnumerator();
            for(int i = 0; i < rows; i++){
                for(int j = 0; j < rows; j++){
                    if(e.MoveNext())
                        _matrix[i][j] = e.Current;
                    else
                        throw new ArgumentException("Could not populate the matrix.");
                }
            }
        }

        public IEnumerator<T> GetEnumerator()
            => new MatrixEnumerator<T>(this);

        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();

        
    }

    public class MatrixEnumerator<T> : IEnumerator<T>
    {
        private Matrix<T> matrix;
        private int row = -1;
        private int col = 0;
        public MatrixEnumerator(Matrix<T> mat)
        {
            matrix = mat;
        }

        public T Current => matrix[row,col];

        object IEnumerator.Current => matrix[row,col];

        public void Dispose()
        {
            
        }

        public bool MoveNext()
        {
            if(row + 1 < matrix.Rows){
                row++;
                return true;
            } else {
                if(col + 1 < matrix.Columns){
                    col++;
                    row = 0;
                    return true;
                } else {
                    return false;
                }
            }
        }

        public void Reset()
        {
            col = 0;
            row = -1;
        }
    }
}