# CP5 — Rate limit no POST /api/donos (429 + Retry-After) e /health fora do teto

> Gerado em 08/10/2026 22:53:39 contra `http://localhost:5091` (ambiente Development, SQLite local com 12 donos).

Política `escrita`: fixed window por IP, **10 requisições a cada 60 segundos**. A 11ª requisição na mesma janela recebe 429.

## Disparando 11 POSTs seguidos

```text
POST #01 -> 201  X-RateLimit-Remaining: 9
POST #02 -> 201  X-RateLimit-Remaining: 8
POST #03 -> 201  X-RateLimit-Remaining: 7
POST #04 -> 201  X-RateLimit-Remaining: 6
POST #05 -> 201  X-RateLimit-Remaining: 5
POST #06 -> 201  X-RateLimit-Remaining: 4
POST #07 -> 201  X-RateLimit-Remaining: 3
POST #08 -> 201  X-RateLimit-Remaining: 2
POST #09 -> 201  X-RateLimit-Remaining: 1
POST #10 -> 201  X-RateLimit-Remaining: 0
```

#### POST #11 — limite estourado

```http
$ curl -i -X POST http://localhost:5091/api/donos -H 'Content-Type: application/json' -d '{"nome":"Cliente Rate Limit 11","telefone":"11988880000","email":"rate11@email.com"}'
HTTP/1.1 429 Too Many Requests
Content-Type: application/problem+json
Retry-After: 57
X-RateLimit-Limit: 10
X-RateLimit-Remaining: 0
X-RateLimit-Reset: 57

{
  "title": "Limite de requisições excedido",
  "status": 429,
  "detail": "Muitas requisições para este endpoint. Tente novamente em 57 segundo(s).",
  "instance": "/api/donos",
  "retryAfterSeconds": 57,
  "traceId": "0HNP5L0H3V7GG:00000001"
}
```

## GET /health logo após o 429 (não divide o teto)

#### /health

```http
$ curl -i "http://localhost:5091/health"
HTTP/1.1 200 OK
Content-Type: application/json

{
  "status": "Healthy",
  "totalDuration": "00:00:00.0095366",
  "checks": [
    {
      "name": "self",
      "status": "Healthy",
      "duration": "00:00:00.0001930",
      "exception": null
    },
    {
      "name": "database",
      "status": "Healthy",
      "duration": "00:00:00.0083423",
      "exception": null
    }
  ]
}
```

## Listagem v2 continua respondendo (política `leitura`, separada: 60 req/60 s)

#### GET v2

```http
$ curl -i "http://localhost:5091/api/donos?pageSize=1"
HTTP/1.1 200 OK
Content-Type: application/json; charset=utf-8
api-supported-versions: 2.0
api-deprecated-versions: 1.0
X-RateLimit-Limit: 60
X-RateLimit-Remaining: 44
X-RateLimit-Reset: 30

{
  "page": 1,
  "pageSize": 1,
  "totalItems": 22,
  "totalPages": 22,
  "hasPrevious": false,
  "hasNext": true,
  "items": [
    {
      "donoId": "31d04533-5722-490c-b2af-5f2a425bb83b",
      "nome": "Ana Beatriz",
      "telefone": "11900000001",
      "email": "ana.beatriz@email.com"
    }
  ]
}
```

