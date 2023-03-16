using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductMvc.Data;
using ProductMvc.Dtoes;
using ProductMvc.Entities;
using ProductMvc.Exceptions;
using ProductMvc.Models;
using ProductMvc.Services;

namespace ProductMvc.Controllers;

public class ProductsController : Controller
{
    private readonly AppDbContext _context;
    private readonly IConfiguration _configuration;
    public ProductsController(AppDbContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    public IActionResult CreateProduct() => View();


    [HttpPost]

    [Authorize(Roles = "admin")]
    public async Task<IActionResult> CreateProduct(CreateProductDto createProductDto)
    {
        if (!ModelState.IsValid) return View();

        if (_context.Products.Any(x => x.Title == createProductDto.Title))
        {
            ModelState.AddModelError("Title", "this name exists");
            return View();
        }

        if (createProductDto.Price == 0)
        {
            ModelState.AddModelError("Price", "it can't be 0");
            return View();
        }

        if (createProductDto.Quantly == 0)
        {
            ModelState.AddModelError("Quently", "it can't be 0");
            return View();
        }

        var product = new Product();

        product.Title = createProductDto.Title;
        product.Quantly = createProductDto.Quantly;
        product.Price = createProductDto.Price;

        await _context.Products.AddAsync(product);
        await _context.SaveChangesAsync();

        return Redirect($"GetProduct?productId={product.Id}");
    }

    
    public async Task<IActionResult> GetProducts()
    {
        var vatRate = _configuration.GetValue<double>("Values:VAT_RATE");

        var products = await _context.Products.ToListAsync();
        var productsVM = new List<GetProductsVM>();

        if (products is null)
            return View(null);
        
        foreach (var product in products)
        {
            var productVM = new GetProductsVM();
            productVM.Id = product.Id;
            productVM.Title = product.Title;
            productVM.Quantly = product.Quantly;
            productVM.Price = product.Price;
            productVM.TotalPriceWithVat = CalculateService.TotalPriceWithVat(vatRate, product.Quantly, product.Price);  // use fuction to find

            productsVM.Add(productVM);
        }
        
        return View(productsVM);
    }
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> GetProduct(int productId)
    {
        var vatRate = _configuration.GetValue<double>("Values:VAT_RATE");
        
        var product = await _context.Products.FirstOrDefaultAsync(x => x.Id == productId);
        if (product is null)
            throw new NotFoundException<Product>();

        var result = product.Adapt<GetProductVM>();
        result.TotalPriceWithVat = CalculateService.TotalPriceWithVat(vatRate, product.Quantly, product.Price);

        return View(result);
    }

    [HttpGet]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> UpdateProduct(int productId, string? error)
    {
        var product = await _context.Products.FirstOrDefaultAsync(x => x.Id == productId);
        ViewData["productId"] = productId;

        if (product is null)
            throw new NotFoundException<Product>();

        if (error is not null)
        {
            ModelState.AddModelError("", error);
            return View();
        }

        return View();
    }


    [HttpPost]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> UpdateProduct(int productId, UpdateProductDto updateProductDto)
    {

        if (!ModelState.IsValid)
        {
            return Redirect($"UpdateProduct?error={"error occured"}&&productId={productId}");
        }

        var product = await _context.Products.FirstAsync(x => x.Id == productId);

        if ((_context.Products.Any(x => x.Title == updateProductDto.Title)) == true)
        {
            ModelState.AddModelError("Name", "name exists");
            return View();
        }


        product.Title = updateProductDto.Title;
        product.Quantly = updateProductDto.Quantly;
        product.Price = updateProductDto.Price;

        _context.Products.Update(product);
        await _context.SaveChangesAsync();

        return LocalRedirect($"/Products/GetProduct?productId={productId}");
    }

    [Authorize(Roles = "admin")]
    public async Task<IActionResult> DeleteProduct(int productId)
    {
        var product = _context.Products.FirstOrDefault(x => x.Id == productId);

        if (product is null)
            throw new NotFoundException<Product>();


        _context.Products.Remove(product);
        await _context.SaveChangesAsync();

        return RedirectToAction("GetProducts");
    }
}
