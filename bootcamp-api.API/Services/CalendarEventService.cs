using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Drawing;
using System.Linq;
using System.Xml.Linq;
using Domain;
using bootcamp_api.Data;
using bootcamp_api.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;

namespace bootcamp_api.Services
{
    public class CalendarEventService : ICalendarEventService
    {

        private readonly PawssierContext _context;
        private readonly IMapper _mapper;

        public CalendarEventService(PawssierContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public Dto.CalendarEvent[] Get(ApiVersion version, int user_id, int month = 0, int year = 0)
        {
            var calendarEvents = new CalendarEvent[0];

            if (month < 0 || month > 12 || year < 0 || (month != 0 && year == 0))
                throw new Exception();
            else if (month == 0 && year == 0)
                calendarEvents = _context.CalendarEvents.Where(e => e.User_id == user_id).ToArray();
            else if (month == 0 && year != 0)
                calendarEvents = _context.CalendarEvents.Where(e => (e.User_id == user_id && e.Date.Year == year)).ToArray();
            else if (month != 0 && year != 0)
                calendarEvents = _context.CalendarEvents.Where(e => (e.User_id == user_id && e.Date.Month == month && e.Date.Year == year)).ToArray();
            else
                throw new Exception();

            var dtos = _mapper.Map<CalendarEvent[], Dto.CalendarEvent[]>(calendarEvents);
            foreach (Dto.CalendarEvent x in dtos)
            {
                x.Link = LinkService.GenerateCalendarEventsLink(version, x.Id);
            }
            return dtos;
        }

        public Dto.CalendarEvent GetById(ApiVersion version, int id)
        {
            var calendarEvent = _context.CalendarEvents.SingleOrDefault(e => e.Id == id);
            if (calendarEvent == null)
                throw new CalendarEventNotFoundException(id);

            var dto = _mapper.Map<CalendarEvent, Dto.CalendarEvent>(calendarEvent);
            dto.Link = LinkService.GenerateCalendarEventsLink(version, dto.Id);
            return dto;
        }

        public Dto.CalendarEvent Add(ApiVersion version, int user_id, Dto.CalendarEvent calendarEvent)
        {
            var dupe = _context.CalendarEvents.SingleOrDefault(e => (e.Date == calendarEvent.Date && e.Name == calendarEvent.Name));
            if (dupe is not null)
                throw new DuplicateCalendarEventException(calendarEvent.Name, calendarEvent.Date);

            DateTime now = DateTime.Now;
            var newCalendarEvent = new CalendarEvent
            {
                Date = calendarEvent.Date,
                AllDay = calendarEvent.AllDay,
                StartTime = calendarEvent.StartTime,
                EndTime = calendarEvent.EndTime,
                Name = calendarEvent.Name,
                Details = calendarEvent.Details,
                DateModified = now,
                DateAdded = now,
                User_id = user_id
            };

            _context.CalendarEvents.Add(newCalendarEvent);
            _context.SaveChanges();

            var dto = _mapper.Map<CalendarEvent, Dto.CalendarEvent>(newCalendarEvent);
            dto.Link = LinkService.GenerateCalendarEventsLink(version, dto.Id);
            return dto;
        }

        public void Delete(int id)
        {
            var calendarEvent = _context.CalendarEvents.SingleOrDefault(e => e.Id == id);
            if (calendarEvent == null)
                throw new CalendarEventNotFoundException(id);

            _context.Remove(calendarEvent);
            _context.SaveChanges();
        }

        public Dto.CalendarEvent Update(ApiVersion version, int id, Dto.CalendarEvent updatedEvent)
        {
            if (id != updatedEvent.Id)
                throw new Exception();

            var calendarEvent = _context.CalendarEvents.SingleOrDefault(e => e.Id == id);
            if (calendarEvent == null)
                throw new CalendarEventNotFoundException(id);

            calendarEvent.Date = updatedEvent.Date;
            calendarEvent.AllDay = updatedEvent.AllDay;
            calendarEvent.StartTime = updatedEvent.StartTime;
            calendarEvent.EndTime = updatedEvent.EndTime;
            calendarEvent.Name = updatedEvent.Name;
            calendarEvent.Details = updatedEvent.Details;
            calendarEvent.DateModified = DateTime.Now;

            _context.SaveChanges();

            var dto = _mapper.Map<CalendarEvent, Dto.CalendarEvent>(calendarEvent);
            dto.Link = LinkService.GenerateCalendarEventsLink(version, dto.Id);
            return dto;
        }


    }
}