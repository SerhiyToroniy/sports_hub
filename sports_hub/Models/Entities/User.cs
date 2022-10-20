using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using sports_hub.Models.Entities.Navigation;

namespace sports_hub.Models.Entities
{
    public class User : IdentityUser
    {
        [Required]
        [Display(Name = "FIRST NAME")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "LAST NAME")]
        public string LastName { get; set; }

        [Display(Name = "PROFILE IMAGE")]
        public byte[] ProfileImage { get; set; }
        public List<UserTeam> UserTeams { get; set; }
        public UserStatus Status { get; set; }
        public string Role { get; set; }
        public DateTime RegistrationDate { get; set; }

        public User()
        {
            UserTeams = new List<UserTeam>();
        }
    }
}
