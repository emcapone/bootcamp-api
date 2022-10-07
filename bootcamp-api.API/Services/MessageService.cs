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
    public class MessageService : IMessageService
    {

        private readonly PawssierContext _context;
        private readonly IMapper _mapper;

        public MessageService(PawssierContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public Dto.Message Add(Dto.Message message)
        {
            var newMessage = new Message
            {
                Category = message.Category,
                Subject = message.Subject,
                Body = message.Body,
                Email = message.Email,
                DateSubmitted = DateTime.Now,
                Resolved = false
            };

            _context.Messages.Add(newMessage);
            _context.SaveChanges();

            var dto = _mapper.Map<Message, Dto.Message>(newMessage);
            return dto;
        }

    }
}