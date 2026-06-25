using OnlineExam.Common;
using OnlineExam.DTOs.AnswerDTOs;
using OnlineExam.Entity;

namespace OnlineExam.ServiceInterfaces
{
    public interface IAnswerService
    {
        Task<Result> AddAnswerListAsync(int examId,string userId, List<CreateAnswerDto> answerList);
        Task<Result<List<StudCompleteListDto>>> GetStudCompeleteExamList(int examId, string userId);
        Task<Result<ExamAndQuestionAnswer?>> GetStudentAnswerAsync(int examId, string userId, string studId);
    }
}
