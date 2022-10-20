using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using sports_hub.Models.Entities.Navigation;

namespace sports_hub.Models.Entities
{
    public class BreakdownSection
    {
        [Key]
        public int Id { get; set; }
        public Category Category { get; set; }
        [Required]
        public int CategoryId { get; set; }
        public Conference Conference { get; set; }
        public int? ConferenceId { get; set; }
        public Team Team { get; set; }
        public int? TeamId { get; set; }
        public bool Visible { get; set; }
        public BreakdownSection()
        {
            Visible = true;
        }
    }
}
