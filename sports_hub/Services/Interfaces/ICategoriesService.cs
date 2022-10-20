using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using sports_hub.Models.Entities.Navigation;

namespace sports_hub.Services.Interfaces
{
    public interface ICategoriesService
    {
        public List<Category> GetCategories();
        public Task<List<Category>> GetCategoriesAsync();
        public Category FindCategory(string name);
        public Category GetFirstCategory();
    }
}
