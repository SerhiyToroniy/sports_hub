using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using sports_hub.Models.Entities.Navigation;

namespace sports_hub.Services.Interfaces
{
    public interface IConferencesService
    {
        public List<Conference> GetConferences(string? category);

        public List<Conference> GetAllConferences();

        public Task<List<Conference>> GetConferencesAsync();
    }
}
