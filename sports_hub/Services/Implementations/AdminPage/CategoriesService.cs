using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using sports_hub.Models.Entities.Navigation;
using sports_hub.Models;
using sports_hub.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace sports_hub.Services.Implementations.AdminPage
{
    public class CategoriesService : ICategoriesService
    {
        private readonly ApplicationContext _context;

        public CategoriesService(ApplicationContext context)
        {
            _context = context;
        }
        public List<Category> GetCategories()
        {
            List<Category> result = _context.Categories.ToList();
            return result;
        }

        public async Task<List<Category>> GetCategoriesAsync()
        {
            List<Category> result = await _context.Categories.ToListAsync();
            return result;
        }


        public Category FindCategory(string name)
        {
            return _context.Categories.FirstOrDefault(u => u.Name == name);
        }
        
        public Category GetFirstCategory()
        {
            return _context.Categories.FirstOrDefault();
        }
        
    }
}
