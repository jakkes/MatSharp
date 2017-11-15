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
        }

        [Fact]
        public void Equality(){
            Matrix a = Matrix.Parse("1 2;3 4");
            
        }

        [Fact]
        public void Determinant(){
            Matrix mat = Matrix.Parse("1 2;3 4");
            Assert.Equal(-2,mat.Determinant);

            Matrix mat2 = Matrix.Parse("1 0 3 7;2 5 4 2;1 3 4 1;1 1 1 4");
            Assert.Equal(3, mat2.Determinant);
        }
    }
}
