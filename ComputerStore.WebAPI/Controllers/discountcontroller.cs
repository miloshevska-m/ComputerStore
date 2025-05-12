using ComputerStore.services.DTOs;
using ComputerStore.services.Interfaces;
using ComputerStore.services.Services;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class Discountcontroller : ControllerBase
    {
    private readonly Idiscountservice _discountService;

    public Discountcontroller(Idiscountservice discountService)
    {
        _discountService = discountService;
    }

    [HttpPost("calculate-discount")]
    public async Task<IActionResult> CalculateDiscount([FromBody] ShoppingbasketDTO basket)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var result = await _discountService.CalculateDiscountAsync(basket);
            return Ok(result);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { Error = ex.Message });
        }
    }
}
