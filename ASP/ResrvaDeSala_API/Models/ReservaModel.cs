using System;

namespace ResrvaDeSala_API.Models
{
    public class ReservaModel
    {
        public int Id { get; set; }
        public int SalaId { get; set; }
        public int UsuarioId { get; set; }
        public DateOnly DataReserva { get; set; }
        public TimeSpan HoraInicio { get; set; }
        public TimeSpan HoraFim { get; set; }
        public DateTime? CriadoEm { get; set; }
        public virtual SalaModel Sala { get; set; }
        public virtual UsuarioModel Usuario { get; set; }
    }
}
