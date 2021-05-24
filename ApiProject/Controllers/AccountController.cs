using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Configuration;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.TwiML;
using Twilio.AspNet.Core;
using Application.IRepository;
using Core.Entities;
using Microsoft.Extensions.Configuration;
using Application.ViewModels;

namespace ApiProject.Controllers
{
    [Route("api/Account")]
    [ApiController]
    public class AccountController : TwilioController
    {
        private readonly IAccount account;
        private readonly IConfiguration _config;
        public AccountController(IAccount account, IConfiguration config)
        {
            this.account = account;
            this._config = config;
        }


        //Login Functionality
        [HttpPost]
        [Route("SignUp")]
        public async Task<IActionResult> SignUp(string firstName, string lastName, string email, string pass, string contact, string cnic)
        {
            try
            {
                User user = new User
                {
                    FirstName = firstName,
                    LastName = lastName,
                    Email = email,
                    Password = pass,
                    Contact = contact,
                    Cnic = cnic
                };

                var data = await account.SignUp(user);
                return StatusCode(StatusCodes.Status200OK, data);
            }
            catch (Exception ex)
            {
                // Log exception code goes here
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login(string email, string pass)
        {
            try
            {
                var data = await account.Login(email, pass);
                return StatusCode(StatusCodes.Status200OK, data);
            }
            catch (Exception ex)
            {
                // Log exception code goes here
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("GetProfile")]
        public async Task<IActionResult> GetProfile(Guid id)
        {
            try
            {
                var data = await account.GetProfile(id);
                return StatusCode(StatusCodes.Status200OK, data);
            }
            catch (Exception ex)
            {
                // Log exception code goes here
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut]
        [Route("ModifyProfile")]
        public async Task<IActionResult> ModifyProfile(Guid id, string firstName, string lastName, string email, string contact, string cnic, IFormFile profilePicture)
        {
            try
            {
                UserModel user = new UserModel
                {
                    Id = id,
                    FirstName = firstName,
                    LastName = lastName,
                    Email = email,
                    Cnic = cnic,
                    Contact = contact,
                    ProfilePicture = profilePicture,
                    RoleId = 1
                };
                var data = await account.ModifyProfile(user);
                return StatusCode(StatusCodes.Status200OK, data);
            }
            catch (Exception ex)
            {
                // Log exception code goes here
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpDelete]
        [Route("DeleteMyAccount")]
        public async Task<IActionResult> DeleteMyAccount(Guid id)
        {
            try
            {
                var data = await account.DeleteMyAccount(id);
                return StatusCode(StatusCodes.Status200OK, data);
            }
            catch (Exception ex)
            {
                // Log exception code goes here
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut]
        [Route("DeactivateMyAccount")]
        public async Task<IActionResult> DeactivateMyAccount(Guid id)
        {
            try
            {
                var data = await account.DeactivateMyAccount(id);
                return StatusCode(StatusCodes.Status200OK, data);
            }
            catch (Exception ex)
            {
                // Log exception code goes here
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }



        [HttpGet]
        [Route("GetUsers")]
        public async Task<IActionResult> GetUsers()
        {
            try
            {
                var data = await account.GetAllUsers();

                return StatusCode(StatusCodes.Status200OK, data);
            }
            catch (Exception ex)
            {
                // Log exception code goes here
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }


        //Two Factor Authentication
        [HttpGet]
        [Route("SendUserOtp")]
        public async Task<IActionResult> SendUserOtp(Guid id)
        {
            try
            {
                var data = await account.GetUserById(id);

               
                var accountSid = _config.GetValue<string>("Twilio:TwilioAccountSid");
                var authToken = _config.GetValue<string>("Twilio:TwilioAuthToken");
                var from = _config.GetValue<string>("Twilio:Phone-no");
                var to = data.Contact;
                TwilioClient.Init(accountSid, authToken);

                var message = await MessageResource.CreateAsync
                    (
                    to : to,
                    from : from,
                    body : "Your medius security code is " + data.OTP
                    );

                return StatusCode(StatusCodes.Status200OK, data);
            }
            catch (Exception ex)
            {
                // Log exception code goes here
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        [Route("AuthenticateUser")]
        public async Task<IActionResult> AuthenticateUser(Guid id, int OTP)
        {
            try
            {
                var data = await account.ValidateOTP(id, OTP);
                return StatusCode(StatusCodes.Status200OK, data);
            }
            catch(Exception ex)
            {
                // Log exception code goes here
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }



        //Password
        [HttpPost]
        [Route("ForgetPassword")]
        public async Task<IActionResult> ForgetPassword(string email)
        {
            try
            {
                var data = await account.ForgetPassword(email);
                return StatusCode(StatusCodes.Status200OK, data);
            }
            catch (Exception ex)
            {
                // Log exception code goes here
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        [Route("ValidateResetPasswordCode")]
        public async Task<IActionResult> ValidateResetPasswordCode(Guid id, long resetCode)
        {
            try
            {
                var data = await account.ValidateResetCode(id, resetCode);
                return StatusCode(StatusCodes.Status200OK, data);
            }
            catch (Exception ex)
            {
                // Log exception code goes here
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        [Route("ResetPassword")]
        public async Task<IActionResult> ResetPassword(Guid id, string newPass)
        {
            try
            {
                var data = await account.ResetPassword(id, newPass);
                return StatusCode(StatusCodes.Status200OK, data);
            }
            catch (Exception ex)
            {
                // Log exception code goes here
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut]
        [Route("ChangePassword")]
        public async Task<IActionResult> ChangePassword(Guid id, string pass, string newPass)
        {
            try
            {
                var data = await account.ChangePassword(id, pass, newPass);
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
