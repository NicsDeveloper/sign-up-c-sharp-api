using MediatR;
using SignUpApi.Application.Commands;
using SignUpApi.Application.DTOs;
using SignUpApi.Domain.Interfaces;

namespace SignUpApi.Application.Handlers
{
  public class UpdateUserProfileCommandHandler(IUserRepository userRepository) : IRequestHandler<UpdateUserProfileCommand, UpdateUserProfileResult>
  {
    private readonly IUserRepository _userRepository = userRepository;

    public async Task<UpdateUserProfileResult> Handle(UpdateUserProfileCommand request, CancellationToken cancellationToken)
    {
      // Buscar usuário
      Domain.Entities.User? user = await _userRepository.GetByIdAsync(request.UserId);
      if (user == null)
      {
        return UpdateUserProfileResult.Failure("Usuário não encontrado");
      }

      // Validar dados
      if (string.IsNullOrWhiteSpace(request.FirstName))
      {
        return UpdateUserProfileResult.Failure("Nome não pode ser vazio");
      }

      if (string.IsNullOrWhiteSpace(request.LastName))
      {
        return UpdateUserProfileResult.Failure("Sobrenome não pode ser vazio");
      }

      // Atualizar perfil
      user.UpdateProfile(request.FirstName, request.LastName);

      // Salvar no repositório
      Domain.Entities.User updatedUser = await _userRepository.UpdateAsync(user);

      // Retornar resultado
      UpdateUserProfileData data = new()
      {
        Id = updatedUser.Id,
        Email = updatedUser.Email.Value,
        FirstName = updatedUser.FirstName,
        LastName = updatedUser.LastName,
        IsActive = updatedUser.IsActive,
        UpdatedAt = updatedUser.UpdatedAt ?? DateTime.UtcNow
      };

      return UpdateUserProfileResult.Success(data);
    }
  }
}
