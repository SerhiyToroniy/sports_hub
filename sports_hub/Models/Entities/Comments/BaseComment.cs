using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sports_hub.Models.Entities.Comments
{
    public class BaseComment
    {
        //public int Id { get; set; }
        public string AuthorName { get; set; }

        public string AuthorSurname { get; set; }

        public byte[] AuthorPhoto { get; set; }

        public bool IsEdited { get; set; }

        public DateTime PublishDate { get; set; }

        public string Content { get; set; }

        public int LikesCount { get; set; }

        public int DislikesCount { get; set; }
    }
}
