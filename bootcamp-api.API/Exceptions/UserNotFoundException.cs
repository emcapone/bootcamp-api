namespace bootcamp_api.Exceptions
{
    public class UserNotFoundException : NotFoundException
    {
        public UserNotFoundException(int id) : base("User", "Id", id) { }
    }
}
