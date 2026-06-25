using FluentValidation;
using OnlineExam.DTOs.QuestionDTOs;

public class CreateQuestionOptionDtoValidator : AbstractValidator<CreateQuestionOptionDto>
{
    public CreateQuestionOptionDtoValidator()
    {
        RuleFor(x => x.Option)
            .NotEmpty().WithMessage("Option text is required.")
            .MaximumLength(200).WithMessage("Option text is too long.");

        RuleFor(x => x.OptionKey)
            .Must(k => k >= 'A' && k <= 'Z').WithMessage("OptionKey must be a letter between A and Z.");
    }
}