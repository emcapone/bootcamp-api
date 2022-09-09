using System.ComponentModel.DataAnnotations;

namespace bootcamp_api.Models;

public record Pet
{
    public int Id { get; set; }
    [Required]
    public string Name { get; set; } = string.Empty;
    [Required]
    public string Breed { get; set; } = string.Empty;
    [Required]
    public string Color { get; set; } = string.Empty;
    [Required]
    public string Description { get; set; } = string.Empty;
    public string Microchip { get; set; } = string.Empty;
    [Required]
    public string Sex { get; set; } = string.Empty;
    [Required]
    public bool Fixed { get; set; }
    [Required]
    public double Weight { get; set; }
    public DateTime? Birthday { get; set; }
    public DateTime? AdoptionDay { get; set; }
    public FileLink? VetRecords { get; set; }
    public FileLink? PetPhoto { get; set; }
    public Prescription[]? Prescriptions { get; set; }
    public Vaccine[]? Vaccines { get; set; }
    public Condition[]? Conditions { get; set; }
}

public record Prescription
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Doctor { get; set; } = string.Empty;
    public DateTime Due { get; set; }
    public int Refills { get; set; }
}

public record Vaccine
{
    public int Id { get; set; }
    [Required]
    public string Name { get; set; } = string.Empty;
    [Required]
    public DateTime DateAdministered { get; set; }
    [Required]
    public DateTime DueDate { get; set; }
}

public record Condition
{
    public int Id { get; set; }
    [Required]
    public string Name { get; set; } = string.Empty;
    public string Notes { get; set; } = string.Empty;
}

public record FileLink
{
    public string DbPath { get; set; } = string.Empty;
}