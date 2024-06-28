using AppCore.Models;
using FluentValidation;

namespace AppCore.Validators;

public class CreateTicketApiModelValidator : AbstractValidator<CreateTicketModel>
{
    public CreateTicketApiModelValidator() {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Name for Ticket is required")
            .MaximumLength(30)
            .WithMessage("Maximum length for ticket name is 30 characters");

        RuleFor(x => x.Price)
            .NotEmpty()
            .WithMessage("Price for Ticket is required");
    }

}