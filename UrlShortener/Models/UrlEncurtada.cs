using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UrlShortener.Models;

[Table("urls_encurtadas")]
public class UrlEncurtada
{
    [Key]
    [Column("url_curta")]
    [StringLength(10)]
    public string UrlCurta { get; set; } = null!;

    [Required]
    [Column("url_original")]
    public string UrlOriginal { get; set; } = null!;

    [Column("criado_em")]
    public DateTime CriadoEm { get; set; } = DateTime.UtcNow;

    [Column("contador_cliques")]
    public long ContadorCliques { get; set; } = 0;
}
