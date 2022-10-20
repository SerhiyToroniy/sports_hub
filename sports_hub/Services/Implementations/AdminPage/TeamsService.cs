using Microsoft.EntityFrameworkCore;
using sports_hub.Models;
using sports_hub.Models.Entities.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using sports_hub.Services.Interfaces;
using sports_hub.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;

namespace sports_hub.Services.Implementations.AdminPage
{
    public class TeamsService : ITeamsService
    {
        private readonly ApplicationContext _context;

        public TeamsService(ApplicationContext context, UserManager<User> userManager)
        {
            _context = context;
        }

        public List<Team> GetTeams(string? conference)
        {
            List<Team> result = new List<Team>();

            if (conference == "All conferences" || string.IsNullOrEmpty(conference))
            {
                result = _context.Teams.Include(u => u.Conference).ToList();
            }
            else
            {
                result = _context.Teams.Include(u => u.Conference).Where(p => p.Conference.Name == conference).ToList();
            }

            return result;
        }

        public Team GetTeamById(int id)
        {
            List<Team> list_of_teams = _context.Teams.Include(c => c.Conference).ThenInclude(c => c.Category).ToList();
            Team result = list_of_teams.FirstOrDefault(predicate => predicate.Id == id);
            return result;
        }

        public Team GetTeamByName(string name)
        {
            Team result = _context.Teams.FirstOrDefault(t => t.Name == name);
            return result;
        }

        public async Task<List<Team>> GetTeamsAsync()
        {
            List<Team> result = await _context.Teams.ToListAsync();
            return result;
        }

        public List<Team> GetUserTeams(string username)
        {
            var u = _context.Users.FirstOrDefault(p => p.UserName == username);
            var users = _context.Users.Include(c => c.UserTeams)
                            .ThenInclude(sc => sc.Team).Where(c => c.UserName == u.UserName)
                            .ToList();
            List<Team> res = new List<Team>();
            foreach (UserTeam sc in users[0].UserTeams)
            {
                res.Add(sc.Team);
            }
            return res;
        }

        public List<Team> GetTeamsBySearch(string search)
        {
            var res = _context.Teams.Where(u => u.Name.StartsWith(search)).ToList();
            return res;
        }

        public void AddTeam(string user_name, string team_name)
        {
            Team team = _context.Teams.FirstOrDefault(t => t.Name == team_name);
            User user = _context.Users.Include(c => c.UserTeams).FirstOrDefault(u => u.UserName == user_name);
            UserTeam u_team = new UserTeam{ Team = team, User = user };
            user.UserTeams.Add(u_team);
            _context.Users.Update(user);
            _context.SaveChanges();
        }

        public void DeleteUserTeam(string user_name, string team_name)
        {
            Team team = _context.Teams.FirstOrDefault(t => t.Name == team_name);
            User user = _context.Users.Include(c => c.UserTeams).FirstOrDefault(u => u.UserName == user_name);
            var user_teams = user.UserTeams;
            foreach(var userteam in user_teams)
            {
                string current_team = _context.Teams.FirstOrDefault(u => u.Id == userteam.TeamId).Name;
                if(current_team == team_name)
                {
                    user.UserTeams.Remove(userteam);
                    _context.Users.Update(user);
                    _context.SaveChanges();
                    break;
                }
            }
        }
    }
}
