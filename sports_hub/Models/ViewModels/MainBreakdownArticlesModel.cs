using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sports_hub.Models.ViewModels
{
    public class MainBreakdownArticlesModel
    {
        public List<ArticleViewModel> Articles { get; set; }

        public string Section { get; set; }
    }
}
