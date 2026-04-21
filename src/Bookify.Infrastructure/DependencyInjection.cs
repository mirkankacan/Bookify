using Bookify.Application.Abstractions.Authentication;
using Bookify.Application.Abstractions.Clock;
using Bookify.Application.Abstractions.Data;
using Bookify.Application.Abstractions.Email;
using Bookify.Domain.Abstractions;
using Bookify.Domain.Apartments;
using Bookify.Domain.Bookings;
using Bookify.Domain.Users;
using Bookify.Infrastructure.Authentication;
using Bookify.Infrastructure.Clock;
using Bookify.Infrastructure.Data;
using Bookify.Infrastructure.Email;
using Bookify.Infrastructure.Repositories;
using Dapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Bookify.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IDateTimeProvider, DateTimeProvider>();
            services.AddTransient<IEmailService, EmailService>();
            AddPersistence(services, configuration);
            AddOptions(services);
            AddAuthentication(services);
            return services;
        }

        private static void AddPersistence(IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("PostgreConnection") ?? throw new ArgumentException(nameof(configuration));
            services.AddDbContext<ApplicationDbContext>(opts =>
            {
                opts.UseNpgsql(connectionString).UseSnakeCaseNamingConvention();
            });

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IBookingRepository, BookingRepository>();
            services.AddScoped<IApartmentRepository, ApartmentRepository>();
            services.AddScoped<IUnitOfWork>(x => x.GetRequiredService<ApplicationDbContext>());

            services.AddSingleton<ISqlConnectionFactory>(_ => new SqlConnectionFactory(connectionString));
            SqlMapper.AddTypeHandler(new DateOnlyTypeHandler());
        }

        public static void AddOptions(IServiceCollection services)
        {
            services.AddOptions<AuthenticationOptions>()
                .BindConfiguration(nameof(AuthenticationOptions))
                .ValidateDataAnnotations()
                .ValidateOnStart();
            services.AddOptions<KeycloakOptions>()
              .BindConfiguration(nameof(KeycloakOptions))
              .ValidateDataAnnotations()
              .ValidateOnStart();
            services.AddSingleton(sp => sp.GetRequiredService<IOptions<AuthenticationOptions>>().Value);
            services.AddSingleton(sp => sp.GetRequiredService<IOptions<KeycloakOptions>>().Value);
        }

        private static void AddAuthentication(IServiceCollection services)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer();
            services.ConfigureOptions<JwtBearerOptionsSetup>();

            services.AddTransient<AdminAuthorizationDelegatingHandler>();
            services.AddHttpClient<IAuthenticationService, AuthenticationService>((serviceProvider, httpClient) =>
            {
                var keycloakOptions = serviceProvider.GetRequiredService<KeycloakOptions>();
                httpClient.BaseAddress = new Uri(keycloakOptions.AdminUrl);
            }).AddHttpMessageHandler<AdminAuthorizationDelegatingHandler>();

            services.AddHttpClient<IJwtService, JwtService>((serviceProvider, httpClient) =>
            {
                var keycloakOptions = serviceProvider.GetRequiredService<KeycloakOptions>();
                httpClient.BaseAddress = new Uri(keycloakOptions.TokenUrl);
            });
        }
    }
}