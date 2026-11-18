
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductRegistrationSystem.Data;
using ProductRegistrationSystem.Models;
using ProductRegistrationSystem.ViewModels;

namespace ProductRegistrationSystem.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProductsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Products
        public async Task<IActionResult> Index(string? search, string? category)
        {
            var query = _context.Products.AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(p =>
                    p.Name.Contains(search) ||
                    p.Description.Contains(search));
            }

            if (!string.IsNullOrWhiteSpace(category))
            {
                query = query.Where(p => p.Category == category);
            }

            ViewBag.Search = search;
            ViewBag.Category = category;

            ViewBag.Categories = await _context.Products
                .Select(p => p.Category)
                .Distinct()
                .OrderBy(c => c)
                .ToListAsync();

            return View(await query
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync());
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            return View(new ProductCreateViewModel());
        }

        // POST: Products/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                var duplicate = await _context.Products.AnyAsync(p =>
                    p.Name.ToLower() == model.Name.Trim().ToLower() &&
                    p.Category.ToLower() == model.Category.Trim().ToLower());

                if (duplicate)
                {
                    ModelState.AddModelError("Name",
                        "Товар с таким названием в данной категории уже существует.");
                }
            }

            if (ModelState.IsValid)
            {
                var product = new Product
                {
                    Name = model.Name.Trim(),
                    Description = model.Description.Trim(),
                    Category = model.Category.Trim(),
                    Price = model.Price,
                    StockQuantity = model.StockQuantity,
                    ImageUrl = model.ImageUrl,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                };

                _context.Products.Add(product);
                await _context.SaveChangesAsync();

                TempData["Success"] = "Товар успешно добавлен.";
                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }


        // GET: Products/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null)
                return NotFound();

            var model = new ProductDetailsViewModel
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Category = product.Category,
                Price = product.Price,
                StockQuantity = product.StockQuantity,
                ImageUrl = product.ImageUrl,
                IsActive = product.IsActive,
                CreatedAt = product.CreatedAt,
                UpdatedAt = product.UpdatedAt
            };

            return View(model);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null)
                return NotFound();

            var model = new ProductEditViewModel
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Category = product.Category,
                Price = product.Price,
                StockQuantity = product.StockQuantity,
                ImageUrl = product.ImageUrl,
                IsActive = product.IsActive
            };

            return View(model);
        }

        // POST: Products/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ProductEditViewModel model)
        {
            if (id != model.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                var product = await _context.Products.FindAsync(id);

                if (product == null)
                    return NotFound();

                product.Name = model.Name.Trim();
                product.Description = model.Description.Trim();
                product.Category = model.Category.Trim();
                product.Price = model.Price;
                product.StockQuantity = model.StockQuantity;
                product.ImageUrl = model.ImageUrl;
                product.IsActive = model.IsActive;
                product.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                TempData["Success"] = "Товар успешно обновлён.";
                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null)
                return NotFound();

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null)
                return NotFound();

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Товар удалён.";
            return RedirectToAction(nameof(Index));
        }
    }
}
