using Microsoft.EntityFrameworkCore.Storage;
using Library.Data;
using Library.Repository;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<DatabaseContext>();
builder.Services.AddScoped<IRepository, Repository>();

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();

public partial class Program { }
