using System.ComponentModel.DataAnnotations;
using System;

namespace Dto
{
    public class Bookmark
    {
        public int Id { get; set; }
        public string Link { get; set; } = string.Empty;
        public int Petfinder_Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Note { get; set; } = string.Empty;
        public DateTime? SavedAt { get; set; }
        public string External_Url { get; set; } = string.Empty;
    }
}