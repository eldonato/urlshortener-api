using UrlShortener.Models;

namespace UrlShortener.Infra.Repositories;

public interface IRepositorioDeUrlEncurtada
{
    Task<UrlEncurtada?> ObterPorUrlCurtaAsync(string urlCurta);
    Task InserirAsync(UrlEncurtada urlEncurtada);
    Task<bool> UrlCurtaJaExisteAsync(string urlCurta);
}
