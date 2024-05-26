using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using SmartSchoolboyApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<SmartSchoolboyBaseContext>(option =>
    option.UseLazyLoadingProxies().UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))); 

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// обработка ошибок HTTP
app.UseStatusCodePagesWithReExecute("/error", "?code={0}");
app.Map("/error", (string code) => $"Error Code: {code}");

app.UseAuthorization();

app.MapControllers();

app.Run();
