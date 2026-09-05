using FluentValidation;
using StoreProject.Application.DTOs.Product;

namespace StoreProject.Application.Validators.Product;

public class CreateProductValidator : AbstractValidator<CreateProductDto>
{
    public CreateProductValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("نام محصول الزامی است.")
            .MaximumLength(200)
            .WithMessage("نام محصول نمی‌تواند بیشتر از 200 کاراکتر باشد.");

        RuleFor(x => x.Description)
            .MaximumLength(2000)
            .WithMessage("توضیحات نمی‌تواند بیشتر از 2000 کاراکتر باشد.");

        RuleFor(x => x.Price)
            .GreaterThan(0)
            .WithMessage("قیمت باید بیشتر از صفر باشد.");

        RuleFor(x => x.Stock)
            .GreaterThanOrEqualTo(0)
            .WithMessage("موجودی نمی‌تواند منفی باشد.");

        RuleFor(x => x.ImageUrl)
            .MaximumLength(500)
            .WithMessage("آدرس تصویر نمی‌تواند بیشتر از 500 کاراکتر باشد.");
    }
}