using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using sports_hub.Models;
using sports_hub.Models.Entities;
using Microsoft.EntityFrameworkCore;
using sports_hub.Services.Implementations.AdminPage;
using sports_hub.Services.Interfaces;
using sports_hub.Models.Entities.Navigation;
using Microsoft.AspNetCore.Http;
using System.IO;
using sports_hub.Services.Implementations;
using System.Dynamic;
using sports_hub.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using PagedList;
using PagedList.Mvc;
using Microsoft.AspNetCore.Identity;

namespace sports_hub.Controllers
{

    [Authorize(Roles = "admin")]
    public class AdminPageController : Controller
    {
        private readonly ApplicationContext _context;
        private readonly IFooterConfigService _footer_provider;
        private readonly IArticlesService _articles_provider;
        private readonly IConferencesService _confs_provider;
        private readonly ITeamsService _teams_provider;
        private readonly ICategoriesService _categories_provider;
        private readonly IImageService _image_provider;
        private readonly ILanguageConfigService _languageConfigService;
        private readonly IUserService _userService;
        private readonly UserManager<User> _userManager;

        public AdminPageController(ApplicationContext context, 
            IFooterConfigService footerService, 
            IArticlesService articlesService, 
            IConferencesService conferencesService, 
            ITeamsService teamsService,
            ICategoriesService categoriesService,
            IImageService imageService,
            IUserService userService,
            UserManager<User> userManager)
        {
            _context = context;
            _footer_provider = footerService;
            _articles_provider = articlesService;
            _confs_provider = conferencesService;
            _teams_provider = teamsService;
            _categories_provider = categoriesService;
            _image_provider = imageService;
            _footer_provider = new FooterConfigService(context);
            _languageConfigService = new LanguagesConfigService(context);
            _userService = userService;
            _userManager = userManager;
        }

        public IActionResult Home()
        {
            return View();
        }

        public IActionResult Lifestyle()
        {
            return View();
        }

        [Route("/AdminPage/{category}/AddNewLifestyleArticle")]
        public IActionResult AddNewLifestyleArticle(string category)
        {
            ViewData["Title"] = category;
            return View("AddNewLifestyleOrDealbookArticle");
        }

        [HttpPost]
        [Route("/AdminPage/{category}/AddNewLifestyleArticle")]
        public IActionResult AddNewLifestyleArticle(string category, [Bind("Alt_picture_text,Headline,Caption,Content,Show_comments")] ArticleLifestyle article, IFormFile image)
        {
            if (article.Content == null)
            {
                article.Content = "";
                ModelState.Remove("Article.Content");
            }

            if (ModelState.IsValid && image != null)
            {
                _articles_provider.AddNewLifestyleOrDealbookArticle(article, image);
            }
            else
            {
                return BadRequest();
            }

            return RedirectToAction("Lifestyle");
        }

        public IActionResult Dealbook()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Languages()
        {
            ViewBag.Languages = _languageConfigService.GetAllLanguages();
            return View();
        }

        [HttpPost]
        public string AddNewLanguage(string[] langs)
        {
            foreach(var lang in langs)
            {
                Language obj = new Language(lang);
                _languageConfigService.AddLanguage(obj);
            }

            string returnUrl = string.Format("{0}://{1}/AdminPage/Languages",
                       HttpContext.Request.Scheme, HttpContext.Request.Host);

            return returnUrl;
        }

        [HttpGet]
        public string LanguageVisibleGet(int id)
        {
            var result = _languageConfigService.GetLanguage(id);

            if (result.Visible == true)
            {
                return "yes";
            }
            else
            {
                return "no";
            }

        }

        [HttpPost]
        public ActionResult ChangeLanguageVisibility(int id, bool vi)
        {
            _languageConfigService.ChangeLangVisibility(id, vi);
            return View();
        }

        [HttpDelete]
        public string DeleteLanguage(int? id)
        {
            _languageConfigService.DeleteLanguage((int)id);
            string baseUrl = string.Format("{0}://{1}/AdminPage/Languages",
                       HttpContext.Request.Scheme, HttpContext.Request.Host);

            return baseUrl;
        }

        [Route("/AdminPage/{category}/AddNewDealbookArticle")]
        public IActionResult AddNewDealbookArticle(string category)
        {
            ViewData["Title"] = category;
            return View("AddNewLifestyleOrDealbookArticle");
        }

