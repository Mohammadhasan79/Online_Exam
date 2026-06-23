using OnlineExam.Entity;
using OnlineExam.Enum;

namespace OnlineExam.DTOs.QuestionDTOs
{
    public class CreateQuestionDto
    {
        public string QuestionText { get; set; } = string.Empty;
        public QuestionType QuestionType { get; set; }
        public char? CurrectAnswer { get; set; }
        public List<CreateQuestionOptionDto> Options { get; set; } = [];
    }
}
