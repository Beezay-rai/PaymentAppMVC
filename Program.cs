using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PayementMVC.Data;
using PayementMVC.Interfaces;
using PayementMVC.Repository;
using PayementMVC.Security;
using PayementMVC.Utility;
using Polly;
using Polly.Timeout;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.AddMemoryCache();
builder.Services.AddControllersWithViews();


//Creating Logger
var logger = LoggerFactory.Create(loggingBuilder => loggingBuilder
    .AddConsole()).CreateLogger("Logs");


builder.Services.AddDbContext<PaymentDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddTransient<ITransaction, TransactionRepository>();
builder.Services.AddScoped<IGlobalVariable, GlobalVariable>();
builder.Services.AddTransient<ITest, Test>();
builder.Services.AddHttpClient("myclient")
    .AddPolicyHandler(PollyPolicy.RetryPolicy(logger))
    .AddPolicyHandler(PollyPolicy.CircuitBreakPolicy(logger));

var app = builder.Build();

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


app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
