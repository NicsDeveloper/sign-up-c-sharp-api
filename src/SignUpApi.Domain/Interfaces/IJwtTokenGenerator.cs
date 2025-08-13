using SignUpApi.Domain.Entities;

namespace SignUpApi.Domain.Interfaces;

public interface IJwtTokenGenerator
{
    string GenerateToken(User user);
    bool ValidateToken(string token);
    Guid? GetUserIdFromToken(string token);
}
