using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace sports_hub.Models.Entities
{
    public class ContactInfo
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        [Required]
        public string Adress { get; set; }
        [Required]
        public string Telephone { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
