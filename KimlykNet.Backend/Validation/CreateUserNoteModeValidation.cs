using FluentValidation;
using KimlykNet.Backend.Models;

namespace KimlykNet.Backend.Validation;

public class CreateUserNoteModeValidation : AbstractValidator<CreateUserNoteModel>
{
    public CreateUserNoteModeValidation()
    {
        RuleFor(m => m.Text).NotEmpty().MaximumLength(4096);
        RuleFor(m => m.Title).MaximumLength(256);
    }
}