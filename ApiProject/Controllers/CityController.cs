using Application.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ApiProject.Controllers
{
    [Route("api/City")]
    [ApiController]
    public class CityController : ControllerBase
    {
        private readonly ICity cityService;
        public CityController(ICity cityService)
        {
            this.cityService = cityService;
        }

        [HttpGet]
        [Route("GetCities")]
        public async Task<IActionResult> GetCities()
        {
            try
            {
                var data = await cityService.GetAllCities();

                return StatusCode(StatusCodes.Status200OK, data);
            }
            catch (Exception ex)
            {
                // Log exception code goes here
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("GetCityById")]
        public async Task<IActionResult> GetCityById(int id)
        {
            try
            {
                var data = await cityService.GetCityById(id);

                return StatusCode(StatusCodes.Status200OK, data);
            }
            catch (Exception ex)
            {
                // Log exception code goes here
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        [Route("AddCity")]
        public async Task<IActionResult> AddCity(string cityName)
        {
            try
            {
                var data = await cityService.AddCity(cityName);
                return StatusCode(StatusCodes.Status200OK, data);
            }
            catch (Exception ex)
            {
                // Log exception code goes here
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut]
        [Route("ModifyCity")]
        public async Task<IActionResult> ModifyCity(int id, string cityName)
        {
            try
            {
                var data = await cityService.UpdateCity(id, cityName);
                return StatusCode(StatusCodes.Status200OK, data);
            }
            catch (Exception ex)
            {
                // Log exception code goes here
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpDelete]
        [Route("DeleteCity")]
        public async Task<IActionResult> DeleteCity(int id)
        {
            try
            {
                var data = await cityService.DeleteCity(id);
                return StatusCode(StatusCodes.Status200OK, data);
            }
            catch (Exception ex)
            {
                // Log exception code goes here
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut]
        [Route("ArchiveCity")]
        public async Task<IActionResult> ArchiveCity(int id)
        {
            try
            {
                var data = await cityService.ArchiveCity(id);
                return StatusCode(StatusCodes.Status200OK, data);
            }
            catch (Exception ex)
            {
                // Log exception code goes here
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
