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
    option.UseLazyLoadingProxies().UseSqlServer(builder.Configuration.GetConnectionString("HostingConnection"))); 

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// ��������� ������ HTTP
app.UseStatusCodePages(async statusCodeContext =>
{
    var response = statusCodeContext.HttpContext.Response;
    var path = statusCodeContext.HttpContext.Request.Path;

    response.ContentType = "text/plain; charset=UTF-8";

    if (response.StatusCode == 400)
        await response.WriteAsync($"Path: {path}. ������ ��� ������� �����������");
    else if (response.StatusCode == 401)
        await response.WriteAsync($"Path: {path}. ������ ������� ��������������, � ������� �� ������� ������������ �������������� ������� ������");
    else if (response.StatusCode == 403)
        await response.WriteAsync($"Path: {path}. � ������� ��� ���������� �� ������ � ������������ �������");
    else if (response.StatusCode == 404)
        await response.WriteAsync($"Resource {path}. �� ������� ����� ����������� ������ �� �������");
    else if (response.StatusCode == 500)
        await response.WriteAsync($"Path: {path}. Server error, the server is not responding");
});

app.UseAuthorization();

app.MapControllers();

app.Run();
