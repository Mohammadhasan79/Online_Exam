namespace OnlineExam.DTOs.AnswerDTOs
{
    public class ExamAndQuestionAnswer
    {
        public string ExamName { get; set; } = string.Empty;
        public string ExamDescription { get; set; } = string.Empty;
        public DateTime StartTime { get; set; }
        public int ExamTime { get; set; }
        public string FistName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string UniversityCode { get; set; } = string.Empty;
        public List<ShowAnswerDto> Answers { get; set; } = [];
    }
}
