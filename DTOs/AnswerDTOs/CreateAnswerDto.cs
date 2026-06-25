using OnlineExam.Entity;
using OnlineExam.Enum;

namespace OnlineExam.DTOs.AnswerDTOs
{
    public class CreateAnswerDto
    {
        public int QuestionId { get; set; }
        public QuestionType QuestionType { get; set; }
        public string? UserAnswer { get; set; }
    }
}
