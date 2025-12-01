namespace ResrvaDeSala_API.DTOs;
public class ReservaDto
{
    public int Id { get; set; }
    public int SalaId { get; set; }
    public int UsuarioId { get; set; }
    public DateOnly DataReserva { get; set; }
    public TimeOnly HoraInicio { get; set; }
    public TimeOnly HoraFim { get; set; }
    public DateTime? CriadoEm { get; set; }
}