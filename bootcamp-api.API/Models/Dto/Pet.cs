using System;
using System.Collections.Generic;

namespace Dto
{
    public class Pet
    {
        public int Id { get; set; }
        public string Name { get; set; } = String.Empty;
        public string Breed { get; set; } = String.Empty;
        public string Color { get; set; } = String.Empty;
        public string Description { get; set; } = String.Empty;
        public string? Microchip { get; set; }
        public string Sex { get; set; } = String.Empty;
        public bool Fixed { get; set; }
        public double Weight { get; set; }
        public DateTime? Birthday { get; set; }
        public DateTime? AdoptionDay { get; set; }
        public FileLink? VetRecords { get; set; }
        public FileLink? PetPhoto { get; set; }
        public Prescription[]? Prescriptions { get; set; }
        public Vaccine[]? Vaccines { get; set; } = new Vaccine[0];
        public Condition[]? Conditions { get; set; } = new Condition[0];
        public string? Link { get; set; } = string.Empty;
    }

    public class Prescription
    {
        public string Name { get; set; } = string.Empty;
        public string Doctor { get; set; } = string.Empty;
        public DateTime Due { get; set; }
        public int Refills { get; set; }
    }

    public class Vaccine
    {
        public string Name { get; set; } = string.Empty;
        public DateTime DateAdministered { get; set; }
        public DateTime DueDate { get; set; }
    }

    public class Condition
    {
        public string Name { get; set; } = string.Empty;
        public string? Notes { get; set; } = string.Empty;
    }
}