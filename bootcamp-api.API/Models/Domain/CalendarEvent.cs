using System.ComponentModel.DataAnnotations;
using System;

namespace Domain
{
    public class CalendarEvent
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public DateTime Date { get; set; }
        [Required]
        public bool AllDay { get; set; } = false;
        public string? StartTime { get; set; } = string.Empty;
        public string? EndTime { get; set; } = string.Empty;
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public string Details { get; set; } = string.Empty;
        [Required]
        public DateTime DateAdded { get; set; }
        [Required]
        public DateTime DateModified { get; set; }
        [Required]
        public string User_id { get; set; } = string.Empty;
    }
}