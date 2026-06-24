using OnlineExam.Entity;
using OnlineExam.Enum;

namespace OnlineExam.DTOs.QuestionDTOs
{
    public class ShowQuestionDto
    {
        public int Id { get; set; }
        public string QuestionText { get; set; } = string.Empty;
        public QuestionType QuestionType { get; set; }
        public List<ShowQuestionOptionDto> Options { get; set; } = [];
    }
}
