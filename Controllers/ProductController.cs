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
    
    public async Task<IActionResult> Index(string search, string category, string sortBy)
    {
        var query = _context.Products.AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            search = search.ToLowerInvariant();
            query = query.Where(p => p.Name.ToLower().Contains(search));
        }

        if (!string.IsNullOrWhiteSpace(category))
        {
            category = category.ToLowerInvariant();
            query = query.Where(p => p.Category.ToLower().Contains(category));
        }
        
        var products = await query.ToListAsync();
        
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
    
    public async Task<IActionResult> Search(string searchQuery, string category, decimal? minPrice, decimal? maxPrice, bool lowStock)
    {
        var products = _context.Products.AsQueryable();
        
        if (!string.IsNullOrEmpty(searchQuery))
        {
            products = products.Where(p => p.Name.ToLower().Contains(searchQuery.ToLower()) || 
                                           p.Category.ToLower().Contains(searchQuery.ToLower()));
        }
        
        if (!string.IsNullOrEmpty(category))
        {
            products = products.Where(p => p.Category == category);
        }
        
        if (minPrice.HasValue)
        {
            products = products.Where(p => p.Price >= minPrice);
        }
        if (maxPrice.HasValue)
        {
            products = products.Where(p => p.Price <= maxPrice);
        }
        
        if (lowStock)
        {
            products = products.Where(p => p.Quantity <= p.LowStockThreshold);
        }
        
        var filteredProducts = await products.ToListAsync();
        
        return PartialView("_ProductListPartial", filteredProducts);
    }



}