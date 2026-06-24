namespace OnlineExam.DTOs.ExamListDTOs
{
    public class ShowStudentAssign
    {
        public int StudentAssignId { get; set; }
        public string? UserName { get; set; } = string.Empty;
        public string? FirstName {  get; set; } = string.Empty;
        public int ExamId { get; set; }
        public string ExamName { get; set; } = string.Empty;
        public DateTime StartTime { get; set; }
        public int ExamTime { get; set; }
    }
}
