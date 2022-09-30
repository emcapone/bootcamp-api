using System.ComponentModel.DataAnnotations;
using System;

namespace Dto
{
    public class CalendarEvent
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public bool AllDay { get; set; } = false;
        public string? StartTime { get; set; } = string.Empty;
        public string? EndTime { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Details { get; set; } = string.Empty;
        public string? Link { get; set; } = string.Empty;
    }
}