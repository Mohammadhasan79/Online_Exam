namespace OnlineExam.Entity
{
    public class ExamList
    {
        public int Id {  get; set; }
        public DateTime StartTime { get; set; }
        public bool IsActive { get; set; }
        public string UserId { get; set; } = string.Empty;
        public int ExamId { get; set; }
        public User? User { get; set; }
        public Exam? Exam { get; set; }
    }
}
