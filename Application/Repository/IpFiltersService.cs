using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.IRepository;
using Core;
using Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Repository
{
    public sealed class IpFiltersService : IIpFilter
    {
        private readonly MediusContext _dbContext;
        private bool _disposed;
        
        public IpFiltersService(MediusContext dbContext)
        {
            this._dbContext = dbContext;
        }
        
        public void Dispose(bool val)
        {
            if (_disposed)
            {
                if (val)
                {
                    // called via myClass.Dispose(). 
                    // OK to use any private object references
                }
                // Release unmanaged resources.
                // Set large fields to null.                
                _disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        //Category
        public async Task<List<IpFilter>> GetAllCategories()
        {
            return await _dbContext.IpFilters.Where(x => x.Type == FilterType.Category && x.IsActive).AsNoTracking().ToListAsync();
        }
        
        public async Task<IpFilter> GetCategoryById(int id)
        {
            return await _dbContext.IpFilters.Where(x => x.Id == id && x.Type == FilterType.Category && x.IsActive).FirstOrDefaultAsync() ?? throw new Exception($"No Category found against id:'{id}'");
        }

        public async Task<IpFilter> AddCategory(string name)
        {
            if (await IsCategoryDuplicate(name)) throw new Exception($"'{name}' already exists. Please choose a different name.");
            IpFilter maxRecord = await _dbContext.IpFilters.OrderByDescending(x => x.Id).FirstOrDefaultAsync();
            var maxId = maxRecord.Id;
            IpFilter category = new IpFilter
            {
                Name = name,
                Type = FilterType.Category,
                Code = "Cat-" + maxId
            };
            await _dbContext.IpFilters.AddAsync(category);
            await _dbContext.SaveChangesAsync();
            return category;
        }

        public async Task<IpFilter> UpdateCategory(int id, string name)
        {
            var category = await _dbContext.IpFilters.FirstOrDefaultAsync(x => x.Id == id) ?? throw new Exception($"No Category found against id:'{id}'");
            if (await IsCategoryDuplicate(id, name)) throw new Exception($"'{name}' already exists. Please choose a different name.");

            category.Name = name;
            category.LastModify = DateTime.Now;
            await _dbContext.SaveChangesAsync();
            return category;
        }
        
        public async Task<IpFilter> DeleteCategory(int id)
        {
            var category = await GetCategoryById(id);
            _dbContext.Remove(category);
            await _dbContext.SaveChangesAsync();
            return category;
        }

        public async Task<IpFilter> ArchiveCategory(int id)
        {
            var category = await GetCategoryById(id);
            category.IsActive = false;
            category.LastModify = DateTime.Now;
            await _dbContext.SaveChangesAsync();
            return category;
        }

        public Task<bool> IsCategoryDuplicate(string name)
            => _dbContext.IpFilters.AnyAsync(x => x.Name.ToLower().Equals(name.ToLower()) && x.Type == FilterType.Category);

        public Task<bool> IsCategoryDuplicate(int id, string name)
             => _dbContext.IpFilters.AnyAsync(x => x.Name.ToLower().Equals(name.ToLower()) && !x.Id.Equals(id) && x.Type == FilterType.Category);

        //Technology
        public async Task<List<IpFilter>> GetAllTechnologies()
        {
            return await _dbContext.IpFilters.Where(x => x.Type == FilterType.Technology && x.IsActive).AsNoTracking().ToListAsync();
        }
       
        public async Task<IpFilter> GetTechnologyById(int id)
        {
            return await _dbContext.IpFilters.Where(x => x.Id == id && x.Type == FilterType.Technology && x.IsActive).AsNoTracking().FirstOrDefaultAsync() ?? throw new Exception($"No technology found against id:'{id}'");
        }

        public async Task<IpFilter> AddTechnology(string name)
        {
            if (await IsTechnologyDuplicate(name)) throw new Exception($"'{name}' already exists. Please choose a different name.");
            
            IpFilter maxRecord = await _dbContext.IpFilters.OrderByDescending(x => x.Id).FirstOrDefaultAsync();
            var maxId = maxRecord.Id;

            IpFilter technology = new IpFilter
            {
                Name = name,
                Type = FilterType.Technology,
                Code = "Tech-" + maxId
            };
            await _dbContext.IpFilters.AddAsync(technology);
            await _dbContext.SaveChangesAsync();
            return technology;
        }

        public async Task<IpFilter> UpdateTechnology(int id, string name)
        {
            var technology = await _dbContext.IpFilters.FirstOrDefaultAsync(x => x.Id == id && x.Type == FilterType.Technology && x.IsActive) ?? throw new Exception($"No Technology found against id:'{id}'");
            if (await IsTechnologyDuplicate(id, name)) throw new Exception($"'{name}' already exists. Please choose a different name.");

            technology.Name = name;
            technology.LastModify = DateTime.Now;
            await _dbContext.SaveChangesAsync();
            return technology;
        }
        
        public async Task<IpFilter> DeleteTechnology(int id)
        {
            var technology = await GetTechnologyById(id);
            _dbContext.Remove(technology);
            await _dbContext.SaveChangesAsync();
            return technology;
        }

        public async Task<IpFilter> ArchiveTechnology(int id)
        {
            var technology = await GetTechnologyById(id);
            technology.IsActive = false;
            technology.LastModify = DateTime.Now;
            await _dbContext.SaveChangesAsync();
            return technology;
        }

        public Task<bool> IsTechnologyDuplicate(string name)
            => _dbContext.IpFilters.AnyAsync(x => x.Name.ToLower().Equals(name.ToLower()) && x.Type == FilterType.Technology);

        public Task<bool> IsTechnologyDuplicate(int id, string name)
             => _dbContext.IpFilters.AnyAsync(x => x.Name.ToLower().Equals(name.ToLower()) && !x.Id.Equals(id) && x.Type == FilterType.Technology);

    }
}
