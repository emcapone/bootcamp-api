namespace bootcamp_api.Exceptions
{
    public class UserNotFoundException : NotFoundException
    {
        public UserNotFoundException(string id) : base("User", "Id", id) { }
    }
}
