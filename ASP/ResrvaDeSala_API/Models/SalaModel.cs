using System;
using System.Collections.Generic;

namespace ResrvaDeSala_API.Models
{
    public class SalaModel
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Tipo { get; set; }
        public int Capacidade { get; set; }
        public string? Localizacao { get; set; }
        public bool Disponivel { get; set; }
        public DateTime? CriadoEm { get; set; }

        public virtual ICollection<ReservaModel> Reservas { get; set; } = new List<ReservaModel>();
    }
}
