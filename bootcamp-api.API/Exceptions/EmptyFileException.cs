using System;

namespace bootcamp_api.Exceptions
{
    public class EmptyFileException : Exception
    {
        public override string Message => $"A non-empty file is required for file upload.";

        public EmptyFileException() { }
    }
}
