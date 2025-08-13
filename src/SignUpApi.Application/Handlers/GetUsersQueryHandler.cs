using MediatR;
using SignUpApi.Application.DTOs;
using SignUpApi.Application.Queries;
using SignUpApi.Domain.Interfaces;

namespace SignUpApi.Application.Handlers
{
  public class GetUsersQueryHandler(IUserRepository userRepository) : IRequestHandler<GetUsersQuery, List<UserDto>>
  {
    private readonly IUserRepository _userRepository = userRepository;

    public async Task<List<UserDto>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
      IEnumerable<Domain.Entities.User> users = await _userRepository.GetAllAsync();

      List<UserDto> userDtos = users
              .Where(u => !request.ActiveOnly || u.IsActive)
              .Select(user => new UserDto
              {
                Id = user.Id,
                Email = user.Email.Value,
                FirstName = user.FirstName,
                LastName = user.LastName,
                IsActive = user.IsActive,
                CreatedAt = user.CreatedAt,
                LastLoginAt = user.LastLoginAt
              })
              .ToList();

      return userDtos;
    }
  }
}
