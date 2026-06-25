using FluentValidation;
using OnlineExam.DTOs.AuthDTOs;

public class LoginDtoValidator : AbstractValidator<LoginDTO>
{
    public LoginDtoValidator()
    {
        RuleFor(x => x.UniversityCode)
            .NotEmpty().WithMessage("UniversityCode is required.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required.");
    }
}