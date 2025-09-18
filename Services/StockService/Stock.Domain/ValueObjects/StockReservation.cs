namespace Stock.Domain.ValueObjects;

public record StockReservation
{
    public Guid OrderId { get; }
    public int Quantity { get; }
    public DateTime ReservedAt { get; }
    public DateTime ExpiresAt { get; }

    public StockReservation(Guid orderId, int quantity, DateTime reservedAt, TimeSpan reservationDuration)
    {
        if (quantity <= 0)
            throw new ArgumentException("Quantity must be greater than zero", nameof(quantity));

        OrderId = orderId;
        Quantity = quantity;
        ReservedAt = reservedAt;
        ExpiresAt = reservedAt.Add(reservationDuration);
    }

    public bool IsExpired => DateTime.UtcNow > ExpiresAt;
    
    public TimeSpan TimeRemaining => ExpiresAt > DateTime.UtcNow 
        ? ExpiresAt - DateTime.UtcNow 
        : TimeSpan.Zero;
}