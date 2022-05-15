using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using CNTO.Launcher.Web.Data;
using CNTO.Launcher;
using CNTO.Launcher.Infrastructure;
using CNTO.Launcher.Application;
using Serilog;
using Serilog.Events;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog();
builder.Host.UseWindowsService();

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.ConfigureApplicationCookie(options => {
    options.Cookie.Name = "CntoLuncher";
    options.LoginPath = "/luncher/Identity/Account/Login";
    options.LogoutPath = "/luncher/Identity/Account/Logout";
});

Action<PageRouteModel> mapLuncherAction = pageRouteModel =>
{
    foreach (var selectorModel in pageRouteModel.Selectors)
        if (selectorModel.AttributeRouteModel != null)
            selectorModel.AttributeRouteModel.Template = "luncher/" + selectorModel.AttributeRouteModel.Template;
};

builder.Services.AddRazorPages(options =>
{
    options.Conventions.AddPageRoute("/Index", "/luncher");
    options.Conventions.AddAreaFolderRouteModelConvention("Identity", "/", mapLuncherAction);
});

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
