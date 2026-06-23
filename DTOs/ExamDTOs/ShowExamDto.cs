using OnlineExam.Entity;

namespace OnlineExam.DTOs.ExamDTOs
{
    public class ShowExamDto
    {
        public int Id { get; set; }
        public string ExamName { get; set; } = string.Empty;
        public string ExamDescription { get; set; } = string.Empty;
        public DateTime StartTime { get; set; }
        public List<Question> Question { get; set; } = [];
    }
}