        [HttpPost]
        [Route("/AdminPage/{category}/AddNewDealbookArticle")]
        public IActionResult AddNewDealbookArticle(string category, [Bind("Alt_picture_text,Headline,Caption,Content,Show_comments")] ArticleDealbook article, IFormFile image)
        {
            if (article.Content == null)
            {
                article.Content = "";
                ModelState.Remove("Article.Content");
            }

            if (ModelState.IsValid && image != null)
            {
                _articles_provider.AddNewLifestyleOrDealbookArticle(article, image);
            }
            else
            {
                return BadRequest();
            }

            return RedirectToAction("Dealbook");
        }

        [Route("/AdminPage/Edit{category}Article/{articleId}")]
        public IActionResult EditLifestyleOrDealbookArticle(string category, int articleId)
        {
            ViewData["Title"] = category;
            var article = _articles_provider.GetLifestyleOrDealbookArticleById(articleId);
            ViewBag.Alt_picture_text = article.Alt_picture_text;
            ViewBag.Caption = article.Caption;
            ViewBag.Headline = article.Headline;
            ViewBag.Content = article.Content;
            ViewBag.Show_comments = article.Show_comments;
            ViewBag.Image = _image_provider.ConvertImageBytesToString(article.Image);
            ViewBag.ArticleId = articleId;
            ViewBag.IsEditing = "True";
            ViewBag.DiscriminatorValue = article.DiscriminatorValue;

            return View("AddNewLifestyleOrDealbookArticle");
        }

        [HttpPut]
        [Route("/AdminPage/Edit{category}Article/{articleId}")]
        public string EditLifestyleOrDealbookArticle(string category, int articleId, string headline, string caption, string alt, string content, bool showComments, IFormFile image, bool imageChanged)
        {
            if (content == null)
            {
                content = "";
            }

            if (_articles_provider.isLifestyleOrDealbookEditValid(headline, caption, alt))
            {
                _articles_provider.UpdateLifestyleOfDealbookArticle(category, articleId, image, imageChanged, headline, caption, alt, content, showComments);
            }
            else
            {
                string url = string.Format("{0}://{1}/AdminPage/Edit{2}Article/{3}",
                       HttpContext.Request.Scheme, HttpContext.Request.Host, category, articleId);

                return url;
            }

            string baseUrl = string.Format("{0}://{1}/AdminPage/{2}/",
                       HttpContext.Request.Scheme, HttpContext.Request.Host, category);

            return baseUrl;
        }

        public IActionResult Surveys()
        {
            return View();
        }

        public IActionResult Banners()
        {
            return View();
        }

        [Route("/AdminPage/{category}/AddNewArticle")]
        public IActionResult AddNewArticle(string category)
        {
            ViewData["Title"] = category;
            return View(_articles_provider.CreateNewArticleViewModel(category));
        }

        [HttpPost]
        [Route("/AdminPage/{category}/AddNewArticle")]
        public IActionResult AddNewArticle(string category, [Bind("Alt_picture_text,Headline,Caption,Content,Show_comments")] Article article, string team, string location, IFormFile image)
        {
            if (article.Content == null)
            {
                article.Content = "";
                ModelState.Remove("Article.Content");
            }
            
            ModelState.Remove("Article.CategoryName");

            if (ModelState.IsValid && _articles_provider.isAddNewArticleValid(team, location, image))
            {
                article.CategoryName = category;
                _articles_provider.AddNewArticle(article, team, location, image);
            }
            else
            {
                return BadRequest();
            }

            return RedirectToAction("Articles", new { category = category });
        }

        public string EditArticleGetUrl(string category, int articleId)
        {
            string url = string.Format("{0}://{1}/AdminPage/EditArticle/{2}/{3}",
                       HttpContext.Request.Scheme, HttpContext.Request.Host, category, articleId);

            return url;
        }

        [HttpGet]
        [Route("/AdminPage/EditArticle/{category}/{articleId}")]
        public IActionResult EditArticle(string category, int articleId)
        {
            ViewData["Title"] = category;
            var article = _articles_provider.GetArticleById(articleId);
            ViewBag.Alt_picture_text = article.Alt_picture_text;
            ViewBag.Caption = article.Caption;
            ViewBag.Headline = article.Headline;
            ViewBag.Content = article.Content;
            ViewBag.Show_comments = article.Show_comments;
            ViewBag.Location = article.LocationId;
            ViewBag.Team = article.TeamId;
            ViewBag.Conference = _articles_provider.GetConferenceIdByTeamId(article.TeamId);
            ViewBag.Image = _image_provider.ConvertImageBytesToString(article.Image);
            ViewBag.ArticleId = articleId;
            ViewBag.IsEditing = "True";

            return View("AddNewArticle", _articles_provider.CreateNewArticleViewModel(category));
        }

