using System;
using System.ComponentModel.DataAnnotations;

namespace Domain
{
    public class Pet
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int User_id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public string Breed { get; set; } = string.Empty;
        [Required]
        public string Color { get; set; } = string.Empty;
        [Required]
        public string Description { get; set; } = string.Empty;
        public string? Microchip { get; set; }
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
        public List<Prescription> Prescriptions { get; set; } = new List<Prescription>();
        public List<Vaccine> Vaccines { get; set; } = new List<Vaccine>();
        public List<Condition> Conditions { get; set; } = new List<Condition>();
        [Required]
        public DateTime DateAdded { get; set; }
        [Required]
        public DateTime DateModified { get; set; }
    }

    public class Prescription
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public string Doctor { get; set; } = string.Empty;
        [Required]
        public DateTime Due { get; set; }
        [Required]
        public int Refills { get; set; }
    }

    public class Vaccine
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public DateTime DateAdministered { get; set; }
        [Required]
        public DateTime DueDate { get; set; }
    }

    public class Condition
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        public string? Notes { get; set; }
    }
}