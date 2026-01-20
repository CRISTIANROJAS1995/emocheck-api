using Domain.Entities;
using Domain.Interfaces.Repositories;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class ExamRepository : GenericRepository<Exam>, IExamRepository
    {
        public ExamRepository(ApplicationDbContext context) : base(context) { }

        public async Task<Exam?> GetExamByIdAsync(int id)
        {
            return await _context.Exam
                .Include(u => u.ExamType)
                .FirstOrDefaultAsync(u => u.ExamID == id);
        }

        public async Task<List<Exam>> GetAllExamByTypeAsync(int examType)
        {
            return await _context.Exam
                .Include(u => u.ExamType)
                .Where(u => u.ExamTypeID == examType)
                .ToListAsync();
        }

        public async Task<Exam?> GetExamByTemplateIdAsync(string templateId)
        {
            return await _context.Exam
                .Include(u => u.ExamType)
                .FirstOrDefaultAsync(u => u.ExternalTemplateId == templateId);
        }

        public async Task<Exam?> GetExamByTemplateIdAndCompanyAsync(string templateId)
        {
            return await _context.Exam
                .Include(u => u.ExamType)
                .FirstOrDefaultAsync(u => u.ExternalTemplateId == templateId);
        }

        public async Task<Exam?> GetExamByTemplateIdAndCompanyAsync(string templateId, string company)
        {
            return await _context.Exam
                .Include(u => u.ExamType)
                .FirstOrDefaultAsync(u => u.ExternalTemplateId == templateId && u.Company == company);
        }

        public async Task<Exam?> GetExamByNameAsync(string name)
        {
            return await _context.Exam
                .Include(u => u.ExamType)
                .FirstOrDefaultAsync(u => u.ExternalExamName == name);
        }

        public async Task<Exam?> GetExamByLocaleAsync(string locale)
        {
            return await _context.Exam
                .Include(u => u.ExamType)
                .FirstOrDefaultAsync(u => u.ExternalLocale == locale);
        }

        public async Task<List<Exam>> GetAllExamByCustomerIdAsync(string customerId)
        {
            return await _context.Exam
                .Include(u => u.ExamType)
                .Where(u => u.ExternalCustomerId == customerId)
                .ToListAsync();
        }

        public async Task<List<Exam>> GetAllExamByCompanyAsync(string company)
        {
            return await _context.Exam
                .Include(u => u.ExamType)
                .Where(u => u.Company == company)
                .ToListAsync();
        }

        public async Task<List<Exam>> GetAllExamAsync()
        {
            return await _context.Exam
                .Include(u => u.ExamType)
                .ToListAsync();
        }
    }
}
