# PetLyve --- Projeto Petshop (CP1, CP2, CP3, CP4 e CP5)

**Grupo:** - Guilherme Sola Garcia - RM563674 - Clayton Alves dos
Santos - RM562285

------------------------------------------------------------------------

## 🎯 Sobre o projeto

O **PetLyve** é uma API REST para gerenciamento de um petshop.

O projeto foi desenvolvido em **.NET 10**, utilizando uma organização
baseada em **Clean Architecture**, com separação entre domínio,
aplicação, infraestrutura e API.

O sistema contempla o fluxo de atendimento de um petshop, desde o
cadastro do dono e do animal até o agendamento e pagamento dos serviços.

------------------------------------------------------------------------

## 🐾 Entidades

O domínio do projeto possui as seguintes entidades:

-   **Dono:** dados do responsável pelo pet.
-   **Animal:** pet que recebe os serviços.
-   **Funcionario:** profissional responsável pelo atendimento.
-   **Servico:** serviço oferecido pelo petshop, como banho e tosa.
-   **Agendamento:** relaciona o animal, funcionário, serviço e data do
    atendimento.
-   **Pagamento:** registra o pagamento relacionado ao agendamento.

------------------------------------------------------------------------

## 🔗 Relacionamentos

-   Um **Dono** pode ter vários **Animais** (1:N).
-   Um **Animal** pode ter vários **Agendamentos** (1:N).
-   Um **Funcionario** pode realizar vários **Agendamentos** (1:N).
-   Um **Servico** pode estar relacionado a vários **Agendamentos**
    (1:N).
-   Um **Agendamento** possui um **Pagamento** (1:1).

# 🏗️ Arquitetura

O projeto foi organizado seguindo os princípios de **Clean
Architecture**:

``` text
PetLyve
├── PetLyve.Domain
├── PetLyve.Application
├── PetLyve.Infrastructure
├── PetLyve.API
├── PetLyve.Domain.Tests
└── PetLyve.Application.Tests
```

### Domain

Contém as entidades e regras relacionadas ao domínio da aplicação.

### Application

Contém os contratos, DTOs, serviços de aplicação e abstrações utilizadas
pela aplicação.

Exemplos:

``` text
IRepository<T>
DonoService
DonoRequestDto
DonoResponseDto
AnimalRequestDto
AnimalResponseDto
ServicoRequestDto
ServicoResponseDto
```

### Infrastructure

Responsável pela persistência dos dados utilizando Entity Framework Core
e SQLite.

Contém:

-   `ApplicationDbContext`
-   Mapeamentos das entidades
-   Migrations
-   Implementação do Repository Genérico

### API

Responsável pela exposição dos recursos por meio de uma API REST.

Contém:

-   Controllers
-   Swagger/OpenAPI
-   Tratamento global de exceções
-   Health Check
-   Configuração de logs
-   Injeção de dependências

### Testes

Foram criados projetos separados para os testes:

``` text
PetLyve.Domain.Tests
PetLyve.Application.Tests
```

------------------------------------------------------------------------

# 💾 Persistência e Banco de Dados

O projeto utiliza **SQLite** como banco de dados.

A escolha foi feita devido à sua simplicidade e portabilidade,
permitindo executar o projeto localmente sem necessidade de configurar
um servidor de banco de dados externo.

A persistência é realizada utilizando:

-   Entity Framework Core
-   SQLite
-   Fluent API
-   Repository Genérico
-   Migrations

------------------------------------------------------------------------

## Repository Genérico

Foi implementado o padrão **Repository Genérico**.

Contrato na camada `Application`:

``` csharp
IRepository<T>
```

Implementação na camada `Infrastructure`:

``` csharp
Repository<T>
```

Operações disponíveis:

``` text
GetAllAsync()
GetByIdAsync(Guid id)
AddAsync(T entity)
DeleteAsync(T entity)
ExistsByIdAsync(Guid id)
```

Registro no sistema de injeção de dependência:

