using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using Domain;
using bootcamp_api.Exceptions;
using Microsoft.EntityFrameworkCore;
using bootcamp_api.Data;

namespace bootcamp_api.Services
{
    public class BookmarkService: IBookmarkService
    {

        private readonly PawssierContext _context;

        public BookmarkService(PawssierContext context)
        {
            _context = context;
        }

        public Bookmark[] GetAll()
        {
            var bookmarks = _context.Bookmarks;

            return bookmarks.OrderBy(b => b.Id).ToArray();
        }

        public Bookmark Get(int id)
        {
            var bookmark = _context.Bookmarks.SingleOrDefault(b => b.Id == id);
            if (bookmark == null)
                throw new BookmarkNotFoundException(id);

            return bookmark;
        }

        public Bookmark Add(Dto.Bookmark bookmark)
        {
            var dupe = _context.Bookmarks.SingleOrDefault(b => b.Petfinder_Id == bookmark.Petfinder_Id);
            if (dupe is not null)
                throw new DuplicateBookmarkException(bookmark.Petfinder_Id);

            DateTime now = DateTime.Now;
            var newBookmark = new Bookmark
            {
                Id = bookmark.Id,
                External_Url = bookmark.External_Url,
                Link = bookmark.Link,
                Title = bookmark.Title,
                Note = bookmark.Note,
                SavedAt = bookmark.SavedAt is null ? now : bookmark.SavedAt,
                Petfinder_Id = bookmark.Petfinder_Id,
                DateModified = now
            };

            _context.Bookmarks.Add(newBookmark);
            _context.SaveChanges();

            return newBookmark;
        }

        public void Delete(int id)
        {
            var bookmark = _context.Bookmarks.SingleOrDefault(b => b.Id == id);
            if (bookmark is null)
                throw new BookmarkNotFoundException(id);

            _context.Remove(bookmark);
            _context.SaveChanges();
        }

        public Bookmark Update(int id, Dto.Bookmark bookmark)
        {
            if (id != bookmark.Id)
                throw new Exception();

            var existingBookmark = _context.Bookmarks.SingleOrDefault(p => p.Id == id);
            if (existingBookmark == null)
                throw new BookmarkNotFoundException(id);

            existingBookmark.Note = bookmark.Note;
            existingBookmark.External_Url = bookmark.External_Url;
            existingBookmark.Petfinder_Id = bookmark.Petfinder_Id;
            existingBookmark.Link = bookmark.Link;
            existingBookmark.Title = bookmark.Title;
            existingBookmark.DateModified = DateTime.Now;

            _context.SaveChanges();

            return existingBookmark;
        }
    }
}