        [HttpPut]
        public string SaveEditedArticle(string category, int articleId, string headline, string caption, string alt, string content, bool showComments, string team, string location, IFormFile image, bool imageChanged)
        {
            if (content == null)
            {
                content = "";
            }

            ModelState.Remove("Article.CategoryName");

            if (_articles_provider.isEditValid(team, location, headline, caption, alt))
            {
                _articles_provider.UpdateArticle(category, articleId, team, location, image, imageChanged, headline, caption, alt, content, showComments);
            }
            else
            {
                string url = string.Format("{0}://{1}/AdminPage/{2}/EditArticle/{3}",
                       HttpContext.Request.Scheme, HttpContext.Request.Host, category, articleId);

                return url;
            }

            string baseUrl = string.Format("{0}://{1}/AdminPage/Articles/{2}/",
                       HttpContext.Request.Scheme, HttpContext.Request.Host, category);

            return baseUrl;
        }

        [Route("/AdminPage/FooterConfig/{section}")]
        [Route("/AdminPage/FooterConfig/{section}/{id}")]
        public ActionResult FooterConfig(string? section, int? id)
        {
            ViewBag.PageList = _footer_provider.GetSectionPages(section);
            ViewBag.Section = _context.Sections.FirstOrDefault(a => a.SectionName == section);
            ViewBag.PrivacyPolicy = _footer_provider.GetPageByName("Privacy policy");
            ViewBag.Provider = _footer_provider;
            ViewBag.Terms = _footer_provider.GetPageByName("Terms and conditions");
            ViewBag.AdminInfo = _context.ContactInfos.FirstOrDefault();

            if (id == null)
            {
                ViewBag.Page = null;
                return View();
            }

            var objPage = _footer_provider.GetSinglePage((int)id);
            ViewBag.Page = objPage;
            return View();
        }

