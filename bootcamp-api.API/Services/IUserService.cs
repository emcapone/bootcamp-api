using System;
using Dto;
using Microsoft.AspNetCore.Mvc;

namespace bootcamp_api.Services
{
    public interface IUserService
    {
        User Authenticate(ApiVersion version, Credentials credentials);
        User Get(ApiVersion version, int id);
        User Add(ApiVersion version, User user);
        void Delete(int id);
        User Update(ApiVersion version, int id, User user);
    }
}