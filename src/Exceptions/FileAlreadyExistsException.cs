
using System;

namespace dgen.Exceptions {

    public class FileAlreadyExistsException : Exception {

        public FileAlreadyExistsException(string message)
            : base(message)
        {
        }

        public FileAlreadyExistsException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}