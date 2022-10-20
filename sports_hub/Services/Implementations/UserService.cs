using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PagedList;
using sports_hub.Models;
using sports_hub.Models.Entities;
using sports_hub.Models.ViewModels;
using sports_hub.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sports_hub.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly ApplicationContext _context;

        public UserService(ApplicationContext context)
        {
            _context = context;
        }

        public List<User> GetUsers(string role, int page_number, int page_size)
        {
            var users = _context.Users.Where(u => u.Role == role).Skip(page_size * (page_number - 1)).Take(page_size).ToList();
            return users;
        }

        public UserInfoViewModel GetUserInfo(string id)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == id);
            UserInfoViewModel result = new UserInfoViewModel();
            result.FirstName = user.FirstName;
            result.LastName = user.LastName;
            result.Registered = user.RegistrationDate.ToShortDateString();
            var users = _context.Users.Include(c => c.UserTeams)
                            .ThenInclude(sc => sc.Team).Where(c => c.Id == id)
                            .ToList();
            result.Teams = users[0].UserTeams.Count;
            result.PassedSurveys = 0;
            return result;
        }
    }
}
