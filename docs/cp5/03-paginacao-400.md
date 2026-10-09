# CP5 — 400 para page / pageSize fora da faixa

> Gerado em 08/10/2026 22:53:39 contra `http://localhost:5091` (ambiente Development, SQLite local com 12 donos).

Regras: `page >= 1` e `1 <= pageSize <= 100`. A validação está em Application (`PageRequest.Create`) e o 400 sai como Problem Details pelo `GlobalExceptionHandler`.

#### page=0

```http
$ curl -i "http://localhost:5091/api/donos?page=0"
HTTP/1.1 400 Bad Request
Content-Type: application/problem+json
api-supported-versions: 2.0
api-deprecated-versions: 1.0
X-RateLimit-Limit: 60
X-RateLimit-Remaining: 54
X-RateLimit-Reset: 51

{
  "title": "Parâmetros de paginação inválidos",
  "status": 400,
  "detail": "O parâmetro 'page' deve ser um inteiro maior ou igual a 1. Valor recebido: 0.",
  "instance": "/api/donos",
  "errors": {
    "page": [
      "O parâmetro 'page' deve ser um inteiro maior ou igual a 1. Valor recebido: 0."
    ]
  },
  "traceId": "0HNP5L0H3V7FS:00000001"
}
```

#### pageSize=9999

```http
$ curl -i "http://localhost:5091/api/donos?pageSize=9999"
HTTP/1.1 400 Bad Request
Content-Type: application/problem+json
api-supported-versions: 2.0
api-deprecated-versions: 1.0
X-RateLimit-Limit: 60
X-RateLimit-Remaining: 53
X-RateLimit-Reset: 50

{
  "title": "Parâmetros de paginação inválidos",
  "status": 400,
  "detail": "O parâmetro 'pageSize' deve ser um inteiro entre 1 e 100. Valor recebido: 9999.",
  "instance": "/api/donos",
  "errors": {
    "pageSize": [
      "O parâmetro 'pageSize' deve ser um inteiro entre 1 e 100. Valor recebido: 9999."
    ]
  },
  "traceId": "0HNP5L0H3V7FT:00000001"
}
```

#### pageSize=0

```http
$ curl -i "http://localhost:5091/api/donos?pageSize=0"
HTTP/1.1 400 Bad Request
Content-Type: application/problem+json
api-supported-versions: 2.0
api-deprecated-versions: 1.0
X-RateLimit-Limit: 60
X-RateLimit-Remaining: 52
X-RateLimit-Reset: 49

{
  "title": "Parâmetros de paginação inválidos",
  "status": 400,
  "detail": "O parâmetro 'pageSize' deve ser um inteiro entre 1 e 100. Valor recebido: 0.",
  "instance": "/api/donos",
  "errors": {
    "pageSize": [
      "O parâmetro 'pageSize' deve ser um inteiro entre 1 e 100. Valor recebido: 0."
    ]
  },
  "traceId": "0HNP5L0H3V7FU:00000001"
}
```

#### page=0 e pageSize=9999 (as duas regras)

```http
$ curl -i "http://localhost:5091/api/donos?page=0&pageSize=9999"
HTTP/1.1 400 Bad Request
Content-Type: application/problem+json
api-supported-versions: 2.0
api-deprecated-versions: 1.0
X-RateLimit-Limit: 60
X-RateLimit-Remaining: 51
X-RateLimit-Reset: 47

{
  "title": "Parâmetros de paginação inválidos",
  "status": 400,
  "detail": "O parâmetro 'page' deve ser um inteiro maior ou igual a 1. Valor recebido: 0. O parâmetro 'pageSize' deve ser um inteiro entre 1 e 100. Valor recebido: 9999.",
  "instance": "/api/donos",
  "errors": {
    "page": [
      "O parâmetro 'page' deve ser um inteiro maior ou igual a 1. Valor recebido: 0."
    ],
    "pageSize": [
      "O parâmetro 'pageSize' deve ser um inteiro entre 1 e 100. Valor recebido: 9999."
    ]
  },
  "traceId": "0HNP5L0H3V7FV:00000001"
}
```

#### page não numérico (model binding do ASP.NET Core)

```http
$ curl -i "http://localhost:5091/api/donos?page=abc"
HTTP/1.1 400 Bad Request
Content-Type: application/problem+json; charset=utf-8
X-RateLimit-Limit: 60
X-RateLimit-Remaining: 50
X-RateLimit-Reset: 45

{
  "type": "https://tools.ietf.org/html/rfc9110#section-15.5.1",
  "title": "One or more validation errors occurred.",
  "status": 400,
  "errors": {
    "page": [
      "The value 'abc' is not valid."
    ]
  },
  "traceId": "00-3088d62abb2dbee4a99afb48e5ad3b5b-99cb5ff59a4d9e11-00"
}
```

