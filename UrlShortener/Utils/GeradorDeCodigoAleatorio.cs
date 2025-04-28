namespace UrlShortener.Utils;

public static class GeradorDeCodigoAleatorio
{
    private const int QuantidadeDeCaracteres = 8;
    private const string CaracteresPermitidos =
        "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

    public static string Gerar(int quantidade = QuantidadeDeCaracteres)
    {
        var resultado = new char[quantidade];
        for (var i = 0; i < quantidade; i++)
        {
            resultado[i] = CaracteresPermitidos[Random.Shared.Next(CaracteresPermitidos.Length)];
        }

        return new string(resultado);
    }
}
