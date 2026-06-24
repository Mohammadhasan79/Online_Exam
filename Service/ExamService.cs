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
        public async Task<Result> AddExamToStudentAsync(string studentId, int examId)
        {

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
                isAccess = userId == exam.CreateBy || await _examRepository.CheckHaveExam(examId, userId);
            }
            else if(userRole == "Student")
            {
                isAccess = await _examRepository
                    .CheckHaveExam(examId, userId) 
                    && exam.StartTime <= DateTime.Now 
                    && exam.StartTime.AddMinutes(exam.ExamTime) > DateTime.Now;    
            }
            if(!isAccess) return Result<ShowExamDto>.Fail("Forbid");

            var showExam = new ShowExamDto
            {
                Id = exam.Id,
                ExamName = exam.ExamName,
                ExamDescription = exam.ExamDescription,
                StartTime = exam.StartTime,
                ExamTime = exam.ExamTime,
                Question = exam.Question.Select(q => new ShowQuestionDto
                {
                    Id = q.Id,
                    QuestionText = q.QuestionText,
                    QuestionType = q.QuestionType,
                    Options = q.Options.Select(o => new ShowQuestionOptionDto
                    {
                        Id = o.Id,
                        Option = o.Option,
                        OptionKey = o.OptionKey
                    }).ToList()
                }).ToList(),
            };
            return Result<ShowExamDto>.Ok(showExam);
        }

    }
}
