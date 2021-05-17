using Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.IRepository
{
    public interface ICity
    {
        Task<List<City>> GetAllCities();
        Task<City> GetCityById(int id);
        Task<City> AddCity(string cityName);
        Task<City> UpdateCity(int id, string cityName);
        Task<City> DeleteCity(int id);
        Task<City> ArchiveCity(int id);
        Task<bool> IsNameDuplicate(string name);
        Task<bool> IsNameDuplicate(int id, string name);
    }
}
