using System;
using Dto;
using Microsoft.AspNetCore.Mvc;

namespace bootcamp_api.Services
{
    public interface ICalendarEventService
    {
        CalendarEvent[] Get(ApiVersion version, int user_id, int month = 0, int year = 0);
        CalendarEvent GetById(ApiVersion version, int id);
        CalendarEvent Add(ApiVersion version, int user_id, CalendarEvent calendarEvent);
        void Delete(int id);
        CalendarEvent Update(ApiVersion version, int id, CalendarEvent calendarEvent);
    }
}