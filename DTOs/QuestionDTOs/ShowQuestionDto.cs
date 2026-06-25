using OnlineExam.Entity;
using OnlineExam.Enum;

namespace OnlineExam.DTOs.QuestionDTOs
{
    public class ShowQuestionDto
    {
        public int QuestionId { get; set; }
        public string QuestionText { get; set; } = string.Empty;
        public QuestionType QuestionType { get; set; }
        public string? ImageUrl { get; set; } = string.Empty;
        public List<ShowQuestionOptionDto> Options { get; set; } = [];
    }
}
