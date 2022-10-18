using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;

namespace Domain
{
    public class Message
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Category { get; set; } = "other";
        [Required]
        public string Subject { get; set; } = string.Empty;
        [Required]
        public string Body { get; set; } = string.Empty;
        [Required]
        public string Email { get; set; } = string.Empty;
        [Required]
        public DateTime DateSubmitted { get; set; }
        [Required]
        public bool Resolved { get; set; } = false;
        public DateTime? DateResolved { get; set; }
    }
}