# PetLyve — Projeto Petshop (CP1, CP2, CP3 e CP4)

**Grupo:**
- Guilherme Sola Garcia - RM563674
- Clayton Alves dos Santos - RM562285

---

## 🎯 Sobre o projeto

O **PetLyve** é uma API REST para gerenciamento de um petshop.

O projeto foi desenvolvido em **.NET 10**, utilizando uma organização baseada em **Clean Architecture**, com separação entre domínio, aplicação, infraestrutura e API.

O sistema contempla o fluxo de atendimento de um petshop, desde o cadastro do dono e do animal até o agendamento e pagamento dos serviços.

---

## 🐾 Entidades

O domínio do projeto possui as seguintes entidades:

- **Dono:** dados do responsável pelo pet.
- **Animal:** pet que recebe os serviços.
- **Funcionario:** profissional responsável pelo atendimento.
- **Servico:** serviço oferecido pelo petshop, como banho e tosa.
- **Agendamento:** relaciona o animal, funcionário, serviço e data do atendimento.
- **Pagamento:** registra o pagamento relacionado ao agendamento.

---

## 🔗 Relacionamentos

- Um **Dono** pode ter vários **Animais** (1:N).
- Um **Animal** pode ter vários **Agendamentos** (1:N).
- Um **Funcionario** pode realizar vários **Agendamentos** (1:N).
- Um **Servico** pode estar relacionado a vários **Agendamentos** (1:N).
- Um **Agendamento** possui um **Pagamento** (1:1).


# 🏗️ Arquitetura

O projeto foi organizado seguindo os princípios de **Clean Architecture**:

```text
PetLyve
├── PetLyve.Domain
├── PetLyve.Application
├── PetLyve.Infrastructure
├── PetLyve.API
├── PetLyve.Domain.Tests
└── PetLyve.Application.Tests
````

### Domain

Contém as entidades e regras relacionadas ao domínio da aplicação.

### Application

Contém os contratos, DTOs, serviços de aplicação e abstrações utilizadas pela aplicação.

Exemplos:

```text
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

Responsável pela persistência dos dados utilizando Entity Framework Core e SQLite.

Contém:

* `ApplicationDbContext`
* Mapeamentos das entidades
* Migrations
* Implementação do Repository Genérico

### API

Responsável pela exposição dos recursos por meio de uma API REST.

Contém:

* Controllers
* Swagger/OpenAPI
* Tratamento global de exceções
* Health Check
* Configuração de logs
* Injeção de dependências

### Testes

Foram criados projetos separados para os testes:

```text
PetLyve.Domain.Tests
PetLyve.Application.Tests
```

---

# 💾 Persistência e Banco de Dados

O projeto utiliza **SQLite** como banco de dados.

A escolha foi feita devido à sua simplicidade e portabilidade, permitindo executar o projeto localmente sem necessidade de configurar um servidor de banco de dados externo.

A persistência é realizada utilizando:

* Entity Framework Core
* SQLite
* Fluent API
* Repository Genérico
* Migrations

---

## Repository Genérico

Foi implementado o padrão **Repository Genérico**.

Contrato na camada `Application`:

```csharp
IRepository<T>
```

Implementação na camada `Infrastructure`:

```csharp
Repository<T>
```

Operações disponíveis:

```text
GetAllAsync()
GetByIdAsync(Guid id)
AddAsync(T entity)
DeleteAsync(T entity)
ExistsByIdAsync(Guid id)
```

Registro no sistema de injeção de dependência:

```csharp
builder.Services.AddScoped(
    typeof(IRepository<>),
    typeof(Repository<>));
```

Dessa forma, o acesso aos dados é realizado por meio de uma abstração, evitando que os controllers dependam diretamente do `DbContext`.

---

# 🗄️ Migrations

Para atualizar o banco de dados para a versão mais recente:

```bash
dotnet ef database update --project PetLyve.Infrastructure --startup-project PetLyve.API
```

