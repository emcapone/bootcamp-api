using Microsoft.EntityFrameworkCore;
using Domain;

namespace bootcamp_api.Data
{
    public class PawssierContext : DbContext
    {

        public PawssierContext(DbContextOptions<PawssierContext> options) : base(options)
        {
        }

        public DbSet<Pet> Pets { get; set; }
        public DbSet<Prescription> Prescriptions { get; set; }
        public DbSet<Vaccine> Vaccines { get; set; }
        public DbSet<Condition> Conditions { get; set; }
        public DbSet<FileLink> FileLinks { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<Bookmark> Bookmarks { get; set; }

        public DbSet<CalendarEvent> CalendarEvents { get; set; }
    }
}