using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Core;
using Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Repository
{
    public sealed class CopyrightService
    {
        private readonly MediusContext _dbContext;
        private bool _disposed;
        public CopyrightService(MediusContext dbContext)
        {
            this._dbContext = dbContext;
        }

        public CopyrightService()
        {
            Dispose(false);
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

        public async Task<List<UsersIp>> GetAllCopyrightsOfUser(Guid userId)
        {
            return await _dbContext.UsersIps
                .Where(x => x.UserId == userId && x.IpType == IpType.Copyright && x.IsActive).ToListAsync();
        }

        public async Task<UsersIp> GetAllParticularCopyrightOfUser(Guid copyrightId, Guid userId)
        {
            return await _dbContext.UsersIps
                .Where(x => x.UserId == userId && x.Id == copyrightId && x.IpType == IpType.Copyright && x.IsActive).FirstOrDefaultAsync();
        }

        public async Task<HttpResponseMessage> DeleteCopyrightOfUser(Guid copyrightId, Guid userId)
        {
            HttpResponseMessage responseMessage = new HttpResponseMessage();
            var copyright = await _dbContext.UsersIps
                .Where(x => x.UserId == userId && x.Id == copyrightId && x.IpType == IpType.Copyright && x.IsActive).FirstOrDefaultAsync();
            if (copyright != null)
            {
                copyright.IsActive = false;
                await _dbContext.SaveChangesAsync();
                responseMessage.StatusCode = HttpStatusCode.OK;
                responseMessage.Content = new StringContent("Success: " + copyright.Title + "deleted successfully.");
            }
            else
            {
                responseMessage.StatusCode = HttpStatusCode.NotFound;
                responseMessage.Content = new StringContent("Not Found: This title ia already taken!");
            }
            return responseMessage;
        }

        public async Task<HttpResponseMessage> AddCopyright(UsersIp usersIp)
        {
            HttpResponseMessage responseMessage = new HttpResponseMessage();
               
            bool exist = await CheckIfExistingName(usersIp.Title);
            if (exist)
            {
                responseMessage.StatusCode = HttpStatusCode.Conflict;
                responseMessage.Content = new StringContent("Conflict: This title ia already taken!");
            }
            else
            {
                usersIp.IpType = IpType.Copyright;
                usersIp.Status = IpStatus.Registered;
                usersIp.CreatedAt = DateTime.Now;
                usersIp.LastModify = DateTime.Now;
                usersIp.IsActive = true;
                if (usersIp.Image != null)
                {
                    
                }
                else
                {
                    usersIp.Image = "Placeholder.jpg";
                }
                await _dbContext.UsersIps.AddAsync(usersIp);
                await _dbContext.SaveChangesAsync();
                responseMessage.StatusCode = HttpStatusCode.OK;
                responseMessage.Content = new StringContent("Success: " + usersIp.IpType + "registered successfully.");
            }

            return responseMessage;
        }

        public async Task<bool> CheckIfExistingName(string name)
        {
            var alreadyExist =
                await _dbContext.UsersIps.Where(x => x.Title == name && x.IsActive).ToListAsync();
            if (alreadyExist != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
