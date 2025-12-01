namespace ResrvaDeSala_API.DTOs;
public class SalaDto
{
    public int Id { get; set; }
    public string Nome { get; set; } = null!;
    public int Capacidade { get; set; }
    public bool? Disponivel { get; set; }
}