using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sports_hub.Models.Entities.Navigation
{
    public class Team : BaseCategory
    {
        public Conference Conference { get; set; }
        public int ConferenceId { get; set; }
        public ICollection<Article> Articles { get; set; }
        public List<UserTeam> UserTeams { get; set; }
        public byte[] Image { get; set; }
        public Team()
        {
            UserTeams = new List<UserTeam>();
        }
    }
}
