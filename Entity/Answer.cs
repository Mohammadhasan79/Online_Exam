namespace OnlineExam.Entity
{
    public class Answer
    {
        public int Id { get; set; }
        public int QuestionId { get; set; }
        public Question? Question { get; set; }
        public string? UserAnswer { get; set; }
        public bool? IsCorrect { get; set; }
        public string UserId { get; set; } = string.Empty;
        public User? User { get; set; }
        public int ExamId { get; set; }
        public Exam? Exam { get; set; }

    }
}
