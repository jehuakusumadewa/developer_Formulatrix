// Validators/TodoRequestValidator.cs
using FluentValidation;
using TodoApi.DTOs.Requests;

namespace TodoApi.Validators
{
    public class TodoRequestValidator : AbstractValidator<TodoRequest>
    {
        public TodoRequestValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required")
                .MaximumLength(200).WithMessage("Title cannot exceed 200 characters");

            RuleFor(x => x.Description)
                .MaximumLength(1000).WithMessage("Description cannot exceed 1000 characters")
                .When(x => !string.IsNullOrEmpty(x.Description));

            RuleFor(x => x.DueDate)
                .GreaterThanOrEqualTo(DateTime.Today).WithMessage("Due date must be today or in the future")
                .When(x => x.DueDate.HasValue);
        }
    }
}