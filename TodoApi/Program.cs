using Microsoft.EntityFrameworkCore;
using TodoApi.Models;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);



builder.Services.AddControllers();
    // .AddJsonOptions(options =>
    // {
    //     options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
    //     options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()); // Si vous souhaitez prendre en charge les enums comme des chaînes.
    // });

builder.Services.AddDbContext<ApiContext>(opt => {
  opt.UseNpgsql("Host=localhost;Port=5432;Database=todo_db;Username=gven;Password=gven");
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddCors(options => {
  options.AddPolicy("AllowSpecificOrigin", builder => {
    builder.WithOrigins("http://localhost:3000")
           .AllowAnyHeader()
           .AllowAnyMethod()
           .WithExposedHeaders("X-Total-Pages")
           .WithExposedHeaders("X-Max-Price")
           .WithExposedHeaders("X-Current-Page");

  });
});

var app = builder.Build();

if (app.Environment.IsDevelopment()) {
  app.UseSwagger();
  app.UseSwaggerUI();

  using (var serviceScope = app.Services.CreateScope()) {
    var services = serviceScope.ServiceProvider;
    SeedData.Initialize(services);
  }
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseCors("AllowSpecificOrigin");

app.UseAuthorization();

app.MapControllers();

app.Run();
