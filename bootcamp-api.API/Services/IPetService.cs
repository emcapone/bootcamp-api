using System;
using Dto;
using Microsoft.AspNetCore.Mvc;

namespace bootcamp_api.Services
{
    public interface IPetService
    {
        PetListItem[] GetAll(ApiVersion version, int user_id);
        Pet Get(ApiVersion version, int id);
        Pet Add(ApiVersion version, int user_id, Pet pet);
        void Delete(int id);
        Pet Update(ApiVersion version, int id, Pet pet);
    }
}