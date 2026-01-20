using Domain.Entities;
using Domain.Interfaces.Repositories;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class ExamResultRepository : GenericRepository<ExamResult>, IExamResultRepository
    {
        public ExamResultRepository(ApplicationDbContext context) : base(context) { }

        public async Task<List<ExamResult>> GetAllAsync()
        {
            return await _context.ExamResult
                .Include(er => er.ExamSubject)
                    .ThenInclude(es => es.Exam)
                        .ThenInclude(e => e.ExamType)
                .Include(er => er.ExamSubject)
                    .ThenInclude(es => es.Subject)
                .ToListAsync();
        }

        public async Task<ExamResult?> GetByIdAsync(int id)
        {
            return await _context.ExamResult
                .Include(er => er.ExamSubject)
                    .ThenInclude(es => es.Exam)
                        .ThenInclude(e => e.ExamType)
                .Include(er => er.ExamSubject)
                    .ThenInclude(es => es.Subject)
                .FirstOrDefaultAsync(er => er.ExamResultID == id);
        }

        public async Task<ExamResult?> GetByExamSubjectIdAsync(int examSubjectId, string resultExamId)
        {
            return await _context.ExamResult
                .Include(er => er.ExamSubject)
                    .ThenInclude(es => es.Exam)
                        .ThenInclude(e => e.ExamType)
                .Include(er => er.ExamSubject)
                    .ThenInclude(es => es.Subject)
                .FirstOrDefaultAsync(er => er.ExamSubjectID == examSubjectId && er.ResultExamId == resultExamId);
        }
    }
}
