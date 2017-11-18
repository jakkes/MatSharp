using System;
using Xunit;
using MatSharp;
using System.Linq;

namespace MatSharp.Test
{
    public class MatrixTest
    {
        [Fact]
        public void Parser()
        {
            Matrix mat = Matrix.Parse("1 2;3 4");
            Assert.True(mat[0,0] == 1);
            Assert.True(mat[0,1] == 2);
            Assert.True(mat[1,0] == 3);
            Assert.True(mat[1,1] == 4);

            Matrix mat2 = Matrix.Parse("1 2 -3;-4 5 6");
            Assert.True(mat2[0,0] == 1);
            Assert.True(mat2[0,1] == 2);
            Assert.True(mat2[0,2] == -3);
            Assert.True(mat2[1,0] == -4);
            Assert.True(mat2[1,1] == 5);
            Assert.True(mat2[1,2] == 6);

            Assert.Throws<FormatException>(() => Matrix.Parse("1 2 3;1 2 3 4"));

        }

        [Fact]
        public void Equality(){
            Matrix a = Matrix.Parse("1 2;3 4");
            Matrix b = Matrix.Parse("1 2;3 4");
            Matrix c = Matrix.Parse("2 1;4 3");
            Matrix d = Matrix.Parse("1 2 3;1 2 3");
            Assert.True(a == b);
            Assert.False(a == c);
            Assert.True(a != c);

            Assert.True(a != d);
            Assert.False(a == d);
        }

        [Fact]
        public void Determinant(){
            Matrix mat = Matrix.Parse("1 2;3 4");
            Assert.Equal(-2,mat.Determinant);

            Matrix mat2 = Matrix.Parse("4 3 2 1;9 8 7 6;12 21 12 43;97 1 32 1");
            Assert.Equal(-19820, mat2.Determinant);
        }

        [Fact]
        public void Adding(){
            Matrix m1 = Matrix.Parse("1 2;3 4");
            Matrix m2 = Matrix.Parse("-1 -2;-3 -4");
            Matrix m3 = Matrix.Parse("0 0;0 0");
            Assert.Equal(m3, m1 + m2);
        }

        [Fact]
        public void MultiplyScalar(){
            Matrix m1 = Matrix.Parse("1 2;3 4");
            Matrix m2 = Matrix.Parse("-1 -2;-3 -4");
            Assert.Equal(m2, -1 * m1);
        }

        [Fact]
        public void OnesZeros(){
            Assert.Equal(
                Matrix.Parse("1 1 1;1 1 1"), Matrix.Ones(2,3)
            );
            Assert.Equal(Matrix.Zeroes(4), Matrix.Ones(4) - Matrix.Ones(4));
        }

        [Fact]
        public void Identity(){
            Assert.Equal(
                Matrix.Parse("1 0 0;0 1 0;0 0 1"),
                Matrix.Identity(3)
            );
        }

        [Fact]
        public void MultiplyMatrices(){
            Matrix mat = Matrix.Parse("4 5 2;0 9 1;3 1 2");
            Assert.Equal(mat, Matrix.Identity(3) * mat);

            Matrix mat2 = Matrix.Parse("3 1 4 5 6;1 2 3 4 5;9 8 7 6 5");
            Matrix mat3 = Matrix.Parse("35 30 45 52 59;18 26 34 42 50;28 21 29 31 33");

            Assert.Equal(mat3, mat * mat2);
            Assert.Throws<ArgumentException>(() => mat2 * mat);
        }

        [Fact]
        public void Rref(){
            Assert.Equal(
                Matrix.Identity(2), 
                Matrix.Parse("1 1;-1 1").Rref
            );

            Assert.Equal(
                Matrix.Parse("1 1;0 0"),
                Matrix.Parse("1 1;1 1").Rref
            );

            Assert.Equal(
                Matrix.Parse("1 0 5 0 0 0;0 1 0 0 1 3;0 0 0 1 -1 -3"),
                Matrix.Parse("-1 2 -5 1 1 3;1 0 5 -1 1 3;1 0 5 1 -1 -3").Rref
            );
        }

        [Fact]
        public void JoinRows(){
            Assert.Equal(
                Matrix.Parse("1 2;3 4;5 6;7 8"),
                Matrix.JoinRows(
                    Matrix.Parse("1 2;3 4"),
                    Matrix.Parse("5 6;7 8")
                )
            );
        }

        [Fact]
        public void JoinColumns(){
            Assert.Equal(
                Matrix.Parse("1 2 3 4;5 6 7 8"),
                Matrix.JoinColumns(
                    Matrix.Parse("1 2;5 6"),
                    Matrix.Parse("3 4;7 8")
                )
            );
        }

        [Fact]
        public void Transpose(){
            Assert.Equal(
                Matrix.Parse("1 2 3;4 5 6"),
                Matrix.Parse("1 4;2 5;3 6").Transpose
            );
        }

        [Fact]
        public void Solve(){
            Assert.Equal(
                Matrix.Parse("1;1"),
                Matrix.Parse("1 0;0 1").Solve(Matrix.Parse("1;1"))
            );

            var x = Matrix.Parse("2 1;1 2");
            var A = Matrix.Parse("3 1;1 3");
            var b = Matrix.Parse("7 5;5 7");

            Assert.Equal(
                b, A*x
            );

            Assert.Equal(
                x,
                MatrixMath.Round(A.Solve(b),2)
            );
        }

        [Fact]
        public void Enumerator(){
            Matrix a = Matrix.Parse("1 2;3 4;5 6");
            Assert.Equal(1, a.ElementAt(0));
            Assert.Equal(3, a.ElementAt(1));
            Assert.Equal(5, a.ElementAt(2));
            Assert.Equal(2, a.ElementAt(3));
            Assert.Equal(4, a.ElementAt(4));
            Assert.Equal(6, a.ElementAt(5));
            Assert.Throws<ArgumentOutOfRangeException>(() => a.ElementAt(6));
        }
    }
}