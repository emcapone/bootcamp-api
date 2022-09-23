using System.ComponentModel.DataAnnotations;
using System;

namespace Dto
{
    public class Bookmark
    {
        public int Id { get; set; }
        public string Petfinder_link { get; set; } = string.Empty;
        public int Petfinder_id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Note { get; set; } = string.Empty;
        public DateTime? SavedAt { get; set; }
        public string External_url { get; set; } = string.Empty;
    }
}