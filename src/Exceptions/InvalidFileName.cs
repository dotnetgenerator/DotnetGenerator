
using System;

namespace dgen.Exceptions {

    public class InvalidFileName : Exception {

        public InvalidFileName(string message)
            : base(message)
        {
        }

        public InvalidFileName(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}