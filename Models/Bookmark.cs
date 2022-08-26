using System.ComponentModel.DataAnnotations;

namespace bootcamp_api.Models;

public record Bookmark
{
    public int Id { get; set; }
    [Required]
    public string Link { get; set; } = string.Empty;
    [Required]
    public int Petfinder_Id { get; set; }
    [Required]
    public string Title { get; set; } = string.Empty;
    [Required]
    public string Note { get; set; } = string.Empty;
    [Required]
    public DateTime SavedAt { get; set; }
    [Required]
    public string External_Url { get; set; } = string.Empty;
}