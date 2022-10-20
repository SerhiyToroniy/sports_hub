using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace sports_hub.Models.Entities.Comments
{
    public class RatingCount
    {
        [Key]
        public int Id { get; set; }

        public User User { get; set; }
        public BaseComment BaseComment { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }

        [ForeignKey("MainComment")]
        public string BaseCommentId { get; set; }
    }
}
