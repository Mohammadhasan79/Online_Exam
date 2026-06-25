using OnlineExam.DTOs.AnswerDTOs;
using OnlineExam.Entity;

namespace OnlineExam.RepositoryInterfaces
{
    public interface IAnswerRepository
    {
        Task<bool> CheckAnswerExistAsync(int examId, string userId);
        Task<bool> CheckAccessAsync(int examId, string userId);
        Task<bool> CheckCreatorAccessAsync(int examId, string userId);
        Task<List<User?>> GetStudCompeletList(int examId);
        Task<ExamAndQuestionAnswer?> GetExamWithAnswersAsync(int examId, string userId);
        Task<bool> IsCorrect(int questionId, string userAnswer);
        Task AddAnswers(List<Answer> answers);
    }
}
