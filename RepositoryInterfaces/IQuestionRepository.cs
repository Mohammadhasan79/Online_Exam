using OnlineExam.Entity;

namespace OnlineExam.RepositoryInterfaces
{
    public interface IQuestionRepository
    {
        Task AddAsync(Question question);
        void UpdateAsync(Question question);
        void DeleteAsync(Question question);
        Task<Question?> GetQuestionById(int questionId);
        Task<bool> CheckDependencyInAnswer(int questionId);

    }
}
