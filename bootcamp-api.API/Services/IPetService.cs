using System;
using Dto;
using Microsoft.AspNetCore.Mvc;

namespace bootcamp_api.Services
{
    public interface IPetService
    {
        PetListItem[] GetAll(ApiVersion version, int user_id);
        Pet Get(int id);
        Pet Add(int user_id, Pet pet);
        void Delete(int id);
        Pet Update(int id, Pet pet);
    }
}