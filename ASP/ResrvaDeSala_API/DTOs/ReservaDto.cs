using System.Text.Json.Serialization;
using ResrvaDeSala_API.Serialization;

namespace ResrvaDeSala_API.DTOs;
public class ReservaDto
{
    public int Id { get; set; }

    [JsonPropertyName("sala_id")]
    public int SalaId { get; set; }

    [JsonPropertyName("usuario_id")]
    public int UsuarioId { get; set; }

    [JsonPropertyName("data_reserva")]
    public DateOnly DataReserva { get; set; }

    [JsonPropertyName("hora_inicio")]
    [JsonConverter(typeof(JsonTimeSpanConverter))]
    public TimeSpan HoraInicio { get; set; }

    [JsonPropertyName("hora_fim")]
    [JsonConverter(typeof(JsonTimeSpanConverter))]
    public TimeSpan HoraFim { get; set; }

    [JsonPropertyName("criado_em")]
    public DateTime? CriadoEm { get; set; }
}