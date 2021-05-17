using Application.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace ApiProject.Controllers
{
    [Route("api/IpFilter")]
    [ApiController]
    public class IpFilterController : ControllerBase
    {
        private readonly IIpFilter ipFilterService;
        public IpFilterController(IIpFilter ipFilterService)
        {
            this.ipFilterService = ipFilterService;
        }

        [HttpGet]
        [Route("GetCategories")]
        public async Task<IActionResult> GetCategories()
        {
            try
            {
                var data = await ipFilterService.GetAllCategories();

                return StatusCode(StatusCodes.Status200OK, data);
            }
            catch (Exception ex)
            {
                // Log exception code goes here
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("GetCategoryById")]
        public async Task<IActionResult> GetCategoryById(int id)
        {
            try
            {
                var data = await ipFilterService.GetCategoryById(id);

                return StatusCode(StatusCodes.Status200OK, data);
            }
            catch (Exception ex)
            {
                // Log exception code goes here
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        [Route("AddCategory")]
        public async Task<IActionResult> AddCategory(string name)
        {
            try
            {
                var data = await ipFilterService.AddCategory(name);
                return StatusCode(StatusCodes.Status200OK, data);
            }
            catch (Exception ex)
            {
                // Log exception code goes here
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut]
        [Route("ModifyCategory")]
        public async Task<IActionResult> ModifyCategory(int id, string name)
        {
            try
            {
                var data = await ipFilterService.UpdateCategory(id, name);
                return StatusCode(StatusCodes.Status200OK, data);
            }
            catch (Exception ex)
            {
                // Log exception code goes here
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpDelete]
        [Route("DeleteCategory")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            try
            {
                var data = await ipFilterService.DeleteCategory(id);
                return StatusCode(StatusCodes.Status200OK, data);
            }
            catch (Exception ex)
            {
                // Log exception code goes here
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut]
        [Route("ArchiveCategory")]
        public async Task<IActionResult> ArchiveCategory(int id)
        {
            try
            {
                var data = await ipFilterService.ArchiveCategory(id);
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
