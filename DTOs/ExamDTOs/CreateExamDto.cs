namespace OnlineExam.DTOs.ExamDTOs
{
    public class CreateExamDto
    {
        public string ExamName { get; set; } = string.Empty;
        public string ExamDescription { get; set; } = string.Empty;
        public DateTime StartTime { get; set; }
        public int ExamTime { get; set; }

    }
}
