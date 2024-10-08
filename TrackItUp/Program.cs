using Microsoft.EntityFrameworkCore;
using TrackItUpBLL.Contracts;
using TrackItUpBLL.Services;
using TrackItUpDAL.Context;
using TrackItUpDAL.Interfaces;
using TrackItUpDAL.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<IHabitService, HabitService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IHabitTrackingService, HabitTrackingService>();

builder.Services.AddScoped<IHabitRepository, HabitRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IHabitTrackingRepository, HabitTrackingRepository>();
//builder.Services.AddAuthorization();
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<TrackItUpContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("TrackItUp")));

var app = builder.Build();

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
