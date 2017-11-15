using System;
using Xunit;
using MatSharp;

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
    }
}
