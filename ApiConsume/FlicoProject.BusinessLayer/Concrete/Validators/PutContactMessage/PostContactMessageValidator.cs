using FlicoProject.DtoLayer;
using FluentValidation;

namespace FlicoProject.BusinessLayer.Validators
{
    public class PutContactMessageDtoValidator : AbstractValidator<PutContactMessageDto>
    {
        public PutContactMessageDtoValidator()
        {
            RuleFor(dto => dto.Answer)
                        .NotEmpty().WithMessage("Name is required");

        }
    }
}
