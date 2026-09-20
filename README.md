# Petlyfe - Projeto Petshop (CP1, CP2 e CP3)

**Grupo:**
* Guilherme Sola Garcia - RM563674
* Clayton Alves dos Santos - RM562285

---

### 🎯 Sobre o Projeto
A gente escolheu o domínio de um **Petshop**. O foco aqui foi modelar como funciona o atendimento, desde o dono chegando com o pet até o pagamento do serviço.

### 🐾 O que foi modelado (Entidades):
* **Dono:** Dados de quem leva o pet.
* **Animal:** O pet que vai receber o serviço.
* **Funcionario:** Quem vai trabalhar no atendimento.
* **Servico:** O que vai ser feito (banho, tosa, etc) e o preço.
* **Agendamento:** Onde a gente junta o pet, o funcionário e o serviço com uma data.
* **Pagamento:** O registro do valor pago pelo agendamento.

### 🔗 Relacionamentos:
* Um **Dono** pode ter vários **Animais** (1:N).
* Um **Animal** pode ter vários **Agendamentos** (1:N).
* Um **Funcionario** faz vários **Agendamentos** (1:N).
* Um **Servico** pode estar em vários **Agendamentos** (1:N).
* Um **Agendamento** gera um **Pagamento** (1:1).

### 🏗️ Estrutura do Código:
O projeto foi dividido seguindo a ideia de **Clean Arch**, com as pastas:
* **Domain:** Onde estão as nossas classes (entidades).
* **Application / Infrastructure / API:** Criamos as pastas para deixar o projeto já estruturado.

---

### 💾 Persistência e Banco de Dados (CP2)

**SGBD Escolhido: SQLite**
Optamos por usar o SQLite devido à sua simplicidade e portabilidade. Ele gera um arquivo `.db` local na raiz do projeto da API, não expondo credenciais reais (como senhas de servidores) no repositório e garantindo que o projeto seja 100% reproduzível ao clonar.

**Estrutura de Dados:**
* Implementamos o padrão **Repository Genérico** na camada `Application`.
* O mapeamento das entidades foi feito utilizando **Fluent API** (`IEntityTypeConfiguration`) na camada de `Infrastructure`, garantindo a correta relação 1:N e 1:1 estipulada no MER.

### 🚀 Como testar o Banco de Dados e Migrations
1. Abra o terminal na raiz do projeto.
2. Atualize o banco para a última versão executando o comando abaixo:
   ```bash
   dotnet ef database update --project PetLyve.Infrastructure --startup-project PetLyve.API


# PetLyve — CP3

---

## Sobre o projeto

O **PetLyve** é uma API REST para gerenciamento de um petshop, desenvolvida em **.NET 10** com organização baseada em **Clean Architecture**.

No CP3, o projeto foi evoluído para disponibilizar os recursos por API REST e recebeu **DTOs, Repository Genérico, Swagger/OpenAPI e tratamento global de exceções**.

---

## Estrutura

```text
PetLyve
├── PetLyve.Domain
├── PetLyve.Application
├── PetLyve.Infrastructure
└── PetLyve.API
```

- **Domain:** entidades e elementos do domínio.
- **Application:** contratos e DTOs.
- **Infrastructure:** persistência, Entity Framework Core e Repository.
- **API:** controllers, Swagger e configuração da aplicação.

A API não acessa o `DbContext` diretamente nos controllers.

---

## Banco de dados

O projeto utiliza **SQLite** com Entity Framework Core, mantendo a persistência e os mapeamentos desenvolvidos no CP2.

Para atualizar o banco:

```bash
dotnet ef database update --project PetLyve.Infrastructure --startup-project PetLyve.API
```

---

# API REST

Foram criados controllers para três recursos:

| Recurso | Endpoints |
|---|---|
| Donos | `GET /api/Donos` · `GET /api/Donos/{id}` · `POST /api/Donos` |
| Animais | `GET /api/Animais` · `GET /api/Animais/{id}` · `POST /api/Animais` |
| Serviços | `GET /api/Servicos` · `GET /api/Servicos/{id}` · `POST /api/Servicos` |

Os controllers são responsáveis apenas por receber a requisição, utilizar o repository, mapear DTOs e retornar a resposta HTTP.

---

## DTOs

Foram criados DTOs de entrada e saída:

```text
DonoRequestDto       DonoResponseDto
AnimalRequestDto     AnimalResponseDto
ServicoRequestDto    ServicoResponseDto
```

Os DTOs evitam a exposição direta das entidades de domínio e possuem validações para os dados recebidos.

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

- `GetAllAsync()`
- `GetByIdAsync(Guid id)`
- `AddAsync(T entity)`
- `DeleteAsync(T entity)`
- `ExistsByIdAsync(Guid id)`

Registro no sistema de injeção de dependência:

```csharp
builder.Services.AddScoped(
    typeof(IRepository<>),
    typeof(Repository<>));
```

Assim, os controllers utilizam o acesso aos dados por meio da abstração, sem depender diretamente do `DbContext`.

---

## Swagger / OpenAPI

Foi configurado o **Swashbuckle** para documentação e testes da API.

A documentação inclui:

- Nome, versão e descrição da API
- Comentários XML dos endpoints
- Tipos de resposta
- Códigos HTTP esperados

Também foi habilitada a geração do XML:

```xml
<GenerateDocumentationFile>true</GenerateDocumentationFile>
```

Com a API executando:

```text
http://localhost:5091/swagger
```

---

## Tratamento global de exceções

Foi criado o `GlobalExceptionHandler`, implementando `IExceptionHandler`.

Configuração:

```csharp
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();
```

O pipeline utiliza:

```csharp
app.UseExceptionHandler();
```

As exceções são convertidas para respostas `ProblemDetails` no formato:

```text
application/problem+json
```

### Mapeamento

| Exceção | HTTP |
|---|---:|
| `ArgumentException` | 400 |
| `KeyNotFoundException` | 404 |
| `InvalidOperationException` | 409 |
| Outras exceções | 500 |

As mensagens retornadas ao cliente são genéricas, evitando exposição de detalhes internos do Entity Framework ou do banco.

Exemplo:

```json
{
  "title": "Recurso não encontrado",
  "status": 404,
  "detail": "O recurso solicitado não foi encontrado."
}
```

---

## Testes realizados

Foram testados:

- GET de donos, animais e serviços
- POST de donos, animais e serviços
- Busca de recurso inexistente
- Tratamento de erro interno
- Respostas da API pelo Swagger

Também foi verificado que erros internos não expõem detalhes da aplicação ao cliente.

---

## Execução

Na raiz da solução:

```bash
dotnet build
dotnet run --project PetLyve.API
```

API:

```text
http://localhost:5091
```

Swagger:

```text
http://localhost:5091/swagger
```

Health Check:

```text
GET /api/health
```

---

## Tecnologias

- .NET 10
- ASP.NET Core
- C#
- Entity Framework Core
- SQLite
- Swagger / OpenAPI
- Clean Architecture
- Repository Genérico
- DTOs
- ProblemDetails

