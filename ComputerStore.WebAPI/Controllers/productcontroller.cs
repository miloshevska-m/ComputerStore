using ComputerStore.services.DTOs;
using ComputerStore.services.Interfaces;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class Productcontroller : ControllerBase
    {
    private readonly Iproductservice _productService;


    public Productcontroller(Iproductservice productService)
    {
        _productService = productService;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var products = await _productService.GetAllAsync();
        return Ok(products);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var product = await _productService.GetByIdAsync(id);
        if (product == null)
        {
            return NotFound();
        }
        return Ok(product);
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] ProductDTO product)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        await _productService.AddAsync(product);
        return CreatedAtAction(nameof(Get), new { id = product.Id }, product);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, [FromBody] ProductDTO product)
    {
        if (id != product.Id)
        {
            return BadRequest();
        }
        await _productService.UpdateAsync(product);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var product = await _productService.GetByIdAsync(id);
        if (product == null)
        {
            return NotFound();
        }
        await _productService.DeleteAsync(id);
        return NoContent();
    }

    [HttpPost("import")]
    public async Task<IActionResult> ImportStock([FromBody] IEnumerable<StockDTO> stockDtos)
    {
        try
        {
            await _productService.ImportStockAsync(stockDtos);
            return Ok("Stock imported successfully.");
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = "Import failed: " + ex.Message });
        }
    }
}