``` csharp
builder.Services.AddScoped(
    typeof(IRepository<>),
    typeof(Repository<>));
```

Dessa forma, o acesso aos dados é realizado por meio de uma abstração,
evitando que os controllers dependam diretamente do `DbContext`.

------------------------------------------------------------------------

# 🗄️ Migrations

Para atualizar o banco de dados para a versão mais recente:

``` bash
dotnet ef database update --project PetLyve.Infrastructure --startup-project PetLyve.API
```

------------------------------------------------------------------------

# 🚀 API REST

Foram criados controllers para os principais recursos da aplicação.

  --------------------------------------------------------------------------
  Recurso    Endpoints
  ---------- ---------------------------------------------------------------
  Donos      `GET /api/Donos` · `GET /api/Donos/{id}` · `POST /api/Donos`

  Animais    `GET /api/Animais` · `GET /api/Animais/{id}` ·
             `POST /api/Animais`

  Serviços   `GET /api/Servicos` · `GET /api/Servicos/{id}` ·
             `POST /api/Servicos`
  --------------------------------------------------------------------------

Os controllers recebem as requisições HTTP e utilizam os serviços da
aplicação para executar as operações.

------------------------------------------------------------------------

# 📦 DTOs

Foram criados DTOs de entrada e saída para evitar a exposição direta das
entidades de domínio.

``` text
DonoRequestDto
DonoResponseDto

AnimalRequestDto
AnimalResponseDto

ServicoRequestDto
ServicoResponseDto
```

Os DTOs também possuem validações para os dados recebidos pela API.

------------------------------------------------------------------------

# ⚙️ Services

Foi criada uma camada de serviços na aplicação para centralizar as
operações relacionadas às regras da aplicação.

Atualmente foi implementado o:

``` text
DonoService
```

O serviço é responsável por operações como:

``` text
GetAllAsync()
GetByIdAsync(Guid id)
CreateAsync(DonoRequestDto request)
```

O `DonoService` utiliza o `IRepository<Dono>` por injeção de
dependência.

Exemplo de registro:

``` csharp
builder.Services.AddScoped<DonoService>();
```

Com isso, o fluxo da aplicação fica organizado da seguinte forma:

``` text
HTTP Request
     ↓
Controller
     ↓
Service
     ↓
IRepository
     ↓
Repository
     ↓
Entity Framework Core
     ↓
SQLite
```

------------------------------------------------------------------------

# 📖 Swagger / OpenAPI

Foi configurado o **Swagger/OpenAPI** para documentação e testes da API.

A documentação apresenta:

-   Nome e versão da API
-   Descrição dos recursos
-   Endpoints disponíveis
-   DTOs utilizados
-   Tipos de resposta
-   Códigos HTTP esperados
-   Comentários XML dos endpoints

Com a API executando em ambiente de desenvolvimento, o Swagger pode ser
acessado em:

``` text
http://localhost:5091/swagger
```

------------------------------------------------------------------------

# ⚠️ Tratamento Global de Exceções

Foi implementado um `GlobalExceptionHandler` utilizando:

``` csharp
IExceptionHandler
```

Configuração:

``` csharp
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();
```

E no pipeline:

``` csharp
app.UseExceptionHandler();
```

As exceções são convertidas para respostas no formato:

``` text
application/problem+json
```

### Mapeamento das exceções

  Exceção                         HTTP
  ----------------------------- ------
  `ArgumentException`              400
  `KeyNotFoundException`           404
  `InvalidOperationException`      409
  Outras exceções                  500

Também foi incluído um `traceId` nas respostas de erro para facilitar a
identificação da requisição nos logs.

Exemplo:

``` json
{
  "type": "https://tools.ietf.org/html/rfc9110#section-15.5.1",
  "title": "One or more validation errors occurred.",
  "status": 400,
  "traceId": "00-ca73da5c84e24cf6c87901c6e1adfe67-bae834f310fa033a-00"
}
```

As mensagens e detalhes internos das exceções não são expostos
desnecessariamente ao cliente.

