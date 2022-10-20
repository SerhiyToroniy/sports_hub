using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using sports_hub.Models.Entities;
using sports_hub.Models.Entities.Navigation;
using sports_hub.Models.ViewModels;

namespace sports_hub.Services.Interfaces
{
    public interface IArticlesService
    {
        public List<Article> GetArticles(string kind_of_sport, string? conference, string? team, bool? status, string? search);

        public void ChangePublishStatus(int id, bool newStatus);

        public void DeleteArticle(int id);

        public Article GetArticleById(int id);

        public void ChangeArticleTeam(int article_id, int id_team);

        public AddNewArticleViewModel CreateNewArticleViewModel(string Category);

        public List<Article> GetArticlesByStatusType(StatusType st);

        public void SaveInMainBreakdownSection(MainBreakdownArticlesModel articles);

        public void PublishInMainBreakdownSection();

        public void AddNewLifestyleOrDealbookArticle(ArticleLifestyleDealbook article, IFormFile image);

        public void AddNewArticle(Article article, string team, string location, IFormFile image);

        public Task<List<List<Article>>> GetArticlesForBreakdownAsync();

        public int GetConferenceIdByTeamId(int teamId);

        public void UpdateArticle(string category, int articleId, string team, string location, IFormFile image, bool imageChanged, string headline, string caption, string alt_picture_text, string content, bool show_comments);

        public void UpdateLifestyleOfDealbookArticle(string category, int articleId, IFormFile image, bool imageChanged, string headline, string caption, string alt_picture_text, string content, bool show_comments);

        public bool isEditValid(string team, string location, string headline, string caption, string alt);

        public bool isLifestyleOrDealbookEditValid(string headline, string caption, string alt);

        public bool isAddNewArticleValid(string team, string location, IFormFile image);

        public ArticleLifestyleDealbook GetLifestyleOrDealbookArticleById(int id);

        public List<Article> GetNFirstPublishedArticles(string kind_of_sport, string? conference, string? team, int n);

        public Task<List<ArticleLifestyleDealbook>> SearchArticlesAsync(string toFind);
    }
}
