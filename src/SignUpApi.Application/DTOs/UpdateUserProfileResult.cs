namespace SignUpApi.Application.DTOs
{
  public class UpdateUserProfileResult
  {
    public bool IsSuccess { get; set; }
    public UpdateUserProfileData? Data { get; set; }
    public List<string> Errors { get; set; } = [];

    public static UpdateUserProfileResult Success(UpdateUserProfileData data)
    {
      return new UpdateUserProfileResult
      {
        IsSuccess = true,
        Data = data
      };
    }

    public static UpdateUserProfileResult Failure(params string[] errors)
    {
      return new UpdateUserProfileResult
      {
        IsSuccess = false,
        Errors = [.. errors]
      };
    }
  }

  public class UpdateUserProfileData
  {
    public Guid Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public DateTime UpdatedAt { get; set; }
  }
}
