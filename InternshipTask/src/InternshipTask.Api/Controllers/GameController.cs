using InternshipTask.Application.DTOs;
using InternshipTask.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;

namespace InternshipTask.Api.Controllers;

[ApiController]
[Route("api/games")]
public sealed class GameController : ControllerBase
{
    private readonly IGameService _gameService;

    public GameController(IGameService gameService) => _gameService = gameService;
    
    [HttpPost]
    [Route("create")]
    [ProducesResponseType(typeof(Guid),StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateGame([FromBody] CreateGameDto dto, CancellationToken token)
    {
        var id = await _gameService.CreateGameAsync(dto, token);
        return StatusCode(StatusCodes.Status201Created, id);
    }

    [HttpGet]
    [Route("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetGame(Guid id, CancellationToken token)
    {
        var result = await _gameService.GetGameAsync(id, token);
        return Ok(result);
    }

    [HttpPost]
    [Route("{id:guid}/moves")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> MakeMove(Guid id, [FromBody] MakeMoveDto dto, CancellationToken token)
    {
        var eTag = Request.Headers.IfMatch.FirstOrDefault();
        dto.ETag = eTag;
        var result = await _gameService.MakeMoveAsync(dto, token);
        
        Response.Headers.ETag = new StringValues($"\"{result.ETag}\"");
        return Ok(result);
    }
}