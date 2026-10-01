using AgendamentoApi.Data;
using AgendamentoApi.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// 1. Configurar o Entity Framework Core para usar SQLite
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// 2. Configurar CORS para permitir chamadas do frontend
builder.Services.AddCors(options =>
{
    options.AddPolicy("PermitirTudo", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// 3. Configurar Controllers + Evitar erro de loop em JSON/Relacionamentos
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
});

// Configurar Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// 4. Inicializar e popular o banco de dados SQLite
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();

    if (!db.Servicos.Any())
    {
        db.Servicos.AddRange(
            new Servico { Nome = "Corte de Cabelo", Preco = 35.00m, DuracaoMinutos = 30 },
            new Servico { Nome = "Barba", Preco = 25.00m, DuracaoMinutos = 20 },
            new Servico { Nome = "Cabelo + Barba", Preco = 55.00m, DuracaoMinutos = 50 }
        );
        db.SaveChanges();
    }
}

// 5. Middlewares
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("PermitirTudo");

app.UseAuthorization();

app.MapControllers();

app.Run();