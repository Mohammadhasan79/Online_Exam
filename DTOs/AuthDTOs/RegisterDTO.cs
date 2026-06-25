namespace OnlineExam.DTOs.AuthDTOs
{
    public class RegisterDTO
    {
        public string UniversityCode { get; set; }
        public string Password { get; set; } = string.Empty;
        public string FirstName { get; set; }
        public string LastName { get; set; }

    }
}
