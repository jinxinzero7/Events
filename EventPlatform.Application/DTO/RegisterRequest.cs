﻿using EventPlatform.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventPlatform.Application.DTO
{
    public class RegisterRequest
    {
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string Username { get; set; }
        public AccountType AccountType { get; set; }
    }
}
