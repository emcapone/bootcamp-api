namespace bootcamp_api.Exceptions
{
	public class PetNotFoundException : NotFoundException
	{
		public PetNotFoundException(int id) : base("Pet", "Id", id) { }
	}
}
