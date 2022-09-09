using bootcamp_api.Models;

namespace bootcamp_api.Services;

public static class PetService
{
    static List<Pet> Pets { get; }
    static int nextId = 3;
    static PetService()
    {
        FileLink VetFile = new FileLink { DbPath = "Resources\\Users\\1\\1\\Vet%20Records\\vet-records.pdf" };
        FileLink PhotoFile = new FileLink { DbPath = "Resources\\Users\\1\\1\\Pet%20Photo\\cat.jpg" };
        Vaccine[] Vaccine = new Vaccine[] { new Vaccine{ Id = 1, Name = "Rabies", DueDate = DateTime.Now } };
        Pets = new List<Pet>
            {
                new Pet { Id = 1, Name = "Cinnamon", Breed = "Rabbit", Color = "Brown",
                    Description = "Funny guy", Microchip = "12345", Sex = "Female", Fixed = true,
                    Weight = 3.1, Birthday = DateTime.Now, AdoptionDay = DateTime.Now, VetRecords = VetFile, PetPhoto = PhotoFile },
                new Pet { Id = 2, Name = "Fruit Loop", Breed = "Rabbit", Color = "Brown",
                    Description = "Funny guy", Microchip = "12345", Sex = "Female", Fixed = true,
                    Weight = 3.1, Birthday = DateTime.Now, AdoptionDay = DateTime.Now,
                    Vaccines = Vaccine }
            };
    }

    public static List<Pet> GetAll() => Pets;

    public static Pet? Get(int id) => Pets.FirstOrDefault(p => p.Id == id);

    public static void Add(Pet pet)
    {
        pet.Id = nextId++;
        Pets.Add(pet);
    }

    public static void Delete(int id)
    {
        var pet = Get(id);
        if (pet is null)
            return;

        Pets.Remove(pet);
    }

    public static void Update(Pet pet)
    {
        var index = Pets.FindIndex(p => p.Id == pet.Id);
        if (index == -1)
            return;

        Pets[index] = pet;
    }
}