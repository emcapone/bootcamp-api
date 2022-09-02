using bootcamp_api.Models;

namespace bootcamp_api.Services;

public static class PetService
{
    static List<Pet> Pets { get; }
    static int nextId = 3;
    static PetService()
    {
        FileLink File = new FileLink { DbLink = "example/folder/file.jpg" };
        Vaccination[] Vaccine = new Vaccination[] { new Vaccination{ Id = 1, Name = "Rabies", DueDate = DateTime.Now } };
        Pets = new List<Pet>
            {
                new Pet { Id = 1, Name = "Cinnamon", Breed = "Rabbit", Color = "Brown", Coat = "Short",
                    Description = "Funny guy", Microchip = "12345", Sex = "Female", Fixed = true,
                    Weight = 3.1, Birthday = DateTime.Now, AdoptionDay = DateTime.Now, PetPhoto = File },
                new Pet { Id = 2, Name = "Fruit Loop", Breed = "Rabbit", Color = "Brown", Coat = "Short",
                    Description = "Funny guy", Microchip = "12345", Sex = "Female", Fixed = true,
                    Weight = 3.1, Birthday = DateTime.Now, AdoptionDay = DateTime.Now,
                    Vaccinations = Vaccine }
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