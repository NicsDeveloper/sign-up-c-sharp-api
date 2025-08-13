using MediatR;
using SignUpApi.Application.DTOs;

namespace SignUpApi.Application.Commands;

public class LoginCommand : IRequest<LoginResult>
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
