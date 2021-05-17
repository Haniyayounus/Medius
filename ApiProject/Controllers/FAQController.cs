using Application.IRepository;
using Application.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace ApiProject.Controllers
{
    [Route("api/FAQ/")]
    [ApiController]
    public class FAQController : ControllerBase
    {
        private readonly IFaq faqService;
        public FAQController(IFaq faqService)
        {
            this.faqService = faqService;
        }

        [HttpGet]
        [Route("GetFAQs")]
        public async Task<IActionResult> GetFAQs()
        {
            try
            {
                var data = await faqService.GetAllFAQs();

                return StatusCode(StatusCodes.Status200OK, data);
            }
            catch (Exception ex)
            {
                // Log exception code goes here
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("GetFaqById")]
        public async Task<IActionResult> GetFaqById(Guid id)
        {
            try
            {
                var data = await faqService.GetFAQById(id);

                return StatusCode(StatusCodes.Status200OK, data);
            }
            catch (Exception ex)
            {
                // Log exception code goes here
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        [Route("AddFAQ")]
        public async Task<IActionResult> AddFAQ(string question, string answer)
        {
            try
            {
                var data = await faqService.AddFAQ(question,answer);
                return StatusCode(StatusCodes.Status200OK, data);
            }
            catch (Exception ex)
            {
                // Log exception code goes here
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut]
        [Route("ModifyFAQ")]
        public async Task<IActionResult> ModifyFAQ(Guid id, string question, string answer)
        {
            try
            {
                var data = await faqService.UpdateFAQ(id, question, answer);
                return StatusCode(StatusCodes.Status200OK, data);
            }
            catch (Exception ex)
            {
                // Log exception code goes here
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpDelete]
        [Route("DeleteFAQ")]
        public async Task<IActionResult> DeleteFAQ(Guid id)
        {
            try
            {
                var data = await faqService.DeleteFAQ(id);
                return StatusCode(StatusCodes.Status200OK, data);
            }
            catch (Exception ex)
            {
                // Log exception code goes here
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut]
        [Route("ArchiveFAQ")]
        public async Task<IActionResult> ArchiveFAQ(Guid id)
        {
            try
            {
                var data = await faqService.ArchiveFAQ(id);
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
