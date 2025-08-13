using MediatR;
using SignUpApi.Application.DTOs;

namespace SignUpApi.Application.Queries;

public class GetUsersQuery : IRequest<List<UserDto>>
{
    public bool ActiveOnly { get; set; } = false;
}
