using FluentValidation;

namespace Order.Application.Features.Orders.Commands.CreateOrder;

public sealed class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
{
    public CreateOrderCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("UserId is required");

        RuleFor(x => x.ShippingAddress)
            .NotNull().WithMessage("Shipping address is required");

        RuleFor(x => x.BillingAddress)
            .NotNull().WithMessage("Billing address is required");

        RuleFor(x => x.OrderItems)
            .NotEmpty().WithMessage("Order must contain at least one item");

        RuleForEach(x => x.OrderItems)
            .SetValidator(new CreateOrderItemValidator());

        When(x => x.ShippingAddress != null, () =>
        {
            RuleFor(x => x.ShippingAddress.Street).NotEmpty();
            RuleFor(x => x.ShippingAddress.City).NotEmpty();
            RuleFor(x => x.ShippingAddress.State).NotEmpty();
            RuleFor(x => x.ShippingAddress.Country).NotEmpty();
            RuleFor(x => x.ShippingAddress.ZipCode).NotEmpty();
        });

        When(x => x.BillingAddress != null, () =>
        {
            RuleFor(x => x.BillingAddress.Street).NotEmpty();
            RuleFor(x => x.BillingAddress.City).NotEmpty();
            RuleFor(x => x.BillingAddress.State).NotEmpty();
            RuleFor(x => x.BillingAddress.Country).NotEmpty();
            RuleFor(x => x.BillingAddress.ZipCode).NotEmpty();
        });
    }
}

public sealed class CreateOrderItemValidator : AbstractValidator<CreateOrderItemDto>
{
    public CreateOrderItemValidator()
    {
        RuleFor(x => x.ProductId)
            .NotEmpty().WithMessage("ProductId is required");

        RuleFor(x => x.ProductName)
            .NotEmpty().WithMessage("Product name is required")
            .MaximumLength(200);

        RuleFor(x => x.UnitPrice)
            .GreaterThan(0).WithMessage("Unit price must be greater than zero");

        RuleFor(x => x.Currency)
            .NotEmpty().WithMessage("Currency is required")
            .Length(3).WithMessage("Currency must be 3 characters");

        RuleFor(x => x.Quantity)
            .GreaterThan(0).WithMessage("Quantity must be greater than zero");
    }
}