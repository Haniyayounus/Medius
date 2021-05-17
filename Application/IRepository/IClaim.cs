using Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Application.IRepository
{
    public interface IClaim
    {
        Task<List<Claim>> GetAllClaims();
        Task<Claim> GetClaimById(Guid id);
        Task<Claim> AddClaim(int claimNo, string description);
        Task<Claim> UpdateClaim(Guid id, int claimNo, string description);
        Task<Claim> DeleteClaim(Guid id);
        Task<Claim> ArchiveClaim(Guid id);
        Task<bool> IsNameDuplicate(int claimNo);
        Task<bool> IsNameDuplicate(Guid id, int claimNo);
    }
}
