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
    public sealed class ClaimsService : IClaim
    {
        private readonly MediusContext _dbContext;
        private bool _disposed;
        
        public ClaimsService(MediusContext dbContext)
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

        public async Task<List<Claim>> GetAllClaims()
        {
            return await _dbContext.Claims.Where(x => x.IsActive).AsNoTracking().ToListAsync();
        }

        public async Task<Claim> GetClaimById(Guid id)
        {
            if (id == Guid.Empty) throw new Exception($"Claim Id is required.");

            return await _dbContext.Claims.Where(x => x.Id == id && x.IsActive).FirstOrDefaultAsync() ?? throw new Exception($"No Claim found against id:'{id}'");
        }

        public async Task<Claim> AddClaim(int claimNo, string description)
        {
            if (claimNo == 0) throw new Exception($"Claim is required. Please write a claim no.");
            if (description == null || description == "") throw new Exception($"Description is required. Please write a description.");

            if (await IsNameDuplicate(claimNo)) throw new Exception($"'{claimNo}' already exists. Please choose a different claim number.");

            Claim claim = new Claim
            {
                ClaimNo = claimNo,
                Description = description
            };
            await _dbContext.Claims.AddAsync(claim);
            await _dbContext.SaveChangesAsync();
            return claim;
        }

        public async Task<Claim> UpdateClaim(Guid id, int claimNo, string description)
        {
            var claim = await _dbContext.Claims.FirstOrDefaultAsync(x => x.Id == id) ?? throw new Exception($"No Claim found against id:'{id}'");
            
            if (claimNo == 0) throw new Exception($"Claim is required. Please write a claim no.");
            if (description == null || description == "") throw new Exception($"Description is required. Please write a description.");

            if (await IsNameDuplicate(id, claimNo)) throw new Exception($"'{claimNo}' already exists. Please choose a different number.");

            claim.ClaimNo = claimNo;
            claim.Description = description;
            claim.LastModify = DateTime.Now;
            await _dbContext.SaveChangesAsync();
            return claim;
        }

        public async Task<Claim> DeleteClaim(Guid id)
        {
            var claim = await GetClaimById(id);
            _dbContext.Remove(claim);
            await _dbContext.SaveChangesAsync();
            return claim;
        }

        public async Task<Claim> ArchiveClaim(Guid id)
        {
            var claim = await GetClaimById(id);
            claim.IsActive = false;
            claim.LastModify = DateTime.Now;
            await _dbContext.SaveChangesAsync();
            return claim;
        }

        public Task<bool> IsNameDuplicate(int claimNo)
            => _dbContext.Claims.AnyAsync(x => x.ClaimNo.Equals(claimNo));

        public Task<bool> IsNameDuplicate(Guid id, int claimNo)
             => _dbContext.Claims.AnyAsync(x => x.ClaimNo.Equals(claimNo) && !x.Id.Equals(id));
    }
}
