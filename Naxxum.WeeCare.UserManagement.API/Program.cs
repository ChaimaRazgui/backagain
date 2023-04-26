using MassTransit;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Naxxum.WeeCare.UserManagement.Application.Repositories;
using Naxxum.WeeCare.UserManagement.Infrastructrue.Data;
using Naxxum.WeeCare.UserManagement.Infrastructrue.Repositories;
using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text;
using Newtonsoft.Json;
using Naxxum.WeeCare.UserManagement.Domain.Entites;
using System;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<UserManagementDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});
// Add services to the container.
builder.Services.AddSingleton<IRabitMQProducerD, RabbitMqProducerD>();
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
builder.Services.AddScoped<IUserDetailsRepository, UserDetailsRepository>();
builder.Services.AddMediatR(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddSingleton<RabbitMQService>();
builder.Services.AddSingleton<TokenService>();
var app = builder.Build();
//rabbitmq configuration
var serviceProvider = app.Services;
using (var scope = serviceProvider.CreateScope())
{
    var rabbitMQService = scope.ServiceProvider.GetService<RabbitMQService>();
}
using (var scope = serviceProvider.CreateScope())
{
    var rabbitMQService = scope.ServiceProvider.GetService<TokenService>();
}

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

