using OnlineExam.Common;
using OnlineExam.DTOs.QuestionDTOs;

namespace OnlineExam.ServiceInterfaces
{
    public interface IQuestionService
    {
        Task<Result> CreateQuestionAsync(int examId, CreateQuestionDto question);
        Task<Result> UpdateQuestionAsync(int questionId, EditQuestionDto question);
        Task<Result> DeleteQuestionAsync(int questionId);
    }
}
