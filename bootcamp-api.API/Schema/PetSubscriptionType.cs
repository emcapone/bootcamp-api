using HotChocolate.Execution;
using HotChocolate.Subscriptions;
using Dto;

namespace bootcamp_api.Schema
{
    public class PetSubscriptionType: ObjectType
    {
        protected override void Configure(IObjectTypeDescriptor descriptor)
        {
            descriptor
            .Field("petAdded")
            .Type<PetType>()
            .Resolve(context => context.GetEventMessage<Pet>())
            .Subscribe(async context =>
            {
                var receiver = context.Service<ITopicEventReceiver>();

                ISourceStream stream =
                    await receiver.SubscribeAsync<string, Pet>("PetAdded");

                return stream;
            });
        }
    }
}
