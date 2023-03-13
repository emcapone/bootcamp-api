using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Drawing;
using System.Linq;
using System.Xml.Linq;
using Domain;
using bootcamp_api.Data;
using bootcamp_api.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;

namespace bootcamp_api.Services
{
    public class UserService : IUserService
    {

        private readonly PawssierContext _context;
        private readonly IMapper _mapper;

        public UserService(PawssierContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public Dto.User Get(ApiVersion version, string id)
        {
            var user = _context.Users.SingleOrDefault(u => u.Id == id);
            if (user == null)
                throw new UserNotFoundException(id);

            var dto = _mapper.Map<User, Dto.User>(user);
            dto.Link = LinkService.GenerateUserLink(version, dto.Id);
            return dto;
        }

        public Dto.User Add(ApiVersion version, Dto.User user)
        {
            var dupe = _context.Users.SingleOrDefault(u => u.Id == user.Id);
            if (dupe is not null)
                throw new DuplicateUserException(dupe.Id);

            var newUser = new User
            {
                Id = user.Id,
                PreferredFirstName= user.PreferredFirstName,
                Username= user.Username
            };

            _context.Users.Add(newUser);
            _context.SaveChanges();

            var dto = _mapper.Map<User, Dto.User>(newUser);
            dto.Link = LinkService.GenerateUserLink(version, dto.Id);
            return dto;
        }

        public void Delete(string id)
        {
            var user = _context.Users.SingleOrDefault(u => u.Id == id);
            if (user == null)
                throw new UserNotFoundException(id);

            _context.Remove(user);
            _context.SaveChanges();
        }

        public Dto.User Update(ApiVersion version, string id, Dto.User updatedUser)
        {
            if (id != updatedUser.Id)
                throw new Exception();

            var currentUser = _context.Users.SingleOrDefault(u => u.Id == id);
            if (currentUser == null)
                throw new UserNotFoundException(id);

            currentUser.PreferredFirstName = updatedUser.PreferredFirstName;
            currentUser.Username = updatedUser.Username;

            _context.SaveChanges();

            var dto = _mapper.Map<User, Dto.User>(currentUser);
            dto.Link = LinkService.GenerateUserLink(version, dto.Id);
            return dto;
        }
    }
}