using MediatR;
using Microsoft.AspNetCore.Mvc;
using SignUpApi.Application.Commands;
using SignUpApi.Application.DTOs;
using SignUpApi.Application.Validators;

namespace SignUpApi.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
  private readonly IMediator _mediator;
  private readonly SignUpCommandValidator _signUpValidator;
  private readonly LoginCommandValidator _loginValidator;

  public AuthController(IMediator mediator)
  {
    _mediator = mediator;
    _signUpValidator = new SignUpCommandValidator();
    _loginValidator = new LoginCommandValidator();
  }

  [HttpPost("signup")]
  public async Task<IActionResult> SignUp([FromBody] SignUpCommand command)
  {
    // Validação
    var validationResult = await _signUpValidator.ValidateAsync(command);
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

  [HttpPost("login")]
  public async Task<IActionResult> Login([FromBody] LoginCommand command)
  {
    // Validação
    var validationResult = await _loginValidator.ValidateAsync(command);
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
