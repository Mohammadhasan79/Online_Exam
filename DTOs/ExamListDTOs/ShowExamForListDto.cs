using OnlineExam.DTOs.QuestionDTOs;

namespace OnlineExam.DTOs.ExamListDTOs
{
    public class ShowExamForListDto
    {
        public int ExamId { get; set; }
        public string ExamName { get; set; } = string.Empty;
        public string ExamDescription { get; set; } = string.Empty;
        public DateTime StartTime { get; set; }
        public int ExamTime { get; set; } = 60;
    }
}
