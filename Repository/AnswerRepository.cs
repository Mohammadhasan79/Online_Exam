using Microsoft.EntityFrameworkCore;
using OnlineExam.DbContext;
using OnlineExam.DTOs.AnswerDTOs;
using OnlineExam.DTOs.QuestionDTOs;
using OnlineExam.Entity;
using OnlineExam.RepositoryInterfaces;

namespace OnlineExam.Repository
{
    public class AnswerRepository : IAnswerRepository
    {
        private readonly OnlineExamDbContext _context;
        public AnswerRepository(OnlineExamDbContext context)
        {
            _context = context;
        }
        public async Task<bool> CheckAnswerExistAsync(int examId, string userId)
        {
            return await _context.Answers.AnyAsync(a => a.ExamId ==  examId && a.UserId == userId);
        }
        public async Task<bool> CheckAccessAsync(int examId, string userId)
        {
            return await _context.StudentAssigns.AnyAsync(a => a.ExamId == examId && a.UserId == userId);
        }
        public async Task<bool> CheckCreatorAccessAsync(int examId, string userId)
        {
            return await _context.Exam.AnyAsync(a => a.Id == examId && a.CreateBy == userId);
        }
        public async Task<List<User?>> GetStudCompeletList(int examId)
        {
            return await _context.Answers.Include(u => u.User)
                .Where(a => a.ExamId == examId)
                .Select(a => a.User)
                .Distinct()
                .ToListAsync();
        }
         public async Task<ExamAndQuestionAnswer?> GetExamWithAnswersAsync(int examId, string userId)
        {
            return await _context.Answers
                .Where(a => a.ExamId == examId && a.UserId == userId)
                .Include(a => a.User)
                .Include(a => a.Exam)
                .Include(a => a.Question)
                    .ThenInclude(q => q!.Options)
                .GroupBy(a => new
                {
                    a.Exam!.ExamName,
                    a.Exam.ExamDescription,
                    a.Exam.StartTime,
                    a.Exam.ExamTime,
                    a.User!.FistName,
                    a.User.LastName,
                    a.User.UniversityCode
                })
                .Select(g => new ExamAndQuestionAnswer
                {
                    ExamName = g.Key.ExamName,
                    ExamDescription = g.Key.ExamDescription,
                    StartTime = g.Key.StartTime,
                    ExamTime = g.Key.ExamTime,
                    FistName = g.Key.FistName,
                    LastName = g.Key.LastName,
                    UniversityCode = g.Key.UniversityCode,
                    Answers = g.Select(a => new ShowAnswerDto
                    {
                        AnswerId = a.Id,
                        QuestionText = a.Question!.QuestionText,
                        QuestionType = a.Question.QuestionType,
                        UserAnswer = a.UserAnswer,
                        IsCorrect = a.IsCorrect,
                        Options = a.Question.Options.Select(o => new ShowQuestionOptionDto
                        {
                            OptionId = o.Id,
                            OptionKey = o.OptionKey,
                            Option = o.Option
                        }).ToList()
                    }).ToList()
                })
                .FirstOrDefaultAsync();
        }
        

        public async Task<bool> IsCorrect(int questionId, string userAnswer)
        {
            var question = await _context.Question.FirstOrDefaultAsync(q => q.Id == questionId);

            if (question == null) return false;

            return question.CurrectAnswer == char.ToUpper(userAnswer[0]);
        }
        public async Task AddAnswers(List<Answer> answers)
        {
            await _context.Answers.AddRangeAsync(answers);
        }
        public void DeleteAnswersAsync(int examId, string studId)
        {
           var answer = _context.Answers.Where(a => a.ExamId == examId && a.UserId == studId);
            _context.Answers.RemoveRange(answer);
        }
    }
}
