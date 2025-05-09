using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Wallet.Api.Exceptions;
using Wallet.Api.IoC;
using Wallet.CrossCutting.Configuration.AppSettings;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services
    .AddOpenApi()
    .AddInjections(builder.Configuration)
    .AddEndpointsApiExplorer()
    .AddExceptionHandler<GlobalExceptionHandler>()
    .AddProblemDetails()
    .AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "Wallet", Version = "v1" });

        var securityScheme = new OpenApiSecurityScheme
        {
            Name = "Authorization",
            Description = "Insira o token JWT no formato: Bearer {seu token}",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.Http,
            Scheme = "bearer",
            BearerFormat = "JWT",
            Reference = new OpenApiReference
            {
                Type = ReferenceType.SecurityScheme,
                Id = "Bearer"
            }
        };

        c.AddSecurityDefinition("Bearer", securityScheme);

        var securityRequirement = new OpenApiSecurityRequirement
        {
            {
                securityScheme,
                []
            }
        };

        c.AddSecurityRequirement(securityRequirement);
    })
    .AddAuthentication(x =>
    {
        x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(x =>
    {
        x.RequireHttpsMetadata = false;
        x.SaveToken = true;
        x.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
        
            ValidIssuer = AppSettings.Jwt.Issuer,
            ValidAudience = AppSettings.Jwt.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(AppSettings.Jwt.Secret))
        };
    });

builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseExceptionHandler();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();