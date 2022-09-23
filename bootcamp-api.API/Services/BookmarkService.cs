using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using Domain;
using bootcamp_api.Exceptions;
using Microsoft.EntityFrameworkCore;
using bootcamp_api.Data;
using AutoMapper;

namespace bootcamp_api.Services
{
    public class BookmarkService: IBookmarkService
    {

        private readonly PawssierContext _context;
        private readonly IMapper _mapper;


        public BookmarkService(PawssierContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }

        public Dto.Bookmark[] GetAll(int user_id)
        {
            var bookmarks = _context.Bookmarks.Where(b => b.User_id == user_id);
            var bookmarkAry = bookmarks.OrderBy(b => b.Id).ToArray();
            return _mapper.Map<Bookmark[], Dto.Bookmark[]>(bookmarkAry);
        }

        public Dto.Bookmark Get(int id)
        {
            var bookmark = _context.Bookmarks.SingleOrDefault(b => b.Id == id);
            if (bookmark == null)
                throw new BookmarkNotFoundException(id);

            return _mapper.Map<Bookmark, Dto.Bookmark>(bookmark);
        }

        public Dto.Bookmark Add(int user_id, Dto.Bookmark bookmark)
        {
            var dupe = _context.Bookmarks.SingleOrDefault(b => b.Petfinder_id == bookmark.Petfinder_id);
            if (dupe is not null)
                throw new DuplicateBookmarkException(bookmark.Petfinder_id);

            DateTime now = DateTime.Now;
            var newBookmark = new Bookmark
            {
                Id = bookmark.Id,
                External_url = bookmark.External_url,
                Petfinder_link = bookmark.Petfinder_link,
                Title = bookmark.Title,
                Note = bookmark.Note,
                SavedAt = bookmark.SavedAt ?? now,
                Petfinder_id = bookmark.Petfinder_id,
                DateModified = now,
                User_id = user_id
            };

            _context.Bookmarks.Add(newBookmark);
            _context.SaveChanges();

            return _mapper.Map<Bookmark, Dto.Bookmark>(newBookmark);
        }

        public void Delete(int id)
        {
            var bookmark = _context.Bookmarks.SingleOrDefault(b => b.Id == id);
            if (bookmark is null)
                throw new BookmarkNotFoundException(id);

            _context.Remove(bookmark);
            _context.SaveChanges();
        }

        public Dto.Bookmark Update(int id, Dto.Bookmark bookmark)
        {
            if (id != bookmark.Id)
                throw new Exception();

            var existingBookmark = _context.Bookmarks.SingleOrDefault(p => p.Id == id);
            if (existingBookmark == null)
                throw new BookmarkNotFoundException(id);

            existingBookmark.Note = bookmark.Note;
            existingBookmark.External_url = bookmark.External_url;
            existingBookmark.Petfinder_id = bookmark.Petfinder_id;
            existingBookmark.Petfinder_link = bookmark.Petfinder_link;
            existingBookmark.Title = bookmark.Title;
            existingBookmark.DateModified = DateTime.Now;

            _context.SaveChanges();

            return _mapper.Map<Bookmark, Dto.Bookmark>(existingBookmark);
        }
    }
}