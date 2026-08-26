using FinanceiroKanban.Models;

namespace FinanceiroKanban.Services;

public class OsDataService
{
    public event Action? OnChange;

    private readonly OsRepository _repository;
    private List<OsItem> _itens = new();

    public OsDataService(OsRepository repository)
    {
        _repository = repository;
    }

    public async Task CarregarAsync()
    {
        _itens = await _repository.ObterTodosAsync();
        OnChange?.Invoke();
    }

    public IEnumerable<OsItem> ObterPorStatus(OsStatus status) =>
        _itens.Where(i => i.Status == status)
              .OrderByDescending(i => i.DataCriacao);

    public async Task AlterarStatusAsync(string codIntOS, OsStatus novoStatus)
    {
        await _repository.AtualizarStatusAsync(codIntOS, novoStatus);

        var item = _itens.FirstOrDefault(i => i.CodIntOS == codIntOS);
        if (item is not null)
        {
            item.Status = novoStatus;
        }

        OnChange?.Invoke();
    }
}