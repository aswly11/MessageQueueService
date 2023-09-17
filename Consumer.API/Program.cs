using Consumer.API.Data;
using Consumer.API.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<ConsumerDBContext>();

builder.Services.AddEntityFrameworkNpgsql()
.AddDbContext<ConsumerDBContext>(opt => opt.UseNpgsql(builder.Configuration.GetConnectionString("ConsumerDbConnection")));


builder.Services.AddHostedService<MessageConsumer>();
var app = builder.Build();
using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;

try
{
    var dbContext = services.GetRequiredService<ConsumerDBContext>(); // Replace with your DbContext type
    dbContext.Database.EnsureCreated();
}
catch (Exception ex)
{
    var logger = services.GetRequiredService<ILogger<Program>>();
    logger.LogError(ex, "An error occurred while ensuring the database is created.");
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
