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
            MatrixObs mat = MatrixObs.Parse("1 2;3 4");
            Assert.True(mat[0,0] == 1);
            Assert.True(mat[0,1] == 2);
            Assert.True(mat[1,0] == 3);
            Assert.True(mat[1,1] == 4);

            MatrixObs mat2 = MatrixObs.Parse("1 2 -3;-4 5 6");
            Assert.True(mat2[0,0] == 1);
            Assert.True(mat2[0,1] == 2);
            Assert.True(mat2[0,2] == -3);
            Assert.True(mat2[1,0] == -4);
            Assert.True(mat2[1,1] == 5);
            Assert.True(mat2[1,2] == 6);

            Assert.Throws<FormatException>(() => MatrixObs.Parse("1 2 3;1 2 3 4"));

        }

        [Fact]
        public void Equality(){
            MatrixObs a = MatrixObs.Parse("1 2;3 4");
            MatrixObs b = MatrixObs.Parse("1 2;3 4");
            MatrixObs c = MatrixObs.Parse("2 1;4 3");
            MatrixObs d = MatrixObs.Parse("1 2 3;1 2 3");
            Assert.True(a == b);
            Assert.False(a == c);
            Assert.True(a != c);

            Assert.True(a != d);
            Assert.False(a == d);
        }

        [Fact]
        public void Determinant(){
            MatrixObs mat = MatrixObs.Parse("1 2;3 4");
            Assert.Equal(-2,mat.Determinant);

            MatrixObs mat2 = MatrixObs.Parse("4 3 2 1;9 8 7 6;12 21 12 43;97 1 32 1");
            Assert.Equal(-19820, mat2.Determinant);
        }

        [Fact]
        public void Adding(){
            MatrixObs m1 = MatrixObs.Parse("1 2;3 4");
            MatrixObs m2 = MatrixObs.Parse("-1 -2;-3 -4");
            MatrixObs m3 = MatrixObs.Parse("0 0;0 0");
            Assert.Equal(m3, m1 + m2);
        }

        [Fact]
        public void MultiplyScalar(){
            MatrixObs m1 = MatrixObs.Parse("1 2;3 4");
            MatrixObs m2 = MatrixObs.Parse("-1 -2;-3 -4");
            Assert.Equal(m2, -1 * m1);
        }

        [Fact]
        public void OnesZeros(){
            Assert.Equal(
                MatrixObs.Parse("1 1 1;1 1 1"), MatrixObs.Ones(2,3)
            );
            Assert.Equal(MatrixObs.Zeroes(4), MatrixObs.Ones(4) - MatrixObs.Ones(4));
        }

        [Fact]
        public void Identity(){
            Assert.Equal(
                MatrixObs.Parse("1 0 0;0 1 0;0 0 1"),
                MatrixObs.Identity(3)
            );
        }

        [Fact]
        public void MultiplyMatrices(){
            MatrixObs mat = MatrixObs.Parse("4 5 2;0 9 1;3 1 2");
            Assert.Equal(mat, MatrixObs.Identity(3) * mat);

            MatrixObs mat2 = MatrixObs.Parse("3 1 4 5 6;1 2 3 4 5;9 8 7 6 5");
            MatrixObs mat3 = MatrixObs.Parse("35 30 45 52 59;18 26 34 42 50;28 21 29 31 33");

            Assert.Equal(mat3, mat * mat2);
            Assert.Throws<ArgumentException>(() => mat2 * mat);
        }

        [Fact]
        public void Rref(){
            Assert.Equal(
                MatrixObs.Identity(2), 
                MatrixObs.Parse("1 1;-1 1").Rref
            );

            Assert.Equal(
                MatrixObs.Parse("1 1;0 0"),
                MatrixObs.Parse("1 1;1 1").Rref
            );

            Assert.Equal(
                MatrixObs.Parse("1 0 5 0 0 0;0 1 0 0 1 3;0 0 0 1 -1 -3"),
                MatrixObs.Parse("-1 2 -5 1 1 3;1 0 5 -1 1 3;1 0 5 1 -1 -3").Rref
            );
        }

        [Fact]
        public void JoinRows(){
            Assert.Equal(
                MatrixObs.Parse("1 2;3 4;5 6;7 8"),
                MatrixObs.JoinRows(
                    MatrixObs.Parse("1 2;3 4"),
                    MatrixObs.Parse("5 6;7 8")
                )
            );
        }

        [Fact]
        public void JoinColumns(){
            Assert.Equal(
                MatrixObs.Parse("1 2 3 4;5 6 7 8"),
                MatrixObs.JoinColumns(
                    MatrixObs.Parse("1 2;5 6"),
                    MatrixObs.Parse("3 4;7 8")
                )
            );
        }

        [Fact]
        public void Transpose(){
            Assert.Equal(
                MatrixObs.Parse("1 2 3;4 5 6"),
                MatrixObs.Parse("1 4;2 5;3 6").Transpose
            );
        }

        [Fact]
        public void Solve(){
            Assert.Equal(
                MatrixObs.Parse("1;1"),
                MatrixObs.Parse("1 0;0 1").Solve(MatrixObs.Parse("1;1"))
            );

            var x = MatrixObs.Parse("2 1;1 2");
            var A = MatrixObs.Parse("3 1;1 3");
            var b = MatrixObs.Parse("7 5;5 7");

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
            MatrixObs a = MatrixObs.Parse("1 2;3 4;5 6");
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