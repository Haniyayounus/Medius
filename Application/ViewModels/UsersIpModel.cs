using Core;
using Microsoft.AspNetCore.Http;
using System;

namespace Application.ViewModels
{
    public class UsersIpModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public IpType IpType { get; set; }
        public string Contact { get; set; }
        public string Application { get; set; }
        public IpStatus Status { get; set; }
        public string Image { get; set; }
        public IFormFile FileDocument { get; set; }
        public Guid ClaimId { get; set; }
        public int CityId { get; set; }
        public int IpFilterId { get; set; }
        public Guid UserId { get; set; }
    }
    public class Document
    {
        public IFormFile FileDocument { get; set; }
        public string DocumentPath { get; set; }
    }
}
