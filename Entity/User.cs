using Microsoft.AspNetCore.Identity;

namespace OnlineExam.Entity
{
    public class User : IdentityUser
    {
        public string FistName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string UniversityCode { get; set; } = string.Empty;
    }
}
