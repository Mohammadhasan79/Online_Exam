using OnlineExam.DTOs.QuestionDTOs;
using OnlineExam.Entity;
using OnlineExam.Enum;

namespace OnlineExam.DTOs.AnswerDTOs
{
    public class ShowAnswerDto
    {
        public int AnswerId { get; set; }
        public string QuestionText { get; set; } = string.Empty;
        public List<ShowQuestionOptionDto> Options { get; set; } = [];
        public QuestionType QuestionType { get; set; }
        public string? UserAnswer { get; set; }
        public bool? IsCorrect { get; set; }
    }
}
