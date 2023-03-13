using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System; 

namespace Domain
{
    public class User
    {
        [Key]
        public string Id { get; set; } = String.Empty;
        public string? PreferredFirstName { get; set; }
        public string? Username { get; set; }

        [ForeignKey("User_id")]
        public List<Pet> Pets { get; set; } = new List<Pet>();
        [ForeignKey("User_id")]
        public List<Bookmark> Bookmarks { get; set; } = new List<Bookmark>();
        [ForeignKey("User_id")]
        public List<CalendarEvent> CalendarEvents { get; set; } = new List<CalendarEvent>();
    }
}