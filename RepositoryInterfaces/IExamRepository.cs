using OnlineExam.DTOs.ExamListDTOs;
using OnlineExam.Entity;

namespace OnlineExam.RepositoryInterfaces
{
    public interface IExamRepository
    {
        Task AddAsync(Exam exam);
        void UpdateAsync(Exam exam);
        void DeleteAsync(Exam exam);
        Task<Exam?> GetByExamIdAsync(int examId);
        Task<bool> CheckHaveExam(int examId, string userId);
        Task<List<ExamForListDto>> GetExamListByUserId(string userId);
        Task<List<UserForListDto>> GetAllUserForList();
    }
}
