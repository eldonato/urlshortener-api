using Microsoft.Extensions.Caching.Distributed;
using UrlShortener.Models;

namespace UrlShortener.Infra.Cache;

public class ServicoDeCacheRedis(
    IDistributedCache cache,
    IConfiguration configuration,
    ILogger<ServicoDeCacheRedis> logger
) : IServicoDeCache
{
    private readonly string _prefixoUrlCache = "url:";

    public async Task<string?> TentarObterPorUrlCurtaAsync(string urlCurta)
    {
        if (string.IsNullOrWhiteSpace(urlCurta))
            return null;

        var chave = ObterChaveDeCacheUrl(urlCurta);
        string? urlOriginal = null;
        try
        {
            urlOriginal = await cache.GetStringAsync(chave);
            if (string.IsNullOrWhiteSpace(urlOriginal))
            {
                logger.LogDebug("Cache não encontrado para a chave {Chave}", chave);
            }
            else
            {
                logger.LogDebug("Cache encontrado para a chave {Chave}", chave);
            }
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Erro ao ler URL do cache para a chave {Chave}", chave);
        }

        return urlOriginal;
    }

    public async Task GuardarUrlEncurtadaAsync(UrlEncurtada urlEncurtada)
    {
        if (
            string.IsNullOrWhiteSpace(urlEncurtada.UrlCurta)
            || string.IsNullOrWhiteSpace(urlEncurtada.UrlOriginal)
        )
            return;

        var chave = ObterChaveDeCacheUrl(urlEncurtada.UrlCurta);
        var options = ObterOpcoesDeCache();
        try
        {
            await cache.SetStringAsync(chave, urlEncurtada.UrlOriginal, options);
            logger.LogDebug("URL guardada para a chave: {Chave}", chave);
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Erro ao guardar chave: {Chave}", chave);
        }
    }

    private string ObterChaveDeCacheUrl(string chave)
    {
        var instancia = configuration.GetValue<string>("CacheSettings:Instance") ?? "";
        return $"{instancia}{_prefixoUrlCache}{chave}";
    }

    private DistributedCacheEntryOptions ObterOpcoesDeCache()
    {
        var options = new DistributedCacheEntryOptions();
        var slidingExpiration =
            configuration.GetValue<int?>("CacheSettings:SlidingExpiration") ?? 10;
        var absoluteExpiration =
            configuration.GetValue<int?>("CacheSettings:AbsoluteExpiration") ?? 10;

        options.SetSlidingExpiration(TimeSpan.FromMinutes(slidingExpiration));
        options.SetAbsoluteExpiration(TimeSpan.FromMinutes(absoluteExpiration));

        return options;
    }
}
