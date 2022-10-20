using Microsoft.AspNetCore.Authentication;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace sports_hub.Models.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        [Display(Name = "FIRST NAME")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "LAST NAME")]
        public string LastName { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "Please enter valid email")]
        [Display(Name = "EMAIL")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "PASSWORD")]
        public string Password { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }
    }
}
