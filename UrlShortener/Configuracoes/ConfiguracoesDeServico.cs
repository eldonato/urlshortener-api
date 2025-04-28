using UrlShortener.Services;

namespace UrlShortener.Configuracoes;

public static class ConfiguracoesDeServico
{
    public static void ConfigurarServicos(this IServiceCollection servicos)
    {
        servicos.AddTransient<IServicoDeUrlEncurtada, ServicoDeUrlEncurtada>();
    }
}
