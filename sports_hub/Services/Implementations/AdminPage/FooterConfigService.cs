using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using sports_hub.Models;
using sports_hub.Models.Entities;
using sports_hub.Services.Interfaces;

namespace sports_hub.Services.Implementations
{
    public class FooterConfigService : IFooterConfigService
    {
        private readonly ApplicationContext _context;

        public FooterConfigService(ApplicationContext context)
        {
            _context = context;
        }

        public List<FooterPage> GetAllPages()
        {
            return _context.FooterPages.ToList();
        }

        public List<FooterPage> GetSectionPages(string section)
        {
            //return _context.Post.ToList().Where(p => p.Section == section).ToList();
            return _context.FooterPages.Where(p => p.Section == section
            && p.Name != "Privacy policy" && p.Name != "Terms and conditions").ToList();
        }

        public FooterPage GetSinglePage(int id)
        {
            return _context.FooterPages.FirstOrDefault(p => p.Id == id);
        }

        public FooterPage GetPageByName(string name)
        {
            return _context.FooterPages.FirstOrDefault(p => p.Name == name);
        }

        public void AddPage(FooterPage post)
        {
            _context.FooterPages.Add(post);
            _context.SaveChanges();
        }

        public void EditPage(FooterPage post)
        {
            _context.FooterPages.Update(post);
            _context.SaveChanges();
        }

        public bool PageExists(int id)
        {
            return _context.FooterPages.Any(e => e.Id == id);
        }

        public int GetIdByName(string name)
        {
            return _context.FooterPages.FirstOrDefault(p => p.Name == name).Id;
        }

        public void DeleteFooterPage(int id)
        {
            var page = _context.FooterPages.Where(a => a.Id == id).FirstOrDefault();
            _context.FooterPages.Remove(page);
            _context.SaveChanges();
        }

        public void ChangeSectionVisibility(string section, bool newStatus)
        {
            var section_info = _context.Sections.Where(a => a.SectionName == section).FirstOrDefault();
            section_info.Visible = newStatus;
            _context.Sections.Update(section_info);
            _context.SaveChanges();
        }

        public void ChangePageVisibility(int id, bool newStatus)
        {
            var result = _context.FooterPages.Where(a => a.Id == id).FirstOrDefault();

            if (result != null)
            {
                result.Visible = newStatus;
                _context.SaveChanges();
            }
        }
    }
}
