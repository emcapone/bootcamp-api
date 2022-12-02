using Dto;
using HotChocolate.Types.Pagination;

namespace bootcamp_api.Schema
{
    public class PetListItemType : ObjectType<PetListItem>
    {
        protected override void Configure(IObjectTypeDescriptor<PetListItem> descriptor)
        {
            descriptor.Field(p => p.Name).Type<StringType>();
            descriptor.Field(p => p.Birthday).Type<DateTimeType>();
            descriptor.Ignore(p => p.Link); //Does not currently work in Hot Chocolate v12
        }
    }
}
