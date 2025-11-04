using Dogshouse.DTOs;
using Dogshouse.Models;
using Dogshouse.Services.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Dogshouse.Controllers;

[ApiController]
[Route("[controller]")]
public class DogsController : ControllerBase
{
    private readonly IDogService _dogService;

    public DogsController(IDogService dogService)
    {
        _dogService = dogService;
    }

    [HttpGet()]
    [ProducesResponseType(typeof(IEnumerable<DogDto>), 200)]
    public async Task<IActionResult> GetAllDogs([FromQuery] QueryParameters parameters)
    {
        var result = await _dogService.GetAllDogsAsync(parameters);
        return Ok(result.Data);
    }

    [HttpPost()]
    [ProducesResponseType(typeof(DogDto), 201)]
    public async Task<IActionResult> AddDog([FromBody] CreateDogDto dto)
    {
        var result = await _dogService.AddDogAsync(dto);
        return Created(string.Empty, result.Data);
    }
}

