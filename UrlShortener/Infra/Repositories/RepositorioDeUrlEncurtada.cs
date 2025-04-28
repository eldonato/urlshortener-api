using Microsoft.EntityFrameworkCore;
using UrlShortener.Models;

namespace UrlShortener.Infra.Repositories;

public class RepositorioDeUrlEncurtada(ApplicationDbContext context) : IRepositorioDeUrlEncurtada
{
    private readonly ApplicationDbContext _contexto =
        context ?? throw new ArgumentNullException(nameof(context));

    public async Task<UrlEncurtada?> ObterPorUrlCurtaAsync(string urlCurta)
    {
        if (string.IsNullOrWhiteSpace(urlCurta))
            return null;

        return await _contexto
            .UrlEncurtada.AsNoTracking()
            .FirstOrDefaultAsync(u => u.UrlCurta == urlCurta);
    }

    public async Task InserirAsync(UrlEncurtada urlEncurtada)
    {
        _contexto.UrlEncurtada.Add(urlEncurtada);
        await _contexto.SaveChangesAsync();
    }

    public async Task<bool> UrlCurtaJaExisteAsync(string urlCurta)
    {
        return await _contexto.UrlEncurtada.AnyAsync(u => u.UrlCurta == urlCurta);
    }
}
