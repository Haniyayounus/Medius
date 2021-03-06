using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core;
using Core.Entities;
using System.Data;
using Microsoft.EntityFrameworkCore;
using Application.IRepository;
using Application.ViewModels;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.IdentityModel.Protocols;
using Microsoft.AspNetCore.Http;

namespace Application.Repository
{
    public sealed class UserIpService : IUserIp
    {
        private readonly MediusContext _dbContext;
        private bool _disposed;
        private readonly IHostingEnvironment _env;

        public UserIpService(MediusContext dbContext, IHostingEnvironment _env)
        {
            this._dbContext = dbContext;
            this._env = _env;
        }
        public UserIpService()
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

        public async Task<List<UsersIp>> GetAllUserRegisteredCases(Guid userId)
        {
            return await _dbContext.UsersIps.Where(x => x.UserId == userId && x.IsActive).AsNoTracking().ToListAsync();
        }

        //Copyright
        public async Task<List<UsersIp>> GetAllCopyrights()
        {
            return await _dbContext.UsersIps.Where(x => x.IpType == IpType.Copyright && x.IsActive).AsNoTracking().ToListAsync();
        }
        public async Task<List<UsersIp>> GetUserCopyrights(Guid userId)
        {
            if (userId == Guid.Empty) throw new Exception($"User Id is required.");

            return await _dbContext.UsersIps.Where(x => x.UserId == userId && x.IpType == IpType.Copyright && x.IsActive).ToListAsync();
        }
        public async Task<UsersIp> GetCopyrightById(Guid userId, Guid id)
        {
            if (id == Guid.Empty) throw new Exception($"Copyright Id is required.");
            if (userId == Guid.Empty) throw new Exception($"User Id is required.");

            return await _dbContext.UsersIps.Where(x => x.UserId == userId && x.Id == id && x.IpType == IpType.Copyright && x.IsActive).FirstOrDefaultAsync() ?? throw new Exception($"No Question found against id:'{id}'"); ;
        }
        public async Task<UsersIp> DeleteCopyright(Guid userId, Guid id)
        {
            var copyright = await GetCopyrightById(userId, id);
            _dbContext.Remove(copyright);
            await _dbContext.SaveChangesAsync();
            return copyright;
        }
        public async Task<UsersIp> ArchiveCopyright(Guid userId, Guid id)
        {
            var copyright = await GetCopyrightById(userId, id);
            copyright.IsActive = false;
            copyright.LastModify = DateTime.Now;
            await _dbContext.SaveChangesAsync();
            return copyright;
        }
        public async Task<UsersIp> AddCopyright(UsersIpModel copyrightModel)
        {
            if (copyrightModel.Title == null || copyrightModel.Title == "") throw new Exception($"Title is required. Please write a Title.");
            if (await IsTitleDuplicate(copyrightModel.Title)) throw new Exception($"'{copyrightModel.Title}' already exists. Please choose a different title.");

            var image = await UploadedImage(copyrightModel.IpType, copyrightModel.ImagePath);
            if (image == null) throw new Exception($"Image is required.");

            var document = await UploadFile(copyrightModel.IpType, copyrightModel.FileDocument);
            if (document == null) throw new Exception($"File is required.");

            UsersIp copyright = new UsersIp
            {
                Title = copyrightModel.Title,
                Description = copyrightModel.Description,
                IpType = IpType.Copyright,
                Contact = copyrightModel.Contact,
                Application = copyrightModel.Application,
                Status = IpStatus.Registered,
                ClaimId = copyrightModel.ClaimId,
                CityId = copyrightModel.CityId,
                UserId = copyrightModel.UserId,
                IpFilterId = copyrightModel.IpFilterId,
                ImagePath = image.ImagePath,
                FileDocument = document.FileDocument,
                DocumentPath = document.DocumentPath
            };
            await _dbContext.UsersIps.AddAsync(copyright);
            await _dbContext.SaveChangesAsync();
            return copyright;
        }

