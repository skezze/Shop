using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop.Application.Services;
using Shop.Data;
using Shop.Domain.Models;
using Shop.Domain.Models.ViewModels;

namespace Shop.Api.Controllers;
[ApiController,Route("api/[controller]/[action]"),Authorize]
public class OrderController:ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly OrderService _orderService;

    public OrderController(ApplicationDbContext context, OrderService orderService)
    {
        _context = context;
        _orderService = orderService;
    }

    [HttpGet]
    public IActionResult GetOrders()
    {
        var orders = _orderService.GetOrders();
        return Ok(orders);
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetOrderById(int id)
    {
        var result = await _orderService.GetOrderById(id);
        if (result==null)
        {
            return BadRequest(new { message = "product isn't finded" });
        }

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateOrder([FromBody] OrderView orderView)
    {
         var orderProducts = async () =>
        {
            var orderProducts = new List<OrderProduct>();
            foreach (var productId in orderView.ProductIds)
            {
                var product = await _context.Products.FirstOrDefaultAsync(x => x.Id == productId);
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
        ;
        var order = new Order
        {
            Adress1 = orderView.Adress1,
            Address2 = orderView.Address2,
            PostCode = orderView.PostCode,
            UserId = orderView.UserId,
            OrderProducts = await orderProducts()
        };
        var result = await _orderService.CreateOrder(order);
        if (result==null)
        {
            return BadRequest(new { message = "order isn't created" });
        }

        return Ok(result);
    }

    [HttpPut, Authorize(Policy = "Admin"),Route("{orderId}/{orderStatus}")]
    public async Task<IActionResult> UpdateOrderStatus( int orderId, OrderStatus orderStatus)
    {
        var result = await _orderService.UpdateOrderStatus(orderId, orderStatus);
        if (result==null)
        {
            return BadRequest(new { message = "error changing orderstatus" });
        }
        return Ok(result);
    }

    [HttpDelete, Authorize(Policy = "Admin"), Route("{orderId}")]
    public async Task<IActionResult> Delete(int orderId)
    {
        var result = await _orderService.Delete(orderId);
        if (result)
        {
            return Ok(new { message = "order deleted" });
        }

        return BadRequest(new { message = "order didn't deleted" });
    }
}