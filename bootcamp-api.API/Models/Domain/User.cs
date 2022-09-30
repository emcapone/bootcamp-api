using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System; 

namespace Domain
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string FirstName { get; set; } = string.Empty;
        [Required]
        public string LastName { get; set; } = string.Empty;
        [Required]
        public DateTime Birthday { get; set; }
        [Required]
        public string Email { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;
        [Required]
        public DateTime DateAdded { get; set; }
        [Required]
        public DateTime DateModified { get; set; }

        [ForeignKey("User_id")]
        public List<Pet> Pets { get; set; } = new List<Pet>();
        [ForeignKey("User_id")]
        public List<Bookmark> Bookmarks { get; set; } = new List<Bookmark>();
        [ForeignKey("User_id")]
        public List<CalendarEvent> CalendarEvents { get; set; } = new List<CalendarEvent>();
    }
}