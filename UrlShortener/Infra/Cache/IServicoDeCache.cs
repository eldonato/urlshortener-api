using UrlShortener.Models;

namespace UrlShortener.Infra.Cache;

public interface IServicoDeCache
{
    Task<string?> TentarObterPorUrlCurtaAsync(string urlCurta);
    Task GuardarUrlEncurtadaAsync(UrlEncurtada urlEncurtada);
}
