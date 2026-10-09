# CP5 — Evidências (versionamento, paginação e rate limit)

Recurso versionado e paginado: **Donos** (`/api/donos`).

| # | Evidência | Arquivo |
|---|---|---|
| 1 | GET v1 (lista) e GET v2 (envelope) no mesmo recurso, por query, header, omissão e segmento de URL | [01-versionamento-v1-v2.md](01-versionamento-v1-v2.md) |
| 2 | Headers `api-supported-versions` e `api-deprecated-versions` | [02-headers-de-versao.md](02-headers-de-versao.md) |
| 3 | 400 de `page` / `pageSize` inválidos | [03-paginacao-400.md](03-paginacao-400.md) |
| 4 | Página 1, 2 e 3 com `pageSize=5`, página além do total e padrões | [04-paginacao-paginas.md](04-paginacao-paginas.md) |
| 5 | 429 com `Retry-After` no `POST /api/donos` e `GET /health` 200 logo depois | [05-rate-limit-429-e-health.md](05-rate-limit-429-e-health.md) |
| 6 | Swagger com os grupos V1 (deprecada) e V2 | [06-swagger-por-versao.md](06-swagger-por-versao.md) · [swagger-v1.png](swagger-v1.png) · [swagger-v2.png](swagger-v2.png) |
| 7 | SQL gerado: `COUNT` + `ORDER BY` + `LIMIT/OFFSET` no banco | [07-paginacao-sql.md](07-paginacao-sql.md) |
| 8 | Saída do `dotnet test` | [08-dotnet-test.md](08-dotnet-test.md) |

## Resumo do que foi implementado

**Versionamento** (`Asp.Versioning.Mvc` + `Asp.Versioning.Mvc.ApiExplorer`)

- `DefaultApiVersion = 2.0`, `AssumeDefaultVersionWhenUnspecified = true`, `ReportApiVersions = true`.
- Leitores combinados: query `api-version`, header `X-Api-Version` e segmento de URL (`/api/v{versao}/donos`).
- `DonosController`: `[ApiVersion("1.0", Deprecated = true)]` e `[ApiVersion("2.0")]`.
  - `GET /api/donos` na 1.0 → array (contrato do CP3), `[MapToApiVersion("1.0")]`.
  - `GET /api/donos` na 2.0 → envelope paginado, `[MapToApiVersion("2.0")]`.
  - `GET /api/donos/{id}` e `POST /api/donos` existem nas duas versões; sem versão caem na 2.0.
  - As duas versões chamam o mesmo `DonoService`.
- `AnimaisController` e `ServicosController` são `[ApiVersionNeutral]`: respondem com ou sem versão e aparecem nos dois documentos do Swagger.

**Paginação (só v2)**

- Query: `page` (padrão 1, mínimo 1) e `pageSize` (padrão 20, de 1 a 100).
- Fora da faixa → 400 `application/problem+json` com `detail` e `errors` por parâmetro.
- Página além do total → 200 com `items: []`.
- Envelope: `page`, `pageSize`, `totalItems`, `totalPages`, `hasPrevious`, `hasNext`, `items`.
- Camadas: Controller lê os parâmetros → `DonoService.GetPagedAsync` valida (`PageRequest.Create`) → `IRepository<T>.GetPagedAsync` faz `Count` + `OrderBy(Nome).ThenBy(DonoId)` + `Skip` + `Take` no `IQueryable`.

**Rate limit** (`Microsoft.AspNetCore.RateLimiting`, nativo)

| Política | Onde | Limite | Janela | Partição |
|---|---|---|---|---|
| `escrita` | `POST /api/donos` (v1 e v2) | 10 requisições | 60 s (fixed window) | IP do cliente |
| `leitura` | `GET /api/donos` v2 (listagem paginada) | 60 requisições | 60 s (fixed window) | IP do cliente |

- Estouro → 429 com header `Retry-After` (segundos até a próxima janela) e Problem Details (`status: 429`, `retryAfterSeconds`, `traceId`).
- Respostas aceitas trazem `X-RateLimit-Limit`, `X-RateLimit-Remaining` e `X-RateLimit-Reset`.
- Não há limitador global; `/health` ainda tem `.DisableRateLimiting()` explícito.
- Limites configuráveis em `appsettings.json` → seção `RateLimiting`.
