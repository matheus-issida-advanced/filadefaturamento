namespace FinanceiroKanban.Models;

/// <summary>
/// Situação da OS dentro do processo de fila de arrependimento.
/// Corresponde às 4 colunas do Kanban do Financeiro.
/// </summary>
public enum OsStatus
{
    EmFila,
    Congelada,
    Cancelada,
    Finalizada
}
