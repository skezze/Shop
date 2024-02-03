using Microsoft.EntityFrameworkCore;
using Shop.Data;
using Shop.Domain.Models;

namespace Shop.Application.Services;

public class OrderService
{
    private readonly ApplicationDbContext _dbContext;

    public OrderService(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public List<Order> GetOrders()
    {
        return _dbContext.Orders.ToList();
    }
    
    public async Task<Order?> GetOrderById(int id)
    {
        if (id > 0)
        {
           var order = await _dbContext.Orders
               .Include(x=>x.User)
               .Include(x=>x.OrderProducts)
               .FirstOrDefaultAsync(x => x.Id == id);
           if (order!=null)
           {
               return order;
           }
        }

        return null;
    }

    public async Task<Order?> CreateOrder(Order order)
    {
        if (order.UserId!=string.Empty && order.OrderProducts.Count>0 && order.Address2!= string.Empty
            && order.Adress1!= string.Empty && order.PostCode>0)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(x=>x.Id==order.UserId);
            
            if (user==null)
            {
                return null;
            }
            
            var finalPrice = async () =>
            {
                int price = 0;
                foreach (var productEntity in order.OrderProducts)
                {
                    var product = await _dbContext.Products.FirstOrDefaultAsync(x => x.Id == productEntity.ProductId);
                    if (product!=null)
                    {
                        price += product.Price;
                    }
                }

                return price;
            };
            var orderproducts = async () =>
            {
                var orderProducts = new List<OrderProduct>();
                foreach (var productEntity in order.OrderProducts)
                {
                    var product = await _dbContext.Products.FirstOrDefaultAsync(x => x.Id == productEntity.ProductId);
                    if (product != null)
                    {
                        var orderProduct = new OrderProduct
                        {
                            // order - ваш созданный заказ
                            Product = product   // текущий продукт
                        };
                        orderProducts.Add(orderProduct);
                    }
                }
                return orderProducts;
            };

            var neworder = new Order()
            {
                User = user,
                Address2 = order.Address2,
                Adress1 = order.Adress1,
                FinalPrice = await finalPrice(),
                PostCode = order.PostCode,
                OrderProducts = await orderproducts(),
                Status = OrderStatus.InProcess
            };
            await _dbContext.Orders.AddAsync(neworder);
            await _dbContext.SaveChangesAsync();
            return neworder;
        }
        return null;
    }
    
    public async Task<Order?> UpdateOrderStatus( int orderId, OrderStatus orderStatus)
    {
        var order = await _dbContext.Orders.FirstOrDefaultAsync(x => x.Id == orderId);
        if (order==null)
        {
            return null;
        }

        if (!Enum.IsDefined(typeof(OrderStatus),orderStatus))
        {
            return null;
        }

        order.Status = orderStatus;
        await _dbContext.SaveChangesAsync();
        return order;
    }

    public async Task<bool> Delete(int orderId)
    {
        var order = await _dbContext.Orders.FirstOrDefaultAsync(x => x.Id == orderId);
        if (order==null)
        {
            return false;
        }

        _dbContext.Orders.Remove(order);
        return true;
    }
}