------------------------------------------------------------------------

# 📊 Observabilidade e Logs

Foi adicionada observabilidade básica utilizando o sistema de logging do
**ASP.NET Core**.

Os controllers utilizam:

``` csharp
ILogger<T>
```

No cadastro de donos são registrados eventos de início, sucesso e erro.

Exemplo de log:

``` text
Dono cadastrado com sucesso.
DonoId=1e2b44ac-d9f2-47f9-bf11-8bc2c5697c96
TraceId=0HNON80PJ5EOL:00000004
```

Os logs utilizam propriedades nomeadas, como:

``` text
Nome
DonoId
TraceId
```

O `TraceId` também é disponibilizado nas respostas de erro, permitindo
relacionar uma requisição HTTP com os registros de log correspondentes.

------------------------------------------------------------------------

# ❤️ Health Check

Foi configurado um Health Check para verificar a disponibilidade da
aplicação e do banco de dados.

Foram registrados:

``` text
self
database
```

O endpoint configurado é:

``` text
GET /health
```

Os códigos HTTP utilizados são:

  Status        HTTP
  ----------- ------
  Healthy        200
  Degraded       200
  Unhealthy      503

A resposta apresenta o status geral e o resultado individual dos checks.

------------------------------------------------------------------------

# 🧪 Testes Automatizados

Foram criados testes automatizados utilizando:

-   **xUnit**
-   **Moq**

Projetos de teste:

``` text
PetLyve.Domain.Tests
PetLyve.Application.Tests
```

## Testes de domínio

Os testes do domínio verificam comportamentos das entidades e suas
regras.

## Testes da Application

Foram criados testes para o `DonoService`, utilizando mocks do
`IRepository<Dono>`.

Entre os cenários testados estão:

-   Criação de um dono.
-   Verificação dos dados retornados após a criação.
-   Verificação de chamada ao repository.
-   Busca de dono existente.
-   Busca de dono inexistente.
-   Lançamento de `KeyNotFoundException`.
-   Listagem de donos.
-   Verificação das chamadas realizadas ao repository.

### Execução dos testes

Na raiz da solução:

``` bash
dotnet test
```

Resultado validado durante o desenvolvimento do CP4:

``` text
Resumo do teste: total: 8; falhou: 0; bem-sucedido: 8; ignorado: 0
```

------------------------------------------------------------------------

# 🧪 Teste da API pelo Swagger

Para executar a aplicação:

``` bash
dotnet run --project PetLyve.API
```

A API será disponibilizada em:

``` text
http://localhost:5091
```

Swagger:

``` text
http://localhost:5091/swagger
```

### Exemplo --- Cadastro de Dono

Endpoint:

``` text
POST /api/Donos
```

JSON:

``` json
{
  "nome": "Teste CP4",
  "telefone": "11999999999",
  "email": "teste@cp4.com"
}
```

Resposta esperada:

``` text
201 Created
```

Exemplo de resposta:

``` json
{
  "donoId": "1e2b44ac-d9f2-47f9-bf11-8bc2c5697c96",
  "nome": "Teste CP4",
  "telefone": "11999999999",
  "email": "teste@cp4.com"
}
```

Durante a validação, também foi confirmado que o registro é persistido
no banco SQLite.

------------------------------------------------------------------------

# ▶️ Como executar o projeto

## 1. Restaurar as dependências

Na raiz da solução:

``` bash
dotnet restore
```

## 2. Atualizar o banco

``` bash
dotnet ef database update --project PetLyve.Infrastructure --startup-project PetLyve.API
```

## 3. Executar os testes

``` bash
dotnet test
```

## 4. Executar a API

``` bash
dotnet run --project PetLyve.API
```

## 5. Acessar o Swagger

``` text
http://localhost:5091/swagger
```

## 6. Verificar o Health Check

``` text
http://localhost:5091/health
```

------------------------------------------------------------------------

# 🧰 Tecnologias

