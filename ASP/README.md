# API ASP.NET Core - Sistema de Reserva de Salas

## 📝 Descrição

Implementação da API de Reserva de Salas utilizando **ASP.NET Core 8.0** com Entity Framework Core, seguindo os princípios de Clean Architecture e SOLID.

## 🏗️ Arquitetura

### Camadas da Aplicação

```
ResrvaDeSala_API/
├── Controllers/           # Controladores da API (Presentation Layer)
│   ├── AuthController.cs
│   ├── ReservasController.cs
│   ├── SalasController.cs
│   └── UsuariosController.cs
│
├── Data/                  # Camada de Dados (Data Layer)
│   ├── ApplicationDbContext.cs
│   └── AutoMapperProfile.cs
│
├── DTOs/                  # Data Transfer Objects
│   ├── ReservaCreateDto.cs
│   ├── ReservaDto.cs
│   ├── SalaDto.cs
│   ├── UsuarioCreateDto.cs
│   └── UsuarioDto.cs
│
├── Models/                # Modelos de Domínio (Domain Layer)
│   ├── ReservaModel.cs
│   ├── SalaModel.cs
│   ├── TipoSala.cs
│   └── UsuarioModel.cs
│
└── Services/              # Serviços da Aplicação (Service Layer)
    └── JwtService.cs
```

## 🛠️ Tecnologias e Pacotes

### Dependências Principais

```xml
<PackageReference Include="AutoMapper.Extensions.Microsoft.DependencyInjection" />
<PackageReference Include="BCrypt.Net-Next" />
<PackageReference Include="DotNetEnv" />
<PackageReference Include="Microsoft.AspNetCore.Authentication.JwtBearer" />
<PackageReference Include="Microsoft.EntityFrameworkCore.Design" />
<PackageReference Include="Microsoft.EntityFrameworkCore.Tools" />
<PackageReference Include="Npgsql.EntityFrameworkCore.PostgreSQL" />
<PackageReference Include="Swashbuckle.AspNetCore" />
```

### Frameworks e Bibliotecas

- **.NET 8.0**: Framework base
- **Entity Framework Core**: ORM
- **Npgsql**: Provider PostgreSQL
- **AutoMapper**: Mapeamento objeto-objeto
- **BCrypt.Net**: Hash de senhas
- **JWT Bearer**: Autenticação
- **Swashbuckle**: Documentação Swagger/OpenAPI

## ⚙️ Configuração

### appsettings.json

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=ReservaSalas;Username=MauroFurtado;Password=MGFurtad0"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}
```

### Variáveis de Ambiente

Crie um arquivo `.env` na raiz do projeto:

```env
ConnectionStrings__DefaultConnection=Host=postgres;Port=5432;Database=ReservaSalas;Username=MauroFurtado;Password=
JWT_SECRET=seu-super-secreto-jwt-aqui-mudar-em-producao
JWT_ISSUER=ReservaSalasAPI
JWT_AUDIENCE=ReservaSalasClients
AUTH_DISABLED=false
```

## 🚀 Executando o Projeto

### Requisitos

- .NET SDK 8.0 ou superior
- PostgreSQL 16+

### Modo Desenvolvimento

```bash
# Restaurar dependências
dotnet restore

# Executar migrations
dotnet ef database update

# Executar aplicação
dotnet run

# Executar em modo watch
dotnet watch run
```

### Docker

```bash
# Build da imagem
docker build -t reserva-asp-api .

# Executar container
docker run -p 5000:8080 \
  -e ConnectionStrings__DefaultConnection="Host=postgres;Port=5432;Database=ReservaSalas;Username=MauroFurtado;Password=MGFurtad0" \
  reserva-asp-api
```

### Com Docker Compose (Recomendado)

```bash
# Do diretório raiz do projeto
docker-compose up -d asp_api
```

## 📡 Endpoints

### Base URL
```
http://localhost:5000/api
```


### Autenticação

#### POST /api/auth/login
```json
// Request
{
  "email": "usuario@exemplo.com",
  "senha": "senha123"
}

// Response 200 OK
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "usuario": {
    "id": 1,
    "nome": "João Silva",
    "email": "usuario@exemplo.com",
    "perfil": "comum"
  }
}
```

### Usuários

#### GET /api/usuarios
```json
// Response 200 OK
[
  {
    "id": 1,
    "nome": "João Silva",
    "email": "joao@exemplo.com",
    "perfil": "admin",
    "criadoEm": "2026-01-20T10:00:00Z"
  }
]
```

#### POST /api/usuarios
```json
// Request
{
  "nome": "Maria Santos",
  "email": "maria@exemplo.com",
  "senha": "senha123",
  "perfil": "comum"
}

