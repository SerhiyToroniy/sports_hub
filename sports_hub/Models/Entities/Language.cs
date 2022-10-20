using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sports_hub.Models.Entities
{
    public class Language
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Visible { get; set; }
        public Language()
        {
            Visible = true;
        }
        public Language(string name)
        {
            Name = name;
            Visible = true;
        }
    }
}
