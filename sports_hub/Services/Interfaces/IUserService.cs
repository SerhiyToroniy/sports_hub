using PagedList;
using sports_hub.Models.Entities;
using sports_hub.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sports_hub.Services.Interfaces
{
    public interface IUserService
    {
        public List<User> GetUsers(string role, int page_number, int page_size);

        public UserInfoViewModel GetUserInfo(string id);
    }
}
