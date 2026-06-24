using OnlineExam.Common;
using OnlineExam.DTOs.ExamDTOs;
using OnlineExam.DTOs.ExamListDTOs;
using OnlineExam.Entity;

namespace OnlineExam.ServiceInterfaces
{
    public interface IExamService
    {
        Task<Result> CreateExamAsync(string userId, CreateExamDto dto);
        Task<Result> UpdateExamAsync(int examId, string userId, CreateExamDto dto);
        Task<Result> DeleteExamAsync(int examId, string userId);
        Task<Result<ShowExamDto>> GetExamAndQuestionByIdAsync(int examId, string userId, string userRole);
        Task<Result<ExamAndUserListDto>> GetUserExamListAsync(string userId);
        Task<Result> AddExamToStudentAsync(string studentId, int examId);
    }
}
