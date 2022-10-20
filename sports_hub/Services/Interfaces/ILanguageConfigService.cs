using sports_hub.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sports_hub.Services.Interfaces
{
    interface ILanguageConfigService
    {
        public List<Language> GetAllLanguages();
        public Language GetLanguage(int id);
        public void ChangeLangVisibility(int id, bool newStatus);
        public void DeleteLanguage(int id);
        public void AddLanguage(Language language);
    }
}
