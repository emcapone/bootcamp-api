using bootcamp_api.Exceptions;
using Microsoft.AspNetCore.Cors;

namespace bootcamp_api.Schema
{
    public class PetMutationType: ObjectType<PetMutations>
    {
        protected override void Configure(IObjectTypeDescriptor<PetMutations> descriptor)
        {
            descriptor.Field(f => f.DeletePet(default!, default!)).Error<PetNotFoundException>();
            descriptor.Field(f => f.AddPet(default!, default!, default!, default!));
            descriptor.Field(f => f.UpdatePet(default!, default!, default!)).Error<PetNotFoundException>();
        }
    }
}
