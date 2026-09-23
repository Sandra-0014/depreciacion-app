using System.Text;
using Pdf.Application;
using Pdf.Domain;
using Pdf.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using QuestPDF.Infrastructure;

QuestPDF.Settings.License = LicenseType.Community;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();

string depreciacionBaseUrl =
    builder.Configuration["Services:DepreciacionApiBaseUrl"]
    ?? throw new InvalidOperationException("No se configuró Services:DepreciacionApiBaseUrl.");

string jwtKey =
    builder.Configuration["Jwt:Key"]
    ?? throw new InvalidOperationException("No se configuró Jwt:Key.");

string jwtIssuer =
    builder.Configuration["Jwt:Issuer"]
    ?? throw new InvalidOperationException("No se configuró Jwt:Issuer.");

string jwtAudience =
    builder.Configuration["Jwt:Audience"]
    ?? throw new InvalidOperationException("No se configuró Jwt:Audience.");

builder.Services.AddHttpClient<IDepreciacionClient, DepreciacionHttpClient>(client =>
{
    client.BaseAddress = new Uri(depreciacionBaseUrl);
})
.ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
{
    // Solo para desarrollo local: acepta el certificado autofirmado de Depreciacion.API.
    ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
});

builder.Services.AddScoped<IPdfGenerator, QuestPdfGenerator>();
builder.Services.AddScoped<ExportAppService>();

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = jwtIssuer,
            ValidateAudience = true,
            ValidAudience = jwtAudience,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };
    });

builder.Services.AddAuthorization();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.MapGet("/health", () => Results.Ok(new { servicio = "Pdf.API", estado = "Funcionando" }));

app.Run();