---

# 🚀 API REST

Foram criados controllers para os principais recursos da aplicação.

| Recurso  | Endpoints                                                             |
| -------- | --------------------------------------------------------------------- |
| Donos    | `GET /api/Donos` · `GET /api/Donos/{id}` · `POST /api/Donos`          |
| Animais  | `GET /api/Animais` · `GET /api/Animais/{id}` · `POST /api/Animais`    |
| Serviços | `GET /api/Servicos` · `GET /api/Servicos/{id}` · `POST /api/Servicos` |

Os controllers recebem as requisições HTTP e utilizam os serviços da aplicação para executar as operações.

---

# 📦 DTOs

Foram criados DTOs de entrada e saída para evitar a exposição direta das entidades de domínio.

```text
DonoRequestDto
DonoResponseDto

AnimalRequestDto
AnimalResponseDto

ServicoRequestDto
ServicoResponseDto
```

Os DTOs também possuem validações para os dados recebidos pela API.

---

# ⚙️ Services

Foi criada uma camada de serviços na aplicação para centralizar as operações relacionadas às regras da aplicação.

Atualmente foi implementado o:

```text
DonoService
```

O serviço é responsável por operações como:

```text
GetAllAsync()
GetByIdAsync(Guid id)
CreateAsync(DonoRequestDto request)
```

O `DonoService` utiliza o `IRepository<Dono>` por injeção de dependência.

Exemplo de registro:

```csharp
builder.Services.AddScoped<DonoService>();
```

Com isso, o fluxo da aplicação fica organizado da seguinte forma:

