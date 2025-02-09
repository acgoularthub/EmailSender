// Program.cs

using EmailValidationAPI.Models;   // Importa o modelo EmailSettings
using EmailValidationAPI.Services;   // Importa o EmailService
using Serilog;                       // Para configuração de logging estruturado

var builder = WebApplication.CreateBuilder(args);

// =======================================================
// Configuração do Serilog para saída de logs no console
// =======================================================
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()         // Configura o log para ser exibido no console
    .CreateLogger();
builder.Logging.ClearProviders();  // Remove os provedores de log padrão
builder.Logging.AddSerilog();      // Adiciona o Serilog

builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));

// Registra o EmailService para que possa ser injetado nos controllers
builder.Services.AddScoped<EmailService>();

// Adiciona suporte para controllers (API REST)
builder.Services.AddControllers();

// Configura o Swagger para gerar a documentação interativa da API
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Middleware para redirecionar requisições HTTP para HTTPS
app.UseHttpsRedirection();

// Middleware para autorização (pode ser configurado futuramente)
app.UseAuthorization();

// Mapeia os endpoints dos controllers
app.MapControllers();

app.Run();
