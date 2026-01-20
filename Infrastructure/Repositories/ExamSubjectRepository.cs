using Domain.Entities;
using Domain.Interfaces.Repositories;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class ExamSubjectRepository : GenericRepository<ExamSubject>, IExamSubjectRepository
    {
        public ExamSubjectRepository(ApplicationDbContext context) : base(context) { }

        public async Task<ExamSubject?> GetExamSubjectByIdAsync(int id)
        {
            return await _context.ExamSubject
                .Include(u => u.Exam)
                .Include(u => u.Exam.ExamType)
                .Include(u => u.Subject)
                .Include(u => u.Subject.Company)
                .Include(u => u.Subject.Company.BusinessGroup)
                .Include(u => u.Subject.Company.Country)
                .Include(u => u.Subject.Company.State)
                .FirstOrDefaultAsync(u => u.ExamSubjectID == id);
        }

        public async Task<ExamSubject?> GetExamSubjectByExamAsync(int examID)
        {
            return await _context.ExamSubject
                .Include(u => u.Exam)
                .Include(u => u.Exam.ExamType)
                .Include(u => u.Subject)
                .Include(u => u.Subject.Company)
                .Include(u => u.Subject.Company.BusinessGroup)
                .Include(u => u.Subject.Company.Country)
                .Include(u => u.Subject.Company.State)
                .FirstOrDefaultAsync(u => u.ExamID == examID);
        }

        public async Task<List<ExamSubject>> GetAllExamSubjectBySubjectAsync(int subjectID)
        {
            return await _context.ExamSubject
                .Include(u => u.Exam)
                .Include(u => u.Exam.ExamType)
                .Include(u => u.Subject)
                .Include(u => u.Subject.Company)
                .Include(u => u.Subject.Company.BusinessGroup)
                .Include(u => u.Subject.Company.Country)
                .Include(u => u.Subject.Company.State)
                .Where(u => u.SubjectID == subjectID)
                .ToListAsync();
        }

        public async Task<ExamSubject?> GetExamSubjectByExternalExamIdAsync(string externalExamId)
        {
            return await _context.ExamSubject
                .Include(u => u.Exam)
                .Include(u => u.Exam.ExamType)
                .Include(u => u.Subject)
                .Include(u => u.Subject.Company)
                .Include(u => u.Subject.Company.BusinessGroup)
                .Include(u => u.Subject.Company.Country)
                .Include(u => u.Subject.Company.State)
                .FirstOrDefaultAsync(u => u.ExternalExamId == externalExamId);
        }

        public async Task<ExamSubject?> GetValidExamSubjectAsync(int examID, int subjectID)
        {
            return await _context.ExamSubject
                .Include(u => u.Exam)
                .Include(u => u.Exam.ExamType)
                .Include(u => u.Subject)
                .Include(u => u.Subject.Company)
                .Include(u => u.Subject.Company.BusinessGroup)
                .Include(u => u.Subject.Company.Country)
                .Include(u => u.Subject.Company.State)
                .FirstOrDefaultAsync(u => u.ExamID == examID && u.SubjectID == subjectID);
        }

        public async Task<List<ExamSubject>> GetAllExamSubjectAsync()
        {
            return await _context.ExamSubject
                .Include(u => u.Exam)
                .Include(u => u.Exam.ExamType)
                .Include(u => u.Subject)
                .Include(u => u.Subject.Company)
                .Include(u => u.Subject.Company.BusinessGroup)
                .Include(u => u.Subject.Company.Country)
                .Include(u => u.Subject.Company.State)
                .ToListAsync();
        }

    }
}
