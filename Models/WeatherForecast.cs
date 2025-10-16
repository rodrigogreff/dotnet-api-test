namespace dotnet_api_test.Models
{
    public class WeatherForecast
    {
        public int Id { get; set; } // Chave primária
        public DateOnly Date { get; set; }
        public int TemperatureC { get; set; }
        public string? Summary { get; set; }

        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
    }
}
