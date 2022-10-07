using System.ComponentModel.DataAnnotations;
using System;

namespace Dto
{
    public class Message
    {
        public int Id { get; set; }
        public string Category { get; set; } = "other";
        public string Subject { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }
}