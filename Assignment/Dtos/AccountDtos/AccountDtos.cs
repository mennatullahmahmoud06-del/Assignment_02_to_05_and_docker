using System.ComponentModel.DataAnnotations;

namespace Assignment.Dtos.AccountDtos
{
    public sealed record UserRegisterRequest(
        [property: Required, StringLength(100, MinimumLength = 2)] string FullName,
        [property: Required, EmailAddress] string Email,
        [property: Required] string Password);

    public sealed record UserLoginRequest(
        [property: Required, EmailAddress] string Email,
        [property: Required] string Password);
}
