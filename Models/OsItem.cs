namespace FinanceiroKanban.Models;

/// <summary>
/// Representa um registro da base "pending OS" (fonte principal de dados).
/// Os nomes dos campos aqui seguem uma versão simplificada/tratada do
/// payload original vindo do Cloud Cockpit.
/// </summary>
public class OsItem
{
    /// <summary>Identificador único da OS (codIntOS na base original).</summary>
    public string CodIntOS { get; set; } = "";

    public string Cliente { get; set; } = "";

    public string Cnpj { get; set; } = "";

    /// <summary>Descrição resumida do produto/licença (skuName).</summary>
    public string Produto { get; set; } = "";

    public decimal Valor { get; set; }

    public DateTime DataCriacao { get; set; }

    /// <summary>Pedido de compra interno (ORD-...), quando existir.</summary>
    public string? Po { get; set; }

    /// <summary>Responsável interno pela criação da OS, quando existir.</summary>
    public string? Responsavel { get; set; }

    /// <summary>Observação/anotação vinda da base original, quando existir.</summary>
    public string? Observacao { get; set; }

    /// <summary>Origem do lançamento: self_service, system, internal, internal_unmatched.</summary>
    public string Origem { get; set; } = "";

    public OsStatus Status { get; set; }
}
