using MediatR;
using SignUpApi.Application.Commands;
using SignUpApi.Application.DTOs;
using SignUpApi.Domain.Interfaces;

namespace SignUpApi.Application.Handlers
{
  public class LoginCommandHandler(
      IUserRepository userRepository,
      IPasswordHasher passwordHasher,
      IJwtTokenGenerator jwtTokenGenerator) : IRequestHandler<LoginCommand, LoginResult>
  {
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IPasswordHasher _passwordHasher = passwordHasher;
    private readonly IJwtTokenGenerator _jwtTokenGenerator = jwtTokenGenerator;

    public async Task<LoginResult> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
      // Buscar usuário por email
      Domain.Entities.User? user = await _userRepository.GetByEmailAsync(request.Email);
      if (user == null)
      {
        return LoginResult.Failure("Credenciais inválidas");
      }

      // Verificar senha
      if (!_passwordHasher.VerifyPassword(request.Password, user.Password.Value))
      {
        return LoginResult.Failure("Credenciais inválidas");
      }

      // Gerar token JWT
      string token = _jwtTokenGenerator.GenerateToken(user);

              // Atualizar último login
        user.RecordLogin();
        await _userRepository.UpdateAsync(user);

      // Retornar resultado
      LoginData loginData = new()
      {
        Token = token,
        User = new UserDto
        {
          Id = user.Id,
          Email = user.Email.Value,
          FirstName = user.FirstName,
          LastName = user.LastName,
          IsActive = user.IsActive
        }
      };

      return LoginResult.Success(loginData);
    }
  }
}
