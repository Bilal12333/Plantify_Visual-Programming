using DAL;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using PlantifyAdmin.Components;
using PlantifyAdmin.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddAuthorizationCore();
builder.Services.AddAuthorization();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/login";
        options.AccessDeniedPath = "/login";
    });

builder.Services.AddScoped<IPlantService, PlantService>();
builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<IBusinessService, BusinessService>();
builder.Services.AddScoped<IPlantationService, PlantationService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddHttpClient<GroqService>();
builder.Services.AddHttpClient<PlantDiseaseService>();
builder.Services.AddScoped<PlantDiseaseService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
}

app.UseStaticFiles();
app.UseAntiforgery();
app.UseAuthentication();
app.UseAuthorization();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.MapPost("/auth/login", async (HttpContext context) =>
{
    var form = await context.Request.ReadFormAsync();
    var username = form["username"].ToString().Trim();
    var password = form["password"].ToString().Trim();
    var returnUrl = form["returnUrl"].ToString();

    var isValid = string.Equals(username, "admin", StringComparison.OrdinalIgnoreCase)
                  && string.Equals(password, "admin123", StringComparison.Ordinal);

    if (!isValid)
    {
        var encodedReturnUrl = Uri.EscapeDataString(returnUrl);
        context.Response.Redirect($"/login?error=1&returnUrl={encodedReturnUrl}");
        return;
    }

    var claims = new List<Claim>
    {
        new(ClaimTypes.Name, username),
        new(ClaimTypes.Role, "Admin")
    };

    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
    var principal = new ClaimsPrincipal(identity);
    await context.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

    if (string.IsNullOrWhiteSpace(returnUrl) || returnUrl == "/")
    {
        returnUrl = "/dashboard";
    }

    context.Response.Redirect(returnUrl);
}).DisableAntiforgery();

app.MapGet("/logout", async (HttpContext context) =>
{
    await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    context.Response.Redirect("/login");
});

app.Run();
