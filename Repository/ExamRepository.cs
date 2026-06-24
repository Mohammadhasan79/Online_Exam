using Microsoft.EntityFrameworkCore;
using OnlineExam.DbContext;
using OnlineExam.DTOs.ExamDTOs;
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
            return await _context.StudentAssigns.AnyAsync(a => a.ExamId == examId && a.UserId == userId);
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
            return await _context.Users.Where(a => !a.UserName!.Contains("@")).Select(u => new UserForListDto
            {
                UserId = u.Id,
                UserName = u.UserName!,
                FirstName = u.FistName,
                LastName = u.LastName
            }).ToListAsync();
        }
        public async Task<bool> CheckUserAndExamExist(string userId, string studentId, int examId)
        {
            var userCheck = await _context.Users.AnyAsync(u => u.Id == studentId);
            var examCheck = await _context.Exam.AnyAsync(e => e.Id == examId && e.CreateBy == userId);
            var duplicateCheck = await _context.StudentAssigns.AnyAsync(a => a.UserId == studentId && a.ExamId == examId);

            return userCheck && examCheck && !duplicateCheck;
        }
        public async Task AddExamToUser(StudentAssign newAdd)
        {
            await _context.StudentAssigns.AddAsync(newAdd);
        }
        public async Task<StudentAssign?> GetStudentAssign(string userId, int studentAssignId)
        {
            return await _context.StudentAssigns
                .Include(e => e.Exam)
                .FirstOrDefaultAsync(s => s.Id == studentAssignId && s.Exam!.CreateBy == userId);
        }
        public void DeleteStudentAssignAsync(StudentAssign studentAssign)
        {
            _context.StudentAssigns.Remove(studentAssign);
        }
        public async Task<List<StudentAssign>> GetProfExamList(string userId)
        {
            return await _context.StudentAssigns
                .Include(e => e.Exam)
                .Include(u => u.User)
                .Where(s => s.Exam!.CreateBy == userId)
                .ToListAsync();
        }
        public async Task<List<StudentAssign>> GetStudentExamList(string userId)
        {
            return await _context.StudentAssigns
                .Include(e => e.Exam)
                .Where(s => s.UserId == userId)
                .ToListAsync();
        }
        public async Task<List<Exam>> GetExamByUserIdAsync(string userId)
        {
            return await _context.Exam.Include(q => q.Question)
                            .ThenInclude(o => o.Options)
                            .Where(a => a.CreateBy == userId).ToListAsync();
        }

    }
}
