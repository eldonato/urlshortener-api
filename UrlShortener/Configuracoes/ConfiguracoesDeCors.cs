namespace UrlShortener.Configuracoes;

public static class ConfiguracoesDeCors
{
    public static void ConfigurarCors(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddDefaultPolicy(builder =>
            {
                builder
                    .WithOrigins("http://localhost:3000", "http://127.0.0.1:3000")
                    .AllowAnyHeader()
                    .AllowAnyMethod();
            });
        });
    }
}
