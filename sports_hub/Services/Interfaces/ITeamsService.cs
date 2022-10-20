using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using sports_hub.Models.Entities.Navigation;

namespace sports_hub.Services.Interfaces
{
    public interface ITeamsService
    {
        public List<Team> GetTeams(string? conference);

        public Team GetTeamById(int id);

        public  Task<List<Team>> GetTeamsAsync();
        
        public List<Team> GetUserTeams(string username);
        
        public List<Team> GetTeamsBySearch(string search);
        
        public Team GetTeamByName(string name);

        public void AddTeam(string user_name, string team_name);

        public void DeleteUserTeam(string user_name, string team_name);
        
    }
}
