using Dto;

namespace bootcamp_api.Schema
{
    public class PetType: ObjectType<Pet>
    {
        protected override void Configure(IObjectTypeDescriptor <Pet> descriptor)
        {
            descriptor.Field(p => p.Name).Type<StringType>();

            descriptor.Field(p => p.Birthday).Type<DateTimeType>();

            descriptor.Field(p => p.Conditions).UseFiltering().UseSorting();

            descriptor.Field(p => p.Vaccines).UseFiltering().UseSorting();

            descriptor.Field(p => p.Prescriptions).UseFiltering().UseSorting();
        }
    }
}