        //Trademark
        public async Task<List<UsersIp>> GetAllTrademarks()
        {
            return await _dbContext.UsersIps.Where(x => x.IpType == IpType.Trademark && x.IsActive).AsNoTracking().ToListAsync();
        }
        public async Task<List<UsersIp>> GetUserTrademarks(Guid userId)
        {
            if (userId == Guid.Empty) throw new Exception($"User Id is required.");

            return await _dbContext.UsersIps.Where(x => x.UserId == userId && x.IpType == IpType.Trademark && x.IsActive).AsNoTracking().ToListAsync();
        }
        public async Task<UsersIp> GetTrademarkById(Guid userId, Guid id)
        {
            if (userId == Guid.Empty) throw new Exception($"User Id is required.");
            if (id == Guid.Empty) throw new Exception($"Trademark Id is required.");

            return await _dbContext.UsersIps.Where(x => x.UserId == userId && x.Id == id && x.IpType == IpType.Trademark && x.IsActive).FirstOrDefaultAsync() ?? throw new Exception($"No Trademark found against id:'{id}'"); ;
        }
        public async Task<UsersIp> DeleteTrademark(Guid userId, Guid id)
        {
            var trademark = await GetTrademarkById(userId, id);
            _dbContext.Remove(trademark);
            await _dbContext.SaveChangesAsync();
            return trademark;
        }
        public async Task<UsersIp> ArchiveTrademark(Guid userId, Guid id)
        {
            var trademark = await GetTrademarkById(userId, id);
            trademark.IsActive = false;
            trademark.LastModify = DateTime.Now;
            await _dbContext.SaveChangesAsync();
            return trademark;
        }
        public async Task<UsersIp> AddTrademark(UsersIpModel trademarkModel)
        {
            if (trademarkModel.Title == null || trademarkModel.Title == "") throw new Exception($"Title is required. Please write a Title.");
            if (await IsTitleDuplicate(trademarkModel.Title)) throw new Exception($"'{trademarkModel.Title}' already exists. Please choose a different title.");

            var image = await UploadedImage(trademarkModel.IpType, trademarkModel.ImagePath);
            if (image == null) throw new Exception($"Image is required.");

            var document = await UploadFile(trademarkModel.IpType, trademarkModel.FileDocument);
            if (document == null) throw new Exception($"File is required.");

            UsersIp trademark = new UsersIp
            {
                Title = trademarkModel.Title,
                Description = trademarkModel.Description,
                IpType = IpType.Trademark,
                Contact = trademarkModel.Contact,
                Application = trademarkModel.Application,
                Status = IpStatus.Registered,
                ClaimId = trademarkModel.ClaimId,
                CityId = trademarkModel.CityId,
                IpFilterId = trademarkModel.IpFilterId,
                UserId = trademarkModel.UserId,
                ImagePath = image.ImagePath,
                FileDocument = document.FileDocument,
                DocumentPath = document.DocumentPath
            };
            await _dbContext.UsersIps.AddAsync(trademark);
            await _dbContext.SaveChangesAsync();
            return trademark;
        }

        //Design
        public async Task<List<UsersIp>> GetAllDesigns()
        {
            return await _dbContext.UsersIps.Where(x => x.IpType == IpType.Design && x.IsActive).AsNoTracking().ToListAsync();
        }
        public async Task<List<UsersIp>> GetUserDesigns(Guid userId)
        {
            if (userId == Guid.Empty) throw new Exception($"User Id is required.");

            return await _dbContext.UsersIps.Where(x => x.UserId == userId && x.IpType == IpType.Design && x.IsActive).AsNoTracking().ToListAsync();
        }
        public async Task<UsersIp> GetDesignById(Guid userId, Guid id)
        {
            if (userId == Guid.Empty) throw new Exception($"User Id is required.");
            if (id == Guid.Empty) throw new Exception($"Design Id is required.");

            return await _dbContext.UsersIps.Where(x => x.UserId == userId && x.Id == id && x.IpType == IpType.Design && x.IsActive).FirstOrDefaultAsync() ?? throw new Exception($"No Design found against id:'{id}'"); ;
        }
        public async Task<UsersIp> DeleteDesign(Guid userId, Guid id)
        {
            var design = await GetDesignById(userId, id);
            _dbContext.Remove(design);
            await _dbContext.SaveChangesAsync();
            return design;
        }
        public async Task<UsersIp> ArchiveDesign(Guid userId, Guid id)
        {
            var design = await GetDesignById(userId, id);
            design.IsActive = false;
            design.LastModify = DateTime.Now;
            await _dbContext.SaveChangesAsync();
            return design;
        }
        public async Task<UsersIp> AddDesign(UsersIpModel designModel)
        {
            if (designModel.Title == null || designModel.Title == "") throw new Exception($"Title is required. Please write a Title.");
            if (await IsTitleDuplicate(designModel.Title)) throw new Exception($"'{designModel.Title}' already exists. Please choose a different title.");

            var image = await UploadedImage(designModel.IpType, designModel.ImagePath);
            if (image == null) throw new Exception($"Image is required.");

            var document = await UploadFile(designModel.IpType, designModel.FileDocument);
            if (document == null) throw new Exception($"File is required.");

            UsersIp design = new UsersIp
            {
                Title = designModel.Title,
                Description = designModel.Description,
                IpType = IpType.Design,
                Contact = designModel.Contact,
                Application = designModel.Application,
                Status = IpStatus.Registered,
                ClaimId = designModel.ClaimId,
                CityId = designModel.CityId,
                UserId = designModel.UserId,
                IpFilterId = designModel.IpFilterId,
                ImagePath = image.ImagePath,
                FileDocument = document.FileDocument,
                DocumentPath = document.DocumentPath
            };
            await _dbContext.UsersIps.AddAsync(design);
            await _dbContext.SaveChangesAsync();
            return design;
        }

