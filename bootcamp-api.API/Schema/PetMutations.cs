using bootcamp_api.Services;
using Microsoft.AspNetCore.Mvc;
using Dto;

namespace bootcamp_api.Schema
{
    public class PetMutations
    {
        [UseMutationConvention(Disable = true)]
        public bool DeletePet([Service] IPetService petService, int id)
        {
            petService.Delete(id);
            return true;
        }

        public Pet AddPet([Service] IPetService petService, int userId, Pet pet)
        {
            return petService.Add(new ApiVersion(1, 0), userId, pet);
        }

        public Pet UpdatePet([Service] IPetService petService, int petId, Pet pet)
        {
            return petService.Update(new ApiVersion(1, 0), petId, pet);
        }
    }
}
