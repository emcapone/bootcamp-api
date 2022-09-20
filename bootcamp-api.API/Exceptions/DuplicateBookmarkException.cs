using System;

namespace bootcamp_api.Exceptions
{
    public class DuplicateBookmarkException : Exception
    {
        private readonly long _id;

        public override string Message => $"A bookmark with the Petfinder id '{_id}' already exists.";

        public DuplicateBookmarkException(long id)
        {
            _id = id;
        }
    }
}