        //Patent
        public async Task<List<UsersIp>> GetAllPatents()
        {
            return await _dbContext.UsersIps.Where(x => x.IpType == IpType.Patent && x.IsActive).AsNoTracking().ToListAsync();
        }
        public async Task<List<UsersIp>> GetUserPatents(Guid userId)
        {
            if (userId == Guid.Empty) throw new Exception($"User Id is required.");

            return await _dbContext.UsersIps.Where(x => x.UserId == userId && x.IpType == IpType.Patent && x.IsActive).AsNoTracking().ToListAsync();
        }
        public async Task<UsersIp> GetPatentById(Guid userId, Guid id)
        {
            if (id == Guid.Empty) throw new Exception($"Patent Id is required.");
            if (userId == Guid.Empty) throw new Exception($"User Id is required.");

            return await _dbContext.UsersIps.Where(x => x.UserId == userId && x.Id == id && x.IpType == IpType.Patent && x.IsActive).FirstOrDefaultAsync() ?? throw new Exception($"No Patent found against id:'{id}'"); ;
        }
        public async Task<UsersIp> DeletePatent(Guid userId, Guid id)
        {
            var patent = await GetPatentById(userId, id);
            _dbContext.Remove(patent);
            await _dbContext.SaveChangesAsync();
            return patent;
        }
        public async Task<UsersIp> ArchivePatent(Guid userId, Guid id)
        {
            var patent = await GetPatentById(userId, id);
            patent.IsActive = false;
            patent.LastModify = DateTime.Now;
            await _dbContext.SaveChangesAsync();
            return patent;
        }
        public async Task<UsersIp> AddPatent(UsersIpModel patentModel)
        {
            if (patentModel.Title == null || patentModel.Title == "") throw new Exception($"Title is required. Please write a Title.");
            if (await IsTitleDuplicate(patentModel.Title)) throw new Exception($"'{patentModel.Title}' already exists. Please choose a different title.");

            var image = await UploadedImage(patentModel.IpType, patentModel.ImagePath);
            if (image == null) throw new Exception($"Image is required.");

            var document = await UploadFile(patentModel.IpType, patentModel.FileDocument);
            if (document == null) throw new Exception($"File is required.");

            UsersIp patent = new UsersIp
            {
                Title = patentModel.Title,
                Description = patentModel.Description,
                IpType = IpType.Patent,
                Contact = patentModel.Contact,
                Application = patentModel.Application,
                Status = IpStatus.Registered,
                ClaimId = patentModel.ClaimId,
                CityId = patentModel.CityId,
                UserId = patentModel.UserId,
                IpFilterId = patentModel.IpFilterId,
                ImagePath = image.ImagePath,
                FileDocument = document.FileDocument,
                DocumentPath = document.DocumentPath
            };
            await _dbContext.UsersIps.AddAsync(patent);
            await _dbContext.SaveChangesAsync();
            return patent;
        }


        //Ttile duplicate
        public Task<bool> IsTitleDuplicate(string title)
            => _dbContext.UsersIps.AnyAsync(x => x.Title.ToLower().Equals(title.ToLower()));

