using Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Application.IRepository
{
    public interface IIpFilter
    {
        //Category
        Task<List<IpFilter>> GetAllCategories();
        Task<IpFilter> GetCategoryById(int id);
        Task<IpFilter> AddCategory(string name);
        Task<IpFilter> UpdateCategory(int id, string name);
        Task<IpFilter> DeleteCategory(int id);
        Task<IpFilter> ArchiveCategory(int id);
        Task<bool> IsCategoryDuplicate(string name);
        Task<bool> IsCategoryDuplicate(int id, string name);

        //Technology
        Task<List<IpFilter>> GetAllTechnologies();
        Task<IpFilter> GetTechnologyById(int id);
        Task<IpFilter> AddTechnology(string name);
        Task<IpFilter> UpdateTechnology(int id, string name);
        Task<IpFilter> DeleteTechnology(int id);
        Task<IpFilter> ArchiveTechnology(int id);
        Task<bool> IsTechnologyDuplicate(string name);
        Task<bool> IsTechnologyDuplicate(int id, string name);
    }
}
