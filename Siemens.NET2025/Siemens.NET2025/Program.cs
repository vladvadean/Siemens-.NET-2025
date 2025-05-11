using System.Text.Json.Serialization;
using Siemens.NET2025.Repository;
using Siemens.NET2025.Service;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });
builder.Services.AddSingleton<BookRepository>();
builder.Services.AddSingleton<BookService>();
builder.Services.AddSingleton<LendingService>();
builder.Services.AddSingleton<LendingRepository>();

builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.MapControllers();

app.Run();