using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace sports_hub.Models.Entities
{
    public class FooterPage
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Content { get; set; }
        public string Section { get; set; }
        public bool Visible { get; set; }

        public FooterPage()
        {
            Visible = true;
        }

        public FooterPage(int id, string name, string content, string section)
        {
            Id = id;
            Name = name;
            Content = content;
            Section = section;
            Visible = true;
        }
    }
}
