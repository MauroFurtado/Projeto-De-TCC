using dotenv.net;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using ResrvaDeSala_API.Models;

namespace ResrvaDeSala_API.Data
{
    public partial class ApplicationDbContext : DbContext
    {
        static ApplicationDbContext()
        {
            TryRegisterPostgresEnums();
        }

        public ApplicationDbContext() { }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public virtual DbSet<ReservaModel> Reservas { get; set; }
        public virtual DbSet<SalaModel> Salas { get; set; }
        public virtual DbSet<UsuarioModel> Usuarios { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (optionsBuilder.IsConfigured)
            {
                return;
            }

            try { DotEnv.Load(); } catch { /* fallback */ }

            var host = Environment.GetEnvironmentVariable("DB_HOST");
            var port = Environment.GetEnvironmentVariable("DB_PORT");
            var database = Environment.GetEnvironmentVariable("POSTGRES_DB");
            var username = Environment.GetEnvironmentVariable("POSTGRES_USER");
            var password = Environment.GetEnvironmentVariable("POSTGRES_PASSWORD");

            var connectionString = $"Host={host};Port={port};Database={database};Username={username};Password={password}";
            optionsBuilder.UseNpgsql(connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasPostgresEnum("perfil_usuario", new[] { "admin", "comum" })
                .HasPostgresEnum<TipoSala>("public", "tipo_sala");

            // Usar nomes exatamente em minúsculas / snake_case para combinar com o banco (evita "Usuarios" com U maiúsculo)
            modelBuilder.Entity<UsuarioModel>(entity =>
            {
                entity.ToTable("usuarios");
                entity.HasKey(e => e.Id).HasName("usuarios_pkey");

                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.Nome).HasColumnName("nome").HasMaxLength(255);
                entity.Property(e => e.Email).HasColumnName("email").HasMaxLength(255);
                entity.Property(e => e.Senha).HasColumnName("senha");
                entity.Property(e => e.Perfil).HasColumnName("perfil").HasColumnType("perfil_usuario").HasDefaultValue("comum");
                entity.Property(e => e.CriadoEm).HasColumnName("criado_em")
                      .HasDefaultValueSql("now()")
                      .HasColumnType("timestamp without time zone");

                entity.HasIndex(e => e.Email, "usuarios_email_key").IsUnique();
            });

            modelBuilder.Entity<SalaModel>(entity =>
            {
                entity.ToTable("salas");
                entity.HasKey(e => e.Id).HasName("salas_pkey");

                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.Nome).HasColumnName("nome").HasMaxLength(255);
                entity.Property(e => e.Tipo).HasColumnName("tipo").HasColumnType("tipo_sala");
                entity.Property(e => e.Capacidade).HasColumnName("capacidade");
                entity.Property(e => e.Localizacao).HasColumnName("localizacao").HasMaxLength(255);
                entity.Property(e => e.Disponivel).HasColumnName("disponivel").HasDefaultValue(true);
                entity.Property(e => e.CriadoEm).HasColumnName("criado_em")
                      .HasDefaultValueSql("now()")
                      .HasColumnType("timestamp without time zone");
            });

            modelBuilder.Entity<ReservaModel>(entity =>
            {
                entity.ToTable("reservas");
                entity.HasKey(e => e.Id).HasName("reservas_pkey");

                entity.HasIndex(e => e.SalaId, "idx_reservas_sala");
                entity.HasIndex(e => e.UsuarioId, "idx_reservas_usuario");
                entity.HasIndex(e => new { e.SalaId, e.DataReserva, e.HoraInicio, e.HoraFim }, "reserva_unica").IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.UsuarioId).HasColumnName("usuario_id");
                entity.Property(e => e.SalaId).HasColumnName("sala_id");
                entity.Property(e => e.DataReserva).HasColumnName("data_reserva");
                entity.Property(e => e.HoraInicio).HasColumnName("hora_inicio");
                entity.Property(e => e.HoraFim).HasColumnName("hora_fim");
                entity.Property(e => e.CriadoEm).HasColumnName("criado_em")
                      .HasDefaultValueSql("now()")
                      .HasColumnType("timestamp without time zone");

                entity.HasOne(d => d.Sala).WithMany(p => p.Reservas)
                      .HasForeignKey(d => d.SalaId)
                      .HasConstraintName("reservas_sala_id_fkey");

                entity.HasOne(d => d.Usuario).WithMany(p => p.Reservas)
                      .HasForeignKey(d => d.UsuarioId)
                      .HasConstraintName("reservas_usuario_id_fkey");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

        private static void TryRegisterPostgresEnums()
        {
            try
            {
                NpgsqlConnection.GlobalTypeMapper.MapEnum<TipoSala>("public.tipo_sala");
            }
            catch (ArgumentException)
            {
                // already mapped
            }
            catch (InvalidOperationException)
            {
                // already mapped
            }
        }
    }
}
