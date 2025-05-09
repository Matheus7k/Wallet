using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Wallet.Api.Behaviors;
using Wallet.Application.Commands.v1.Authenticate.PostAuthenticate;
using Wallet.Application.Factories.v1;
using Wallet.Application.Profiles.v1;
using Wallet.Application.Services.v1;
using Wallet.CrossCutting.Configuration.AppSettings;
using Wallet.CrossCutting.Configuration.AppSettings.Models;
using Wallet.CrossCutting.Configuration.ResourceCatalog;
using Wallet.Domain.Interfaces.v1.Factories;
using Wallet.Domain.Interfaces.v1.Repositories;
using Wallet.Domain.Interfaces.v1.Services;
using Wallet.Infrastructure.Data.Command.Context.v1;
using Wallet.Infrastructure.Data.Command.Repositories.v1;

namespace Wallet.Api.IoC;

public static class Bootstrapper
{
    public static IServiceCollection AddInjections(this IServiceCollection services, IConfiguration configuration)
    {
        InjectAppSettings(configuration);
        InjectValidator(services);
        InjectMediator(services);
        InjectAutoMapper(services);
        InjectCommands(services);
        InjectServices(services);
        InjectFactories(services);
        InjectRepositories(services);
        InjectContext(services);
        InjectResourceCatalog(services);
        
        return services;
    }

    private static void InjectAppSettings(IConfiguration configuration) =>
        AppSettings.Initialize(
            configuration.GetSection("Database").Get<DatabaseSettings>(), 
            configuration.GetSection("Jwt").Get<JwtSettings>(),
            configuration.GetSection("PasswordHash").Get<PasswordHashSettings>());
    
    private static void InjectValidator(this IServiceCollection services) =>
        services.AddValidatorsFromAssemblyContaining<PostAuthenticateCommandValidator>();
    
    private static void InjectMediator(this IServiceCollection services) =>
        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssemblyContaining<PostAuthenticateCommand>();
            config.AddOpenBehavior(typeof(ValidationBehavior<,>));
        });
    
    private static void InjectAutoMapper(this IServiceCollection services) =>
        services.AddAutoMapper(typeof(UserProfile).Assembly);
    
    private static void InjectCommands(this IServiceCollection services) =>
        services.AddSingleton(typeof(PostAuthenticateCommandHandler).Assembly);

    private static void InjectServices(this IServiceCollection services)
    {
        services.AddSingleton<ITokenService, TokenService>();
        services.AddSingleton<IPasswordEncryptorService, PasswordEncryptorService>();
    }
    
    private static void InjectFactories(this IServiceCollection services) =>
        services.AddSingleton<IWalletTransactionFactory, WalletTransactionFactory>();
    
    private static void InjectRepositories(this IServiceCollection services)
    {
        services.AddScoped<IUserCommandRepository, UserCommandRepository>();
    }
    
    private static void InjectContext(this IServiceCollection services)
    {
        services.AddDbContext<ContextDb>(opt =>
        {
            var connectionString =
                $"Server={AppSettings.Database.Host};" +
                $"Database={AppSettings.Database.Base};" +
                $"User Id={AppSettings.Database.User};" +
                $"Password={AppSettings.Database.Password}";
            
            opt.UseNpgsql(connectionString);
        });
    }

    private static void InjectResourceCatalog(this IServiceCollection services) =>
        services.AddSingleton<ResourceCatalog>();
}