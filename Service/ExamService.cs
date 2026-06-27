using System.Security.Claims;
using OnlineExam.Common;
using OnlineExam.DTOs.ExamDTOs;
using OnlineExam.DTOs.ExamListDTOs;
using OnlineExam.DTOs.QuestionDTOs;
using OnlineExam.Entity;
using OnlineExam.RepositoryInterfaces;
using OnlineExam.ServiceInterfaces;
using OnlineExam.UnitOfWork;

namespace OnlineExam.Service
{
    public class ExamService : IExamService
    {
        private readonly IExamRepository _examRepository;
        private readonly IUnitOfWork _unitOfWork;
        public ExamService(IExamRepository examRepository, IUnitOfWork unitOfWork)
        {
            _examRepository = examRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<Result<ExamAndUserListDto>> GetUserExamListAsync(string userId)
        {
            var exams = await _examRepository.GetExamListByUserId(userId);
            var users = await _examRepository.GetAllUserForList();
            return Result<ExamAndUserListDto>.Ok(new ExamAndUserListDto
            {
                ExamsList = exams,
                UserList = users
            });
        }
        public async Task<Result> AddExamToStudentAsync(string userId, string studentId, int examId)
        {
            bool checkExist = await _examRepository.CheckUserAndExamExist(userId, studentId, examId);

            if (!checkExist) 
                return Result.Fail("User/Exam Duplicate/NotExist or You do not have access to this Exam.");
            var newAdd = new StudentAssign
            {
                IsActive = true,
                UserId = studentId,
                ExamId = examId
            };
            await _examRepository.AddExamToUser(newAdd);
            await _unitOfWork.SaveChangesAsync();
            return Result.Ok("Add Student Success");

        }
        public async Task<Result> CreateExamAsync(string userId, CreateExamDto dto)
        {
            if (dto == null) return Result.Fail("Data Entry Is Null");

            var exam = new Exam
            {
                ExamName = dto.ExamName,
                ExamDescription = dto.ExamDescription,
                StartTime = dto.StartTime,
                ExamTime = dto.ExamTime,
                CreateBy = userId
            };

            await _examRepository.AddAsync(exam);
            await _unitOfWork.SaveChangesAsync();
            return Result.Ok();
        }
        public async Task<Result> UpdateExamAsync(int examId, string userId, CreateExamDto dto)
        {
            if (dto == null) return Result.Fail("Data Entry Is Null");
            
            var exam = await _examRepository.GetByExamIdAsync(examId);
            if (exam == null) return Result.Fail("Exam Not Found");

            if (userId != exam.CreateBy) return Result.Fail("Forbid");


            exam.ExamName = dto.ExamName;
            exam.ExamDescription = dto.ExamDescription;
            exam.StartTime = dto.StartTime;
            exam.ExamTime = dto.ExamTime;

            _examRepository.UpdateAsync(exam);
            await _unitOfWork.SaveChangesAsync();
            return Result.Ok();
        }
        public async Task<Result> DeleteExamAsync(int examId, string userId)
        {
            var dependency = await _examRepository.CheckDependencyInAnswer(examId);
            if (dependency) return Result.Fail("This Exam Have Ansewr ");

            var exam = await _examRepository.GetByExamIdAsync(examId);
            if (exam == null) return Result.Fail("Exam Not Found");

            if (userId != exam.CreateBy) return Result.Fail("Forbid");

            _examRepository.DeleteAsync(exam);
            await _unitOfWork.SaveChangesAsync();
            return Result.Ok();
        }
        public async Task<Result<ShowExamDto>> GetExamAndQuestionByIdAsync(int examId, string userId, string userRole)
        {
            bool isAccess = false;
            var exam = await _examRepository.GetByExamIdAsync(examId);
            if (exam == null) return Result<ShowExamDto>.Fail("Exam Not Found");

            if (userRole == "Prof")
            {
                isAccess = userId == exam.CreateBy;
            }
            else if(userRole == "Student")
            {
                isAccess = await _examRepository
                    .CheckHaveExam(examId, userId) 
                    && exam.StartTime <= DateTime.UtcNow 
                    && exam.StartTime.AddMinutes(exam.ExamTime) > DateTime.UtcNow;    
            }
            if(!isAccess) return Result<ShowExamDto>.Fail("Forbid");

            var showExam = new ShowExamDto
            {
                ExamId = exam.Id,
                ExamName = exam.ExamName,
                ExamDescription = exam.ExamDescription,
                StartTime = exam.StartTime,
                ExamTime = exam.ExamTime,
                Question = exam.Question.Select(q => new ShowQuestionDto
                {
                    QuestionId = q.Id,
                    QuestionText = q.QuestionText,
                    QuestionType = q.QuestionType,
                    ImageUrl = q.ImageUrl,
                    Options = q.Options.Select(o => new ShowQuestionOptionDto
                    {
                        OptionId = o.Id,
                        Option = o.Option,
                        OptionKey = o.OptionKey
                    }).ToList()
                }).ToList(),
            };
            return Result<ShowExamDto>.Ok(showExam);
        }

        public async Task<Result<List<ShowExamForListDto>>> GetExamAndQuestionByUserIdAsync(string userId)
        {
            var exams = await _examRepository.GetExamByUserIdAsync(userId);
            return Result<List<ShowExamForListDto>>.Ok(exams.Select(e => new ShowExamForListDto
            {
                ExamId = e.Id,
                ExamName = e.ExamName,
                ExamDescription = e.ExamDescription,
                StartTime = e.StartTime,
                ExamTime = e.ExamTime,
            }).ToList());
            
        }


        public async Task<Result> DeleteStudentAssignAsync(string userId, int studentAssignId)
        {
            var studentAssign = await _examRepository.GetStudentAssign(userId, studentAssignId);

            if (studentAssign == null)
                return Result.Fail("StudentAssign Not Exist or You do not have access to this Exam.");

             _examRepository.DeleteStudentAssignAsync(studentAssign);

            await _unitOfWork.SaveChangesAsync();
            return Result.Ok("Delete Success");
        }
        public async Task<Result<List<ShowStudentAssign>>> GetStudentAssignListAsync(string userId, string userRole)
        {
            if(userRole == "Prof")
            {
                var profAssign = await _examRepository.GetProfExamList(userId);
                return Result<List<ShowStudentAssign>>.Ok(
                   profAssign.Select(a => new ShowStudentAssign
                   {
                       StudentAssignId = a.Id,
                       UserName = a.User!.UserName,
                       FirstName = a.User.FistName,
                       ExamId = a.ExamId,
                       ExamName = a.Exam!.ExamName,
                       StartTime = a.Exam.StartTime,
                       ExamTime = a.Exam.ExamTime,
                   }).ToList());
            }
            if (userRole != "Student") return Result<List<ShowStudentAssign>>.Fail($"Your UnAuthorize {userRole}");

            var studAssign = await _examRepository.GetStudentExamList(userId);


                return Result<List<ShowStudentAssign>>.Ok(
                       studAssign.Select(a => new ShowStudentAssign
                       {
                           StudentAssignId = a.Id,
                           ExamId = a.ExamId,
                           ExamName = a.Exam!.ExamName,
                           StartTime = a.Exam.StartTime,
                           ExamTime = a.Exam.ExamTime,
                       }).ToList());

        }
    }
}
