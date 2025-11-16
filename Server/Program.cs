using EzeePdf.Components;
using EzeePdf.Core;
using EzeePdf.Core.DB;
using EzeePdf.Core.Enums;
using EzeePdf.Core.Model.Config;
using EzeePdf.Core.Services;
using EzeePdf.Core.Services.DI;
using EzeePdf.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.Circuits;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using MudBlazor.Services;
using Syncfusion.Blazor;

string settingsFile;
EnumEnvironmentType environmentType;
IServiceProvider? serviceProvider = null;

#if PROD
    environmentType = EnumEnvironmentType.Production;
    settingsFile = "appsettings.Production.json";
#elif UAT
    environmentType = EnumEnvironmentType.UAT;
    settingsFile = "appsettings.UAT.json";
#else
environmentType = EnumEnvironmentType.Development;
settingsFile = "appsettings.Development.json";
#endif

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseKestrel(x =>
{
    x.AddServerHeader = false;
});

builder.Services.Configure<Server>(builder.Configuration.GetSection("EzeePdfHost"));
builder.Services.Configure<Database>(builder.Configuration.GetSection("EzeePdfDatabase"));
builder.Services.Configure<Jwt>(builder.Configuration.GetSection("EzeePdfJwt"));
builder.Services.Configure<LogConfig>(builder.Configuration.GetSection("EzeePdfLogConfig"));

var config = builder.Configuration
    .AddJsonFile(settingsFile)
        .AddEnvironmentVariables();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddSignalR(o => { o.MaximumReceiveMessageSize = 102400000; });
builder.Services.AddMemoryCache();
builder.Services.AddSyncfusionBlazor();

builder.Services.AddMudServices();

//Authentication
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(x =>
    {
        x.Cookie.Name = $"{Constants.APP_ID}_auth_token";
        x.LoginPath = "/login";
        x.Cookie.MaxAge = TimeSpan.FromMinutes(5);
        x.AccessDeniedPath = "/access-deined";
    });

builder.Services.AddAuthentication();
builder.Services.AddCascadingAuthenticationState();
//builder.Services.AddScoped<AuthenticationStateProvider, EzeePdfAuthenticationStateProvider>();  

builder.Services.AddDbContextPool<EzeepdfContext>(x =>
{
    x.UseSqlServer(AppConfig.Instance.EzeePdfDatabase.ConnectionString);
});

builder.Services.AddHttpClient();
builder.Services.AddScoped<IResolver, Resolver>(s => new Resolver(s));
builder.Services.AddSingleton(s => new InternalResolver(s));
builder.Services.AddSingleton<IServiceResolver, HttpContextServiceResolver>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<CircuitHandler, AppCircuitHandler>();
builder.Services.AddScoped<IUserSessionService, UserSessionService>();

builder.Services.AddCoreService();

var app = builder.Build();

serviceProvider = app.Services;
ServiceLocator.SetProvider(serviceProvider.GetService<IServiceResolver>());

try
{
    ConfigLoader.Load(serviceProvider.GetService);
}
catch (Exception exception)
{
    Utils.PrintError(exception.Message);
    return;
}

Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense(AppConfig.Instance.EzeePdfHost.RuntimeKey);

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

//app.UseHttpsRedirection();

app.UseAntiforgery();

//Authentication
app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});

Utils.PrintMessage($"Application starting on [{AppConfig.Instance.EzeePdfHost.Url}] in {environmentType} mode");

app.Use(async (context, next) =>
{
    context.Response.Headers["Cache-Control"] = "no-cache, no-store, must-revalidate";
    context.Response.Headers["Pragma"] = "no-cache";
    context.Response.Headers["Expires"] = "0";
    await next();
});

app.Run(AppConfig.Instance.EzeePdfHost.Url!);
