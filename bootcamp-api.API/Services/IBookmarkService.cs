using System;

namespace bootcamp_api.Services
{
    public interface IBookmarkService
    {
        Domain.Bookmark[] GetAll();
        Domain.Bookmark Get(int id);
        Domain.Bookmark Add(Dto.Bookmark bookmark);
        void Delete(int id);
        Domain.Bookmark Update(int id, Dto.Bookmark bookmark);
    }
}