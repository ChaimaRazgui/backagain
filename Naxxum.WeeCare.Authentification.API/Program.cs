using MassTransit;
using Naxxum.WeeCare.Authentification.API.Extensions;
using Naxxum.WeeCare.Authentification.Application.Abstractions;
using Naxxum.WeeCare.Authentification.Application.Extensions;
using Naxxum.WeeCare.Authentification.Application.Services;
using Naxxum.WeeCare.Authentification.Infrastructure.Data;
using Naxxum.WeeCare.Authentification.Infrastructure.Extensions;
using Naxxum.WeeCare.Authentification.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSqlServer<AppDbContext>(
    builder.Configuration.GetConnectionString("DefaultConnection"), null,
    optionsBuilder => optionsBuilder.EnableSensitiveDataLogging(builder.Environment.IsDevelopment()));
// Add services to the container.
builder.Services.AddScoped<IRabitMQProducer, RabbitMq>();
builder.Services
    .AddApiServices(builder.Configuration)
    .AddApplicationServices(builder.Configuration)
    .AddInfrastructureServices(builder.Configuration);
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(option =>
{
    option.AddPolicy("MyPolicy", builder =>
    {
        builder.AllowAnyOrigin()
        .AllowAnyHeader()
        .AllowAnyMethod();
    });
});
builder.Services.AddSingleton<RabbitMQService>();
builder.Services.AddSingleton<TokenConsumer>();
var app = builder.Build();
var serviceProvider = app.Services;
using (var scope = serviceProvider.CreateScope())
{
    var rabbitMQService = scope.ServiceProvider.GetService<RabbitMQService>();
}
using (var scope = serviceProvider.CreateScope())
{
    var rabbitMQService = scope.ServiceProvider.GetService<TokenConsumer>();
}
await AppDbContextSeeder.SeedAsync(app.Services);
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("MyPolicy");

app.UseAuthorization();

app.MapControllers();

app.Run();
