using System.Collections.Generic;
using OnlineExam.Common;
using OnlineExam.DTOs.AnswerDTOs;
using OnlineExam.Entity;
using OnlineExam.Enum;
using OnlineExam.RepositoryInterfaces;
using OnlineExam.ServiceInterfaces;
using OnlineExam.UnitOfWork;

namespace OnlineExam.Service
{
    public class AnswerService : IAnswerService
    {
        private readonly IAnswerRepository _answerRepository;
        private readonly IUnitOfWork _unitOfWork;
        public AnswerService(IAnswerRepository answerRepository, IUnitOfWork unitOfWork)
        {
            _answerRepository = answerRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<Result> AddAnswerListAsync(int examId, string userId, List<CreateAnswerDto> answerList)
        {
            bool accessCheck = await _answerRepository.CheckAccessAsync(examId, userId);

            if (!accessCheck) return Result.Fail("Forbid.");

            if (answerList == null || !answerList.Any()) return Result.Fail("Data Entry Is Null");

            bool duplicateCheck =await _answerRepository.CheckAnswerExistAsync(examId, userId);

            if(duplicateCheck) return Result.Fail("You have already registered an answer.");

            var answers = new List<Answer>();

            foreach (var a in answerList)
            {
                var answer = new Answer
                {
                    QuestionId = a.QuestionId,
                    UserAnswer = a.UserAnswer,
                    IsCorrect = a.QuestionType == QuestionType.MultiChoice
                        ? await _answerRepository.IsCorrect(a.QuestionId, a.UserAnswer!)
                        : null,
                    UserId = userId,
                    ExamId = examId
                };
                answers.Add(answer);
            }

            await _answerRepository.AddAnswers(answers);
            await _unitOfWork.SaveChangesAsync();
            return Result.Ok();
        }
        public async Task<Result<List<StudCompleteListDto>>> GetStudCompeleteExamList(int examId, string userId)
        {
            var checkAccess = await _answerRepository.CheckCreatorAccessAsync(examId, userId);

            if(!checkAccess) return Result<List<StudCompleteListDto>>.Fail("Forbid.");
            var users = await _answerRepository.GetStudCompeletList(examId);
            return Result<List<StudCompleteListDto>>.Ok(users.Select(u => new StudCompleteListDto
            {
                StudId = u!.Id,
                UserName = u.UserName!,
                FirstName = u.FistName,
                LastName = u.LastName
            }).ToList());
        }
        public async Task<Result<ExamAndQuestionAnswer?>> GetStudentAnswerAsync(int examId, string userId, string studId)
        {
            var checkAccess = await _answerRepository.CheckCreatorAccessAsync(examId, userId);

            if (!checkAccess) return Result<ExamAndQuestionAnswer?>.Fail("Forbid.");
            var answer = await _answerRepository.GetExamWithAnswersAsync(examId, studId);
            return Result <ExamAndQuestionAnswer?>.Ok(answer);

        }
        public async Task<Result> DeleteStudentAnswerAsync(int examId, string userId, string studId)
        {
            var checkAccess = await _answerRepository.CheckCreatorAccessAsync(examId, userId);

            if (!checkAccess) return Result.Fail("Forbid.");
            _answerRepository.DeleteAnswersAsync(examId, studId);
            await _unitOfWork.SaveChangesAsync();
            return Result.Ok();
        }

    }
}
