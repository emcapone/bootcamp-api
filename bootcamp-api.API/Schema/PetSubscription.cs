using Dto;

namespace bootcamp_api.Schema
{
    public class PetSubscription
    {
        [Subscribe]
        public Pet PetAdded([EventMessage] Pet pet)
            => pet;
    }
}
