namespace OnlineExam.DTOs.ExamListDTOs
{
    public class ExamAndUserListDto
    {
        public List<ExamForListDto> ExamsList { get; set; } = [];
        public List<UserForListDto> UserList { get; set; } = [];
    }
}
