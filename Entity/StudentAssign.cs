namespace OnlineExam.Entity
{
    public class StudentAssign
    {
        public int Id {  get; set; }
        public bool IsActive { get; set; } = true;
        public string UserId { get; set; } = string.Empty;
        public int ExamId { get; set; }
        public User? User { get; set; }
        public Exam? Exam { get; set; }
    }
}
