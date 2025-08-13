using MediatR;
using SignUpApi.Application.DTOs;

namespace SignUpApi.Application.Commands;

public class SignUpCommand : IRequest<SignUpResult>
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
}
