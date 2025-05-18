namespace UrlShortener.Configuracoes;

public static class ConfiguracoesDeCors
{
    public static void ConfigurarCors(this WebApplicationBuilder builder)
    {
        var frontendBaseUrl = builder.Configuration.GetValue<string>("FrontendSettings:BaseUrl");
        var allowedOrigins =
            builder.Configuration.GetSection("FrontendSettings:AllowedOrigins")?.Get<string[]>()
            ?? [];

        var validAllowedOrigins = allowedOrigins.Where(o => !string.IsNullOrEmpty(o)).ToArray();

        builder.Services.AddCors(options =>
        {
            options.AddPolicy(
                "WithOrigin",
                policy =>
                {
                    policy.WithOrigins(validAllowedOrigins).AllowAnyHeader().AllowAnyMethod();
                }
            );
            options.AddPolicy(
                "AnyOrigin",
                policy =>
                {
                    policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
                }
            );
        });
    }
}
