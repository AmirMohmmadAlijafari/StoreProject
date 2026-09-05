namespace StoreProject.Domain.Entities;

public class Cart
{
    public Guid Id { get; set; }

    public Guid CustomerId { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public Customer Customer { get; set; } = null!;

    public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
}