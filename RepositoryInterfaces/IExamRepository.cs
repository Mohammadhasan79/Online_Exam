using OnlineExam.Entity;

namespace OnlineExam.RepositoryInterfaces
{
    public interface IExamRepository
    {
        Task AddAsync(Exam exam);
        void UpdateAsync(Exam exam);
        void DeleteAsync(Exam exam);
        Task<Exam?> GetByExamIdAsync(int examId);
    }
}
