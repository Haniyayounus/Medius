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
    public sealed class FaqService : IFaq
    {
        private readonly MediusContext _dbContext;
        private bool _disposed;

        public FaqService(MediusContext dbContext)
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

        public async Task<List<FAQ>> GetAllFAQs()
        {
            return await _dbContext.FAQs.Where(x => x.IsActive).AsNoTracking().ToListAsync();
        }

        public async Task<FAQ> GetFAQById(Guid id)
        {
            if (id == Guid.Empty) throw new Exception($"Question Id is required.");

            return await _dbContext.FAQs.Where(x => x.Id == id && x.IsActive).FirstOrDefaultAsync() ?? throw new Exception($"No Question found against id:'{id}'");
        }

        public async Task<FAQ> AddFAQ(string question, string answer)
        {
            if (question == null || question == "") throw new Exception($"Question is required. Please write a question.");
            if (answer == null || answer == "") throw new Exception($"Answer is required. Please write an answer.");
            
            if (await IsQuestionDuplicate(question)) throw new Exception($"'{question}' already exists. Please choose a different question.");
            
            FAQ faq = new FAQ
            {
                Question = question,
                Answer = answer
            };
            await _dbContext.FAQs.AddAsync(faq);
            await _dbContext.SaveChangesAsync();
            return faq;
        }

        public async Task<FAQ> UpdateFAQ(Guid id, string question, string answer)
        {
            var faq = await _dbContext.FAQs.FirstOrDefaultAsync(x => x.Id == id) ?? throw new Exception($"No FAQ found against id:'{id}'");

            if (question == null || question == "") throw new Exception($"Question is required. Please write a question.");
            if (answer == null || answer == "") throw new Exception($"Answer is required. Please write an answer.");

            if (await IsQuestionDuplicate(id, question)) throw new Exception($"'{question}' already exists. Please choose a different name.");
            
            faq.Question = question;
            faq.Answer = answer;
            faq.LastModify = DateTime.Now;
            await _dbContext.SaveChangesAsync();
            return faq;
        }

        public async Task<FAQ> DeleteFAQ(Guid id)
        {
            var faq = await GetFAQById(id);
            _dbContext.Remove(faq);
            await _dbContext.SaveChangesAsync();
            return faq;
        }

        public async Task<FAQ> ArchiveFAQ(Guid id)
        {
            var faq = await GetFAQById(id);
            faq.IsActive = false;
            faq.LastModify = DateTime.Now;
            await _dbContext.SaveChangesAsync();
            return faq;
        }

        public Task<bool> IsQuestionDuplicate(string question)
            => _dbContext.FAQs.AnyAsync(x => x.Question.ToLower().Equals(question.ToLower()));

        public Task<bool> IsQuestionDuplicate(Guid id, string question)
             => _dbContext.FAQs.AnyAsync(x => x.Question.ToLower().Equals(question.ToLower()) && !x.Id.Equals(id));
    }
}
