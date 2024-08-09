using FluentValidation;
using TRS.CoreApi.Models;

namespace TRS.CoreApi.Validators;

public class CreateAccountApiModelValidator : AbstractValidator<CreateAccountModel>
{
    public CreateAccountApiModelValidator()
    {
        RuleFor(x => x.Username)
            .NotEmpty()
            .WithMessage("Username is required")
            .MaximumLength(15)
            .WithMessage("Username must be less than 15 characters");

        RuleFor(x => x.Email).NotEmpty().WithMessage("Email is required");

        RuleFor(x => x.FirstName)
            .NotEmpty()
            .WithMessage("FirstName is required")
            .MaximumLength(25)
            .WithMessage("FirstName must be less than 25 characters");

        RuleFor(x => x.LastName)
            .NotEmpty()
            .WithMessage("LastName is required")
            .MaximumLength(25)
            .WithMessage("LastName must be less than 25 characters");

        RuleFor(x => x.Address)
            .NotEmpty()
            .WithMessage("Address is required")
            .Matches(".*[0-9].*")
            .WithMessage("Address must contain a number")
            .MaximumLength(60)
            .WithMessage("Address must be less than 60 characters");

        RuleFor(x => x.PhoneNumber)
            .NotEmpty()
            .WithMessage("PhoneNumber is required")
            .MaximumLength(15)
            .WithMessage("PhoneNumber must be less than 15 characters");
    }
}
