using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using PayementMVC.Data;
using PayementMVC.Interfaces;
using PayementMVC.Repository;
using PayementMVC.Security;
using PayementMVC.Utility;
using PaymentApp.Areas.Admin;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.AddMemoryCache();
builder.Services.AddControllersWithViews();


//Creating Logger
var logger = new LoggerConfiguration()
    .WriteTo.File("./Logs/log-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Services.AddDbContext<PaymentDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddTransient<ITransaction, TransactionRepository>();
builder.Services.AddTransient<IOpenApi, OpenApiRepository>();
builder.Services.AddScoped<IGlobalVariable, GlobalVariable>();
builder.Services.AddTransient<ITest, Test>();
builder.Services.AddHttpClient("myclient");
//    .AddPolicyHandler(PollyPolicy.RetryPolicy(logger))
//    .AddPolicyHandler(PollyPolicy.CircuitBreakPolicy(logger));
builder.Services.AddScoped<DatabaseUtilities>();
builder.Services.AddCors(config =>
{
    config.AddPolicy("AllowSpecificOrigin",
            a => a.AllowAnyOrigin() // Replace with your API origin
                              .AllowAnyHeader()
                              .AllowAnyMethod());
});
var app = builder.Build();
logger.Information("App Initialized !");

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseCors("AllowSpecificOrigin"); // Enable CORS using the specified policy

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
