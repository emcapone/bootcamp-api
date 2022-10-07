namespace bootcamp_api.Exceptions
{
    public class CalendarEventNotFoundException : NotFoundException
    {
        public CalendarEventNotFoundException(int id) : base("Calendar Event", "Id", id) { }
    }
}
