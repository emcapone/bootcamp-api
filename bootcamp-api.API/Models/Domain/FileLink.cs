using System.ComponentModel.DataAnnotations;

namespace Domain
{
    public class FileLink
    {
        public int Id { get; set; }
        [Required]
        public string DbPath { get; set; } = string.Empty;
    }
}