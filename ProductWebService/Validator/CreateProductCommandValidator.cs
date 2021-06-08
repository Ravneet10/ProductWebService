using ProductWebService.Command;
using FluentValidation;

namespace ProductWebService.Validator
{
    public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
    {
        public CreateProductCommandValidator()
        {
            RuleFor(r => r.Id)
                .NotEmpty()
                .WithMessage("ID is required.");

            RuleFor(r => r.Name)
                .NotEmpty()
                .WithMessage("Name is required.");

            RuleFor(r => r.Description)
                .NotEmpty()
                .WithMessage("Description is required.")
                .Length(1,10).WithMessage("Description cannot be more than 100 characters");
        }

    }
}
