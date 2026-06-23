using Microsoft.EntityFrameworkCore;
using OnlineExam.DbContext;
using OnlineExam.Entity;
using OnlineExam.RepositoryInterfaces;

namespace OnlineExam.Repository
{
    public class ExamRepository : IExamRepository
    {
        private readonly OnlineExamDbContext _context;
        public ExamRepository(OnlineExamDbContext context)
        {
            _context = context;
        }
        public async Task AddAsync(Exam exam)
        {
            await _context.Exam.AddAsync(exam);
        }
        public void UpdateAsync(Exam exam)
        {
            _context.Exam.Update(exam);
        }
        public void DeleteAsync(Exam exam)
        {
            _context.Exam.Remove(exam);
        }
        public async Task<Exam?> GetByExamIdAsync(int examId)
        {
            return await _context.Exam.FirstOrDefaultAsync(a => a.Id == examId);
        }
    }
}
