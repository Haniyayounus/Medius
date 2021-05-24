using Application.IRepository;
using Application.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiProject.Controllers
{
    [Route("api/UserIp")]
    [ApiController]
    public class UserIpController : ControllerBase
    {
        private readonly IUserIp userIpService;
        public UserIpController(IUserIp userIpService)
        {
            this.userIpService = userIpService;
        }

        //All IP
        [HttpGet]
        [Route("GetAllUserRegisteredCases")]
        public async Task<IActionResult> GetAllUserRegisteredCases(Guid userId)
        {
            try
            {
                var data = await userIpService.GetAllUserRegisteredCases(userId);

                return StatusCode(StatusCodes.Status200OK, data);
            }
            catch (Exception ex)
            {
                // Log exception code goes here
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        //Copyright
        [HttpGet]
        [Route("GetCopyrights")]
        public async Task<IActionResult> GetCopyrights()
        {
            try
            {
                var data = await userIpService.GetAllCopyrights();

                return StatusCode(StatusCodes.Status200OK, data);
            }
            catch (Exception ex)
            {
                // Log exception code goes here
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("GetCopyrightById")]
        public async Task<IActionResult> GetCopyrightById(Guid userId, Guid id)
        {
            try
            {
                var data = await userIpService.GetCopyrightById(userId, id);

                return StatusCode(StatusCodes.Status200OK, data);
            }
            catch (Exception ex)
            {
                // Log exception code goes here
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("GetUserCopyrights")]
        public async Task<IActionResult> GetUserCopyrights(Guid userId)
        {
            try
            {
                var data = await userIpService.GetUserCopyrights(userId);

                return StatusCode(StatusCodes.Status200OK, data);
            }
            catch (Exception ex)
            {
                // Log exception code goes here
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        
        [HttpDelete]
        [Route("DeleteCopyright")]
        public async Task<IActionResult> DeleteCopyright(Guid userId, Guid id)
        {
            try
            {
                var data = await userIpService.DeleteCopyright(userId, id);
                return StatusCode(StatusCodes.Status200OK, data);
            }
            catch (Exception ex)
            {
                // Log exception code goes here
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut]
        [Route("ArchiveCopyright")]
        public async Task<IActionResult> ArchiveCopyright(Guid userId, Guid id)
        {
            try
            {
                var data = await userIpService.ArchiveCopyright(userId, id);
                return StatusCode(StatusCodes.Status200OK, data);
            }
            catch (Exception ex)
            {
                // Log exception code goes here
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        [Route("AddCopyright")]
        public async Task<IActionResult> AddCopyright(string title, IFormFile imagePath, string Description, string Contact, string Application, IFormFile FileDocument, Guid ClaimId, int CityId, int IpFilterId, Guid UserId)
        {
            try
            {
                UsersIpModel model = new UsersIpModel()
                {
                    Title = title,
                    ImagePath = imagePath,
                    Description = Description,
                    Contact = Contact,
                    ClaimId = ClaimId,
                    Application = Application,
                    CityId = CityId,
                    FileDocument = FileDocument,
                    UserId = UserId,
                    IpFilterId = IpFilterId,
                    IpType = Core.IpType.Copyright
                };
                var data = await userIpService.AddCopyright(model);
                return StatusCode(StatusCodes.Status200OK, data);
            }
            catch (Exception ex)
            {
                // Log exception code goes here
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        
        
        //Trademark
        [HttpGet]
        [Route("GetTrademarks")]
        public async Task<IActionResult> GetTrademarks()
        {
            try
            {
                var data = await userIpService.GetAllTrademarks();

                return StatusCode(StatusCodes.Status200OK, data);
            }
            catch (Exception ex)
            {
                // Log exception code goes here
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("GetTrademarkById")]
        public async Task<IActionResult> GetTrademarkById(Guid userId, Guid id)
        {
            try
            {
                var data = await userIpService.GetTrademarkById(userId, id);

                return StatusCode(StatusCodes.Status200OK, data);
            }
            catch (Exception ex)
            {
                // Log exception code goes here
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("GetUserTrademarks")]
        public async Task<IActionResult> GetUserTrademarks(Guid userId)
        {
            try
            {
                var data = await userIpService.GetUserTrademarks(userId);

                return StatusCode(StatusCodes.Status200OK, data);
            }
            catch (Exception ex)
            {
                // Log exception code goes here
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        
        [HttpDelete]
        [Route("DeleteTrademark")]
        public async Task<IActionResult> DeleteTrademark(Guid userId, Guid id)
        {
            try
            {
                var data = await userIpService.DeleteTrademark(userId, id);
                return StatusCode(StatusCodes.Status200OK, data);
            }
            catch (Exception ex)
            {
                // Log exception code goes here
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut]
        [Route("ArchiveTrademark")]
        public async Task<IActionResult> ArchiveTrademark(Guid userId, Guid id)
        {
            try
            {
                var data = await userIpService.ArchiveTrademark(userId, id);
                return StatusCode(StatusCodes.Status200OK, data);
            }
            catch (Exception ex)
            {
                // Log exception code goes here
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        [Route("AddTrademark")]
        public async Task<IActionResult> AddTrademark(string title, IFormFile imagePath, string Description, string Contact, string Application, IFormFile FileDocument, Guid ClaimId, int CityId, int IpFilterId, Guid UserId)
        {
            try
            {
                UsersIpModel model = new UsersIpModel()
                {
                    Title = title,
                    ImagePath = imagePath,
                    Description = Description,
                    Contact = Contact,
                    ClaimId = ClaimId,
                    Application = Application,
                    CityId = CityId,
                    FileDocument = FileDocument,
                    UserId = UserId,
                    IpFilterId = IpFilterId,
                    IpType = Core.IpType.Trademark
                };
                var data = await userIpService.AddTrademark(model);
                return StatusCode(StatusCodes.Status200OK, data);
            }
            catch (Exception ex)
            {
                // Log exception code goes here
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }


        //Design
        [HttpGet]
        [Route("GetDesigns")]
        public async Task<IActionResult> GetDesigns()
        {
            try
            {
                var data = await userIpService.GetAllDesigns();

                return StatusCode(StatusCodes.Status200OK, data);
            }
            catch (Exception ex)
            {
                // Log exception code goes here
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("GetDesignById")]
        public async Task<IActionResult> GetDesignById(Guid userId, Guid id)
        {
            try
            {
                var data = await userIpService.GetDesignById(userId, id);

                return StatusCode(StatusCodes.Status200OK, data);
            }
            catch (Exception ex)
            {
                // Log exception code goes here
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("GetUserDesigns")]
        public async Task<IActionResult> GetUserDesigns(Guid userId)
        {
            try
            {
                var data = await userIpService.GetUserDesigns(userId);

                return StatusCode(StatusCodes.Status200OK, data);
            }
            catch (Exception ex)
            {
                // Log exception code goes here
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpDelete]
        [Route("DeleteDesign")]
        public async Task<IActionResult> DeleteDesign(Guid userId, Guid id)
        {
            try
            {
                var data = await userIpService.DeleteDesign(userId, id);
                return StatusCode(StatusCodes.Status200OK, data);
            }
            catch (Exception ex)
            {
                // Log exception code goes here
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut]
        [Route("ArchiveDesign")]
        public async Task<IActionResult> ArchiveDesign(Guid userId, Guid id)
        {
            try
            {
                var data = await userIpService.ArchiveDesign(userId, id);
                return StatusCode(StatusCodes.Status200OK, data);
            }
            catch (Exception ex)
            {
                // Log exception code goes here
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        [Route("AddDesign")]
        public async Task<IActionResult> AddDesign(string title, IFormFile imagePath, string Description, string Contact, string Application, IFormFile FileDocument, Guid ClaimId, int CityId, int IpFilterId, Guid UserId)
        {
            try
            {
                UsersIpModel model = new UsersIpModel()
                {
                    Title = title,
                    ImagePath = imagePath,
                    Description = Description,
                    Contact = Contact,
                    ClaimId = ClaimId,
                    Application = Application,
                    CityId = CityId,
                    FileDocument = FileDocument,
                    UserId = UserId,
                    IpFilterId = IpFilterId,
                    IpType = Core.IpType.Design
                };
                var data = await userIpService.AddDesign(model);
                return StatusCode(StatusCodes.Status200OK, data);
            }
            catch (Exception ex)
            {
                // Log exception code goes here
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }


        //Patent
        [HttpGet]
        [Route("GetPatents")]
        public async Task<IActionResult> GetPatents()
        {
            try
            {
                var data = await userIpService.GetAllPatents();

                return StatusCode(StatusCodes.Status200OK, data);
            }
            catch (Exception ex)
            {
                // Log exception code goes here
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("GetPatentById")]
        public async Task<IActionResult> GetPatentById(Guid userId, Guid id)
        {
            try
            {
                var data = await userIpService.GetPatentById(userId, id);

                return StatusCode(StatusCodes.Status200OK, data);
            }
            catch (Exception ex)
            {
                // Log exception code goes here
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("GetUserPatents")]
        public async Task<IActionResult> GetUserPatents(Guid userId)
        {
            try
            {
                var data = await userIpService.GetUserPatents(userId);

                return StatusCode(StatusCodes.Status200OK, data);
            }
            catch (Exception ex)
            {
                // Log exception code goes here
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpDelete]
        [Route("DeletePatent")]
        public async Task<IActionResult> DeletePatent(Guid userId, Guid id)
        {
            try
            {
                var data = await userIpService.DeletePatent(userId, id);
                return StatusCode(StatusCodes.Status200OK, data);
            }
            catch (Exception ex)
            {
                // Log exception code goes here
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut]
        [Route("ArchivePatent")]
        public async Task<IActionResult> ArchivePatent(Guid userId, Guid id)
        {
            try
            {
                var data = await userIpService.ArchivePatent(userId, id);
                return StatusCode(StatusCodes.Status200OK, data);
            }
            catch (Exception ex)
            {
                // Log exception code goes here
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        [Route("AddPatent")]
        [RequestSizeLimit(1073741824)]
        public async Task<IActionResult> AddPatent(string title, IFormFile imagePath, string Description, string Contact, string Application, IFormFile FileDocument, Guid ClaimId, int CityId, int IpFilterId, Guid UserId)
        {
            try
            {
                UsersIpModel model = new UsersIpModel()
                {
                    Title = title,
                    ImagePath = imagePath,
                    Description = Description,
                    Contact = Contact,
                    ClaimId = ClaimId,
                    Application = Application,
                    CityId = CityId,
                    FileDocument = FileDocument,
                    UserId = UserId,
                    IpFilterId = IpFilterId,
                    IpType = Core.IpType.Patent
                };
                var data = await userIpService.AddPatent(model);
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
