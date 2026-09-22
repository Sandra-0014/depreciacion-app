using System.Text;
using Depreciacion.Application;
using Depreciacion.Domain;
using Depreciacion.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();

string connectionString =
    builder.Configuration.GetConnectionString("DepreciacionDb")
    ?? throw new InvalidOperationException("No se configuró ConnectionStrings:DepreciacionDb.");

string jwtKey =
    builder.Configuration["Jwt:Key"]
    ?? throw new InvalidOperationException("No se configuró Jwt:Key.");

string jwtIssuer =
    builder.Configuration["Jwt:Issuer"]
    ?? throw new InvalidOperationException("No se configuró Jwt:Issuer.");

string jwtAudience =
    builder.Configuration["Jwt:Audience"]
    ?? throw new InvalidOperationException("No se configuró Jwt:Audience.");

builder.Services.AddDbContext<DepreciacionDbContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddScoped<IActivoRepository, ActivoRepository>();
builder.Services.AddScoped<CalculoDepreciacionService>();
builder.Services.AddScoped<DepreciacionAppService>();

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

app.MapGet("/health", () => Results.Ok(new { servicio = "Depreciacion.API", estado = "Funcionando" }));

app.Run();