namespace bootcamp_api.Exceptions
{
	public class BookmarkNotFoundException : NotFoundException
	{
		public BookmarkNotFoundException(int id) : base("Bookmark", "Id", id) { }
	}
}