-   .NET 10
-   C#
-   ASP.NET Core
-   Entity Framework Core
-   SQLite
-   Swagger / OpenAPI
-   Swashbuckle
-   xUnit
-   Moq
-   Clean Architecture
-   Repository Genérico
-   DTOs
-   ProblemDetails
-   Health Checks
-   `ILogger`
-   Migrations

------------------------------------------------------------------------

# 📌 Status do projeto

O projeto contempla as entregas desenvolvidas nos ciclos **CP1, CP2,
CP3, CP4 e CP5**, incluindo:

-   Modelagem das entidades e relacionamentos do domínio.

-   Persistência com SQLite, Entity Framework Core e migrations.

-   API REST organizada com Clean Architecture e Repository Genérico.

-   DTOs e serviços na camada Application.

-   Swagger/OpenAPI com documentação separada por versão.

-   Versionamento da listagem de donos: v1 depreciada e v2 atual.

-   Paginação da listagem v2 com envelope, totais e validação dos
    parâmetros.

-   Rate limit por IP com política fixed window e resposta 429.

-   Tratamento global de exceções com Problem Details e `traceId`.

-   Health Check da aplicação e do banco de dados, excluído do rate
    limit.

-   ## Logs estruturados e testes automatizados com xUnit e Moq.

# 🚀 CP5 --- Versionamento de API, Paginação e Rate Limit

No CP5, a API foi evoluída para manter compatibilidade com o contrato
anterior e oferecer uma listagem paginada. O recurso escolhido foi
**Donos**. A versão 1.0 continua disponível, mas está marcada como
depreciada; a versão 2.0 é a versão padrão.

## Resumo das alterações

-   **Versionamento:** v1.0 depreciada e v2.0 atual no mesmo recurso.
-   **Compatibilidade:** a v1 mantém a listagem como array; a v2 retorna
    um envelope paginado.
-   **Paginação:** consulta feita no banco usando contagem, ordenação
    estável, `Skip` e `Take`.
-   **Rate limit:** políticas fixed window por IP para cadastro e
    listagem paginada.
-   **Swagger:** documentação separada por versão.
-   **Health Check:** `/health` continua disponível e não consome o
    limite das políticas.
-   **Arquitetura:** as duas versões reutilizam o mesmo `DonoService`,
    sem duplicar a regra de aplicação.

## Como executar

Na raiz da solução:

``` bash
dotnet restore
dotnet ef database update --project PetLyve.Infrastructure --startup-project PetLyve.API
dotnet test
dotnet run --project PetLyve.API
```

A configuração HTTP do perfil de desenvolvimento usa
`http://localhost:5091`. O perfil HTTPS também disponibiliza
`https://localhost:7214`.

  ---------------------------------------------------------------------------------------
  Recurso                             URL
  ----------------------------------- ---------------------------------------------------
  Swagger UI                          `http://localhost:5091/swagger`

  Swagger v1                          `http://localhost:5091/swagger/v1/swagger.json`

  Swagger v2                          `http://localhost:5091/swagger/v2/swagger.json`

  Health Check                        `http://localhost:5091/health`

  Listagem de donos v1                `http://localhost:5091/api/Donos?api-version=1.0`

  Listagem de donos v2                `http://localhost:5091/api/Donos?api-version=2.0`
  ---------------------------------------------------------------------------------------

> O Swagger é habilitado no ambiente `Development`. A interface
> apresenta os grupos v1 (deprecada) e v2.

## Versionamento da API

O versionamento utiliza `Asp.Versioning.Mvc` e
`Asp.Versioning.Mvc.ApiExplorer`, com `DefaultApiVersion = 2.0`,
`AssumeDefaultVersionWhenUnspecified = true` e
`ReportApiVersions = true`.

A versão pode ser selecionada de três formas:

### 1. Query string

``` http
GET /api/Donos?api-version=1.0
```

Retorna o contrato antigo: um array de donos, sem paginação.

``` http
GET /api/Donos?api-version=2.0
```

