using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using dotnet_api_test.Models;
using Azure.Identity; // necessário para autenticação com o Key Vault

var builder = WebApplication.CreateBuilder(args);

// 🔐 Integração com Azure Key Vault
builder.Configuration.AddAzureKeyVault(
    new Uri("https://keyvaultapps01.vault.azure.net/"),
    new DefaultAzureCredential());

// 💾 Configuração do EF Core com a connection string do Key Vault
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// 🔍 Swagger e endpoints
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// 🌐 Aceita cabeçalhos do Azure para HTTPS e proxy
app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedProto
});

// 🔒 HTTPS apenas em Development
if (app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

// 📘 Swagger em Dev e Produção
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "DotnetApi V1");
});

// 🌦️ Endpoint que retorna os dados do banco de dados
app.MapGet("/weatherforecast", async (AppDbContext db) =>
{
    var forecasts = await db.WeatherForecasts.ToListAsync();
    return Results.Ok(forecasts);
})
.WithName("GetWeatherForecast")
.WithOpenApi();

app.Run();