using Microsoft.EntityFrameworkCore;
using AgendamentoApi.Data;
using AgendamentoApi.Models;

var builder = WebApplication.CreateBuilder(args);

// 1. Registra os Controllers e Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 2. Configura a política de CORS para liberar o Frontend
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// 3. Configura a conexão com o PostgreSQL
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

var app = builder.Build();

// 4. Popula os dados iniciais de serviços no banco (se estiver vazio)
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    
    // Garante que o banco e as tabelas existam
    db.Database.EnsureCreated();

    if (!db.Servicos.Any())
    {
        db.Servicos.AddRange(
            new Servico { Nome = "Corte de Cabelo", Preco = 45.00m, DuracaoMinutos = 30 },
            new Servico { Nome = "Barba", Preco = 30.00m, DuracaoMinutos = 20 }
        );
        db.SaveChanges();
    }
}

// 5. Ativa a interface e endpoint do Swagger
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Agendamento API v1");
});

// 6. Ativa a política de CORS
app.UseCors("AllowFrontend");

app.MapControllers();

app.Run();