namespace ResrvaDeSala_API.DTOs;
public class UsuarioDto
{
    public int Id { get; set; }
    public string Nome { get; set; } = null!;
    public string Email { get; set; } = null!;
    public DateTime? CriadoEm { get; set; }
}