using Application.ViewModels;
using Core;
using Core.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Application.IRepository
{
    public interface IUserIp
    {
        //All IP
        Task<List<UsersIp>> GetAllUserRegisteredCases(Guid userId);

        //Copyright
        Task<List<UsersIp>> GetAllCopyrights();
        Task<List<UsersIp>> GetUserCopyrights(Guid userId);
        Task<UsersIp> GetCopyrightById(Guid userId, Guid id);
        Task<UsersIp> DeleteCopyright(Guid userId, Guid id);
        Task<UsersIp> ArchiveCopyright(Guid userId, Guid id);
        Task<UsersIp> AddCopyright(UsersIpModel copyrightModel);

        //Trademark
        Task<List<UsersIp>> GetAllTrademarks();
        Task<List<UsersIp>> GetUserTrademarks(Guid userId);
        Task<UsersIp> GetTrademarkById(Guid userId, Guid id);
        Task<UsersIp> DeleteTrademark(Guid userId, Guid id);
        Task<UsersIp> ArchiveTrademark(Guid userId, Guid id);
        Task<UsersIp> AddTrademark(UsersIpModel trademarkModel);

        //Design
        Task<List<UsersIp>> GetAllDesigns();
        Task<List<UsersIp>> GetUserDesigns(Guid userId);
        Task<UsersIp> GetDesignById(Guid userId, Guid id);
        Task<UsersIp> DeleteDesign(Guid userId, Guid id);
        Task<UsersIp> ArchiveDesign(Guid userId, Guid id);
        Task<UsersIp> AddDesign(UsersIpModel designModel);

        //Patent
        Task<List<UsersIp>> GetAllPatents();
        Task<List<UsersIp>> GetUserPatents(Guid userId);
        Task<UsersIp> GetPatentById(Guid userId, Guid id);
        Task<UsersIp> DeletePatent(Guid userId, Guid id);
        Task<UsersIp> ArchivePatent(Guid userId, Guid id);
        Task<UsersIp> AddPatent(UsersIpModel patentModel);

        //methods
        Task<bool> IsTitleDuplicate(string title);
        Task<bool> IsTitleDuplicate(Guid id, string title);
        string EncodeImageToBase64(IpType ipType, string image, string title);
        Task<Document> UploadFile(IpType ipType, IFormFile file);
        string UploadedImage(IpType ipType, IFormFile image);
    }
}
