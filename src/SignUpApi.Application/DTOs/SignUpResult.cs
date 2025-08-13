namespace SignUpApi.Application.DTOs;

public class SignUpResult
{
    public bool IsSuccess { get; set; }
    public SignUpData? Data { get; set; }
    public List<string> Errors { get; set; } = new();

    public static SignUpResult Success(SignUpData data)
    {
        return new SignUpResult
        {
            IsSuccess = true,
            Data = data
        };
    }

    public static SignUpResult Failure(params string[] errors)
    {
        return new SignUpResult
        {
            IsSuccess = false,
            Errors = errors.ToList()
        };
    }
}

public class SignUpData
{
    public string Token { get; set; } = string.Empty;
    public UserDto User { get; set; } = new();
}

public class UserDto
{
    public Guid Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public bool IsActive { get; set; }
    public DateTime? LastLoginAt { get; set; }
}
