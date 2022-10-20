using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using sports_hub.Models;
using sports_hub.Models.Entities;
using sports_hub.Models.Entities.Navigation;
using Microsoft.AspNetCore.Authorization;
using System.Drawing;
using System.IO;
using sports_hub.Models.ViewModels;
using Microsoft.AspNetCore.Routing;
using sports_hub.Models.Entities.Comments;
using sports_hub.Services.Interfaces;
using sports_hub.Services.Implementations.AdminPage;

namespace sports_hub.Controllers
{
    public class ArticleController : Controller
    {

        private readonly ApplicationContext _applicationContext;
        private readonly IArticlesService _articles_provider;

        public ArticleController(ApplicationContext applicationContext, IArticlesService articlesService)
        {
            _applicationContext = applicationContext;
            _articles_provider = articlesService;
        }
        public IActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Comment(CommentViewModel model)
        {
            var MainComments = _applicationContext.MainComments;
            var SubComments = _applicationContext.SubComments;
            if (model.MainCommentId == 0)
            {
                MainComments.Add(new MainComment
                {
                    ArticleId = model.ArticleId,
                    Content = model.CommentText,
                    PublishDate = DateTime.Now,
                    AuthorName = model.Name,
                    AuthorSurname = model.Surname,
                    AuthorPhoto = model.Photo,
                    IsEdited = false
                });
            }
            else
            {
                SubComments.Add(new SubComment
                {
                    ArticleId = model.ArticleId,
                    MainCommentId = model.MainCommentId,
                    AuthorName = model.Name,
                    AuthorSurname = model.Surname,
                    AuthorPhoto = model.Photo,
                    Content = model.CommentText,
                    IsEdited = false,
                    PublishDate = DateTime.Now,
                });
            }
            await _applicationContext.SaveChangesAsync();
            return RedirectToAction("GetArticle", new { id = model.ArticleId });
        }

        [HttpPost]
        public async Task<JsonResult> LikeComment(string UserId, int ArticleId, int CommentId, bool isMainComment, bool IsIncrement)
        {
            if (IsIncrement)
            {
                if (isMainComment)
                {
                    var mainComment = await _applicationContext.MainComments.FindAsync(CommentId);
                    mainComment.LikesCount += 1;
                }
                else
                {
                    var subComment = await _applicationContext.SubComments.FindAsync(CommentId);
                    subComment.LikesCount += 1;
                }
            }
            else
            {
                if (isMainComment)
                {
                    var mainComment = await _applicationContext.MainComments.FindAsync(CommentId);
                    mainComment.LikesCount -= 1;
                }
                else
                {
                    var subComment = await _applicationContext.SubComments.FindAsync(CommentId);
                    subComment.LikesCount -= 1;
                }
            }
            await _applicationContext.SaveChangesAsync();
            return Json(true);
        }

        [HttpPost]
        public async Task<JsonResult> DislikeComment(string UserId, int ArticleId, int CommentId, bool isMainComment, bool IsIncrement)
        {
            if (IsIncrement)
            {
                if (isMainComment)
                {
                    var mainComment = await _applicationContext.MainComments.FindAsync(CommentId);
                    mainComment.DislikesCount += 1;
                }
                else
                {
                    var subComment = await _applicationContext.SubComments.FindAsync(CommentId);
                    subComment.DislikesCount += 1;
                }
            }
            else
            {
                if (isMainComment)
                {
                    var mainComment = await _applicationContext.MainComments.FindAsync(CommentId);
                    mainComment.DislikesCount -= 1;
                }
                else
                {
                    var subComment = await _applicationContext.SubComments.FindAsync(CommentId);
                    subComment.DislikesCount -= 1;
                }
            }
            await _applicationContext.SaveChangesAsync();
            return Json(true);
        }

