using NpgsqlTypes;

namespace ResrvaDeSala_API.Models
{
    public enum TipoSala
    {
        [PgName("laboratorio")]
        Laboratorio,

        [PgName("aula")]
        Aula,

        [PgName("reuniao")]
        Reuniao,

        [PgName("auditório")]
        Auditorio
    }
}
