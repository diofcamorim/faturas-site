using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
})
.AddCookie()
.AddOpenIdConnect(options =>
{
    options.Authority = "https://SEU-AUTHENTIK-PUBLICO/application/o/webloginapp/";
    options.ClientId = "CLIENT_ID_AQUI";
    options.ClientSecret = "CLIENT_SECRET_AQUI";

    options.ResponseType = "code";
    options.SaveTokens = true;
    options.RequireHttpsMetadata = true;
});

builder.Services.AddAuthorization();

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/", () => "Site de Faturas - Público");

app.MapGet("/faturas", (HttpContext ctx) =>
{
    var nome = ctx.User.Identity?.Name ?? "Utilizador";
    return $"Bem-vindo {nome}. Aqui estão as suas faturas.";
})
.RequireAuthorization();

app.MapGet("/logout", async (HttpContext ctx) =>
{
    await ctx.SignOutAsync();
    return Results.Redirect("/");
});

app.Run();