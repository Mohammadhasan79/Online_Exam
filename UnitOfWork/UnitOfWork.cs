using OnlineExam.DbContext;

namespace OnlineExam.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly OnlineExamDbContext _context;
        public UnitOfWork(OnlineExamDbContext context)
        {
            _context = context;
        }
        public Task<int> SaveChangesAsync()
        {
            return _context.SaveChangesAsync();
        }
    }
}
