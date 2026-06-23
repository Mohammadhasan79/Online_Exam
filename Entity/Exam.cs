namespace OnlineExam.Entity
{
    public class Exam
    {
        public int Id { get; set; }
        public string ExamName { get; set; } = string.Empty;
        public string CreateBy { get; set; } = string.Empty ;
        public string ExamDescription { get; set; } = string.Empty;
        public DateTime StartTime { get; set; }
        public List<Question> Question { get; set; } = new List<Question>();

    }
}
