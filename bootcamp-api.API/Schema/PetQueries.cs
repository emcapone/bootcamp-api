using bootcamp_api.Services;
using Dto;
using Microsoft.AspNetCore.Mvc;

namespace bootcamp_api.Schema
{
    public class PetQueries
    {
        [UsePaging(IncludeTotalCount = true)]
        [UseFiltering]
        [UseSorting]
        public List<PetListItem> GetPetListItems([Service] IPetService petService, int userId)
        {
            return petService.GetAll(new ApiVersion(1, 0), userId).ToList();
        }

        public Pet GetPetById([Service] IPetService petService, int id)
        {
            return petService.Get(new ApiVersion(1, 0), id);
        }
    }
}
