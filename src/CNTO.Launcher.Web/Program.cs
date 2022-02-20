using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using CNTO.Launcher.Web.Data;
using CNTO.Launcher;
using CNTO.Launcher.Infrastructure;
using CNTO.Launcher.Application;
using Serilog;
using Serilog.Events;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateLogger();


var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog();

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddRazorPages();

// Register Luncher services
LauncherParameters launcherParameters = builder.Configuration.GetSection("Luncher").Get<LauncherParameters>();

builder.Services.AddSingleton<IRepositoryCollection>(sp =>
{
    var filesystemRepositoryCollection = new FilesystemRepositoryCollection(launcherParameters.Repositories);
    filesystemRepositoryCollection.Load();
    return filesystemRepositoryCollection;
});

builder.Services.AddTransient<IProcessRunner, WindowsProcessRunner>();
builder.Services.AddSingleton<LauncherParameters>(launcherParameters);
builder.Services.AddTransient<IExecutionContextStore, JsonExecutionStore>();
builder.Services.AddSingleton<LauncherService>();
builder.Services.AddTransient<AutoRestartService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    //app.UseHsts();
}

//app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.Run();
