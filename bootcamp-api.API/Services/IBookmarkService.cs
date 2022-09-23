using System;
using Dto;

namespace bootcamp_api.Services
{
    public interface IBookmarkService
    {
        Bookmark[] GetAll(int user_id, int api_version);
        Bookmark Get(int id, int api_version);
        Bookmark Add(int user_id, int api_version, Bookmark bookmark);
        void Delete(int id);
        Bookmark Update(int id, int api_version, Bookmark bookmark);
    }
}