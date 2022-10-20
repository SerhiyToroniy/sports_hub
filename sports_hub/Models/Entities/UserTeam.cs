using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using sports_hub.Models.Entities.Navigation;

namespace sports_hub.Models.Entities
{
    public class UserTeam
    {
        public string UserId { get; set; }
        public User User { get; set; }

        public int TeamId { get; set; }
        public Team Team { get; set; }
    }
}
