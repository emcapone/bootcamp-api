using Dto;

namespace bootcamp_api.Schema
{
    public class PetQueriesType : ObjectType<PetQueries>
    {
        protected override void Configure(IObjectTypeDescriptor<PetQueries> descriptor)
        {
            descriptor.Field(p => p.GetPetListItems(default!, default!)).Type<ListType<PetListItemType>>();
            descriptor.Field(p => p.GetPetById(default!, default!)).Type<PetType>();
        }
    }
}
