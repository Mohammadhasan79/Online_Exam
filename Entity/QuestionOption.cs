namespace OnlineExam.Entity
{
    public class QuestionOption
    {
        public int Id { get; set; }
        public char OptionKey { get; set; }
        public string Option { get; set; } = string.Empty;
        public int QuestionId { get; set; }
        public Question? Question { get; set; }
    }
}
