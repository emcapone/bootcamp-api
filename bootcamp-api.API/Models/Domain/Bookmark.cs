using System.ComponentModel.DataAnnotations;
using System;

namespace Domain
{
    public class Bookmark
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string User_id { get; set; } = string.Empty;
        [Required]
        public int Petfinder_id { get; set; }
        [Required]
        public string Title { get; set; } = string.Empty;
        [Required]
        public string Note { get; set; } = string.Empty;
        [Required]
        public DateTime SavedAt { get; set; }
        [Required]
        public string External_url { get; set; } = string.Empty;
        [Required]
        public DateTime DateModified { get; set; }
    }
}