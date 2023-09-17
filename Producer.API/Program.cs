using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Producer.API.Data;
using Producer.API.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IMessageProducer, MessageProducer>();
var connectionString = builder.Configuration.GetConnectionString("ProducerDbConnection");
builder.Services.AddScoped<ProducerDBContext>();

builder.Services.AddEntityFrameworkNpgsql().AddDbContext<ProducerDBContext>(options =>
{
    options.UseNpgsql(connectionString);
});

var app = builder.Build();
using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;

try
{
    var dbContext = services.GetRequiredService<ProducerDBContext>(); // Replace with your DbContext type
    dbContext.Database.EnsureCreated();
}
catch (Exception ex)
{
    var logger = services.GetRequiredService<ILogger<Program>>();
    logger.LogError(ex, "An error occurred while ensuring the database is created.");
}

// Conf
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
