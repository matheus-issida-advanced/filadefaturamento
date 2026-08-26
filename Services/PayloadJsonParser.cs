using System.Globalization;
using System.Text.Json;

namespace FinanceiroKanban.Services;

/// <summary>
/// Extrai os campos que a tela precisa de dentro do payload_json da tabela pending_os.
/// Feito de forma defensiva (TryGetProperty) porque o JSON já mostrou variações
/// entre linhas (ex.: valorUnitario às vezes vem como texto, às vezes como número).
/// </summary>
public static class PayloadJsonParser
{
    public record DadosPedido(string Cliente, string Cnpj, string Produto, decimal Valor);

    public static DadosPedido ExtrairDadosPedido(string payloadJson)
    {
        using var doc = JsonDocument.Parse(payloadJson);

        if (!doc.RootElement.TryGetProperty("item", out var item))
        {
            return new DadosPedido("", "", "", 0m);
        }

        string cliente = LerTexto(item, "CustomerName");
        string cnpj = LerTexto(item, "clienteCnpj");
        string produto = LerTexto(item, "cDescServ");
        decimal valor = LerDecimalFlexivel(item, "valorUnitario");

        return new DadosPedido(cliente, cnpj, produto, valor);
    }

    private static string LerTexto(JsonElement el, string propriedade) =>
        el.TryGetProperty(propriedade, out var v) ? (v.GetString() ?? "") : "";

    private static decimal LerDecimalFlexivel(JsonElement el, string propriedade)
    {
        if (!el.TryGetProperty(propriedade, out var v))
        {
            return 0m;
        }

        return v.ValueKind switch
        {
            JsonValueKind.Number => v.GetDecimal(),
            JsonValueKind.String when decimal.TryParse(
                v.GetString(), NumberStyles.Any, CultureInfo.InvariantCulture, out var d) => d,
            _ => 0m
        };
    }
}