        public Task<bool> IsTitleDuplicate(Guid id, string title)
             => _dbContext.UsersIps.AnyAsync(x => x.Title.ToLower().Equals(title.ToLower()) && !x.Id.Equals(id));

        public string EncodeImageToBase64(IpType ipType, string image, string title)
        {
            string filePath = null;
            if (ipType == IpType.Patent)
                filePath = Path.Combine(_env.ContentRootPath, "Files", "Images", "Patent");
            else if (ipType == IpType.Copyright)
                filePath = Path.Combine(_env.ContentRootPath, "Files", "Images", "Copyright");
            else if (ipType == IpType.Trademark)
                filePath = Path.Combine(_env.ContentRootPath, "Files", "Images", "Trademark");
            else if (ipType == IpType.Design)
                filePath = Path.Combine(_env.ContentRootPath, "Files", "Images", "Design");

            if (image != null)
            {
                File.WriteAllBytes(filePath + "" + title + ".png", Convert.FromBase64String(image));
            }
            return image;
        }

        public async Task<Document> UploadFile(IpType ipType, IFormFile file)
        {
            var filePath = "";
            if (ipType == IpType.Patent)
                filePath = Path.Combine(_env.ContentRootPath, "Files", "Document", "Patent");
            else if (ipType == IpType.Copyright)
                filePath = Path.Combine(_env.ContentRootPath, "Files", "Document", "Copyright");
            else if (ipType == IpType.Trademark)
                filePath = Path.Combine(_env.ContentRootPath, "Files", "Document", "Trademark");
            else if (ipType == IpType.Design)
                filePath = Path.Combine(_env.ContentRootPath, "Files", "Document", "Design");

            string fileName;
            Document document = new Document();
            try
            {
                var extension = "." + file.FileName.Split('.')[file.FileName.Split('.').Length - 1];
                //fileName = DateTime.Now.Ticks + extension; //Create a new Name for the file due to security reasons.
                fileName = file.FileName + extension;
                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                }

                var path = Path.Combine(filePath, fileName);

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                document = new Document
                {
                    DocumentPath = path,
                    FileDocument = file
                };
            }
            catch (Exception e)
            {
                //log error
                e.Message.ToString();
            }

            return document;
        }

        //Image upload
        public async Task<Images> UploadedImage(IpType ipType, IFormFile image)
        {
            string filePath = null;
            if (ipType == IpType.Patent)
                filePath = Path.Combine(_env.ContentRootPath, "Files", "Images", "Patent");
            else if (ipType == IpType.Copyright)
                filePath = Path.Combine(_env.ContentRootPath, "Files", "Images", "Copyright");
            else if (ipType == IpType.Trademark)
                filePath = Path.Combine(_env.ContentRootPath, "Files", "Images", "Trademark");
            else if (ipType == IpType.Design)
                filePath = Path.Combine(_env.ContentRootPath, "Files", "Images", "Design");
            string fileName;
            Images images = new Images();

            try
            {
                var extension = "." + image.FileName.Split('.')[image.FileName.Split('.').Length - 1];
                fileName = image.FileName + extension;

                string uniqueFileName = null;

                if (image != null)
                {
                    string uploadsFolder = Path.Combine(_env.WebRootPath, filePath);
                    uniqueFileName = Guid.NewGuid().ToString() + "_" + image.FileName;
                    filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        image.CopyTo(fileStream);
                    }
                }

                images = new Images
                {
                    ImagePath = filePath,
                    Image = image
                };
            }
            catch (Exception e)
            {
                //log error
                e.Message.ToString();
            }

            return images;
        }

        //delete an type of ip
        public async Task<UsersIp> DeleteIP(Guid userId, Guid id)
        {
            var ip = await GetIPById(userId, id);
            _dbContext.Remove(ip);
            await _dbContext.SaveChangesAsync();
            return ip;
        }
        public async Task<UsersIp> GetIPById(Guid userId, Guid id)
        {
            if (id == Guid.Empty) throw new Exception($"IP Id is required.");
            if (userId == Guid.Empty) throw new Exception($"User Id is required.");

            return await _dbContext.UsersIps.Where(x => x.UserId == userId && x.Id == id && x.IsActive).FirstOrDefaultAsync() ?? throw new Exception($"No IP found against id:'{id}'"); ;
        }

    }
}
