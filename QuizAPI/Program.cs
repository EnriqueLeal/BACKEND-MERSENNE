using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using API.Models;
using AutoMapper;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.FileProviders;
using System.Security.Claims;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);


// Generar una clave aleatoria
var key = new byte[32]; // 256 bits
using (var rng = RandomNumberGenerator.Create())
{
    rng.GetBytes(key);
}


// Codificar la clave en base64
var base64Key = Convert.ToBase64String(key);

builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("Jwt"));
//builder.Services.AddAuthentication("Bearer").AddJwtBearer();

/*builder.Services.AddAuthentication(optiones =>
{
    optiones.DefaultAuthenticateScheme =
             JwtBearerDefaults.AuthenticationScheme;
    optiones.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    optiones.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
            .AddJwtBearer(o =>
            {
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidAudience = builder.Configuration["JwtSecurityToken:Audience"],
                    ValidIssuer = builder.Configuration["JwtSecurityToken:Issuer"],
                    IssuerSigningKey = new SymmetricSecurityKey
                    (Encoding.UTF8.GetBytes(builder.Configuration["JwtSecurityToken:Key"])),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero,
                };
            });*/

var key2 = Encoding.ASCII.GetBytes(builder.Configuration["JwtSecurityToken:Key"]); // Cambia esto por tu clave secreta
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false; // Para pruebas, c�mbialo a true en producci�n
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key2),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});
// Agregar servicios al contenedor.
builder.Services.AddControllers();
    // Obtener m�s informaci�n sobre c�mo configurar Swagger/OpenAPI en https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();

    // Configurar Swagger
    builder.Services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
        c.OperationFilter<AddAuthorizationHeaderParameterOperationFilter>(); // Agregar el filtro de operaci�n personalizado aqu�
    });

// Configurar AutoMapper
builder.Services.AddAutoMapper(typeof(Program));


// Configurar AutoMapper
builder.Services.AddAutoMapper(typeof(Program));


builder.Services.AddDbContext<QuizDbContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("DevConnection")));

var app = builder.Build();

app.MapGet("/", () => "Hello, World!");
app.MapGet("/secret", (ClaimsPrincipal user) => $"Hello {user.Identity?.Name}. My secret")
    .RequireAuthorization();


app.UseCors(options =>
    options.WithOrigins(builder.Configuration["MySettingFront"], builder.Configuration["MySettingBack"])
           .AllowAnyMethod()
           .AllowAnyHeader());

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(builder.Environment.ContentRootPath, "Images")),
    RequestPath = "/Images"
});

// Configurar el pipeline de solicitudes HTTP.
// Configurar Swagger si estamos en modo de desarrollo
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Mi API V1");
    });
}

// Configurar el pipeline de solicitudes HTTP
app.UseRouting();
    app.UseAuthentication();
    app.UseAuthorization();
    app.UseEndpoints(endpoints =>
    {
        endpoints.MapControllers();
    });

    app.UseSwagger(options =>
    {
        options.SerializeAsV2 = true;
    });

app.Run();
