using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sports_hub.Models.Entities.Navigation
{
    public class Location
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Article> Articles { get; set; } = new List<Article>();
    }
}
