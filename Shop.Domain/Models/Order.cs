namespace Shop.Domain.Models;

public class Order
{
    public int Id { get; set; }
    public string Adress1 { get; set; }
    public string Address2 { get; set; }
    public int PostCode { get; set; }
    public int FinalPrice { get; set; }
    public User User { get; set; }
    public string UserId { get; set; }
    public OrderStatus Status { get; set; }
    
    public List<OrderProduct> OrderProducts { get; set; }
}