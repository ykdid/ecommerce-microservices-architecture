using Product.Domain.Common;

namespace Product.Domain.ValueObjects;

public sealed class Price : ValueObject
{
    public decimal Amount { get; }

    private Price() { }

    public Price(decimal amount)
    {
        if (amount < 0)
            throw new ArgumentOutOfRangeException(nameof(amount), "Price cannot be negative.");
        
        Amount = amount;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Amount;
    }

    public override string ToString() => $"{Amount:C}";
}