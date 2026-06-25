namespace OnlineExam.DTOs.QuestionDTOs
{
    public class ShowQuestionOptionDto
    {
        public int OptionId { get; set; }
        public char OptionKey { get; set; }
        public string Option { get; set; } = string.Empty;
    }
}
