using System;

namespace bootcamp_api.Exceptions
{
    public class UnauthorizedException : Exception
    {
        public override string Message => $"The password and email combination do not have any matching users.";

        public UnauthorizedException() { }
    }
}
