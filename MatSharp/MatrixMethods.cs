using System;
using System.Collections.Generic;
using System.Linq;

namespace MatSharp {

    public static class MatrixExtensionMethods {
        /// <summary>
        ///     Returns the determinant.
        /// </summary>
        public static double Determinant(this Matrix < double > matrix) => matrix.Determinant(Enumerable.Range(0, matrix.Rows), Enumerable.Range(0, matrix.Columns));
        private static double Determinant(this Matrix < double > matrix, IEnumerable < int > rows, IEnumerable < int > cols) {
            int colCount = cols.Count();
            int rowCount = rows.Count();
            if (colCount != rowCount)
                throw new ArgumentException("Matrix does not have same number of rows as columns.");
            if (colCount == 1)
                return matrix[rows.First(), cols.First()];
            else {
                double det = 0;

                int colIndex = 0;
                foreach(var col in cols) {
                    det += matrix[rows.First(), col] * matrix.Determinant(rows.Skip(1), cols.Where(x => x != col)) * (colIndex % 2 == 0 ? 1 : -1);
                    colIndex++;
                }

                return det;
            }
        }

        public static Matrix < double > RREF(this Matrix < double > matrix) {
            var mat = matrix.Clone();

            int row = 0;

            for (int i = 0; i < mat.Columns; i++) {

                bool oneFixed = false;

                for (int j = row; j < mat.Rows; j++) {
                    if (mat[j, i] != 0) {
                        mat.InterchangeRows(row, j);
                        oneFixed = true;
                        break;
                    }
                }

                if (!oneFixed) continue;

                mat.MultiplyRow(row, 1 / mat[row, i]);

                for (int j = 0; j < mat.Rows; j++) {
                    if (j == row)
                        continue;
                    if (mat[j, i] == 0)
                        continue;

                    mat.AddRow(row, j, -1 * mat[j, i]);
                }

                row++;
            }

            return mat;
        }
        private static void InterchangeRows(this Matrix < double > matrix, int row1, int row2) {
            for (int i = 0; i < matrix.Columns; i++) {
                double temp = matrix[row1, i];
                matrix[row1, i] = matrix[row2, i];
                matrix[row2, i] = temp;
            }
        }
        private static void MultiplyRow(this Matrix < double > matrix, int row, double multiplier) {
            for (int i = 0; i < matrix.Columns; i++)
                matrix[row, i] *= multiplier;
        }
        private static void AddRow(this Matrix < double > matrix, int row, int to) => matrix.AddRow(row, to, 1);
        private static void AddRow(this Matrix < double > matrix, int row, int to, double multiplier) {
            for (int i = 0; i < matrix.Columns; i++)
                matrix[to, i] += multiplier * matrix[row, i];
        }

        public static Matrix < double > Add(this Matrix < double > m, Matrix < double > matrix) => m.Add(matrix, 1);
        public static Matrix < double > Add(this Matrix < double > m, Matrix < double > matrix, double multiplier) {
            if (m.Rows != matrix.Rows || m.Columns != matrix.Columns)
                throw new ArgumentException("Invalid matrix dimensions.");

            Matrix < double > mat = new Matrix < double > (m.Rows, m.Columns);

            for (int i = 0; i < mat.Rows; i++)
                for (int j = 0; j < mat.Columns; j++)
                    mat[i, j] = m[i, j] + multiplier * matrix[i, j];

            return mat;
        }

        public static Matrix<double> Multiply(this Matrix<double> m, double multiplier) 
            => new Matrix<double>(m.Select(x => x * multiplier), m.Rows);
            
        public static Matrix < double > Multiply(this Matrix < double > m, Matrix < double > matrix) => m.Multiply(matrix, 1);
        public static Matrix < double > Multiply(this Matrix < double > m, Matrix < double > matrix, double multiplier) {
            if (m.Columns != matrix.Rows)
                throw new ArgumentException("Invalid matrix dimensions.");

            Matrix < double > mat = new Matrix < double > (m.Rows, matrix.Columns);

            for (int i = 0; i < mat.Rows; i++) {
                for (int j = 0; j < mat.Columns; j++) {
                    mat[i, j] = 0;
                    for (int k = 0; k < m.Columns; k++) {
                        mat[i, j] += m[i, k] * matrix[k, j] * multiplier;
                    }
                }
            }

            return mat;
        }

