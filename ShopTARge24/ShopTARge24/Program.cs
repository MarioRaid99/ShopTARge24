using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using ShopTARge24.ApplicationServices.Services;
using ShopTARge24.Core.ServiceInterface;
using ShopTARge24.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddScoped<ISpaceshipServices, SpaceshipServices>();
builder.Services.AddScoped<IKindergartenService, KindergartenService>();
builder.Services.AddScoped<IFileService, FileService>();

builder.Services.AddScoped<IFileServices, FileServices>();

// register real estate service
builder.Services.AddScoped<IRealEstateServices, RealEstateServices>();

builder.Services.AddScoped<IWeatherForecastServices, WeatherForecastServices>();

builder.Services.AddScoped<IChuckNorrisServices, ChuckNorrisServices>();
builder.Services.AddHttpClient<IChuckNorrisServices, ChuckNorrisServices>();

builder.Services.AddScoped<IOpenWeatherServices, OpenWeatherServices>();

builder.Services.AddDbContext<ShopTARge24Context>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.UseStaticFiles();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();