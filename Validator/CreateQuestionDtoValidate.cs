using FluentValidation;
using OnlineExam.DTOs.QuestionDTOs;

public class CreateQuestionDtoValidator : AbstractValidator<CreateQuestionDto>
{
    public CreateQuestionDtoValidator()
    {
        RuleFor(x => x.QuestionText)
            .NotEmpty().WithMessage("QuestionText is required.")
            .MaximumLength(1000).WithMessage("QuestionText is too long.");

        RuleFor(x => x.QuestionType)
            .IsInEnum().WithMessage("QuestionType is invalid.");

    }
}