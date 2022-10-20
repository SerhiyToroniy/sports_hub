using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using sports_hub.Models.Entities;

namespace sports_hub.Services.Interfaces
{
    public interface IFooterConfigService
    {
        public List<FooterPage> GetAllPages();

        public List<FooterPage> GetSectionPages(string section);
        
        public FooterPage GetSinglePage(int id);
        
        public FooterPage GetPageByName(string name);

        public void AddPage(FooterPage post);
        
        public void EditPage(FooterPage post);

        public bool PageExists(int id);

        public int GetIdByName(string name);
        
        public void DeleteFooterPage(int id);
        
        public void ChangeSectionVisibility(string section, bool newStatus);
        
        public void ChangePageVisibility(int id, bool newStatus);
    }
}
