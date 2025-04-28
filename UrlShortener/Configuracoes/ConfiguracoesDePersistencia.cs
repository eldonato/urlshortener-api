using Microsoft.EntityFrameworkCore;
using UrlShortener.Infra;

namespace UrlShortener.Configuracoes;

public static class ConfiguracoesDePersistencia
{
    public static void ConfigurarPersistencias(
        this IServiceCollection services,
        ConfigurationManager configuracao
    )
    {
        var postgresConnectionString = configuracao.GetConnectionString("PostgresConnection");
        if (string.IsNullOrWhiteSpace(postgresConnectionString))
        {
            throw new InvalidOperationException(
                "Connection string 'PostgresConnection' não encontrada."
            );
        }

        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(postgresConnectionString)
        );

        var redisConnectionString = configuracao.GetConnectionString("RedisConnection");
        if (string.IsNullOrWhiteSpace(redisConnectionString))
        {
            throw new InvalidOperationException(
                "Connection string 'RedisConnectionString' não encontrada."
            );
        }

        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = redisConnectionString;
            options.InstanceName = configuracao.GetValue<string>("CacheSettings:InstanceName");
        });
    }
}
