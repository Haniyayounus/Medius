using Application.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiProject.Controllers
{
    [Route("api/Claim")]
    [ApiController]
    public class ClaimController : ControllerBase
    {
        private readonly IClaim claimService;
        public ClaimController(IClaim claimService)
        {
            this.claimService = claimService;
        }

        [HttpGet]
        [Route("GetClaims")]
        public async Task<IActionResult> GetClaims()
        {
            try
            {
                var data = await claimService.GetAllClaims();

                return StatusCode(StatusCodes.Status200OK, data);
            }
            catch (Exception ex)
            {
                // Log exception code goes here
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("GetClaimById")]
        public async Task<IActionResult> GetClaimById(Guid id)
        {
            try
            {
                var data = await claimService.GetClaimById(id);

                return StatusCode(StatusCodes.Status200OK, data);
            }
            catch (Exception ex)
            {
                // Log exception code goes here
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        [Route("AddClaim")]
        public async Task<IActionResult> AddClaim(int claimNo, string description)
        {
            try
            {
                var data = await claimService.AddClaim(claimNo, description);
                return StatusCode(StatusCodes.Status200OK, data);
            }
            catch (Exception ex)
            {
                // Log exception code goes here
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut]
        [Route("ModifyClaim")]
        public async Task<IActionResult> ModifyClaim(Guid id, int claimNo, string description)
        {
            try
            {
                var data = await claimService.UpdateClaim(id, claimNo, description);
                return StatusCode(StatusCodes.Status200OK, data);
            }
            catch (Exception ex)
            {
                // Log exception code goes here
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpDelete]
        [Route("DeleteClaim")]
        public async Task<IActionResult> DeleteClaim(Guid id)
        {
            try
            {
                var data = await claimService.DeleteClaim(id);
                return StatusCode(StatusCodes.Status200OK, data);
            }
            catch (Exception ex)
            {
                // Log exception code goes here
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut]
        [Route("ArchiveClaim")]
        public async Task<IActionResult> ArchiveClaim(Guid id)
        {
            try
            {
                var data = await claimService.ArchiveClaim(id);
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
