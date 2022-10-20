using sports_hub.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using sports_hub.Models.Entities;
using Microsoft.EntityFrameworkCore;
using sports_hub.Models.Entities.Navigation;
using sports_hub.Services.Interfaces;
using sports_hub.Models.ViewModels;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace sports_hub.Services.Implementations.AdminPage
{
    public class ArticlesService : IArticlesService
    {
        private readonly ApplicationContext _context;
        private readonly IImageService _image_provider;

        public ArticlesService(ApplicationContext context, IImageService imageService)
        {
            _context = context;
            _image_provider = imageService;
        }

        public List<Article> GetArticles(string kind_of_sport, string? conference, string? team, bool? status, string? search)
        {
            var res = _context.Articles.Include(u => u.Team).ThenInclude(u => u.Conference).ThenInclude(u => u.Category).Include(u => u.Location);
            res.Where(u => u.Team.Conference.Category.Name == kind_of_sport);
            
            if(!string.IsNullOrEmpty(conference) && conference != "All conferences")
            {
                res.Where(u => u.Team.Conference.Name == conference);
            }
            
            if (!string.IsNullOrEmpty(team) && team != "All teams")
            {
                res.Where(u => u.Team.Name == team);
            }
            
            if (status.HasValue)
            {
                res.Where(u => u.Published == status);
            }
            
            if(!string.IsNullOrEmpty(search))
            {
                res.Where(u => u.Headline.Contains(search) || u.Content.Contains(search));
            }
            
            return res.ToList();
        }

        public List<Article> GetNFirstPublishedArticles(string kind_of_sport, string? conference, string? team, int n)
        {
            var res = _context.Articles.Include(u => u.Team).ThenInclude(u => u.Conference).ThenInclude(u => u.Category).Include(u => u.Location);
            res.Where(u => u.Team.Conference.Category.Name == kind_of_sport);

            if (!string.IsNullOrEmpty(conference) && conference != "All conferences")
            {
                res.Where(u => u.Team.Conference.Name == conference);
            }

            if (!string.IsNullOrEmpty(team) && team != "All teams")
            {
                res.Where(u => u.Team.Name == team);
            }

            
            res.OrderBy(x=>x.DateTime);
            return res.Where(u => u.Published == true).Take(n).ToList();
        }

        public async Task<List<ArticleLifestyleDealbook>> SearchArticlesAsync(string toFind)
        {
            var articles = await _context.ArticlesLifestyleDealbook
                .Where(v => v.Published)
                .Where(v => v.Caption.Contains(toFind) || v.Content.Contains(toFind))
                .OrderByDescending(v => v.DateTime)
                .Take(Constants.ToSearch)
                .Include(v => (v as Article).Team)
                .ThenInclude(v => v.Conference)
                .ThenInclude(v => v.Category)
                .ToListAsync();

            return articles;
        }

        public void ChangePublishStatus(int id, bool newStatus)
        {
            var a = _context.Articles.FirstOrDefault(p => p.Id == id);
            a.Published = newStatus;
            _context.Articles.Update(a);
            _context.SaveChanges();
        }

        public void DeleteArticle(int id)
        {
            var a = _context.Articles.FirstOrDefault(p => p.Id == id);
            _context.Articles.Remove(a);
            _context.SaveChanges();
        }

        public Article GetArticleById(int id)
        {
            var a = _context.Articles.FirstOrDefault(p => p.Id == id);
            return a;
        }

        public ArticleLifestyleDealbook GetLifestyleOrDealbookArticleById(int id)
        {
            var article = _context.ArticlesLifestyleDealbook.FirstOrDefault(p => p.Id == id);
            return article;
        }

        public void ChangeArticleTeam(int article_id, int id_team)
        {
            var a = _context.Articles.FirstOrDefault(p=>p.Id == article_id);
            var t = _context.Teams.FirstOrDefault(p => p.Id == id_team);
            a.Team = t;
            _context.Articles.Update(a);
            _context.SaveChanges();
        }

        public AddNewArticleViewModel CreateNewArticleViewModel(string category)
        {
            return new AddNewArticleViewModel
            {
                Conferences = _context.Conferences.Where(c => c.Category.Name == category).ToList(),
                Locations = _context.Locations.ToList(),
                Teams = _context.Teams.Where(t => t.Conference.Category.Name == category).ToList()
            };
        }

        public void AddNewLifestyleOrDealbookArticle(ArticleLifestyleDealbook article, IFormFile image)
        {
            var imageData = _image_provider.ConvertIFormFileToBytes(image);
            article.Image = imageData;

            _context.ArticlesLifestyleDealbook.Add(article);
            _context.SaveChanges();
        }

        public void AddNewArticle(Article article, string team, string location, IFormFile image)
        {
            var imageData = _image_provider.ConvertIFormFileToBytes(image);
            article.Image = imageData;
            article.LocationId = int.Parse(location);
            article.TeamId = int.Parse(team);
            article.DateTime = DateTime.Now;
            _context.Articles.Add(article);
            _context.SaveChanges();
        }

        public async Task<List<List<Article>>> GetArticlesForBreakdownAsync()
        {
            List<List<Article>> toReturn = new();
            List<BreakdownSection> breakdownSections = await _context.BreakdownSections.ToListAsync();

            foreach (var breakdownSection in breakdownSections)
            {
                if (breakdownSection.Visible)
                {
                    List<Article> articles;

                    if (breakdownSection.TeamId != null)
                    {
                        articles = await _context.Articles
                            .Where(v => v.TeamId == breakdownSection.TeamId)
                            .Take(Constants.MaxBreakDownArticles)
                            .ToListAsync();
                    }
                    else if (breakdownSection.ConferenceId != null)
                    {
                        articles = await _context.Articles
                            .Include(q => q.Team)
                            .Where(q => q.Team.ConferenceId == breakdownSection.ConferenceId)
                            .Take(Constants.MaxBreakDownArticles)
                            .ToListAsync();
                    }
                    else
                    {
                        articles = await _context.Articles
                            .Include(q => q.Team)
                            .ThenInclude(q => q.Conference)
                            .Where(q => q.Team.Conference.CategoryId == breakdownSection.CategoryId)
                            .Take(Constants.MaxBreakDownArticles)
                            .ToListAsync();
                    }

                    toReturn.Add(articles);
                }
            }

            return toReturn;
        }

        public int GetConferenceIdByTeamId(int teamId)
        {
            var team = _context.Teams.FirstOrDefault(t => t.Id == teamId);
            var conference = _context.Conferences.FirstOrDefault(c => c.Id == team.ConferenceId);
            return conference.Id;
        }

        public void UpdateArticle(string category, int articleId, string team, string location, IFormFile image, bool imageChanged, string headline, string caption, string alt_picture_text, string content, bool show_comments)
        {
            var articleEdited = GetArticleById(articleId);
            articleEdited.LocationId = int.Parse(location);
            articleEdited.TeamId = int.Parse(team);
            articleEdited.CategoryName = category;
            articleEdited.Headline = headline;
            articleEdited.Caption = caption;
            articleEdited.Alt_picture_text = alt_picture_text;
            articleEdited.Content = content;
            articleEdited.Show_comments = show_comments;

            if (imageChanged)
            {
                var imageData = _image_provider.ConvertIFormFileToBytes(image);
                articleEdited.Image = imageData;
            }

            _context.Articles.Update(articleEdited);
            _context.SaveChanges();
        }

        public void UpdateLifestyleOfDealbookArticle(string category, int articleId, IFormFile image, bool imageChanged, string headline, string caption, string alt_picture_text, string content, bool show_comments)
        {
            var articleEdited = GetLifestyleOrDealbookArticleById(articleId);
            articleEdited.Headline = headline;
            articleEdited.Caption = caption;
            articleEdited.Alt_picture_text = alt_picture_text;
            articleEdited.Content = content;
            articleEdited.Show_comments = show_comments;

            if (imageChanged)
            {
                var imageData = _image_provider.ConvertIFormFileToBytes(image);
                articleEdited.Image = imageData;
            }

            _context.ArticlesLifestyleDealbook.Update(articleEdited);
            _context.SaveChanges();
        }

        public bool isEditValid(string team, string location, string headline, string caption, string alt)
        {
            return !string.IsNullOrEmpty(team) && !string.IsNullOrEmpty(location) && !string.IsNullOrEmpty(headline) && !string.IsNullOrEmpty(caption) && !string.IsNullOrEmpty(alt);
        }

        public bool isLifestyleOrDealbookEditValid(string headline, string caption, string alt)
        {
            return !string.IsNullOrEmpty(headline) && !string.IsNullOrEmpty(caption) && !string.IsNullOrEmpty(alt);
        }

        public bool isAddNewArticleValid(string team, string location, IFormFile image)
        {
            return !string.IsNullOrEmpty(team) && !string.IsNullOrEmpty(location) && image != null;
        }

        public List<Article> GetArticlesByStatusType(StatusType st)
        {
            List<Article> result = _context.Articles.Where(u => u.StatusMainArticle == st).ToList();
            return result;
        }

        public void SaveInMainBreakdownSection(MainBreakdownArticlesModel articles)
        {
            var a = articles.Articles;
            var section = articles.Section;
            if (section == "main-articles")
            {
                foreach (var item in a)
                {
                    var db_article = _context.Articles.FirstOrDefault(u => u.Headline == item.headline);
                    db_article.ShowOnMainPage = item.status;
                    db_article.StatusMainArticle = StatusType.Saved;
                    _context.Articles.Update(db_article);
                }
                _context.SaveChanges();
            }
        }
        
        public void PublishInMainBreakdownSection()
        {
            List<Article> articles = _context.Articles.ToList();
            foreach (var article_obj in articles)
            {
                var article = _context.Articles.FirstOrDefault(u => u.Id == article_obj.Id);
                if (article.StatusMainArticle == StatusType.Published)
                {
                    article.StatusMainArticle = StatusType.None;
                }
                if (article.StatusMainArticle == StatusType.Saved)
                {
                    article.StatusMainArticle = StatusType.Published;
                }
                _context.Articles.Update(article);
            }
            _context.SaveChanges();
        }
    }
}
