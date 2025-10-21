using Microsoft.EntityFrameworkCore;
using LibHub.BookService.Data;
using LibHub.BookService.Data.Repositories;
using LibHub.BookService.Services;
using Consul;
using LibHub.BookService.Infrastructure.Discovery;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<BookDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IBookRepository, BookRepository>();
builder.Services.AddScoped<IBookService, LibHub.BookService.Services.BookService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add Consul Client configuration
builder.Services.AddSingleton<IConsulClient, ConsulClient>(p => new ConsulClient(consulConfig =>
{
    var address = builder.Configuration["Consul:Address"];
    if (address != null)
    {
        consulConfig.Address = new Uri(address);
    }
}));

// Add our custom Consul registration service
builder.Services.AddHostedService<ConsulHostedService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();

app.MapGet("/health", () => Results.Ok());

app.Run();
