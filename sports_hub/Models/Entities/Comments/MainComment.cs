using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sports_hub.Models.Entities.Comments
{
    public class MainComment: BaseComment
    {
        public int Id { get; set; }
        public int ArticleId { get; set; }
    }
}
