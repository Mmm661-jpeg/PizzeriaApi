using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PizzeriaApi.Domain.RequestModels
{
    public class UpdateUserReq
    {
        [Required(ErrorMessage = "UserId is required.")]
        public string UserId { get; set; } = null!;

        [StringLength(256, ErrorMessage = "Username cannot be longer than 256 characters.")]
        public string? UserName { get; set; }

        [StringLength(256, ErrorMessage = "Username cannot be longer than 256 characters.")]

        public string? Email { get; set; }

        [StringLength(15, ErrorMessage = "Phonenumber cannot be longer than 15 characters.", MinimumLength = 10)]

        public string? PhoneNumber { get; set; }

        [StringLength(100, ErrorMessage = "Password must be at least {2} characters long.", MinimumLength = 6)]

        public string? Password { get; set; }
    }
}
