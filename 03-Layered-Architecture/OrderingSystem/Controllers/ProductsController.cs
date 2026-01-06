using Microsoft.AspNetCore.Mvc;
using OrderingSystem.DataAccess.Entities;
using OrderingSystem.DataValidation;
using OrderingSystem.DataValidation.Models;
using OrderingSystem.DataValidation.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace UserInterface.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IProductValidationService _productValidationService;

    public ProductsController(IProductValidationService productValidationService)
    {
        _productValidationService = productValidationService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
    {
        return Ok(await _productValidationService.GetAllProductsAsync());
    }

    [HttpPost]
    public async Task<ActionResult<Product>> CreateProduct(CreateProductRequest request)
    {
        try
        {
            var product = await _productValidationService.ValidateAndCreateProductAsync(request);
            // Assuming GetProducts doesn't support by ID for now, or we can add Method.
            // But CreatedAtAction needs a method. I'll just use GetProducts (list) for now or add GetProductById to Service if I want correctness.
            // Refactoring to keep it simple, returning Ok or Created w/o location if specific getter missing.
            // Or I can add `GetProduct` to Service.
            // Previous code had `CreatedAtAction(nameof(GetProducts), new { id = ... }, product)`.
            // But `GetProducts` returned a list. That was weird in original code too?
            // "GetProducts" -> List. CreatedAtAction needs a URL?
            // Usually we have GetProduct(id).
            // I'll just return Created(uri, body).
            return Created($"/api/products/{product.Id}", product);
        }
        catch (ValidationException ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
