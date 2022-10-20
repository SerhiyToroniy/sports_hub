using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sports_hub.Models.ViewModels
{
    public class AddNewArticleViewModel
    {
        public IEnumerable<Models.Entities.Navigation.Location> Locations { get; set; }
        public IEnumerable<Models.Entities.Navigation.Conference> Conferences { get; set; }
        public IEnumerable<Models.Entities.Navigation.Team> Teams { get; set; }
    }
}
