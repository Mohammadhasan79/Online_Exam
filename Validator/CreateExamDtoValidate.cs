using FluentValidation;
using OnlineExam.DTOs.ExamDTOs;

public class CreateExamDtoValidator : AbstractValidator<CreateExamDto>
{
    public CreateExamDtoValidator()
    {
        RuleFor(x => x.ExamName)
            .NotEmpty().WithMessage("ExamName is required.")
            .MaximumLength(100).WithMessage("ExamName is too long.");

        RuleFor(x => x.ExamDescription)
            .NotEmpty().WithMessage("ExamDescription is required.")
            .MaximumLength(500).WithMessage("ExamDescription is too long.");

        RuleFor(x => x.StartTime)
            .GreaterThan(DateTime.UtcNow).WithMessage("StartTime must be in the future.");

        RuleFor(x => x.ExamTime)
            .GreaterThan(0).WithMessage("ExamTime must be greater than 0.");
    }
}