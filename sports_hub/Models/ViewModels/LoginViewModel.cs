using Microsoft.AspNetCore.Authentication;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace sports_hub.Models.ViewModels
{
    public class LoginViewModel
    {

        [Required]
        [EmailAddress(ErrorMessage = "Please enter valid email")]
        [Display(Name = "EMAIL")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "PASSWORD")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public IList<AuthenticationScheme> ExternalLogins { get; set; }

    }
}
