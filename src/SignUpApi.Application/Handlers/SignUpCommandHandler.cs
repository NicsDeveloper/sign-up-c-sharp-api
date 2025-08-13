using MediatR;
using SignUpApi.Application.Commands;
using SignUpApi.Application.DTOs;
using SignUpApi.Domain.Entities;
using SignUpApi.Domain.Interfaces;

namespace SignUpApi.Application.Handlers;

public class SignUpCommandHandler : IRequestHandler<SignUpCommand, SignUpResult>
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;

    public SignUpCommandHandler(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        IJwtTokenGenerator jwtTokenGenerator)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _jwtTokenGenerator = jwtTokenGenerator;
    }

    public async Task<SignUpResult> Handle(SignUpCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // Verificar se o email já existe
            var existingUser = await _userRepository.GetByEmailAsync(request.Email);
            if (existingUser != null)
            {
                return SignUpResult.Failure("Email já está em uso");
            }

            // Hash da senha
            var hashedPassword = _passwordHasher.HashPassword(request.Password);

            // Criar o usuário com a senha hasheada
            var user = new User(request.Email, hashedPassword, request.FirstName, request.LastName);

            // Salvar no repositório
            var savedUser = await _userRepository.AddAsync(user);

            // Gerar token JWT
            var token = _jwtTokenGenerator.GenerateToken(savedUser);

            // Mapear para DTO
            var userDto = new UserDto
            {
                Id = savedUser.Id,
                Email = savedUser.Email.Value,
                FirstName = savedUser.FirstName,
                LastName = savedUser.LastName,
                CreatedAt = savedUser.CreatedAt,
                IsActive = savedUser.IsActive
            };

            var signUpData = new SignUpData
            {
                Token = token,
                User = userDto
            };

            return SignUpResult.Success(signUpData);
        }
        catch (Exception)
        {
            return SignUpResult.Failure("Erro interno do servidor");
        }
    }
}
