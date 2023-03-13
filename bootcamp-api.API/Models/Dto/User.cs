using System.ComponentModel.DataAnnotations;
using System;

namespace Dto
{
    public class User
    {
        public string Id { get; set; } = string.Empty;
        public string? PreferredFirstName { get; set; }
        public string? Username { get; set; }
        public string? Link { get; set; } = string.Empty;
    }
}