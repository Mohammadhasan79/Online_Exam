using OnlineExam.DTOs.ExamDTOs;
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
        Task<bool> CheckUserAndExamExist(string userId, string studentId, int examId);
        Task AddExamToUser(StudentAssign newAdd);
        Task<StudentAssign?> GetStudentAssign(string userId, int studentAssignId);
        void DeleteStudentAssignAsync(StudentAssign studentAssign);
        Task<List<StudentAssign>> GetProfExamList(string userId);
        Task<List<StudentAssign>> GetStudentExamList(string userId);
        Task<List<Exam>> GetExamByUserIdAsync(string userId);
    }
}
