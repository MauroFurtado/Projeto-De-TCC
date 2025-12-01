using System;
using System.Collections.Generic;

namespace ResrvaDeSala_API.Models
{
    public class UsuarioModel
    {
        public int Id { get; set; }
        public string Nome { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Senha { get; set; } = null!;
        public string Perfil { get; set; } = null!;
        public DateTime? CriadoEm { get; set; }

        public virtual ICollection<ReservaModel> Reservas { get; set; } = new List<ReservaModel>();
    }
}
