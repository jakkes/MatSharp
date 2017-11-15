using System;
using MatSharp;

namespace DebugHelper
{
    class Program
    {
        static void Main(string[] args)
        {
            Matrix mat2 = Matrix.Parse("4 3 2 1;9 8 7 6;12 21 12 43;97 1 32 1");
            Console.WriteLine(mat2.Determinant);
        }
    }
}
