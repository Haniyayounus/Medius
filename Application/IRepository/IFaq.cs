using Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Application.IRepository
{
    public interface IFaq
    {
        Task<List<FAQ>> GetAllFAQs();
        Task<FAQ> GetFAQById(Guid id);
        Task<FAQ> AddFAQ(string question, string answer);
        Task<FAQ> UpdateFAQ(Guid id, string question, string answer);
        Task<FAQ> DeleteFAQ(Guid id);
        Task<FAQ> ArchiveFAQ(Guid id);
        Task<bool> IsQuestionDuplicate(string question);
        Task<bool> IsQuestionDuplicate(Guid id, string question);
    }
}
