using FlicoProject.DtoLayer;
using FluentValidation;

namespace FlicoProject.BusinessLayer.Validators
{
    public class PostContactMessageDtoValidator : AbstractValidator<PostContactMessageDto>
    {
        public PostContactMessageDtoValidator()
        {
            RuleFor(dto => dto.Name)
                        .NotEmpty().WithMessage("Name is required")
                        .MaximumLength(50).WithMessage("Name must be no longer than 50 characters");

            RuleFor(dto => dto.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Invalid email format")
                .MaximumLength(100).WithMessage("Email must be no longer than 100 characters");

            RuleFor(dto => dto.Subject)
                .NotEmpty().WithMessage("Subject is required")
                .MaximumLength(100).WithMessage("Subject must be no longer than 100 characters");

            RuleFor(dto => dto.Message)
                .NotEmpty().WithMessage("Message is required")
                .MaximumLength(1000).WithMessage("Message must be no longer than 1000 characters");
        }
    }
}
