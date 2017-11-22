using System.Collections.Generic;

namespace MatSharp {

    public static class MatrixFactory{

        public static Matrix<double> Identity(int size){
            var mat = new Matrix<double>(size,size);
            for(int i = 0; i < size; i++)
                mat[i,i] = 1;

            return mat;
        }

        public static Matrix<double> Zeroes(int rows, int cols){
            return new Matrix<double>(rows,cols);
        }
        public static Matrix<double> Zeroes(int size)
            => Zeroes(size,size);

        public static Matrix<double> Ones(int rows, int cols){
            var mat = new Matrix<double>(rows,cols);
            for(int i = 0; i < rows; i++)
                for(int j = 0; j < cols; j++)
                    mat[i,j] = 1;

            return mat;
        }

        public static Matrix<double> Ones(int size)
            => Ones(size, size);

        public static Matrix<double> Parse(string text)
            => Parse(text, ';',' ');
        public static Matrix<double> Parse(string text, char rowSep, char colSep){
            List<double> values = new List<double>();
            
            string str = "";
            int rows = 0;

            foreach(char c in text){
                if(c == rowSep){
                    values.Add(double.Parse(str.Trim()));
                    rows++;
                } else if(c == colSep){values.Add(double.Parse(str.Trim()));
                    values.Add(double.Parse(str.Trim()));
                } else {
                    str += c;
                }
            }
            values.Add(double.Parse(str.Trim()));

            return new Matrix<double>(values, rows);
        }
    }
}