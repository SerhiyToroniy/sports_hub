using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using sports_hub.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using sports_hub.Models.Entities.Navigation;
using sports_hub.Models.ViewModels;
using System.Reflection;
using sports_hub.Services.Interfaces;
using PagedList;
using PagedList.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace sports_hub.Controllers
{
    public class DataController : Controller
    {
        private readonly ApplicationContext _context;
        private readonly ICategoriesService _categoriesService;
        private readonly IConferencesService _conferencesService;
        private readonly ITeamsService _teamsService;
        private readonly IArticlesService _articlesService;
        private readonly IImageService _imageService;
        private readonly IUserService _userService;

        public DataController(ApplicationContext context,
            ICategoriesService categoriesService,
            IConferencesService conferencesService,
            ITeamsService teamsService,
            IArticlesService articlesService,
            IImageService imageService,
            IUserService userService
            )
        {
            _context = context;
            _categoriesService = categoriesService;
            _conferencesService = conferencesService;
            _teamsService = teamsService;
            _articlesService = articlesService;
            _imageService = imageService;
            _userService = userService;
        }
        public async Task<IActionResult> Categories() 
        { 
            return Ok(await _categoriesService.GetCategoriesAsync()); 
        }

        public async Task<IActionResult> AllConferences()
        {
            return Ok(await _conferencesService.GetConferencesAsync());
        }

        public IActionResult Conferences(string CategoryName) 
        {
            return Json(_conferencesService.GetConferences(CategoryName));
        }

        public async Task<IActionResult> AllTeams()
        {
            return Ok(await _teamsService.GetTeamsAsync());
        }

        public IActionResult Teams(string ConferenceName) 
        {
            return Json( _teamsService.GetTeams(ConferenceName)); 
        }

        public List<Article> SavedArticles()
        {
            return _context.Articles.Where(u => u.StatusMainArticle == StatusType.Saved).ToList();
        }

        public IActionResult Articles(string category, string conference, string team)
        {
            string article_category = category != "Not selected" ? category : null;
            string article_conference = conference != "Not selected" ? conference : null;
            string article_team = team != "Not selected" ? team : null;
            return Json(_articlesService.GetArticles(article_category, article_conference, article_team, null, null));
        }

        [ActionName("AllArticlesView")]
        public IActionResult Articles(string category, string conference, string team, string status, string search, int page_size, int page_number)
        {
            bool? article_status = null;
            if (status == "published")
            {
                article_status = true;
            }
            else if (status == "unpublished")
            {
                article_status = false;
            }
            
            return Json(_articlesService.GetArticles(category, conference, team, article_status, search).ToPagedList(page_number, page_size));
        }

        public string GetImage(int id)
        {
            byte[] image = _articlesService.GetArticleById(id).Image;
            return _imageService.ConvertImageBytesToString(image);
        }

        public async Task<IActionResult> SaveChanges([FromBody]SaveChangesViewModel model)
        {
            if (ModelState.IsValid)
            {
                await _context.Categories.AddRangeAsync(model.Categories);
                await _context.Conferences.AddRangeAsync(model.Conferences);
                await _context.Teams.AddRangeAsync(model.Teams);

                _context.Teams.UpdateRange(model.TeamsEdited);
                _context.Conferences.UpdateRange(model.ConferencesEdited);
                _context.Categories.UpdateRange(model.CategoriesEdited);

                await _context.SaveChangesAsync();

                _context.Teams.RemoveRange(model.TeamsDeleted);
                _context.Conferences.RemoveRange(model.ConferencesDeleted);
                _context.Categories.RemoveRange(model.CategoriesDeleted);

                await _context.SaveChangesAsync();
                return Ok();
            }
            else
                return BadRequest();
        }

        public IActionResult GetUserTeams(string username)
        {
            return Json(_teamsService.GetUserTeams(username));
        } 

        public IActionResult GetTeamsTeamHub(string search)
        {
            return Json(_teamsService.GetTeamsBySearch(search));
        }

        public IActionResult GetTeam(string name)
        {
            return Json(_teamsService.GetTeamByName(name));
        }

        public void AddTeam(string user_name, string team_name)
        {
            _teamsService.AddTeam(user_name, team_name);
        }

        public void DeleteUserTeam(string user_name, string team_name)
        {
            _teamsService.DeleteUserTeam(user_name, team_name);
        }

        public IActionResult GetUsersByRole(string role, int page_size, int page_number)
        {
            var res = _userService.GetUsers(role, page_number, page_size);
            return Json(res);
        }

        public IActionResult GetUserInfo(string id)
        {
            var res = _userService.GetUserInfo(id);
            return Json(res);
        }
    }
}
