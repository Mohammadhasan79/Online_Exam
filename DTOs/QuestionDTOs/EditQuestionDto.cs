using OnlineExam.Enum;

namespace OnlineExam.DTOs.QuestionDTOs
{
    public class EditQuestionDto
    {
        public string QuestionText { get; set; } = string.Empty;
        public char? CurrectAnswer { get; set; }
        public List<CreateQuestionOptionDto> Options { get; set; } = [];
    }
}
