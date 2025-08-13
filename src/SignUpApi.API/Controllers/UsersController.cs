using MediatR;
using Microsoft.AspNetCore.Mvc;
using SignUpApi.Application.Commands;
using SignUpApi.Application.Queries;
using SignUpApi.Application.Validators;

namespace SignUpApi.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
  private readonly IMediator _mediator;
  private readonly UpdateUserProfileCommandValidator _updateProfileValidator;

  public UsersController(IMediator mediator)
  {
    _mediator = mediator;
    _updateProfileValidator = new UpdateUserProfileCommandValidator();
  }

  [HttpGet]
  public async Task<IActionResult> GetUsers([FromQuery] bool activeOnly = false)
  {
    var query = new GetUsersQuery { ActiveOnly = activeOnly };
    var users = await _mediator.Send(query);

    return Ok(users);
  }

  [HttpGet("{id}")]
  public IActionResult GetUser(Guid id)
  {
    return NotFound("Funcionalidade em desenvolvimento");
  }

  [HttpPut("{id}/profile")]
  public async Task<IActionResult> UpdateProfile(Guid id, [FromBody] UpdateUserProfileCommand command)
  {
    // Validar se o ID da URL corresponde ao ID do comando
    if (id != command.UserId)
    {
      return BadRequest(new { errors = new[] { "ID da URL deve corresponder ao ID do comando" } });
    }

    // Validação dos dados
    var validationResult = await _updateProfileValidator.ValidateAsync(command);
    if (!validationResult.IsValid)
    {
      var errors = validationResult.Errors.Select(e => e.ErrorMessage);
      return BadRequest(new { errors });
    }

    // Executar comando
    var result = await _mediator.Send(command);

    if (result.IsSuccess)
    {
      return Ok(result.Data);
    }

    return BadRequest(new { errors = result.Errors });
  }
}
