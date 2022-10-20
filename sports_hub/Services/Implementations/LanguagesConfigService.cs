using sports_hub.Models;
using sports_hub.Models.Entities;
using sports_hub.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sports_hub.Services.Implementations
{
    public class LanguagesConfigService : ILanguageConfigService
    {
        private readonly ApplicationContext _context;

        public LanguagesConfigService(ApplicationContext context)
        {
            _context = context;
        }

        public List<Language> GetAllLanguages()
        {
            return _context.Languages.ToList();
        }

        public Language GetLanguage(int id)
        {
            return _context.Languages.FirstOrDefault(p => p.Id == id);
        }

        public void ChangeLangVisibility(int id, bool newStatus)
        {
            var result = _context.Languages.Where(a => a.Id == id).FirstOrDefault();

            if (result != null)
            {
                result.Visible = newStatus;
                _context.SaveChanges();
            }
        }

        public void DeleteLanguage(int id)
        {
            var page = _context.Languages.Where(a => a.Id == id).FirstOrDefault();
            _context.Languages.Remove(page);
            _context.SaveChanges();
        }

        public void AddLanguage(Language language)
        {
            if (!IsLangInDataBase(language))
            {
                _context.Languages.Add(language);
                _context.SaveChanges();
            }
        }

        private bool IsLangInDataBase(Language language)
        {
            var result = _context.Languages.Where(a => a.Name == language.Name).FirstOrDefault();
            
            if (result != null)
            {
                return true;
            }

            return false;
        }
    }
}
