using System;
using System.Collections.Generic;

namespace Dto
{
	public class PetListItem
	{
		public int Id { get; set; }
		public string Name { get; set; } = String.Empty;
		public string Breed { get; set; } = String.Empty;
		public string? Microchip { get; set; } = String.Empty;
		public string Sex { get; set; } = String.Empty;
		public DateTime? Birthday { get; set; }
		public DateTime? AdoptionDay { get; set; }
		public FileLink? PetPhoto { get; set; }
		public int PrescriptionsCount { get; set; }
		public int VaccinesCount { get; set; }
		public int ConditionsCount { get; set; }
		public string Link { get; set; } = String.Empty;
	}
}