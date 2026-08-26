using Dapper;
using FinanceiroKanban.Models;
using Microsoft.Data.SqlClient;

namespace FinanceiroKanban.Services;

public class OsRepository
{
    private readonly string _connectionString;

    public OsRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("FinanceiroDb")
            ?? throw new InvalidOperationException(
                "Connection string 'FinanceiroDb' não encontrada. Configure com 'dotnet user-secrets'.");
    }

    public async Task<List<OsItem>> ObterTodosAsync()
    {
        const string sql = @"
            SELECT cod_int_os, payload_json, attribution, matched_ordernumber,
                   salesperson, detected_at, release_at, status, status_reason
            FROM pending_os";

        using var connection = new SqlConnection(_connectionString);
        var linhas = await connection.QueryAsync<PendingOsRow>(sql);

        var itens = new List<OsItem>();
        foreach (var linha in linhas)
        {
            var dados = PayloadJsonParser.ExtrairDadosPedido(linha.payload_json);

            itens.Add(new OsItem
            {
                CodIntOS = linha.cod_int_os,
                Cliente = dados.Cliente,
                Cnpj = dados.Cnpj,
                Produto = dados.Produto,
                Valor = dados.Valor,
                DataCriacao = linha.detected_at,
                Po = linha.matched_ordernumber,
                Responsavel = linha.salesperson,
                Observacao = linha.status_reason,
                Origem = linha.attribution ?? "",
                Status = MapearStatus(linha.status)
            });
        }

        return itens;
    }

    public async Task AtualizarStatusAsync(string codIntOS, OsStatus novoStatus)
    {
        const string sql = @"
            UPDATE pending_os
            SET status = @Status
            WHERE cod_int_os = @CodIntOS";

        using var connection = new SqlConnection(_connectionString);
        await connection.ExecuteAsync(sql, new
        {
            CodIntOS = codIntOS,
            Status = MapearStatusParaTexto(novoStatus)
        });
    }

    private static OsStatus MapearStatus(string status) => status switch
    {
        "queued" => OsStatus.EmFila,
        "manual_hold" => OsStatus.Congelada,
        "canceled" => OsStatus.Cancelada,
        "billed" => OsStatus.Finalizada,
        _ => OsStatus.EmFila
    };

    private static string MapearStatusParaTexto(OsStatus status) => status switch
    {
        OsStatus.EmFila => "queued",
        OsStatus.Congelada => "manual_hold",
        OsStatus.Cancelada => "canceled",
        OsStatus.Finalizada => "billed",
        _ => "queued"
    };

    private sealed class PendingOsRow
    {
        public string cod_int_os { get; set; } = "";
        public string payload_json { get; set; } = "";
        public string? attribution { get; set; }
        public string? matched_ordernumber { get; set; }
        public string? salesperson { get; set; }
        public DateTime detected_at { get; set; }
        public DateTime release_at { get; set; }
        public string status { get; set; } = "";
        public string? status_reason { get; set; }
    }
}