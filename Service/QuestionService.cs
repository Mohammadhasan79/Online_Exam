using OnlineExam.Common;
using OnlineExam.DTOs.QuestionDTOs;
using OnlineExam.Entity;
using OnlineExam.RepositoryInterfaces;
using OnlineExam.ServiceInterfaces;
using OnlineExam.UnitOfWork;

namespace OnlineExam.Service
{
    public class QuestionService : IQuestionService
    {
        private readonly IQuestionRepository _questionRepository;
        private readonly IUnitOfWork _unitOfWork;
        public QuestionService(IQuestionRepository questionRepository,IUnitOfWork unitOfWork)
        {
            _questionRepository = questionRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> CreateQuestionAsync(int examId, CreateQuestionDto questionDto)
        {
            if (questionDto == null) return Result.Fail("Data Entry Is Null");
            var question = new Question
            {
                QuestionText = questionDto.QuestionText,
                QuestionType = questionDto.QuestionType,
                CurrectAnswer = questionDto.CurrectAnswer,
                ImageUrl = questionDto.ImageUrl,
                ExamId = examId,
                Options = questionDto.Options.Select(o => new QuestionOption
                {
                    Option = o.Option,
                    OptionKey = o.OptionKey
                }).ToList()
            };
            await _questionRepository.AddAsync(question);
            await _unitOfWork.SaveChangesAsync();
            return  Result.Ok();
        }
        public async Task<Result> UpdateQuestionAsync(int questionId, EditQuestionDto questionDto)
        {
            if (questionDto == null) return Result.Fail("Data Entry Is Null");

            var question = await _questionRepository.GetQuestionById(questionId);
            if (question == null) return Result.Fail("Question Not Exist");

            question.QuestionText = questionDto.QuestionText;
            question.CurrectAnswer = questionDto.CurrectAnswer;
            question.ImageUrl = questionDto.ImageUrl;

            question.Options.Clear();
            question.Options = questionDto.Options.Select(o =>
            new QuestionOption
            {
                OptionKey = o.OptionKey,
                Option = o.Option,
                QuestionId = questionId
            }).ToList();


            _questionRepository.UpdateAsync(question);
            await _unitOfWork.SaveChangesAsync();
            return Result.Ok();
        }
        public async Task<Result> DeleteQuestionAsync(int questionId)
        {
            var question = await _questionRepository.GetQuestionById(questionId);
            if (question == null) return Result.Fail("Question Not Exist");
            _questionRepository.DeleteAsync(question);
            await _unitOfWork.SaveChangesAsync();
            return Result.Ok();
        }
    }
}
