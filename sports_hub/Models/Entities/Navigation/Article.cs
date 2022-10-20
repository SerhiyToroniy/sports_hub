using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
namespace sports_hub.Models.Entities.Navigation
{
    public class Article : ArticleLifestyleDealbook
    {
        [Required]
        public int TeamId { get; set; }
        public Team Team { get; set; }
        [Required]
        public int LocationId { get; set; }
        public Location Location { get; set; }
        public Category Category { get; set; }
        [Required]
        public string CategoryName { get; set; }
        [Required]
        public StatusType StatusMainArticle { get; set; }

        public bool ShowOnMainPage { get; set; }

        public string DisplayedContent
        {
            get
            {
                if (Content != null)
                {
                    string displayedContent = Regex.Replace(Content, "<.*?>", String.Empty);
                    displayedContent = Regex.Replace(displayedContent, "&nbsp;", String.Empty);
                    if (displayedContent.Length > 0)
                    {
                        int end = displayedContent.Length;
                        return displayedContent.Length <= 170 ? displayedContent[0..end] : displayedContent[0..168];
                    }
                }
                return "";
            }
        }
    }
}