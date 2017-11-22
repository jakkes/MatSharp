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
            Matrix<double> mat = new Matrix<double>(new double[]{1,2,3,4},2);
            Assert.True(mat[0,0] == 1);
            Assert.True(mat[0,1] == 2);
            Assert.True(mat[1,0] == 3);
            Assert.True(mat[1,1] == 4);

            Matrix<double> mat2 = new Matrix<double>(new double[]{1,2,3,4,5,6},2);
            Assert.True(mat2[0,0] == 1);
            Assert.True(mat2[0,1] == 2);
            Assert.True(mat2[0,2] == -3);
            Assert.True(mat2[1,0] == -4);
            Assert.True(mat2[1,1] == 5);
            Assert.True(mat2[1,2] == 6);

            Assert.Throws<ArgumentException>(() => new Matrix<double>(new double[]{1,2,3,1,2,3,4},2));

        }

        [Fact]
        public void Equality(){
            Matrix<double> a = new Matrix<double>(new double[]{1,2,3,4},2);
            Matrix<double> b = new Matrix<double>(new double[]{1,2,3,4},2);
            Matrix<double> c = new Matrix<double>(new double[]{2,1,4,3},2);
            Matrix<double> d = new Matrix<double>(new double[]{1,2,3,4,5,6},2);
            Assert.True(a == b);
            Assert.False(a == c);
            Assert.True(a != c);

            Assert.True(a != d);
            Assert.False(a == d);
        }

        [Fact]
        public void Determinant(){
            var mat = MatrixFactory.Parse("1 2;3 4");
            Assert.Equal(-2,mat.Determinant());

            var mat2 = MatrixFactory.Parse("4 3 2 1;9 8 7 6;12 21 12 43;97 1 32 1");
            Assert.Equal(-19820, mat2.Determinant());
        }

        [Fact]
        public void Adding(){
            var m1 = MatrixFactory.Parse("1 2;3 4");
            var m2 = MatrixFactory.Parse("-1 -2;-3 -4");
            var m3 = MatrixFactory.Parse("0 0;0 0");
            Assert.Equal(m3, m1.Add(m2));
        }

        [Fact]
        public void MultiplyScalar(){
            var m1 = MatrixFactory.Parse("1 2;3 4");
            var m2 = MatrixFactory.Parse("-1 -2;-3 -4");
            Assert.Equal(m2, m1.Multiply(-1));
        }

        [Fact]
        public void OnesZeros(){
            Assert.Equal(
                MatrixFactory.Parse("1 1 1;1 1 1"), MatrixFactory.Ones(2,3)
            );
            Assert.Equal(MatrixFactory.Zeroes(4), MatrixFactory.Ones(4).Subtract(MatrixFactory.Ones(4)));
        }

        [Fact]
        public void Identity(){
            Assert.Equal(
                MatrixFactory.Parse("1 0 0;0 1 0;0 0 1"),
                MatrixFactory.Identity(3)
            );
        }

        [Fact]
        public void MultiplyMatrices(){
            var mat = MatrixFactory.Parse("4 5 2;0 9 1;3 1 2");
            Assert.Equal(mat, MatrixFactory.Identity(3).Multiply(mat));

            var mat2 = MatrixFactory.Parse("3 1 4 5 6;1 2 3 4 5;9 8 7 6 5");
            var mat3 = MatrixFactory.Parse("35 30 45 52 59;18 26 34 42 50;28 21 29 31 33");

            Assert.Equal(mat3, mat.Multiply(mat2));
            Assert.Throws<ArgumentException>(() => mat2.Multiply(mat));        
        }

        [Fact]
        public void Rref(){
            Assert.Equal(
                MatrixFactory.Identity(2), 
                MatrixFactory.Parse("1 1;-1 1").RREF()
            );

            Assert.Equal(
                MatrixFactory.Parse("1 1;0 0"),
                MatrixFactory.Parse("1 1;1 1").RREF()
            );

            Assert.Equal(
                MatrixFactory.Parse("1 0 5 0 0 0;0 1 0 0 1 3;0 0 0 1 -1 -3"),
                MatrixFactory.Parse("-1 2 -5 1 1 3;1 0 5 -1 1 3;1 0 5 1 -1 -3").RREF()
            );
        }

        [Fact]
        public void JoinRows(){
            Assert.Equal(
                MatrixFactory.Parse("1 2;3 4;5 6;7 8"),
                Matrix<double>.JoinRows(
                    MatrixFactory.Parse("1 2;3 4"),
                    MatrixFactory.Parse("5 6;7 8")
                )
            );
        }

        [Fact]
        public void JoinColumns(){
            Assert.Equal(
                MatrixFactory.Parse("1 2 3 4;5 6 7 8"),
                Matrix<double>.JoinColumns(
                    MatrixFactory.Parse("1 2;5 6"),
                    MatrixFactory.Parse("3 4;7 8")
                )
            );
        }

        [Fact]
        public void Transpose(){
            Assert.Equal(
                MatrixFactory.Parse("1 2 3;4 5 6"),
                MatrixFactory.Parse("1 4;2 5;3 6").GetTranspose()
            );
        }

        [Fact]
        public void Solve(){
            Assert.Equal(
                MatrixFactory.Parse("1;1"),
                MatrixFactory.Parse("1 0;0 1").Solve(MatrixFactory.Parse("1;1"))
            );

            var x = MatrixFactory.Parse("2 1;1 2");
            var A = MatrixFactory.Parse("3 1;1 3");
            var b = MatrixFactory.Parse("7 5;5 7");

            Assert.Equal(
                b, A.Multiply(x)
            );

            Assert.Equal(
                x,
                MatrixMath.Round(A.Solve(b),2)
            );
        }

        [Fact]
        public void Enumerator(){
            var a = MatrixFactory.Parse("1 2;3 4;5 6");
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