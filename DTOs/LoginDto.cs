using System.ComponentModel.DataAnnotations;

namespace AuthApi.DTOs;

public class LoginDto
{
    [Required(ErrorMessage = "O nome e obrigatorio.")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "A senha e obrigatoria.")]
    public string Password { get; set; } = string.Empty;
}
