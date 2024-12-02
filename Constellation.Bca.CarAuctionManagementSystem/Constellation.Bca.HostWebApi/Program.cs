using Constellation.Bca.Infrastructure;
using Constellation.Bca.Application;
using System.Text.Json.Serialization;
using Constellation.Bca.HostWebApi.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.IncludeMemoryCache();
builder.Services.AddMediatrHost();
builder.Services.AddMapsterHost();
builder.Services.AddRepositoryDependencyInjection();
builder.Services.AddServicesDepencyInjection();
builder.Services.AddControllers()
                .AddJsonOptions(opt => { opt.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()); });

builder.Services.AddDatabase();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options => // UseSwaggerUI is called only in Development.
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        options.RoutePrefix = string.Empty;
    });
}

// Configure the HTTP request pipeline.
app.UseHttpsRedirection();
app.MapControllers();
app.UseMiddleware<ExceptionHandlerMiddleware>();
app.Run();
