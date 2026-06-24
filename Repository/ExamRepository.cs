using Microsoft.EntityFrameworkCore;
using OnlineExam.DbContext;
using OnlineExam.DTOs.ExamListDTOs;
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
            return await _context.Exam.Include(q => q.Question)
                .ThenInclude(o => o.Options)
                .FirstOrDefaultAsync(a => a.Id == examId);
        }
        public async Task<bool> CheckHaveExam(int examId, string userId)
        {
            return await _context.ExamLists.AnyAsync(a => a.ExamId == examId && a.UserId == userId);
        }
        public async Task<List<ExamForListDto>> GetExamListByUserId(string userId)
        {
            return await _context.Exam.Where(a => a.CreateBy == userId).Select(e => new ExamForListDto
            {
                ExamId = e.Id,
                ExamName = e.ExamName
            }).ToListAsync();
        }
        public async Task<List<UserForListDto>> GetAllUserForList()
        {
            return await _context.Users.Where(a => !a.UserName.Contains("@")).Select(u => new UserForListDto
            {
                UserId = u.Id,
                UserName = u.UserName,
                FirstName = u.FistName,
                LastName = u.LastName
            }).ToListAsync();
        }

    }
}
