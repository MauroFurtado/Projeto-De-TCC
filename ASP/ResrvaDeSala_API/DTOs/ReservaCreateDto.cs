namespace ResrvaDeSala_API.DTOs;
public class ReservaCreateDto
{
    public int SalaId { get; set; }
    public int UsuarioId { get; set; }
    public DateOnly DataReserva { get; set; }
    public TimeOnly HoraInicio { get; set; }
    public TimeOnly HoraFim { get; set; }
    public string? Descricao { get; set; }
}