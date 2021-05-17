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
    public sealed class CityService : ICity
    {
        private readonly MediusContext _dbContext;
        private bool _disposed;
        
        public CityService(MediusContext dbContext)
        {
            this._dbContext = dbContext;
        }

        private void Dispose(bool val)
        {
            if (!_disposed)
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
        
        public async Task<List<City>> GetAllCities()
        {
            return await _dbContext.Cities.Where(x => x.IsActive).AsNoTracking().ToListAsync();
        }
       
        public async Task<City> GetCityById(int id)
        {
            if (id == 0) throw new Exception($"City Id is required.");
           
            return await _dbContext.Cities.Where(x => x.Id == id && x.IsActive).FirstOrDefaultAsync() ?? throw new Exception($"No City found against id:'{id}'");
        }

        public async Task<City> AddCity(string cityName)
        {
            if (cityName == null || cityName == "") throw new Exception($"CityName is required. Please write a name.");

            if (await IsNameDuplicate(cityName)) throw new Exception($"'{cityName}' already exists. Please choose a different name.");

            City city = new City
            {
                CityName = cityName
            };
            await _dbContext.Cities.AddAsync(city);
            await _dbContext.SaveChangesAsync();
            return city;
        }

        public async Task<City> UpdateCity(int id, string cityName)
        {

            var city = await _dbContext.Cities.FirstOrDefaultAsync(x => x.Id == id) ?? throw new Exception($"No City found against id:'{id}'");

            if (cityName == null || cityName == "") throw new Exception($"CityName is required. Please write a name.");
            if (await IsNameDuplicate(id, cityName)) throw new Exception($"'{cityName}' already exists. Please choose a different name.");

            city.CityName = cityName;
            city.LastModify = DateTime.Now;
            await _dbContext.SaveChangesAsync();
            return city;
        }

        public async Task<City> DeleteCity(int id)
        {
            var city = await GetCityById(id);
            _dbContext.Remove(city);
            await _dbContext.SaveChangesAsync();
            return city;
        }
      
        public async Task<City> ArchiveCity(int id)
        {
            var city = await GetCityById(id);
            city.IsActive = false;
            city.LastModify = DateTime.Now;
            await _dbContext.SaveChangesAsync();
            return city;
        }
        
        public Task<bool> IsNameDuplicate(string name)
            => _dbContext.Cities.AnyAsync(x => x.CityName.ToLower().Equals(name.ToLower()));

        public Task<bool> IsNameDuplicate(int id, string name)
             => _dbContext.Cities.AnyAsync(x => x.CityName.ToLower().Equals(name.ToLower()) && !x.Id.Equals(id));
    }
}
