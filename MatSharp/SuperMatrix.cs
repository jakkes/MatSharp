using System;

namespace MatSharp {
    public class SuperMatrix < T > {
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
        private T[][] _matrix;

        public Matrix(int rows, int cols) {
            _matrix = new T[rows][];
            for (int i = 0; i < cols; i++)
                _matrix[i] = new T[cols];
        }


    }
}