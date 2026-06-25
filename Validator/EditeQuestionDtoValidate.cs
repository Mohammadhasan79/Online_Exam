using FluentValidation;
using OnlineExam.DTOs.QuestionDTOs;

public class EditQuestionDtoValidator : AbstractValidator<EditQuestionDto>
{
    public EditQuestionDtoValidator()
    {
        RuleFor(x => x.QuestionText)
            .NotEmpty().WithMessage("QuestionText is required.")
            .MaximumLength(1000).WithMessage("QuestionText is too long.");

        RuleFor(x => x.CurrectAnswer)
            .NotNull().When(x => x.Options.Any())
            .WithMessage("CorrectAnswer is required when options are provided.");

        RuleForEach(x => x.Options)
            .SetValidator(new CreateQuestionOptionDtoValidator());
    }
}