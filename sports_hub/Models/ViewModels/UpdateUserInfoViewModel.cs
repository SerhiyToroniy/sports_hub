using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace sports_hub.Models.ViewModels
{
    public class UpdateUserInfoViewModel
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

        public Image ProfileImage { get; set; }
    }
}
