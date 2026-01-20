using Domain.Entities;
using Domain.Interfaces.Repositories;
using Infrastructure.Data;

namespace Infrastructure.Repositories
{
    public class ExamTypeRepository : GenericRepository<ExamType>, IExamTypeRepository
    {
        public ExamTypeRepository(ApplicationDbContext context) : base(context) { }
    }
}
