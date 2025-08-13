using MediatR;
using SignUpApi.Application.DTOs;

namespace SignUpApi.Application.Commands
{
  public class UpdateUserProfileCommand : IRequest<UpdateUserProfileResult>
  {
    public Guid UserId { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
  }
}
