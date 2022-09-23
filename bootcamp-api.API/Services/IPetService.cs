using System;

namespace bootcamp_api.Services
{
    public interface IPetService
    {
        Domain.Pet[] GetAll();
        Domain.Pet Get(int id);
        Domain.Pet Add(Dto.Pet pet);
        void Delete(int id);
        Domain.Pet Update(int id, Dto.Pet pet);
    }
}