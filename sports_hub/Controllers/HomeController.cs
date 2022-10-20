using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using sports_hub.Models;
using sports_hub.Models.Entities.Navigation;
using sports_hub.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using sports_hub.Services.Interfaces;

namespace sports_hub.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationContext _context;
        private readonly IArticlesService _articles_provider;
        private readonly IConferencesService _confs_provider;
        private readonly ITeamsService _teams_provider;
        private readonly ICategoriesService _categories_provider;

        public HomeController(ILogger<HomeController> logger, ApplicationContext context,
            IArticlesService articlesService,
            IConferencesService conferencesService,
            ITeamsService teamsService,
            ICategoriesService categoriesService)
        {
            _logger = logger;
            _context = context;
            _articles_provider = articlesService;
            _confs_provider = conferencesService;
            _categories_provider = categoriesService;
            _teams_provider = teamsService;
        }

        public async Task<IActionResult> IndexAsync()
        {
            ViewBag.AvailableLanguages = _context.Languages.ToList();
            ViewBag.urlForShareButtons = Constants.ShareButtonsURL;

            var mainArticles = _context.Articles.Where(i => i.Published && i.StatusMainArticle == StatusType.Published && i.ShowOnMainPage).ToList<Article>();
            var mainArticle = mainArticles.FirstOrDefault();

            if (mainArticle != null && mainArticles.Count > 0)
            {
                ViewBag.MainArticleImage = mainArticle.Image;
                ViewBag.MainArticles = mainArticles;
                ViewBag.MainArticle = mainArticle;
                ViewBag.MainArticleHeadline = mainArticle.Headline;
                ViewBag.MainArticleCaption = mainArticle.Caption;
                ViewBag.MainArticleId = mainArticle.Id;
                ViewBag.MainArticleCategoryName = mainArticle.CategoryName;
            }

            ViewBag.AvailableLanguages = _context.Languages.ToList();

            //Setting breakdown articles
            List<List<Article>> breakdownAtrciles = await _articles_provider.GetArticlesForBreakdownAsync();
            ViewBag.breakdownArticles = breakdownAtrciles;
            HttpContext.Session.SetInt32("LeftBarIndex", 0);
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Error()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SetLanguage(string culture, string returnUrl)
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
            );

            return LocalRedirect(returnUrl);

        }

        [Route("/TeamPortal/{Category}/{Conference}/{Team}")]
        public ActionResult TeamPortal(string? category, string? conference, string? team)
        {
            ViewBag.Conf = conference ?? "All conferences";
            ViewBag.Team = team ?? "All teams";
            ViewBag.Category = category;
            ViewBag.AditionalArticleCount = 4;
            ViewBag.Articles = _articles_provider.GetNFirstPublishedArticles(category, conference, team, 5);
            return View();
        }

        [Route("/CategoryPortal/{Category}")]
        public ActionResult CategoryPortal(string? category)
        {
            ViewBag.Category = category;
            int categoryPosition = _categories_provider.FindCategory(category).Order;
            HttpContext.Session.SetInt32("LeftBarIndex", categoryPosition);
            ViewBag.AditionalArticleCount = 4;
            ViewBag.Articles = _articles_provider.GetNFirstPublishedArticles(category, null, null, 5);
            return View("TeamPortal");
        }
    }
}