using System.Text;
using AutoMapper;
using dotenv.net;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using ResrvaDeSala_API.Data;
using ResrvaDeSala_API.Models;
using ResrvaDeSala_API.Services;

DotEnv.Load();

var builder = WebApplication.CreateBuilder(args);

var jwtSecret = Environment.GetEnvironmentVariable("JWT_SECRET") ?? "seu-super-secreto-padrao-mudar-em-producao";
var jwtIssuer = Environment.GetEnvironmentVariable("JWT_ISSUER") ?? "ReservaSalasAPI";
var jwtAudience = Environment.GetEnvironmentVariable("JWT_AUDIENCE") ?? "ReservaSalasClients";
var authDisabled = Environment.GetEnvironmentVariable("AUTH_DISABLED") ?? "false";

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    var conn = builder.Configuration.GetConnectionString("DefaultConnection")
               ?? Environment.GetEnvironmentVariable("ConnectionStrings__DefaultConnection");

    if (string.IsNullOrWhiteSpace(conn))
    {
        throw new InvalidOperationException("Connection string 'DefaultConnection' not found in configuration or env vars.");
    }

    options.UseNpgsql(conn);
});

// AutoMapper 
builder.Services.AddAutoMapper(cfg => 
{
    cfg.AddMaps(AppDomain.CurrentDomain.GetAssemblies());
});

// Registrar JwtService - SEMPRE
builder.Services.AddScoped<IJwtService, JwtService>();

// Configurar JWT nos servi�os
builder.Configuration["Jwt:Secret"] = jwtSecret;
builder.Configuration["Jwt:Issuer"] = jwtIssuer;
builder.Configuration["Jwt:Audience"] = jwtAudience;

// JWT Authentication - SEMPRE configurar, mesmo se desabilitado
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
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
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret))
    };

    // Se AUTH_DISABLED=true, aceitar qualquer requisi��o
    if (authDisabled.ToLower() == "true")
    {
        options.Events = new JwtBearerEvents
        {
            OnTokenValidated = context =>
            {
                return Task.CompletedTask;
            },
            OnAuthenticationFailed = context =>
            {
                // Criar um ClaimsPrincipal an�nimo para bypass
                var claims = new[] { new System.Security.Claims.Claim("bypass", "true") };
                var identity = new System.Security.Claims.ClaimsIdentity(claims, "Bypass");
                var principal = new System.Security.Claims.ClaimsPrincipal(identity);
                
                context.Principal = principal;
                context.Success();
                return Task.CompletedTask;
            }
        };
    }
});

// Configurar Authorization
builder.Services.AddAuthorization(options =>
{
    if (authDisabled.ToLower() == "true")
    {
        // Pol�tica que permite tudo quando auth est� desabilitada
        options.DefaultPolicy = new Microsoft.AspNetCore.Authorization.AuthorizationPolicyBuilder()
            .RequireAssertion(_ => true)
            .Build();
    }
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler();
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

// SEMPRE usar Authentication e Authorization (necess�rio para [Authorize] funcionar)
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();
