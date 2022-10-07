using System;
using Dto;
using Microsoft.AspNetCore.Mvc;

namespace bootcamp_api.Services
{
    public interface IMessageService
    {
        Message Add(Message message);
    }
}