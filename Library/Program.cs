using Microsoft.EntityFrameworkCore.Storage;
using Library.Data;
using Library.Endpoints;
using Library.Repository;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<DatabaseContext>();
builder.Services.AddScoped<IRepository, Repository>();

// Used for CORS
string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

// Used for CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
      builder =>
      {
          builder.WithOrigins(
            "http://localhost:5173", "*")
            .AllowAnyHeader()
            .AllowAnyMethod();
      });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.ConfigureLibraryApiEndpoint();

app.UseCors(MyAllowSpecificOrigins);

app.Run();

public partial class Program { }
