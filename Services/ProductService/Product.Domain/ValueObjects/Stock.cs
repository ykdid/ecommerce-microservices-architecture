using Product.Domain.Common;

namespace Product.Domain.ValueObjects;

public sealed class Stock : ValueObject
{
    public int Quantity { get; }

    private Stock() { }

    public Stock(int quantity)
    {
        if (quantity < 0)
            throw new ArgumentOutOfRangeException(nameof(quantity), "Stock cannot be negative.");
        
        Quantity = quantity;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Quantity;
    }

    public override string ToString() => Quantity.ToString();
}