        [Route("/AdminPage/Articles/{category}")]
        public ActionResult Articles(string? category)
        {
            ViewBag.PageSize = Constants.PageSizeForArticles;
            ViewBag.PageNumber = Constants.InitialPageNumber;
            ViewBag.Category = category;

            dynamic all_objects = new ExpandoObject();
            all_objects.Categories = _categories_provider.GetCategories();
            all_objects.Conferences = _confs_provider.GetAllConferences();
            all_objects.Teams = _teams_provider.GetTeams("All conferences");
            return View(all_objects);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind("Id,Content,Name,Section")] FooterPage post)
        {
            if (ModelState.IsValid)
            {
                _footer_provider.AddPage(post);
                string baseUrl = string.Format("{0}://{1}/AdminPage/FooterConfig/{2}/{3}",
                       HttpContext.Request.Scheme, HttpContext.Request.Host, post.Section, post.Id);
                return Redirect(baseUrl);
            }

            return View(post);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, [Bind("Id,Content,Name,Section,Visible")] FooterPage post)
        {
            if (id != post.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _footer_provider.EditPage(post);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_footer_provider.PageExists(post.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                string baseUrl = string.Format("{0}://{1}/AdminPage/FooterConfig/{2}",
                       HttpContext.Request.Scheme, HttpContext.Request.Host, post.Section);
                return Redirect(baseUrl);
            }
            return Redirect(post.Section);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditAdmin([Bind("Id,Adress,Telephone,Email")] ContactInfo info)
        {
            if (ModelState.IsValid)
            {
                _context.ContactInfos.Update(info);
                _context.SaveChanges();
                string baseUrl = string.Format("{0}://{1}/AdminPage/FooterConfig/{2}",
                       HttpContext.Request.Scheme, HttpContext.Request.Host, "CompanyInfo");
                return Redirect(baseUrl);
            }

            return Redirect("CompanyInfo");
        }

        [HttpPost]
        public ActionResult ChangePageVisibility(int id, bool vi)
        {
            var result = _context.FooterPages.Where(a => a.Id == id).FirstOrDefault();
            
            if (result != null)
            {
                result.Visible = vi;
                _context.SaveChanges();
            }

            return View();

        }

        [HttpPost]
        public string ChangeSectionVisibility(string section, bool newStatus)
        {
            _footer_provider.ChangeSectionVisibility(section, newStatus);
            string baseUrl = string.Format("{0}://{1}/AdminPage/FooterConfig/{2}",
                      HttpContext.Request.Scheme, HttpContext.Request.Host, section);

            return baseUrl;
        }

        [HttpGet]
        public string VisibleGet(int id)
        {
            var result = _footer_provider.GetSinglePage(id);

            if (result.Visible == true)
            {
                return "yes";
            }
            else
            {
                return "no";
            }

        }

        [HttpDelete]
        public string DeleteFooterPage(int? id, string section)
        {
            _footer_provider.DeleteFooterPage((int)id);
            string baseUrl = string.Format("{0}://{1}/AdminPage/FooterConfig/{2}",
                       HttpContext.Request.Scheme, HttpContext.Request.Host, section);

            return baseUrl;
        }

        [HttpPost] 
        public string ChangePublishStatus(string category, int id, bool newStatus)
        {
            _articles_provider.ChangePublishStatus(id, newStatus);
            string baseUrl = string.Format("{0}://{1}/AdminPage/Articles/{2}",
                       HttpContext.Request.Scheme, HttpContext.Request.Host, category);

            return baseUrl;
        }

        [HttpDelete]
        public string DeleteArticle(string category, int id)
        {
            _articles_provider.DeleteArticle(id);
            string baseUrl = string.Format("{0}://{1}/AdminPage/Articles/{2}",
                       HttpContext.Request.Scheme, HttpContext.Request.Host, category);

            return baseUrl;
        }

        [HttpGet]
        public string SearchArticles(string category, string conf, string team, bool ?status, string search)
        {
            string baseUrl = string.Format("{0}://{1}/AdminPage/Articles/{2}?conference={3}&team={4}&status={5}&search={6}",
                       HttpContext.Request.Scheme, HttpContext.Request.Host, category, conf, team, status, search);

            return baseUrl;
        }

        [HttpPut]
        public string EditArticleInfo(int id, int id_team)
        {
            _articles_provider.ChangeArticleTeam(id, id_team);
            var t = _teams_provider.GetTeamById(id_team);
            string baseUrl = string.Format("{0}://{1}/AdminPage/Articles/{2}?conference={3}&team={4}",
                       HttpContext.Request.Scheme, HttpContext.Request.Host, t.Conference.Category.Name, t.Conference.Name, t.Name);

            return baseUrl;
        }
        //Navigation admin side
        public IActionResult InformationArchitecture()
        {
            return View();
        }

        [Route("/AdminPage/AddMainArticles")]
        public ActionResult AddMainArticles()
        {
            string first_category = _categories_provider.GetFirstCategory().Name;
            ViewBag.ConfsNBA = _confs_provider.GetConferences(first_category);
            ViewBag.DefaultArticles = _articles_provider.GetArticles(first_category, null, null, null, null);
            ViewBag.PublishedMainArticles = _articles_provider.GetArticlesByStatusType(StatusType.Published);
            ViewBag.SavedMainArticles = _articles_provider.GetArticlesByStatusType(StatusType.Saved);
            return View();
        }

        [HttpPost]
        public string SaveArticlesMainBreakdown([FromBody] MainBreakdownArticlesModel articles)
        {
            _articles_provider.SaveInMainBreakdownSection(articles);
            string baseUrl = string.Format("{0}://{1}/AdminPage/AddMainArticles",
                       HttpContext.Request.Scheme, HttpContext.Request.Host);

            return baseUrl;
        }

        [HttpPost]
        public string PublishArticlesMainBreakdown()
        {
            _articles_provider.PublishInMainBreakdownSection();
            string baseUrl = string.Format("{0}://{1}/AdminPage/AddMainArticles",
                       HttpContext.Request.Scheme, HttpContext.Request.Host);
            return baseUrl;
        }
        public IActionResult GetUserInfo()
        {
            string id = _userManager.FindByNameAsync(HttpContext.User.Identity.Name).Result.Id;
            var res = _userService.GetUserInfo(id);
            return Json(res);
        }
    }
    
}

