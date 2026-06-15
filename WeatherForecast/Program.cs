using System.Text.Json.Serialization;
using Microsoft.OpenApi;
using WeatherForecast.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IScheduleGeneratorService, ScheduleGeneratorService>();

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters
            .Add(new JsonStringEnumConverter());
    });

builder.Services.AddEndpointsApiExplorer();
var app = builder.Build();
app.UseSwaggerUI(options =>
{
     options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
     options.RoutePrefix = String.Empty;
});

app.MapControllers();
app.Run();