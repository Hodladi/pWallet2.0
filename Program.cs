using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SimpLN.Components.Account;
using SimpLN.Data;
using SimpLN.Frontend;
using SimpLN.Models.Config;
using SimpLN.Repositories;
using SimpLN.Services;
using SimpLN.Services.UserServices;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
	.AddInteractiveServerComponents();

builder.WebHost.UseUrls("http://0.0.0.0:4949");

builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<IdentityUserAccessor>();
builder.Services.AddScoped<IdentityRedirectManager>();
builder.Services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();

builder.Services.AddAuthentication(options =>
	{
		options.DefaultScheme = IdentityConstants.ApplicationScheme;
		options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
	})
	.AddIdentityCookies();

builder.Services.AddAuthorization();

builder.Services.AddDbContext<AppDbContext>(options => 
	options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentityCore<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = false)
	.AddEntityFrameworkStores<AppDbContext>()
	.AddSignInManager()
	.AddDefaultTokenProviders();

builder.Services.AddSingleton<IEmailSender<ApplicationUser>, IdentityNoOpEmailSender>();

builder.Services.AddHttpClient();

builder.Services.AddTransient<IPrerenderService, PrerenderService>();
builder.Services.AddTransient<IWalletService, WalletService>();

builder.Services.AddSingleton<IConfiguration>(builder.Configuration);

builder.Services.AddSingleton<LoadingService>();
builder.Services.AddSingleton<StateContainerService>();
builder.Services.AddScoped<QrCodeService>();

builder.Services.AddSingleton<IInvoiceDecodeService, InvoiceDecodeService>();
builder.Services.AddScoped<IInvoiceTypeService, InvoiceTypeService>();
builder.Services.AddScoped<WebSocketService>();
builder.Services.AddScoped<IPayService, PayService>();
builder.Services.AddScoped<NotificationService>();
builder.Services.AddTransient<ConfigRepository>();
builder.Services.AddTransient<IConfigService, ConfigService>();
builder.Services.AddTransient<CloudflareDnsService>();
builder.Services.AddSingleton<IBitcoinPriceService, BitcoinPriceService>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
	var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
	await dbContext.Database.MigrateAsync();
}

if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Error", createScopeForErrors: true);
}

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
	.AddInteractiveServerRenderMode();

app.MapAdditionalIdentityEndpoints();

app.Run();