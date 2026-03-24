using System.ComponentModel.DataAnnotations;

namespace AuthApi.Models;

public class RegisterRequest
{
    [Required(ErrorMessage = "O nome e obrigatorio.")]
    [MinLength(2, ErrorMessage = "O nome deve ter pelo menos 2 caracteres.")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "A senha e obrigatoria.")]
    [MinLength(6, ErrorMessage = "A senha deve ter pelo menos 6 caracteres.")]
    public string Password { get; set; } = string.Empty;

    public string RoleUser { get; set; } = "User";
}