Retorna o envelope paginado da v2.

### 2. Header

``` http
GET /api/Donos
X-Api-Version: 1.0
```

O header `X-Api-Version: 2.0` seleciona a v2. Query string e header são
leitores suportados pela configuração.

### 3. Segmento de URL

``` http
GET /api/v1/Donos
GET /api/v2/Donos
```

### 4. Sem informar a versão

``` http
GET /api/Donos
```

Quando nenhuma versão é informada, a API utiliza a **v2.0** por padrão.

As respostas incluem os headers de versão suportada e depreciada, como
`api-supported-versions` e `api-deprecated-versions`.

### Contrato de cada versão

  -----------------------------------------------------------------------
  Versão                  Status                  Resposta da listagem
  ----------------------- ----------------------- -----------------------
  v1.0                    Depreciada, mantida     Array de donos, sem
                          para compatibilidade    paginação

  v2.0                    Atual e padrão          Envelope paginado com
                                                  itens e totais
  -----------------------------------------------------------------------

A listagem v1 e a v2 utilizam o mesmo `DonoService`. Os endpoints de
consulta por ID e cadastro de donos também estão disponíveis nas versões
do controller. O cadastro (`POST /api/Donos`) não precisa de versão
explícita porque a versão padrão é a v2.

## Paginação da listagem v2

Apenas a listagem v2 é paginada. A v1 preserva o contrato antigo.

  Parâmetro      Padrão Regra
  ------------ -------- ----------------------------
  `page`            `1` Inteiro maior ou igual a 1
  `pageSize`       `20` Inteiro de 1 a 100

Exemplos:

``` http
GET /api/Donos?page=1&pageSize=20
GET /api/Donos?api-version=2.0&page=2&pageSize=5
GET /api/v2/Donos?page=3&pageSize=5
```

Se `page` for menor que 1 ou `pageSize` estiver fora do intervalo de 1 a
100, a API responde **400 Bad Request** com Problem Details
(`application/problem+json`) e informações sobre o parâmetro inválido.

Uma página que ultrapasse o total de páginas responde **200 OK**, com
`items` vazio. Isso não é tratado como `404`.

### Formato da resposta v2

``` json
{
  "page": 1,
  "pageSize": 20,
  "totalItems": 42,
  "totalPages": 3,
  "hasPrevious": false,
  "hasNext": true,
  "items": [
    {
      "donoId": "00000000-0000-0000-0000-000000000000",
      "nome": "Exemplo",
      "telefone": "11999999999",
      "email": "exemplo@email.com"
    }
  ]
}
```

Os valores do exemplo são ilustrativos. A resposta real depende dos
dados presentes no banco.

A implementação está distribuída entre as camadas:

1.  **Controller:** recebe `page` e `pageSize`.
2.  **Application:** `PageRequest` valida os parâmetros e
    `DonoService.GetPagedAsync` monta o envelope `PagedResponseDto`.
3.  **Infrastructure:** `IRepository<T>.GetPagedAsync` executa `Count`,
    ordenação, `Skip` e `Take` sobre a consulta antes de materializar os
    resultados.
4.  **Ordenação estável:** a listagem é ordenada por nome e, em caso de
    empate, por `DonoId`, evitando sobreposição inesperada entre páginas
    consecutivas.

## Rate limit

A API utiliza o middleware nativo `Microsoft.AspNetCore.RateLimiting`,
com política **fixed window** particionada pelo IP do cliente. As
políticas são aplicadas apenas aos endpoints marcados; não existe
limitador global.

  -----------------------------------------------------------------------------
  Política        Endpoint                          Limite               Janela
  --------------- ------------------- -------------------- --------------------
  `escrita`       `POST /api/Donos`     10 requisições por          60 segundos
                                                        IP 

  `leitura`       Listagem v2 de        60 requisições por          60 segundos
                  `GET /api/Donos`                      IP 
  -----------------------------------------------------------------------------

