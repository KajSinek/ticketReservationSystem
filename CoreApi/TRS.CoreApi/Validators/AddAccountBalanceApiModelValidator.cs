using FluentValidation;
using TRS.CoreApi.Models;

namespace TRS.CoreApi.Validators;

public class AddAccountBalanceApiModelValidator : AbstractValidator<UpdateAccountBalanceModel>
{
    public AddAccountBalanceApiModelValidator()
    {
        RuleFor(x => x.Value)
            .NotEmpty()
            .WithMessage("Value is required")
            .GreaterThan(0)
            .WithMessage("Value must be greater than 0");
    }
}
