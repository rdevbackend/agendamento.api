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
    options.AddPolicy("AllowAll", policy =>
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

// 4. Configurar Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// 5. Inicializar e popular o banco de dados SQLite
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

// 6. Middlewares para servir os arquivos estáticos (HTML/CSS/JS da pasta wwwroot)
app.UseDefaultFiles(); // Procura automaticamente pelo index.html
app.UseStaticFiles();  // Serve o conteúdo de wwwroot

// 7. Configuração do Swagger (Liberado em Produção e Desenvolvimento)
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "API Agendamento v1");
    c.RoutePrefix = "swagger"; // Garante acesso via /swagger
});

app.UseCors("AllowAll");
app.UseAuthorization();
app.MapControllers();

app.Run();