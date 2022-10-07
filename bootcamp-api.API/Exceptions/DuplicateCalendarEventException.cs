using System;

namespace bootcamp_api.Exceptions
{
    public class DuplicateCalendarEventException : Exception
    {
        private readonly string _name;

        private readonly DateTime _date;

        public override string Message => $"A calendar event with the name '{_name}' already exists on {_date}.";

        public DuplicateCalendarEventException(string name, DateTime date)
        {
            _name = name;
            _date = date;
        }
    }
}