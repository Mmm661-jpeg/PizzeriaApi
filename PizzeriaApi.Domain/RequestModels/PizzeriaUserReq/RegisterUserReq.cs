﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PizzeriaApi.Domain.RequestModels.PizzeriaUserReq
{
    public class RegisterUserReq
    {
        [Required(ErrorMessage = "Username is required.")]
        [StringLength(256, ErrorMessage = "Username cannot be longer than 256 characters.")]
        public string UserName { get; set; } = null!;

        [Required(ErrorMessage = "Email is required.")]
        [StringLength(256, ErrorMessage = "Email cannot be longer than 256 characters.")]

        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Password is required.")]
        [StringLength(100, ErrorMessage = "Password must be at least {2} characters long.", MinimumLength = 6)]

        public string Password { get; set; } = null!;

        [Required(ErrorMessage = "Phonenumber is required")]
        [StringLength(15, ErrorMessage = "Phonenumber cannot be longer than 15 characters.", MinimumLength = 10)]
        public string PhoneNumber { get; set; } = null!;


    }
}
