using System;

namespace bootcamp_api.Exceptions
{
    public class DuplicateUserException : Exception
    {
        private readonly string _id;

        public override string Message => $"A user with the ID {_id} already exists.";

        public DuplicateUserException(string id)
        {
            _id = id;
        }
    }
}
