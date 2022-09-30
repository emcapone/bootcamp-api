using System;

namespace bootcamp_api.Exceptions
{
    public class DuplicateUserException : Exception
    {
        private readonly string _email;

        public override string Message => $"A user with the email {_email} already exists.";

        public DuplicateUserException(string email)
        {
            _email = email;
        }
    }
}
