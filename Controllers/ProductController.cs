using assignment1.Data;
using Microsoft.AspNetCore.Mvc;
using assignment1.Models;
using Microsoft.EntityFrameworkCore;

namespace assignment1.Controllers;

public class ProductController : Controller
{
    private readonly ApplicationDBContext _context;
    public ProductController(ApplicationDBContext context)
    {
        _context = context;
    }
    private static List<Product> _products = new List<Product>()
    {
        new Product { Id = 1, Name = "Laptop", Category = "Electronics", Price = 1200.00m, Quantity = 5, LowStockThreshold = 2 },
        new Product { Id = 2, Name = "T-Shirt", Category = "Clothing", Price = 15.99m, Quantity = 20, LowStockThreshold = 5 },
        new Product { Id = 3, Name = "Coffee Maker", Category = "Appliances", Price = 75.50m, Quantity = 8, LowStockThreshold = 3 },
        new Product { Id = 4, Name = "Smartphone", Category = "Electronics", Price = 999.99m, Quantity = 3, LowStockThreshold = 1 }
    };
    
    public async Task<IActionResult> Index(string search, string category, string sortBy)
    {
        // Use IQueryable for initial query
        var query = _context.Products.AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            // Use ToLower for server-side evaluation
            search = search.ToLowerInvariant();
            query = query.Where(p => p.Name.ToLower().Contains(search));
        }

        if (!string.IsNullOrWhiteSpace(category))
        {
            // Use ToLower for server-side evaluation
            category = category.ToLowerInvariant();
            query = query.Where(p => p.Category.ToLower().Contains(category));
        }

        // Execute the query and convert to a list
        var products = await query.ToListAsync();

        // Perform client-side operations if needed
        products = sortBy switch
        {
            "price" => products.OrderBy(p => p.Price).ToList(),
            "name" => products.OrderBy(p => p.Name).ToList(),
            "quantity" => products.OrderBy(p => p.Quantity).ToList(),
            _ => products
        };

        return View(products);
    }

    public IActionResult Create()
    {
        return View(new Product());
    }

    [HttpPost]
    public async Task<IActionResult> Create(Product product)
    {
        if (ModelState.IsValid)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        else
        {
            foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
            {
                Console.WriteLine(error.ErrorMessage);
            }
        }
        return View(product);
    }

    public async Task<IActionResult> Delete(int id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product != null)
        {
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
        }
        return RedirectToAction("Index");
    }

    public IActionResult About()
    {
        return View();
    }
}