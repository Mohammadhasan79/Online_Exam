using Microsoft.EntityFrameworkCore;
using OnlineExam.DbContext;
using OnlineExam.Entity;
using OnlineExam.RepositoryInterfaces;

namespace OnlineExam.Repository
{
    public class QuestionRepository : IQuestionRepository
    {
        private readonly OnlineExamDbContext _context;
        public QuestionRepository(OnlineExamDbContext context)
        {
            _context = context;
        }
        public async Task AddAsync(Question question)
        {
            await _context.Question.AddAsync(question);
        }
        public void UpdateAsync(Question question)
        {
             _context.Question.Update(question);
        }
        public void DeleteAsync(Question question)
        {
            _context.Question.Remove(question);
        }

        public async Task<Question?> GetQuestionById(int questionId)
        {
            return await _context.Question
                .Include(o => o.Options)
                .FirstOrDefaultAsync(a => a.Id == questionId);
        }
        public async Task<bool> CheckDependencyInAnswer(int questionId)
        {
            return await _context.Answers.AnyAsync(a => a.QuestionId == questionId);
        }
    }
}
