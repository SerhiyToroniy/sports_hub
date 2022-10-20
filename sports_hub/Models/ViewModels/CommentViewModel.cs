using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace sports_hub.Models.ViewModels
{
    public class CommentViewModel
    {
        [Required]
        public int ArticleId { get; set; }
        public int MainCommentId { get; set; }
        [Required]
        public string CommentText { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Surname { get; set; }
        [Required]
        public byte[] Photo { get; set; }
        public int SortByValue { get; set; }
    }
}
