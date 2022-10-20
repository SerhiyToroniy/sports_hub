using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sports_hub.Models.ViewModels
{
    public class UserInfoViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Registered { get; set; }
        public int Teams { get; set; }
        public int PassedSurveys { get; set; }
    }
}
