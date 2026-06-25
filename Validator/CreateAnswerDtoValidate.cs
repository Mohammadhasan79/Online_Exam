using FluentValidation;
using OnlineExam.DTOs.AnswerDTOs;

public class CreateAnswerDtoValidator : AbstractValidator<CreateAnswerDto>
{
    public CreateAnswerDtoValidator()
    {
        RuleFor(x => x.QuestionId)
            .GreaterThan(0).WithMessage("QuestionId is required.");

        RuleFor(x => x.QuestionType)
            .IsInEnum().WithMessage("QuestionType is invalid.");

        RuleFor(x => x.UserAnswer)
            .NotEmpty().WithMessage("UserAnswer is required.")
            .MaximumLength(500).WithMessage("UserAnswer is too long.");
    }
}