# Fila de Arrependimento — Protótipo (Financeiro)

Protótipo em Blazor Server (C#/.NET 8) da tela de Kanban do Financeiro,
com 4 colunas: **OS em fila**, **OS congeladas**, **OS canceladas** e
**OS finalizadas**.

## Como rodar

```bash
dotnet restore
dotnet run
```

Depois acesse a URL indicada no terminal (ex.: `https://localhost:5001`).

## O que já funciona

- As 4 colunas, cada uma com contador de itens e total em R$.
- Dados carregados a partir da base "pending OS" colada (17 registros reais,
  mapeados assim para as colunas):
  - `queued` → **OS em fila**
  - `manual_hold` → **OS congeladas**
  - `billed` → **OS finalizadas**
  - `Cancelada` não existe ainda na base de exemplo — a coluna começa vazia.
- Cada card tem o menu "⋯" com as ações **Voltar para fila**, **Congelar**
  e **Cancelar**. A opção correspondente ao status atual do card fica
  desabilitada.
- "Cancelar" pede confirmação (via `confirm()` do navegador) antes de mover o card.
- Sem drag-and-drop, conforme combinado — a movimentação é só pelo menu.

## O que é só protótipo (não é regra de negócio real ainda)

- Os dados ficam em memória (`Services/OsDataService.cs`), por sessão do
  navegador. Não há persistência real.
- Qualquer usuário pode mover qualquer card para qualquer status — a regra
  dos 7 dias e outras validações de transição **não estão implementadas
  aqui**. Elas devem viver no backend (Azure Functions), não na tela, como
  discutido na arquitetura:

  ```
  Cloud Cockpit → Logic App → Azure Functions → Azure SQL → esta tela
  ```

## Próximos passos sugeridos

1. Trocar `OsDataService` por um `HttpClient` que chama a API real
   (`GET /api/orders`, `PUT /api/orders/{id}/status`).
2. Tratar o erro retornado pela API quando uma transição não for permitida
   (ex.: tentar liberar/voltar para fila antes do prazo) e mostrar a
   mensagem no lugar de mover o card.
3. Adicionar autenticação (Entra ID) e restringir a tela ao grupo do
   Financeiro.
4. Se o Financeiro pedir drag-and-drop de verdade, avaliar uma biblioteca
   JS de drag-and-drop chamada via JS interop, ou um componente dedicado.
