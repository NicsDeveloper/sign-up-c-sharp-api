namespace SignUpApi.Application.DTOs
{
  public class LoginResult
  {
    public bool IsSuccess { get; set; }
    public LoginData? Data { get; set; }
    public List<string> Errors { get; set; } = [];

    public static LoginResult Success(LoginData data)
    {
      return new LoginResult
      {
        IsSuccess = true,
        Data = data
      };
    }

    public static LoginResult Failure(params string[] errors)
    {
      return new LoginResult
      {
        IsSuccess = false,
        Errors = [.. errors]
      };
    }
  }

  public class LoginData
  {
    public string Token { get; set; } = string.Empty;
    public UserDto User { get; set; } = new();
  }
}
