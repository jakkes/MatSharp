using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace MatSharp {
    public class SuperMatrix<T> : IEnumerable<T>
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
        public SuperMatrix<T> Transpose {
            get {
                SuperMatrix<T> mat = new SuperMatrix<T>(Columns,Rows);
                for(int i = 0; i < Rows; i++)
                    for(int j = 0; j < Columns; j++)
                        mat[j,i] = this[i,j];

                return mat;
            }
        }

        /// <summary>
        ///     Copies the matrix into a new reference
        /// </summary>
        public SuperMatrix<T> Clone() => SubMatrix(Enumerable.Range(0, Rows), Enumerable.Range(0, Columns));
        /// <summary>
        ///     Returns a submatrix
        /// </summary>
        public SuperMatrix<T> SubMatrix(int rowFrom, int rowCount, int colFrom, int colCount)
            => SubMatrix(Enumerable.Range(rowFrom, rowCount), Enumerable.Range(colFrom, colCount));
        /// <summary>
        ///     Returns a submatrix containing the rows and columns supplied.
        /// </summary>
        public SuperMatrix<T> SubMatrix(IEnumerable<int> rows, IEnumerable<int> cols)
        {
            int colCount = cols.Count();
            int rowCount = rows.Count();
            SuperMatrix<T> mat = new SuperMatrix<T>(rowCount, colCount);
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

        protected SuperMatrix() { }
        public SuperMatrix(int rows, int cols)
        {
            _matrix = new T[rows][];
            for (int i = 0; i < rows; i++)
                _matrix[i] = new T[cols];
        }

        public IEnumerator<T> GetEnumerator()
            => new MatrixEnumerator<T>(this);

        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();
    }

    public class MatrixEnumerator<T> : IEnumerator<T>
    {
        private SuperMatrix<T> matrix;
        private int row = -1;
        private int col = 0;
        public MatrixEnumerator(SuperMatrix<T> mat)
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