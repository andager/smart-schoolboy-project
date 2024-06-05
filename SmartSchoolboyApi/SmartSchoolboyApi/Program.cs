using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using SmartSchoolboyApi.Models;
using System.IO.Compression;

var builder = WebApplication.CreateBuilder(args);

// ������������ AcceptHeader ��� �������� � api
builder.Services.AddControllers(option =>
{
    option.RespectBrowserAcceptHeader = true;
});

// ���������� ������� ������
builder.Services.AddResponseCompression(option =>
{
    option.EnableForHttps = true; // ��������� ��� �����������   HTTPS

    // ������� �������� ������� ������
    option.Providers.Add<BrotliCompressionProvider>();
    option.Providers.Add<GzipCompressionProvider>();
});

// ��������� ������ ������ ��� Brotli
builder.Services.Configure<BrotliCompressionProviderOptions>(option =>
{
    option.Level = CompressionLevel.Fastest;
});


// ��������� ������ ������ ��� Gzip
builder.Services.Configure<GzipCompressionProviderOptions>(option =>
{
    option.Level = CompressionLevel.Fastest;
});

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<SmartSchoolboyBaseContext>(option =>
    option.UseLazyLoadingProxies().UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))); 

var app = builder.Build();

// ����������� ������
app.UseResponseCompression();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// ��������� ������ HTTP
app.UseStatusCodePagesWithReExecute("/error", "?code={0}");
app.Map("/error", (string code) => $"Error Code: {code}");

app.UseAuthorization();

app.MapControllers();

// ������ ��� ������
//app.Run(async (context) =>
//{
//    var response = context.Response;
//    response.Headers.ContentLanguage = "ru-RU";
//    response.Headers.ContentType = "application/json; charset=utf-8";
//    await response.WriteAsync(response.HttpContext.ToString());
//});

app.Run();