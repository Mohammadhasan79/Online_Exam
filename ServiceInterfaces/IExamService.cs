using OnlineExam.Common;
using OnlineExam.DTOs.ExamDTOs;

namespace OnlineExam.ServiceInterfaces
{
    public interface IExamService
    {
        Task<Result> CreateExamAsync(string userId, CreateExamDto dto);
        Task<Result> UpdateExamAsync(int examId, string userId, CreateExamDto dto);
        Task<Result> DeleteExamAsync(int examId, string userId);
    }
}
