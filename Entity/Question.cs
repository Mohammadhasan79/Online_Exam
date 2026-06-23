using OnlineExam.Enum;

namespace OnlineExam.Entity
{
    public class Question
    {
        public int Id { get; set; }
        public string QuestionText { get; set; } = string.Empty;
        public QuestionType QuestionType { get; set; }
        public char? CurrectAnswer { get; set; }
        public int ExamId { get; set; }
        public Exam? Exam { get; set; }
        public List<QuestionOption> Options { get; set; } = new List<QuestionOption>();
    }
}
