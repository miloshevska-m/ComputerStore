﻿using ComputerStore.services.DTOs;
using ComputerStore.services.Interfaces;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class Categorycontroller : ControllerBase
    {
    private readonly Icategoryservice _categoryService;

    public Categorycontroller(Icategoryservice categoryService)
    {
        _categoryService = categoryService;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var categories = await _categoryService.GetAllAsync();
        return Ok(categories);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var category = await _categoryService.GetByIdAsync(id);
        if (category == null)
        {
            return NotFound();
        }
        return Ok(category);
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] CategoryDTO category)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        await _categoryService.AddAsync(category);

        return CreatedAtAction(nameof(Get), new { id = category.Id }, category);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, [FromBody] CategoryDTO category)
    {
        if (id != category.Id)
        {
            return BadRequest();
        }
        await _categoryService.UpdateAsync(category);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var category = await _categoryService.GetByIdAsync(id);
        if (category == null)
        {
            return NotFound();
        }
        await _categoryService.DeleteAsync(id);
        return NoContent();
    }
}

