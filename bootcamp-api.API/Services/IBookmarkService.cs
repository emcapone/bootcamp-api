using System;
using Dto;
using Microsoft.AspNetCore.Mvc;

namespace bootcamp_api.Services
{
    public interface IBookmarkService
    {
        Bookmark[] GetAll(ApiVersion version, int user_id, int pf_version);
        Bookmark Get(ApiVersion version, int id, int pf_version);
        Bookmark Add(ApiVersion version, int user_id, int pf_version, Bookmark bookmark);
        void Delete(int id);
        Bookmark Update(ApiVersion version, int id, int pf_version, Bookmark bookmark);
    }
}