using DoacaoWorker.Application.Services;
using DoacaoWorker.Domain.Interfaces.Repositories;
using DoacaoWorker.Domain.Interfaces.Services;
using DoacaoWorker.Infrastructure.Data;
using DoacaoWorker.Infrastructure.Repositories;
using DoacaoWorker.Worker.Consumers;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace DoacaoWorker.Worker.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IArrecadacaoService, ArrecadacaoService>();
            services.AddScoped<ICampanhaRepository, CampanhaRepository>();

            return services;
        }

        public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("CampanhasDB");

            services.AddDbContext<DoacaoWorkerDbContext>(options => options.UseSqlServer(connectionString));

            return services;
        }

        public static IServiceCollection AddMessaging(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMassTransit(x =>
            {
                x.AddConsumer<DoacaoRecebidaConsumer>();

                x.UsingRabbitMq((ctx, cfg) =>
                {
                    cfg.Host(configuration["RabbitMQ:Host"], configuration["RabbitMQ:VHost"], h =>
                    {
                        h.Username(configuration["RabbitMQ:Username"]);
                        h.Password(configuration["RabbitMQ:Password"]);
                    });

                    // Falha transitória (ex.: banco fora do ar): 3 novas tentativas; depois vai para a fila _error
                    cfg.UseMessageRetry(r => r.Interval(3, TimeSpan.FromSeconds(5)));

                    cfg.ConfigureEndpoints(ctx);
                });
            });

            return services;
        }
    }
}
