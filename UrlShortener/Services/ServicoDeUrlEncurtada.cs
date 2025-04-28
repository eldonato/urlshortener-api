using UrlShortener.Infra.Cache;
using UrlShortener.Infra.Repositories;
using UrlShortener.Models;
using UrlShortener.Utils;

namespace UrlShortener.Services;

public class ServicoDeUrlEncurtada(
    IServicoDeCache cache,
    ILogger<ServicoDeUrlEncurtada> logger,
    IRepositorioDeUrlEncurtada repositorio
) : IServicoDeUrlEncurtada
{
    private const int NumeroMaximoDeTentativas = 5;

    public async Task<string?> CriarUrlEncurtada(string urlOriginal)
    {
        if (UrlInvalida(urlOriginal))
        {
            logger.LogWarning("Url informada inválida: {Url}", urlOriginal);
        }

        string urlCurta;
        int tentativas = 0;

        do
        {
            if (tentativas >= NumeroMaximoDeTentativas)
            {
                logger.LogError(
                    "Falha ao gerar urlCurta única. "
                        + "Número máximo de tentativas: {tentativas}. "
                        + "Url original: {urlOriginal}",
                    NumeroMaximoDeTentativas,
                    urlOriginal
                );

                return null;
            }

            urlCurta = GeradorDeCodigoAleatorio.Gerar();
            tentativas++;
        } while (await repositorio.UrlCurtaJaExisteAsync(urlCurta));

        logger.LogInformation(
            "Url curta {urlCurta} gerada para URL {urlOriginal}, após {tentativas} tentativas",
            urlCurta,
            urlOriginal,
            tentativas
        );

        var urlEncurtada = new UrlEncurtada { UrlOriginal = urlOriginal, UrlCurta = urlCurta };

        try
        {
            await repositorio.InserirAsync(urlEncurtada);
            await cache.GuardarUrlEncurtadaAsync(urlEncurtada);
        }
        catch (Exception ex)
        {
            logger.LogError(
                ex,
                "Erro ao salvar UrlEncurtada com urlCurta: {urlCurta}. Possível race condition",
                urlCurta
            );
            return null;
        }

        return urlEncurtada.UrlCurta;
    }

    public async Task<string?> ObterUrlOriginal(string urlCurta)
    {
        if (string.IsNullOrEmpty(urlCurta))
        {
            return null;
        }

        var urlOriginal = await cache.TentarObterPorUrlCurtaAsync(urlCurta);
        if (urlOriginal == null)
        {
            urlOriginal = repositorio.ObterPorUrlCurtaAsync(urlCurta).Result?.UrlOriginal;
            if (urlOriginal != null)
            {
                logger.LogDebug(
                    "URL encontrada no banco. Guardando chave no cache para código {urlCurta}",
                    urlCurta
                );
                await cache.TentarObterPorUrlCurtaAsync(urlCurta);
            }
            else
            {
                logger.LogWarning("Url Curta {urlCurta} não encontrada no banco", urlCurta);
                return null;
            }
        }

        return urlOriginal;
    }

    private static bool UrlInvalida(string urlOriginal)
    {
        Uri.TryCreate(urlOriginal, UriKind.Absolute, out var uri);
        var uriNaoEhHttp = Uri.UriSchemeHttp != uri?.Scheme;
        var uriNaoEhHttps = Uri.UriSchemeHttps != uri?.Scheme;

        return uri == null || (uriNaoEhHttp && uriNaoEhHttps);
    }
}
