using BCrypt.Net;
using DataAccess.DataAccsess.Abstract;
using DataAccess.DataAccsess.Concrete;
using DataAccess.Services.Abstract;
using DataAccess.Services.Concrete;
using Entites.Dtos;
using Infrastructure.ExternalServices.GoogleBooks;
using Kütüphane_Yonetim_Sistemi.Context;
using Kütüphane_Yonetim_Sistemi.DataAccsess.Abstract;
using Kütüphane_Yonetim_Sistemi.DataAccsess.Concrete;
using Kütüphane_Yonetim_Sistemi.Helpers;
using Kütüphane_Yonetim_Sistemi.Services.Abstract;
using Kütüphane_Yonetim_Sistemi.Services.Concrete;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Events;
using System.Security.Cryptography;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<LibraryContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("sqlConnection")));
builder.Services.AddHttpClient<IGoogleBooksService, GoogleBooksService>(client =>
{
    client.BaseAddress = new Uri("https://www.googleapis.com/books/v1/");
});
builder.Services.AddAutoMapper(typeof(GeneralMapping).Assembly);
builder.Services.AddScoped<IBookRepository, BookRepository>();
builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IRentalService, RentalService>();
builder.Services.AddScoped<IRentalRepository, RentalRepository>();
builder.Services.AddScoped<IDashboardService, DashboardService>();
builder.Services.AddScoped<IDashboardRepository, DashboardRepository>();
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
    .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Warning)
    .WriteTo.Console()
    .WriteTo.File("Logs/log-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();
builder.Host.UseSerilog();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
});
//Aes işlemleri
var encryptionKey = builder.Configuration["Encryption:Key"];
EncryptionHelper.SetKey(encryptionKey!);
builder.Services.AddSession();
var app = builder.Build();



// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
     // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseSession();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}")
    .WithStaticAssets();


app.Run();
