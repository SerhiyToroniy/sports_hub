using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace sports_hub.Models.Entities.Navigation
{
    public abstract class ArticleLifestyleDealbook
    {
        public int Id { get; set; }
        public byte[] Image { get; set; }
        [Required]
        public string Headline { get; set; }
        [Required]
        public string Caption { get; set; }
        [Required]
        public string Alt_picture_text { get; set; }
        [Required]
        public string Content { get; set; }
        public bool Published { get; set; }
        public bool Show_comments { get; set; }
        [Required]
        public DateTime DateTime { get; set; }
        public string DiscriminatorValue
        {
            get
            {
                return this.GetType().Name;
            }
        }
    }
}
