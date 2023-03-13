using System;
using Dto;
using Microsoft.AspNetCore.Mvc;

namespace bootcamp_api.Services
{
    public interface IUserService
    {
        User Get(ApiVersion version, string id);
        User Add(ApiVersion version, User user);
        void Delete(string id);
        User Update(ApiVersion version, string id, User user);
    }
}