        [HttpGet]
        public ActionResult GetArticle(int id, int commentsSortedBy, int visisbleCommentsCounter)
        {
            ViewBag.AvailableLanguages = _applicationContext.Languages.ToList();
            ViewBag.urlForShareButtons = Constants.ShareButtonsURL;

            var MainComments = new List<MainComment>();
            var SubComments = new List<SubComment>();
            switch (commentsSortedBy)
            {
                case 0:
                    MainComments = _applicationContext.MainComments.Where(i => i.ArticleId == id).OrderByDescending(l => l.LikesCount).ToList();
                    SubComments = _applicationContext.SubComments.Where(i => i.ArticleId == id).OrderByDescending(l => l.LikesCount).ToList();
                    break;

                case 1:
                    MainComments = _applicationContext.MainComments.Where(i => i.ArticleId == id).OrderBy(l => l.PublishDate).ToList();
                    SubComments = _applicationContext.SubComments.Where(i => i.ArticleId == id).OrderBy(l => l.PublishDate).ToList();
                    break;

                case 2:
                    MainComments = _applicationContext.MainComments.Where(i => i.ArticleId == id).OrderByDescending(l => l.PublishDate).ToList();
                    SubComments = _applicationContext.SubComments.Where(i => i.ArticleId == id).OrderByDescending(l => l.PublishDate).ToList();
                    break;
                default:
                    MainComments = _applicationContext.MainComments.Where(i => i.ArticleId == id).OrderByDescending(l => l.LikesCount).ToList();
                    SubComments = _applicationContext.SubComments.Where(i => i.ArticleId == id).OrderByDescending(l => l.LikesCount).ToList();
                    break;
            }

            ViewBag.MainComments = MainComments;
            ViewBag.SubComments = SubComments;
            ViewBag.CommentsCount = ViewBag.MainComments.Count + ViewBag.SubComments.Count;
            ViewBag.CommentsSortedBy = commentsSortedBy;
            ViewBag.VisisbleCommentsCounter = visisbleCommentsCounter;

            if (visisbleCommentsCounter <= Constants.minCommentsCount)
            {
                ViewBag.VisisbleCommentsCounter = Constants.minCommentsCount;
                ViewBag.ShowMoreButtonText = "Show more";
                ViewBag.ShowMoreButtonRotationDegree = "0deg";
            }
            if (visisbleCommentsCounter >= Constants.maxCommentsCount)
            {
                ViewBag.VisisbleCommentsCounter = Constants.maxCommentsCount;
                ViewBag.ShowMoreButtonText = "Show less";
                ViewBag.ShowMoreButtonRotationDegree = "180deg";
            }
            if (visisbleCommentsCounter >= MainComments.Count + SubComments.Count)
            {
                ViewBag.VisisbleCommentsCounter = MainComments.Count + SubComments.Count;
                ViewBag.ShowMoreButtonText = "Show less";
                ViewBag.ShowMoreButtonRotationDegree = "180deg";
            }

            try
            {
                const int MoreArticlesSideMaxNumber = 3;
                var mainArticle = _applicationContext.Articles.Where(c => c.Id == id).FirstOrDefault();

                ViewBag.ArticleId = mainArticle.Id;
                ViewBag.ArticleImage = mainArticle.Image;
                ViewBag.ArticleTitle = mainArticle.Headline;
                ViewBag.Category = mainArticle.CategoryName;
                ViewBag.Description = mainArticle.Content;
                ViewBag.MoreArticlesSideMaxNumber = MoreArticlesSideMaxNumber;
                ViewBag.AvailableLanguages = _applicationContext.Languages.ToList();
                ViewBag.ViewComments = mainArticle.Show_comments;

                var moreArticles = _applicationContext.Articles.Where(c => c.Published && c.Id != mainArticle.Id && c.CategoryName == mainArticle.CategoryName).Take(MoreArticlesSideMaxNumber * 2).ToList<Article>();
                ViewBag.MoreArticles = moreArticles;

                return View();
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpGet]
        public async Task<IActionResult> FindArticles([FromQuery] string toFind)
        {
            if(ModelState.IsValid)
            {
                var toReturn = await _articles_provider.SearchArticlesAsync(toFind);
                return Ok(toReturn);
            }
            else
            {
                return BadRequest();
            }
        }
    }
}
