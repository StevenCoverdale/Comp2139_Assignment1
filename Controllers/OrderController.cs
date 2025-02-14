using assignment1.Data;
using Microsoft.AspNetCore.Mvc;
using assignment1.Models;
using Microsoft.EntityFrameworkCore;

namespace assignment1.Controllers;

public class OrderController : Controller
{
    private readonly ApplicationDBContext _context;

    public OrderController(ApplicationDBContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Create()
    {
        var products = await _context.Products.ToListAsync();
        return View(products);
    }

    [HttpPost]
    public async Task<IActionResult> Create(List<OrderItem> orderItems)
    {
        if (orderItems == null || !orderItems.Any())
        {
            return RedirectToAction("Create");
        }

        var order = new Order
        {
            OrderDate = DateTime.UtcNow,
            OrderItems = orderItems,
            TotalPrice = orderItems.Sum(item => item.Price * item.Quantity)
        };

        _context.Orders.Add(order);
        await _context.SaveChangesAsync();

        return RedirectToAction("Confirmation", new { id = order.Id });
    }

    public async Task<IActionResult> Confirmation(int id)
    {
        var order = await _context.Orders
            .Include(o => o.OrderItems)
            .ThenInclude(oi => oi.Product)
            .FirstOrDefaultAsync(o => o.Id == id);

        if (order == null)
        {
            return NotFound();
        }

        return View(order);
    }
}

