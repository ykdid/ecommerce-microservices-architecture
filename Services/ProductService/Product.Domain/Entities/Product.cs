using Product.Domain.Common;
using Product.Domain.ValueObjects;

namespace Product.Domain.Entities;

public sealed class Product : BaseEntity
{
    public string Name { get; private set; }
    public Price Price { get; private set; }
    public Stock Stock { get; private set; }
    public Stock Type { get; set; }

    private Product() { }

    public Product(string name, Price price, Stock stock)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Price = price ?? throw new ArgumentNullException(nameof(price));
        Stock = stock ?? throw new ArgumentNullException(nameof(stock));
    }

    public void UpdateStock(int newQuantity)
    {
        Stock = new Stock(newQuantity);
    }

    public void ChangePrice(decimal newPrice)
    {
        Price = new Price(newPrice);
    }
}