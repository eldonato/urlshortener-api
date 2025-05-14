using Microsoft.AspNetCore.Mvc;
using UrlShortener.Services;

namespace UrlShortener.Controllers;

[ApiController]
[Route("/api/v1/[controller]")]
public class UrlEncurtadaController(
    IConfiguration configuracao,
    IServicoDeUrlEncurtada servico,
    ILogger<UrlEncurtadaController> logger
) : ControllerBase
{
    [HttpGet("/{urlCurta}")]
    [ProducesResponseType(StatusCodes.Status302Found)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RedirecionarParaUrlOriginal(string urlCurta)
    {
        if (string.IsNullOrWhiteSpace(urlCurta))
        {
            logger.LogWarning("Tentativa de redirecionamento com url curta vazio.");
            return BadRequest("Url Curta inválida");
        }

        logger.LogInformation("Tentando redirecionar a url curta {urlCurta}", urlCurta);
        var urlOriginal = await servico.ObterUrlOriginal(urlCurta);

        if (string.IsNullOrEmpty(urlOriginal))
        {
            var frontendBaseUrl = configuracao.GetValue<string>("FrontendSettings:BaseUrl")!;
            logger.LogWarning("Url curta {urlCurta} não encontrada", urlCurta);
            return Redirect(frontendBaseUrl);
        }

        logger.LogInformation(
            "Redirecionando '{urlCurta}' para '{urlOriginal}'",
            urlCurta,
            urlOriginal
        );
        return Redirect(urlOriginal);
    }

    [HttpPost("Encurtar")]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> EncurtarUrlOriginal([FromBody] string urlOriginal)
    {
        if (string.IsNullOrWhiteSpace(urlOriginal))
        {
            logger.LogWarning("Tentativa de encurtar com url original vazia");
            return BadRequest("URL original é obrigatória e não pode ser vazia");
        }

        logger.LogInformation("Tentando encurtar '{urlOriginal}'", urlOriginal);

        var request = HttpContext.Request;

        var baseUrl = $"{request.Scheme}://{request.Host}";

        var urlCurta = await servico.CriarUrlEncurtada(urlOriginal);

        if (urlCurta == null)
        {
            logger.LogInformation("Erro ao encurtar URL: '{urlOriginal}'", urlOriginal);
            return StatusCode(
                StatusCodes.Status500InternalServerError,
                "Ocorreu um erro ao processar sua solicitação. " + "Tente novamente mais tarde."
            );
        }

        var urlCompleta = $"{baseUrl}/{urlCurta}";

        return Ok(urlCompleta);
    }
}
