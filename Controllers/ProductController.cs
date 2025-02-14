using Microsoft.AspNetCore.Mvc;
using assignment1.Models;

namespace assignment1.Controllers;

public class ProductController : Controller
{
    private static List<Product> _products = new List<Product>()
    {
        new Product { Id = 1, Name = "Laptop", Category = "Electronics", Price = 1200.00m, Quantity = 5, LowStockThreshold = 2 },
        new Product { Id = 2, Name = "T-Shirt", Category = "Clothing", Price = 15.99m, Quantity = 20, LowStockThreshold = 5 },
        new Product { Id = 3, Name = "Coffee Maker", Category = "Appliances", Price = 75.50m, Quantity = 8, LowStockThreshold = 3 },
        new Product { Id = 4, Name = "Smartphone", Category = "Electronics", Price = 999.99m, Quantity = 3, LowStockThreshold = 1 }
    };
    
    public IActionResult Index(string search, string category, string sortBy)
    {
        var products = _products.AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            products = products.Where(p => p.Name.ToLower().Contains(search.ToLower(), System.StringComparison.OrdinalIgnoreCase));
        }

        if (!string.IsNullOrWhiteSpace(category))
        {
            products = products.Where(p => p.Category.ToLower().Contains(category.ToLower(), System.StringComparison.OrdinalIgnoreCase));
        }

        products = sortBy switch
        {
            "price" => products.OrderBy(p => p.Price),
            "name" => products.OrderBy(p => p.Name),
            "quantity" => products.OrderBy(p => p.Quantity),
            _ => products
        };
        
        return View(products.ToList());
    }

    public IActionResult Create()
    {
        return View(new Product());
    }

    [HttpPost]
    public IActionResult Create(Product product)
    {
        if (ModelState.IsValid)
        {
            product.Id = _products.Count + 1;
            _products.Add(product);
            return RedirectToAction("Index");
        }
        return View(product);
    }

    public IActionResult Delete(int id)
    {
        var product = _products.FirstOrDefault(p => p.Id == id);
        if (product != null)
        {
            _products.Remove(product);
        }
        return RedirectToAction("Index");
    }
}