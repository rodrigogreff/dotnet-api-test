using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// 🧩 Configura a Connection String do Azure SQL
var connectionString = "Server=tcp:sql-backend-server.database.windows.net,1433;Initial Catalog=WeatherForecast-db;Persist Security Info=False;User ID=SEU_USUARIO;Password=SUA_SENHA;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

// 🧠 Adiciona o contexto do banco
builder.Services.AddDbContext<WeatherDbContext>(options =>
    options.UseSqlServer(connectionString));

// Swagger / API Explorer
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Aceita cabeçalhos do Azure para HTTPS e proxy
app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedProto
});

// Configura HTTPS apenas em Development
if (app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

// Swagger disponível em todos os ambientes
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "DotnetApi V1");
});

// 🔹 Endpoint que busca dados direto do Azure SQL
app.MapGet("/weatherforecast", async (WeatherDbContext db) =>
{
    var data = await db.WeatherForecasts.ToListAsync();
    return Results.Ok(data);
})
.WithName("GetWeatherForecast")
.WithOpenApi();

app.Run();


// 🧱 Modelo e Contexto EF Core
public class WeatherForecast
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public int TemperatureC { get; set; }
    public string? Summary { get; set; }
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}

public class WeatherDbContext : DbContext
{
    public WeatherDbContext(DbContextOptions<WeatherDbContext> options) : base(options) { }

    public DbSet<WeatherForecast> WeatherForecasts { get; set; }
}
