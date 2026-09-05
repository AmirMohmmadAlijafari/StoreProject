using FluentValidation;
using StoreProject.Application.DTOs.Category;

namespace StoreProject.Application.Validators.Category;

public class CreateCategoryValidator : AbstractValidator<CreateCategoryDto>
{
    public CreateCategoryValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("نام دسته‌بندی الزامی است.")
            .MaximumLength(100)
            .WithMessage("نام دسته‌بندی نمی‌تواند بیشتر از 100 کاراکتر باشد.");

        RuleFor(x => x.Description)
            .MaximumLength(500)
            .WithMessage("توضیحات نمی‌تواند بیشتر از 500 کاراکتر باشد.");
    }
}