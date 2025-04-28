namespace UrlShortener.Services;

public interface IServicoDeUrlEncurtada
{
    Task<string?> CriarUrlEncurtada(string urlOriginal);
    Task<string?> ObterUrlOriginal(string urlCurta);
}
