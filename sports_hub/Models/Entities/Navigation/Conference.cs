using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sports_hub.Models.Entities.Navigation
{
    public class Conference : BaseCategory
    {
        public Category Category { get; set; }
        public int CategoryId { get; set; }
        public ICollection<Team> Teams { get; set; }
    }
}
