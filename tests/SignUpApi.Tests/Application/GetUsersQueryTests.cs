using FluentAssertions;
using MediatR;
using Moq;
using SignUpApi.Application.DTOs;
using SignUpApi.Application.Handlers;
using SignUpApi.Application.Queries;
using SignUpApi.Domain.Entities;
using SignUpApi.Domain.Interfaces;

namespace SignUpApi.Tests.Application;

public class GetUsersQueryTests
{
    private readonly Mock<IUserRepository> _mockUserRepository;
    private readonly GetUsersQueryHandler _handler;

    public GetUsersQueryTests()
    {
        _mockUserRepository = new Mock<IUserRepository>();
        _handler = new GetUsersQueryHandler(_mockUserRepository.Object);
    }

    [Fact]
    public async Task Handle_WithExistingUsers_ShouldReturnUsersList()
    {
        // Arrange
        var query = new GetUsersQuery();
        var users = new List<User>
        {
            new User("user1@example.com", "Password123!", "João", "Silva"),
            new User("user2@example.com", "Password456!", "Maria", "Santos"),
            new User("user3@example.com", "Password789!", "Pedro", "Oliveira")
        };

        _mockUserRepository
            .Setup(x => x.GetAllAsync())
            .ReturnsAsync(users);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Count.Should().Be(3);
        result.Should().Contain(u => u.Email == "user1@example.com");
        result.Should().Contain(u => u.Email == "user2@example.com");
        result.Should().Contain(u => u.Email == "user3@example.com");
    }

    [Fact]
    public async Task Handle_WithNoUsers_ShouldReturnEmptyList()
    {
        // Arrange
        var query = new GetUsersQuery();
        var users = new List<User>();

        _mockUserRepository
            .Setup(x => x.GetAllAsync())
            .ReturnsAsync(users);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task Handle_WithActiveUsersOnly_ShouldReturnOnlyActiveUsers()
    {
        // Arrange
        var query = new GetUsersQuery { ActiveOnly = true };
        var users = new List<User>
        {
            new User("user1@example.com", "Password123!", "João", "Silva"),
            new User("user2@example.com", "Password456!", "Maria", "Santos")
        };

        // Desativar o segundo usuário
        users[1].Deactivate();

        _mockUserRepository
            .Setup(x => x.GetAllAsync())
            .ReturnsAsync(users);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Count.Should().Be(1);
        result.Should().Contain(u => u.Email == "user1@example.com");
        result.Should().NotContain(u => u.Email == "user2@example.com");
    }
}