Os valores são configuráveis na seção `RateLimiting` de
`PetLyve.API/appsettings.json`, por meio de `PermitLimit` e
`WindowSeconds`.

Ao exceder o limite, a API retorna:

-   HTTP **429 Too Many Requests**;
-   header `Retry-After`, com os segundos até poder tentar novamente;
-   corpo JSON no formato `application/problem+json`, com `status`,
    mensagem, `retryAfterSeconds` e `traceId`.

Nas respostas processadas pelas políticas, também são disponibilizados
os headers `X-RateLimit-Limit`, `X-RateLimit-Remaining` e
`X-RateLimit-Reset`.

O endpoint `GET /health` está explicitamente fora do rate limit. Mesmo
após atingir o limite do `POST /api/Donos`, o health check pode
continuar sendo consultado para verificar o estado da API e do banco.

## Outros endpoints e compatibilidade

O versionamento não foi aplicado à API inteira. Os recursos **Animais**
e **Serviços** são neutros em relação à versão e continuam acessíveis. A
implementação anterior dos endpoints foi preservada.

  -----------------------------------------------------------------------
  Recurso                             Endpoints principais
  ----------------------------------- -----------------------------------
  Donos                               `GET /api/Donos`,
                                      `GET /api/Donos/{id}`,
                                      `POST /api/Donos`

  Animais                             `GET /api/Animais`,
                                      `GET /api/Animais/{id}`,
                                      `POST /api/Animais`

  Serviços                            `GET /api/Servicos`,
                                      `GET /api/Servicos/{id}`,
                                      `POST /api/Servicos`

  Health Check                        `GET /health`
  -----------------------------------------------------------------------

## Testes automatizados

Execute todos os testes da solução na raiz:

``` bash
dotnet test
```

Além dos testes existentes de domínio e aplicação, o projeto contém
testes de paginação para validar parâmetros, envelope, totais, ordenação
e uso do repositório paginado. A suíte de aplicação também verifica que
a listagem paginada não utiliza `GetAllAsync()`.

## Evidências do CP5

As evidências estão organizadas em [`docs/cp5/`](docs/cp5/README.md):

  ---------------------------------------------------------------------------------------------------------------
  Evidência                           Arquivo
  ----------------------------------- ---------------------------------------------------------------------------
  Respostas v1 e v2, seleção de       [`01-versionamento-v1-v2.md`](docs/cp5/01-versionamento-v1-v2.md)
  versão por query/header/URL         

  Headers de versões suportadas e     [`02-headers-de-versao.md`](docs/cp5/02-headers-de-versao.md)
  depreciadas                         

  Erros 400 de paginação inválida     [`03-paginacao-400.md`](docs/cp5/03-paginacao-400.md)

  Páginas, totais e comportamento da  [`04-paginacao-paginas.md`](docs/cp5/04-paginacao-paginas.md)
  página além do total                

  429, `Retry-After` e `/health`      [`05-rate-limit-429-e-health.md`](docs/cp5/05-rate-limit-429-e-health.md)
  respondendo 200                     

  Swagger por versão                  [`06-swagger-por-versao.md`](docs/cp5/06-swagger-por-versao.md)

  SQL gerado para paginação           [`07-paginacao-sql.md`](docs/cp5/07-paginacao-sql.md)

  Resultado de `dotnet test`          [`08-dotnet-test.md`](docs/cp5/08-dotnet-test.md)
  ---------------------------------------------------------------------------------------------------------------

As capturas de tela do Swagger também estão em `docs/cp5/swagger-v1.png`
e `docs/cp5/swagger-v2.png`.

------------------------------------------------------------------------

## Relação entre os ciclos

  Ciclo   Entrega
  ------- -----------------------------------------------------------
  CP1     Modelagem do domínio e entidades
  CP2     Banco de dados, EF Core e migrations
  CP3     API REST, DTOs, Repository Genérico e tratamento de erros
  CP4     Health Check, logs e testes automatizados
  CP5     Versionamento de API, paginação no banco e rate limit
