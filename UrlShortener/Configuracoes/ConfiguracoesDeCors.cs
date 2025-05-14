namespace UrlShortener.Configuracoes;

public static class ConfiguracoesDeCors
{
    public static void ConfigurarCors(this WebApplicationBuilder builder)
    {
        var frontendBaseUrl = builder.Configuration.GetValue<string>("FrontendSettings:BaseUrl");
        var allowedOrigins = builder
            .Configuration.GetSection("FrontendSettings:AllowedOrigins")
            .Get<string[]>();

        var validAllowedOrigins = allowedOrigins?.Where(o => !string.IsNullOrEmpty(o)).ToArray();

        if (validAllowedOrigins?.Length > 0)
        {
            builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(policy =>
                {
                    policy.WithOrigins(validAllowedOrigins).AllowAnyHeader().AllowAnyMethod();
                });
            });
        }
    }
}
