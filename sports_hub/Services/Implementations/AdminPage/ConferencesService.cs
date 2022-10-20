using Microsoft.EntityFrameworkCore;
using sports_hub.Models;
using sports_hub.Models.Entities.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using sports_hub.Services.Interfaces;

namespace sports_hub.Services.Implementations.AdminPage
{
    public class ConferencesService : IConferencesService
    {
        private readonly ApplicationContext _context;

        public ConferencesService(ApplicationContext context)
        {
            _context = context;
        }

        public List<Conference> GetConferences(string? category)
        {
            List<Conference> result = _context.Conferences.Include(u => u.Category).Where(p => p.Category.Name == category).ToList();
            return result;
        }

        public List<Conference> GetAllConferences()
        {
            List<Conference> result = _context.Conferences.Include(u => u.Category).ToList();
            return result;
        }

        public async Task<List<Conference>> GetConferencesAsync()
        {
            List<Conference> result = await _context.Conferences.ToListAsync();
            return result;
        }
    }
}