```text
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

---

# 📖 Swagger / OpenAPI

Foi configurado o **Swagger/OpenAPI** para documentação e testes da API.

A documentação apresenta:

* Nome e versão da API
* Descrição dos recursos
* Endpoints disponíveis
* DTOs utilizados
* Tipos de resposta
* Códigos HTTP esperados
* Comentários XML dos endpoints

Com a API executando em ambiente de desenvolvimento, o Swagger pode ser acessado em:

```text
http://localhost:5091/swagger
```

---

# ⚠️ Tratamento Global de Exceções

Foi implementado um `GlobalExceptionHandler` utilizando:

```csharp
IExceptionHandler
```

Configuração:

```csharp
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();
```

E no pipeline:

```csharp
app.UseExceptionHandler();
```

As exceções são convertidas para respostas no formato:

```text
application/problem+json
```

### Mapeamento das exceções

| Exceção                     | HTTP |
| --------------------------- | ---: |
| `ArgumentException`         |  400 |
| `KeyNotFoundException`      |  404 |
| `InvalidOperationException` |  409 |
| Outras exceções             |  500 |

Também foi incluído um `traceId` nas respostas de erro para facilitar a identificação da requisição nos logs.

Exemplo:

```json
{
  "type": "https://tools.ietf.org/html/rfc9110#section-15.5.1",
  "title": "One or more validation errors occurred.",
  "status": 400,
  "traceId": "00-ca73da5c84e24cf6c87901c6e1adfe67-bae834f310fa033a-00"
}
```

As mensagens e detalhes internos das exceções não são expostos desnecessariamente ao cliente.

---

# 📊 Observabilidade e Logs

Foi adicionada observabilidade básica utilizando o sistema de logging do **ASP.NET Core**.

Os controllers utilizam:

```csharp
ILogger<T>
```

No cadastro de donos são registrados eventos de início, sucesso e erro.

Exemplo de log:

```text
Dono cadastrado com sucesso.
DonoId=1e2b44ac-d9f2-47f9-bf11-8bc2c5697c96
TraceId=0HNON80PJ5EOL:00000004
```

Os logs utilizam propriedades nomeadas, como:

```text
Nome
DonoId
TraceId
```

O `TraceId` também é disponibilizado nas respostas de erro, permitindo relacionar uma requisição HTTP com os registros de log correspondentes.

---

# ❤️ Health Check

Foi configurado um Health Check para verificar a disponibilidade da aplicação e do banco de dados.

Foram registrados:

```text
self
database
```

O endpoint configurado é:

```text
GET /health
```

Os códigos HTTP utilizados são:

| Status    | HTTP |
| --------- | ---: |
| Healthy   |  200 |
| Degraded  |  200 |
| Unhealthy |  503 |

A resposta apresenta o status geral e o resultado individual dos checks.

---

# 🧪 Testes Automatizados

Foram criados testes automatizados utilizando:

* **xUnit**
* **Moq**

Projetos de teste:

```text
PetLyve.Domain.Tests
PetLyve.Application.Tests
```

## Testes de domínio

Os testes do domínio verificam comportamentos das entidades e suas regras.

## Testes da Application

Foram criados testes para o `DonoService`, utilizando mocks do `IRepository<Dono>`.

Entre os cenários testados estão:

* Criação de um dono.
* Verificação dos dados retornados após a criação.
* Verificação de chamada ao repository.
* Busca de dono existente.
* Busca de dono inexistente.
* Lançamento de `KeyNotFoundException`.
* Listagem de donos.
* Verificação das chamadas realizadas ao repository.

### Execução dos testes

Na raiz da solução:

```bash
dotnet test
```

Resultado validado durante o desenvolvimento do CP4:

```text
Resumo do teste: total: 8; falhou: 0; bem-sucedido: 8; ignorado: 0
```

---

# 🧪 Teste da API pelo Swagger

Para executar a aplicação:

```bash
dotnet run --project PetLyve.API
```

A API será disponibilizada em:

```text
http://localhost:5091
```

Swagger:

```text
http://localhost:5091/swagger
```

### Exemplo — Cadastro de Dono

Endpoint:

```text
POST /api/Donos
```

JSON:

```json
{
  "nome": "Teste CP4",
  "telefone": "11999999999",
  "email": "teste@cp4.com"
}
```

Resposta esperada:

```text
201 Created
```

Exemplo de resposta:

```json
{
  "donoId": "1e2b44ac-d9f2-47f9-bf11-8bc2c5697c96",
  "nome": "Teste CP4",
  "telefone": "11999999999",
  "email": "teste@cp4.com"
}
```

Durante a validação, também foi confirmado que o registro é persistido no banco SQLite.

---

# ▶️ Como executar o projeto

## 1. Restaurar as dependências

Na raiz da solução:

```bash
dotnet restore
```

## 2. Atualizar o banco

```bash
dotnet ef database update --project PetLyve.Infrastructure --startup-project PetLyve.API
```

## 3. Executar os testes

```bash
dotnet test
```

## 4. Executar a API

```bash
dotnet run --project PetLyve.API
```

## 5. Acessar o Swagger

```text
http://localhost:5091/swagger
```

## 6. Verificar o Health Check

```text
http://localhost:5091/health
```

---

# 🧰 Tecnologias

* .NET 10
* C#
* ASP.NET Core
* Entity Framework Core
* SQLite
* Swagger / OpenAPI
* Swashbuckle
* xUnit
* Moq
* Clean Architecture
* Repository Genérico
* DTOs
* ProblemDetails
* Health Checks
* `ILogger`
* Migrations

---

# 📌 Status do projeto

O projeto contempla as entregas desenvolvidas nos ciclos **CP1, CP2, CP3 e CP4**, incluindo:

* Modelagem das entidades do domínio.
* Relacionamentos entre entidades.
* Persistência com SQLite.
* Entity Framework Core.
* Migrations.
* Repository Genérico.
* API REST.
* DTOs.
* Swagger/OpenAPI.
* Tratamento global de exceções.
* ProblemDetails.
* Health Checks.
* Logs e observabilidade.
* Services na camada Application.
* Testes automatizados com xUnit.
* Testes de serviços utilizando Moq.

````

