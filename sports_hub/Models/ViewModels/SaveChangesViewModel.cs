using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using sports_hub.Models.Entities.Navigation;

namespace sports_hub.Models.ViewModels
{
    public class SaveChangesViewModel
    {
        public List<Category> Categories { get; set; }
        public List<Conference> Conferences { get; set; }
        public List<Team> Teams { get; set; }

        public List<Category> CategoriesDeleted { get; set; }
        public List<Conference> ConferencesDeleted { get; set; }
        public List<Team> TeamsDeleted { get; set; }

        public List<Category> CategoriesEdited { get; set; }
        public List<Conference> ConferencesEdited { get; set; }
        public List<Team> TeamsEdited { get; set; }
    }
}
