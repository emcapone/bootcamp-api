using bootcamp_api.Services;
using Microsoft.AspNetCore.Mvc;
using Dto;
using HotChocolate.Subscriptions;

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

        public async Task<Pet> AddPet([Service] ITopicEventSender sender, [Service] IPetService petService, int userId, Pet pet)
        {
            var newPet = petService.Add(new ApiVersion(1, 0), userId, pet);
            await sender.SendAsync(nameof(PetSubscription.PetAdded), newPet);
            return newPet;
        }

        public Pet UpdatePet([Service] IPetService petService, int petId, Pet pet)
        {
            return petService.Update(new ApiVersion(1, 0), petId, pet);
        }
    }
}
