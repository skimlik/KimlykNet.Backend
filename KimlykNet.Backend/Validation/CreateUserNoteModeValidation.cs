using FluentValidation;
using KimlykNet.Backend.Models;

namespace KimlykNet.Backend.Validation;

public class CreateUserNoteModeValidation : AbstractValidator<CreateUserNoteModel>
{
    public CreateUserNoteModeValidation()
    {
        RuleFor(m => m.Text).NotEmpty().MaximumLength(256);
        RuleFor(m => m.Title).MaximumLength(4096);
    }
}