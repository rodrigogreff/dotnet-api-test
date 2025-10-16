using Microsoft.EntityFrameworkCore;

namespace dotnet_api_test.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        // Exemplo de tabela WeatherForecast (ajuste conforme seu modelo real)
        public DbSet<WeatherForecast> WeatherForecasts { get; set; }
    }
}
