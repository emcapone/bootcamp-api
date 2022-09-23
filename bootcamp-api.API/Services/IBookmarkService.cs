using System;
using Dto;

namespace bootcamp_api.Services
{
    public interface IBookmarkService
    {
        Bookmark[] GetAll(int user_id);
        Bookmark Get(int id);
        Bookmark Add(int user_id, Bookmark bookmark);
        void Delete(int id);
        Bookmark Update(int id, Bookmark bookmark);
    }
}