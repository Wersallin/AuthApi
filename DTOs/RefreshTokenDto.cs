using System.ComponentModel.DataAnnotations;

namespace AuthApi.DTOs;

public class RefreshTokenDto
{
    [Required(ErrorMessage = "O refresh token e obrigatorio.")]
    public string RefreshToken { get; set; } = string.Empty;
}
