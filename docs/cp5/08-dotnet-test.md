# CP5 — Saída do dotnet test

Executado na raiz da solution em 08/10/2026 22:55. Inclui os testes do CP4 (Domain e Application) e os novos testes de paginação (`PageRequestTests` e `DonoServiceTests.GetPagedAsync_*`). Não sobe API nem banco: o repositório é mockado com Moq.

```bash
dotnet test --logger "console;verbosity=normal"
```

```text
  Aprovado PetLyve.Application.Tests.Pagination.PageRequestTests.Create_ComValoresForaDoIntervalo_DeveLancarInvalidPaginationException(page: 1, pageSize: 0, parametroInvalido: "pageSize") [30 ms]
  Aprovado PetLyve.Application.Tests.Pagination.PageRequestTests.Create_ComValoresForaDoIntervalo_DeveLancarInvalidPaginationException(page: 1, pageSize: 9999, parametroInvalido: "pageSize") [< 1 ms]
  Aprovado PetLyve.Application.Tests.Pagination.PageRequestTests.Create_ComValoresForaDoIntervalo_DeveLancarInvalidPaginationException(page: 1, pageSize: -5, parametroInvalido: "pageSize") [< 1 ms]
  Aprovado PetLyve.Application.Tests.Pagination.PageRequestTests.Create_ComValoresForaDoIntervalo_DeveLancarInvalidPaginationException(page: 0, pageSize: 20, parametroInvalido: "page") [< 1 ms]
  Aprovado PetLyve.Application.Tests.Pagination.PageRequestTests.Create_ComValoresForaDoIntervalo_DeveLancarInvalidPaginationException(page: -1, pageSize: 20, parametroInvalido: "page") [< 1 ms]
  Aprovado PetLyve.Application.Tests.Pagination.PageRequestTests.Create_ComValoresForaDoIntervalo_DeveLancarInvalidPaginationException(page: 1, pageSize: 101, parametroInvalido: "pageSize") [< 1 ms]
  Aprovado PetLyve.Application.Tests.Pagination.PageRequestTests.Create_ComValoresPadrao_DeveUsarPagina1ETamanho20 [7 ms]
  Aprovado PetLyve.Application.Tests.Pagination.PageRequestTests.Create_ComPageEPageSizeInvalidos_DeveReportarAsDuasRegras [1 ms]
  Aprovado PetLyve.Application.Tests.Pagination.PageRequestTests.Create_ComValoresNoIntervalo_DeveCriarPedidoDePagina(page: 1, pageSize: 1) [< 1 ms]
  Aprovado PetLyve.Application.Tests.Pagination.PageRequestTests.Create_ComValoresNoIntervalo_DeveCriarPedidoDePagina(page: 5, pageSize: 100) [< 1 ms]
  Aprovado PetLyve.Application.Tests.Pagination.PageRequestTests.Create_ComValoresNoIntervalo_DeveCriarPedidoDePagina(page: 1, pageSize: 20) [< 1 ms]
  Aprovado PetLyve.Application.Tests.Pagination.PageRequestTests.Create_ComValoresNoIntervalo_DeveCriarPedidoDePagina(page: 2147483647, pageSize: 100) [< 1 ms]
  Aprovado PetLyve.Application.Tests.Pagination.PageRequestTests.PagedResponse_DeveCalcularTotalPagesComTeto(totalItems: 5, pageSize: 2, totalPagesEsperado: 3) [1 ms]
  Aprovado PetLyve.Application.Tests.Pagination.PageRequestTests.PagedResponse_DeveCalcularTotalPagesComTeto(totalItems: 21, pageSize: 20, totalPagesEsperado: 2) [< 1 ms]
  Aprovado PetLyve.Application.Tests.Pagination.PageRequestTests.PagedResponse_DeveCalcularTotalPagesComTeto(totalItems: 1, pageSize: 20, totalPagesEsperado: 1) [< 1 ms]
  Aprovado PetLyve.Application.Tests.Pagination.PageRequestTests.PagedResponse_DeveCalcularTotalPagesComTeto(totalItems: 20, pageSize: 20, totalPagesEsperado: 1) [< 1 ms]
  Aprovado PetLyve.Application.Tests.Pagination.PageRequestTests.PagedResponse_DeveCalcularTotalPagesComTeto(totalItems: 137, pageSize: 20, totalPagesEsperado: 7) [< 1 ms]
  Aprovado PetLyve.Domain.Tests.Entities.ServicoTests.AlterarPreco_ComPrecoValido_DeveAtualizarPreco [16 ms]
  Aprovado PetLyve.Domain.Tests.Entities.ServicoTests.AlterarPreco_ComPrecoInvalido_DeveLancarArgumentException(precoInvalido: -10) [1 ms]
  Aprovado PetLyve.Domain.Tests.Entities.ServicoTests.AlterarPreco_ComPrecoInvalido_DeveLancarArgumentException(precoInvalido: -50,5) [< 1 ms]
  Aprovado PetLyve.Domain.Tests.Entities.ServicoTests.AlterarPreco_ComPrecoInvalido_DeveLancarArgumentException(precoInvalido: 0) [< 1 ms]
Execução de Teste Bem-sucedida.
Total de testes: 4
     Aprovados: 4
Tempo total: 1,8147 Segundos
  Aprovado PetLyve.Application.Tests.Pagination.PageRequestTests.PagedResponse_DeveCalcularTotalPagesComTeto(totalItems: 0, pageSize: 20, totalPagesEsperado: 0) [< 1 ms]
  Aprovado PetLyve.Application.Tests.Services.DonoServiceTests.CreateAsync_DeveCriarDonoEChamarRepositorio [219 ms]
  Aprovado PetLyve.Application.Tests.Services.DonoServiceTests.GetPagedAsync_ComIntervaloValido_DeveDevolverEnvelopeComTotais [35 ms]
  Aprovado PetLyve.Application.Tests.Services.DonoServiceTests.GetByIdAsync_QuandoDonoNaoExiste_DeveLancarKeyNotFoundException [10 ms]
  Aprovado PetLyve.Application.Tests.Services.DonoServiceTests.GetAllAsync_DeveRetornarTodosOsDonos [2 ms]
  Aprovado PetLyve.Application.Tests.Services.DonoServiceTests.GetPagedAsync_ComIntervaloInvalido_NaoDeveConsultarRepositorio(page: 1, pageSize: 0) [3 ms]
  Aprovado PetLyve.Application.Tests.Services.DonoServiceTests.GetPagedAsync_ComIntervaloInvalido_NaoDeveConsultarRepositorio(page: 1, pageSize: 9999) [1 ms]
  Aprovado PetLyve.Application.Tests.Services.DonoServiceTests.GetPagedAsync_ComIntervaloInvalido_NaoDeveConsultarRepositorio(page: 0, pageSize: 20) [< 1 ms]
  Aprovado PetLyve.Application.Tests.Services.DonoServiceTests.GetPagedAsync_DeveOrdenarPorNomeEDesempatarPorId [21 ms]
  Aprovado PetLyve.Application.Tests.Services.DonoServiceTests.GetPagedAsync_ComPaginaAlemDoTotal_DeveDevolverItensVazios [8 ms]
  Aprovado PetLyve.Application.Tests.Services.DonoServiceTests.GetByIdAsync_QuandoDonoExiste_DeveRetornarDono [5 ms]
Execução de Teste Bem-sucedida.
Total de testes: 28
     Aprovados: 28
Tempo total: 1,9743 Segundos
```
