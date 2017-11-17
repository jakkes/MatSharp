using System;

namespace MatSharp {
    public class NoSolutionException : Exception {
        public NoSolutionException() {}
        public NoSolutionException(string message) : base(message) {}
    }
}