        public static Matrix < bool > Equals(this Matrix < double > m, Matrix < double > matrix) {
            if (m.Rows != matrix.Rows || m.Columns != matrix.Columns)
                throw new ArgumentException("Invalid matrix dimensions.");

            Matrix < bool > mat = new Matrix < bool > (m.Rows, m.Columns);

            for (int i = 0; i < mat.Rows; i++)
                for (int j = 0; j < mat.Columns; j++)
                    mat[i, j] = m[i, j] == matrix[i, j];

            return mat;
        }

        public static Matrix < bool > GreaterThan(this Matrix < double > m, Matrix < double > matrix) {
            if (m.Rows != matrix.Rows || m.Columns != matrix.Columns)
                throw new ArgumentException("Invalid matrix dimensions.");

            Matrix < bool > mat = new Matrix < bool > (m.Rows, m.Columns);

            for (int i = 0; i < mat.Rows; i++)
                for (int j = 0; j < mat.Columns; j++)
                    mat[i, j] = m[i, j] > matrix[i, j];

            return mat;
        }
        public static Matrix < bool > GreaterEqual(this Matrix < double > m, Matrix < double > matrix) {
            if (m.Rows != matrix.Rows || m.Columns != matrix.Columns)
                throw new ArgumentException("Invalid matrix dimensions.");

            Matrix < bool > mat = new Matrix < bool > (m.Rows, m.Columns);

            for (int i = 0; i < mat.Rows; i++)
                for (int j = 0; j < mat.Columns; j++)
                    mat[i, j] = m[i, j] >= matrix[i, j];

            return mat;
        }

        public static Matrix < bool > LessThan(this Matrix < double > m, Matrix < double > matrix) {
            if (m.Rows != matrix.Rows || m.Columns != matrix.Columns)
                throw new ArgumentException("Invalid matrix dimensions.");

            Matrix < bool > mat = new Matrix < bool > (m.Rows, m.Columns);

            for (int i = 0; i < mat.Rows; i++)
                for (int j = 0; j < mat.Columns; j++)
                    mat[i, j] = m[i, j] < matrix[i, j];

            return mat;
        }
        public static Matrix < bool > LessEqual(this Matrix < double > m, Matrix < double > matrix) {
            if (m.Rows != matrix.Rows || m.Columns != matrix.Columns)
                throw new ArgumentException("Invalid matrix dimensions.");

            Matrix < bool > mat = new Matrix < bool > (m.Rows, m.Columns);

            for (int i = 0; i < mat.Rows; i++)
                for (int j = 0; j < mat.Columns; j++)
                    mat[i, j] = m[i, j] <= matrix[i, j];

            return mat;
        }

        /// <summary>
        ///     Solves the equation Ax = b, where A is the current object
        /// </summary>
        public static Matrix < double > Solve(this Matrix < double > m, Matrix < double > b) {
            if (m.Rows != b.Rows)
                throw new ArgumentException("Matrix dimensions do not agree");

            var mat = m.JoinColumns(b);

            var rref = mat.RREF();

            if (rref.leadingElements().Any(x => x > m.Columns - 1))
                throw new NoSolutionException("System cannot be solved.");

            return rref.SubMatrix(0, m.Rows, m.Columns, b.Columns);
        }
        /// <summary>
        ///     Returns the columns of the leading elements of each row. If there is no non-zero element, -1 is used
        /// </summary>
        private static IEnumerable < int > leadingElements(this Matrix < double > m) {
            int[] cols = new int[m.Rows];
            for (int i = 0; i < m.Rows; i++) {
                cols[i] = -1;
                for (int j = 0; j < m.Columns; j++) {
                    if (m[i, j] != 0) {
                        cols[i] = j;
                        break;
                    }
                }
            }
            return cols;
        }

    }

}