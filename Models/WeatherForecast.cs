CREATE TABLE [dbo].[WeatherForecast] (
    [Id] INT IDENTITY(1,1) PRIMARY KEY,
    [Date] DATETIME NOT NULL,
    [TemperatureC] INT NOT NULL,
    [Summary] NVARCHAR(100) NULL
);