// Response 201 Created
{
  "id": 2,
  "nome": "Maria Santos",
  "email": "maria@exemplo.com",
  "perfil": "comum",
  "criadoEm": "2026-01-22T14:30:00Z"
}
```

### Salas

#### GET /api/salas
```json
// Response 200 OK
[
  {
    "id": 1,
    "nome": "Sala 101",
    "tipo": "aula",
    "capacidade": 30,
    "localizacao": "Bloco A",
    "disponivel": true,
    "criadoEm": "2026-01-15T08:00:00Z"
  }
]
```

#### POST /api/salas
```json
// Request
{
  "nome": "Laboratório 205",
  "tipo": "laboratorio",
  "capacidade": 25,
  "localizacao": "Bloco B"
}

// Response 201 Created
{
  "id": 5,
  "nome": "Laboratório 205",
  "tipo": "laboratorio",
  "capacidade": 25,
  "localizacao": "Bloco B",
  "disponivel": true,
  "criadoEm": "2026-01-22T14:35:00Z"
}
```

### Reservas

#### GET /api/reservas
```json
// Response 200 OK
[
  {
    "id": 1,
    "usuarioId": 1,
    "salaId": 5,
    "dataReserva": "2026-02-10",
    "horaInicio": "10:00:00",
    "horaFim": "12:00:00",
    "criadoEm": "2026-01-22T14:40:00Z"
  }
]
```

#### POST /api/reservas
```json
// Request
{
  "usuarioId": 1,
  "salaId": 5,
  "dataReserva": "2026-02-10",
  "horaInicio": "10:00",
  "horaFim": "12:00"
}

// Response 201 Created
{
  "id": 10,
  "usuarioId": 1,
  "salaId": 5,
  "dataReserva": "2026-02-10",
  "horaInicio": "10:00:00",
  "horaFim": "12:00:00",
  "criadoEm": "2026-01-22T14:40:00Z"
}
```


## 🏛️ Padrões Utilizados

### Repository Pattern
```csharp
// Implementado através do DbContext do Entity Framework
public class ApplicationDbContext : DbContext
{
    public DbSet<UsuarioModel> Usuarios { get; set; }
    public DbSet<SalaModel> Salas { get; set; }
    public DbSet<ReservaModel> Reservas { get; set; }
}
```

### Dependency Injection
```csharp
// Program.cs
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddAutoMapper(typeof(AutoMapperProfile));
```

### DTO Pattern
```csharp
// Separação entre entidades de domínio e DTOs
public class UsuarioDto
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public string Email { get; set; }
    public string Perfil { get; set; }
}
```

## 📊 Performance

### Otimizações Implementadas

- **AsNoTracking()**: Queries apenas leitura
- **Índices no Banco**: Campos frequentemente consultados
- **AutoMapper**: Mapeamento otimizado
- **Connection Pooling**: Pool de conexões PostgreSQL
- **Async/Await**: Operações assíncronas

### Exemplo de Query Otimizada

```csharp
public async Task<List<ReservaDto>> GetReservasAsync()
{
    return await _context.Reservas
        .AsNoTracking()
        .Include(r => r.Usuario)
        .Include(r => r.Sala)
        .Select(r => new ReservaDto
        {
            Id = r.Id,
            UsuarioId = r.UsuarioId,
            SalaId = r.SalaId,
            DataReserva = r.DataReserva,
            HoraInicio = r.HoraInicio,
            HoraFim = r.HoraFim
        })
        .ToListAsync();
}
```

## 🔒 Segurança

### JWT Authentication

```csharp
// Validação de Token
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtIssuer,
            ValidAudience = jwtAudience,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtSecret))
        };
    });
```

### Password Hashing

```csharp
// BCrypt para hash de senhas
using BCrypt.Net;

var hashedPassword = BCrypt.HashPassword(senha);
var isValid = BCrypt.Verify(senha, hashedPassword);
```

## 🐛 Troubleshooting

### Erro: "Connection refused"
```bash
# Verificar se PostgreSQL está rodando
docker-compose ps postgres

# Verificar string de conexão
echo $ConnectionStrings__DefaultConnection
```


### Erro: "Port already in use"
```bash
# Alterar porta em launchSettings.json
"applicationUrl": "http://localhost:5001"
```

## 📚 Recursos Adicionais

- [Documentação ASP.NET Core](https://docs.microsoft.com/aspnet/core)
- [Entity Framework Core](https://docs.microsoft.com/ef/core)
- [JWT Best Practices](https://tools.ietf.org/html/rfc8725)

## 🤝 Contribuindo

1. Fork o projeto
2. Crie uma branch para sua feature (`git checkout -b feature/AmazingFeature`)
3. Commit suas mudanças (`git commit -m 'feat: Add some AmazingFeature'`)
4. Push para a branch (`git push origin feature/AmazingFeature`)
5. Abra um Pull Request

---

**Versão**: 1.0.0  
**Última Atualização**: Janeiro 2026
