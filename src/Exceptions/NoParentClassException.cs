
using System;

namespace dgen.Exceptions {

    public class NoParentClassException : Exception {

        public NoParentClassException(string message)
            : base(message)
        {
        }

        public NoParentClassException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}