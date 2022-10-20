using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sports_hub.Models.Entities
{
    public class Section
    {
        public int Id { get; set; }
        public string SectionName { get; set; }
        public bool Visible { get; set; }
        public Section()
        {
            Visible = true;
        }
    }
}
