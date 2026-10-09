# CP5 — GET /api/donos: v1 (lista) x v2 (envelope)

> Gerado em 08/10/2026 22:53:39 contra `http://localhost:5091` (ambiente Development, SQLite local com 12 donos).

Recurso versionado: **Donos**. As duas versões chamam o mesmo `DonoService`.

## v1 (deprecada) — contrato antigo do CP3: array completo, sem paginação

#### Versão por query string

```http
$ curl -i "http://localhost:5091/api/donos?api-version=1.0"
HTTP/1.1 200 OK
Content-Type: application/json; charset=utf-8
api-supported-versions: 2.0
api-deprecated-versions: 1.0

[
  {
    "donoId": "8d0793db-4522-495a-ac70-8cdb38cce288",
    "nome": "Clayton",
    "telefone": "11999999999",
    "email": "clayton@email.com"
  },
  {
    "donoId": "31d04533-5722-490c-b2af-5f2a425bb83b",
    "nome": "Ana Beatriz",
    "telefone": "11900000001",
    "email": "ana.beatriz@email.com"
  },
  {
    "donoId": "2ba64994-d559-49ea-971a-faa83c4705fc",
    "nome": "Bruno Costa",
    "telefone": "11900000002",
    "email": "bruno.costa@email.com"
  },
  {
    "donoId": "b26d3eef-5948-4c48-8b0e-1c9dbd02d669",
    "nome": "Carla Mendes",
    "telefone": "11900000003",
    "email": "carla.mendes@email.com"
  },
  {
    "donoId": "41f73bb5-29ab-4e96-9268-4bcea15d25be",
    "nome": "Diego Alves",
    "telefone": "11900000004",
    "email": "diego.alves@email.com"
  },
  {
    "donoId": "e4909add-a0be-44bc-b5bc-082c3b207cb9",
    "nome": "Eduarda Lima",
    "telefone": "11900000005",
    "email": "eduarda.lima@email.com"
  },
  {
    "donoId": "7bd4ca4c-5a33-426d-9f04-47d8047ce417",
    "nome": "Fabio Nunes",
    "telefone": "11900000006",
    "email": "fabio.nunes@email.com"
  },
  {
    "donoId": "b5dc5fab-8319-4ffa-b898-ac276d52ca26",
    "nome": "Gabriela Rocha",
    "telefone": "11900000007",
    "email": "gabriela.rocha@email.com"
  },
  {
    "donoId": "0212b017-bb76-4442-b552-27b7d1ef1051",
    "nome": "Henrique Dias",
    "telefone": "11900000008",
    "email": "henrique.dias@email.com"
  },
  {
    "donoId": "483806b1-a352-4457-b8fd-df73b55e320a",
    "nome": "Isabela Martins",
    "telefone": "11900000009",
    "email": "isabela.martins@email.com"
  },
  {
    "donoId": "beed93c7-54e9-458a-b1e2-ab916e31e97c",
    "nome": "Joao Pedro",
    "telefone": "11900000010",
    "email": "joao.pedro@email.com"
  },
  {
    "donoId": "1bc5052d-ce91-42ff-8031-c10a30b4e072",
    "nome": "Larissa Teixeira",
    "telefone": "11900000011",
    "email": "larissa.teixeira@email.com"
  }
]
```

#### Versão por header

```http
$ curl -i -H "X-Api-Version: 1.0" "http://localhost:5091/api/donos"
HTTP/1.1 200 OK
Content-Type: application/json; charset=utf-8
api-supported-versions: 2.0
api-deprecated-versions: 1.0

[
  {
    "donoId": "8d0793db-4522-495a-ac70-8cdb38cce288",
    "nome": "Clayton",
    "telefone": "11999999999",
    "email": "clayton@email.com"
  },
  {
    "donoId": "31d04533-5722-490c-b2af-5f2a425bb83b",
    "nome": "Ana Beatriz",
    "telefone": "11900000001",
    "email": "ana.beatriz@email.com"
  },
  {
    "donoId": "2ba64994-d559-49ea-971a-faa83c4705fc",
    "nome": "Bruno Costa",
    "telefone": "11900000002",
    "email": "bruno.costa@email.com"
  },
  {
    "donoId": "b26d3eef-5948-4c48-8b0e-1c9dbd02d669",
    "nome": "Carla Mendes",
    "telefone": "11900000003",
    "email": "carla.mendes@email.com"
  },
  {
    "donoId": "41f73bb5-29ab-4e96-9268-4bcea15d25be",
    "nome": "Diego Alves",
    "telefone": "11900000004",
    "email": "diego.alves@email.com"
  },
  {
    "donoId": "e4909add-a0be-44bc-b5bc-082c3b207cb9",
    "nome": "Eduarda Lima",
    "telefone": "11900000005",
    "email": "eduarda.lima@email.com"
  },
  {
    "donoId": "7bd4ca4c-5a33-426d-9f04-47d8047ce417",
    "nome": "Fabio Nunes",
    "telefone": "11900000006",
    "email": "fabio.nunes@email.com"
  },
  {
    "donoId": "b5dc5fab-8319-4ffa-b898-ac276d52ca26",
    "nome": "Gabriela Rocha",
    "telefone": "11900000007",
    "email": "gabriela.rocha@email.com"
  },
  {
    "donoId": "0212b017-bb76-4442-b552-27b7d1ef1051",
    "nome": "Henrique Dias",
    "telefone": "11900000008",
    "email": "henrique.dias@email.com"
  },
  {
    "donoId": "483806b1-a352-4457-b8fd-df73b55e320a",
    "nome": "Isabela Martins",
    "telefone": "11900000009",
    "email": "isabela.martins@email.com"
  },
  {
    "donoId": "beed93c7-54e9-458a-b1e2-ab916e31e97c",
    "nome": "Joao Pedro",
    "telefone": "11900000010",
    "email": "joao.pedro@email.com"
  },
  {
    "donoId": "1bc5052d-ce91-42ff-8031-c10a30b4e072",
    "nome": "Larissa Teixeira",
    "telefone": "11900000011",
    "email": "larissa.teixeira@email.com"
  }
]
```

#### Versão por segmento de URL (opcional)

```http
$ curl -i "http://localhost:5091/api/v1/donos"
HTTP/1.1 200 OK
Content-Type: application/json; charset=utf-8
api-supported-versions: 2.0
api-deprecated-versions: 1.0

[
  {
    "donoId": "8d0793db-4522-495a-ac70-8cdb38cce288",
    "nome": "Clayton",
    "telefone": "11999999999",
    "email": "clayton@email.com"
  },
  {
    "donoId": "31d04533-5722-490c-b2af-5f2a425bb83b",
    "nome": "Ana Beatriz",
    "telefone": "11900000001",
    "email": "ana.beatriz@email.com"
  },
  {
    "donoId": "2ba64994-d559-49ea-971a-faa83c4705fc",
    "nome": "Bruno Costa",
    "telefone": "11900000002",
    "email": "bruno.costa@email.com"
  },
  {
    "donoId": "b26d3eef-5948-4c48-8b0e-1c9dbd02d669",
    "nome": "Carla Mendes",
    "telefone": "11900000003",
    "email": "carla.mendes@email.com"
  },
  {
    "donoId": "41f73bb5-29ab-4e96-9268-4bcea15d25be",
    "nome": "Diego Alves",
    "telefone": "11900000004",
    "email": "diego.alves@email.com"
  },
  {
    "donoId": "e4909add-a0be-44bc-b5bc-082c3b207cb9",
    "nome": "Eduarda Lima",
    "telefone": "11900000005",
    "email": "eduarda.lima@email.com"
  },
  {
    "donoId": "7bd4ca4c-5a33-426d-9f04-47d8047ce417",
    "nome": "Fabio Nunes",
    "telefone": "11900000006",
    "email": "fabio.nunes@email.com"
  },
  {
    "donoId": "b5dc5fab-8319-4ffa-b898-ac276d52ca26",
    "nome": "Gabriela Rocha",
    "telefone": "11900000007",
    "email": "gabriela.rocha@email.com"
  },
  {
    "donoId": "0212b017-bb76-4442-b552-27b7d1ef1051",
    "nome": "Henrique Dias",
    "telefone": "11900000008",
    "email": "henrique.dias@email.com"
  },
  {
    "donoId": "483806b1-a352-4457-b8fd-df73b55e320a",
    "nome": "Isabela Martins",
    "telefone": "11900000009",
    "email": "isabela.martins@email.com"
  },
  {
    "donoId": "beed93c7-54e9-458a-b1e2-ab916e31e97c",
    "nome": "Joao Pedro",
    "telefone": "11900000010",
    "email": "joao.pedro@email.com"
  },
  {
    "donoId": "1bc5052d-ce91-42ff-8031-c10a30b4e072",
    "nome": "Larissa Teixeira",
    "telefone": "11900000011",
    "email": "larissa.teixeira@email.com"
  }
]
```

## v2 (atual) — envelope paginado

#### Sem versão (cai na 2.0)

```http
$ curl -i "http://localhost:5091/api/donos"
HTTP/1.1 200 OK
Content-Type: application/json; charset=utf-8
api-supported-versions: 2.0
api-deprecated-versions: 1.0
X-RateLimit-Limit: 60
X-RateLimit-Remaining: 59
X-RateLimit-Reset: 60

{
  "page": 1,
  "pageSize": 20,
  "totalItems": 12,
  "totalPages": 1,
  "hasPrevious": false,
  "hasNext": false,
  "items": [
    {
      "donoId": "31d04533-5722-490c-b2af-5f2a425bb83b",
      "nome": "Ana Beatriz",
      "telefone": "11900000001",
      "email": "ana.beatriz@email.com"
    },
    {
      "donoId": "2ba64994-d559-49ea-971a-faa83c4705fc",
      "nome": "Bruno Costa",
      "telefone": "11900000002",
      "email": "bruno.costa@email.com"
    },
    {
      "donoId": "b26d3eef-5948-4c48-8b0e-1c9dbd02d669",
      "nome": "Carla Mendes",
      "telefone": "11900000003",
      "email": "carla.mendes@email.com"
    },
    {
      "donoId": "8d0793db-4522-495a-ac70-8cdb38cce288",
      "nome": "Clayton",
      "telefone": "11999999999",
      "email": "clayton@email.com"
    },
    {
      "donoId": "41f73bb5-29ab-4e96-9268-4bcea15d25be",
      "nome": "Diego Alves",
      "telefone": "11900000004",
      "email": "diego.alves@email.com"
    },
    {
      "donoId": "e4909add-a0be-44bc-b5bc-082c3b207cb9",
      "nome": "Eduarda Lima",
      "telefone": "11900000005",
      "email": "eduarda.lima@email.com"
    },
    {
      "donoId": "7bd4ca4c-5a33-426d-9f04-47d8047ce417",
      "nome": "Fabio Nunes",
      "telefone": "11900000006",
      "email": "fabio.nunes@email.com"
    },
    {
      "donoId": "b5dc5fab-8319-4ffa-b898-ac276d52ca26",
      "nome": "Gabriela Rocha",
      "telefone": "11900000007",
      "email": "gabriela.rocha@email.com"
    },
    {
      "donoId": "0212b017-bb76-4442-b552-27b7d1ef1051",
      "nome": "Henrique Dias",
      "telefone": "11900000008",
      "email": "henrique.dias@email.com"
    },
    {
      "donoId": "483806b1-a352-4457-b8fd-df73b55e320a",
      "nome": "Isabela Martins",
      "telefone": "11900000009",
      "email": "isabela.martins@email.com"
    },
    {
      "donoId": "beed93c7-54e9-458a-b1e2-ab916e31e97c",
      "nome": "Joao Pedro",
      "telefone": "11900000010",
      "email": "joao.pedro@email.com"
    },
    {
      "donoId": "1bc5052d-ce91-42ff-8031-c10a30b4e072",
      "nome": "Larissa Teixeira",
      "telefone": "11900000011",
      "email": "larissa.teixeira@email.com"
    }
  ]
}
```

#### Versão explícita por query string

```http
$ curl -i "http://localhost:5091/api/donos?api-version=2.0&pageSize=3"
HTTP/1.1 200 OK
Content-Type: application/json; charset=utf-8
api-supported-versions: 2.0
api-deprecated-versions: 1.0
X-RateLimit-Limit: 60
X-RateLimit-Remaining: 58
X-RateLimit-Reset: 59

{
  "page": 1,
  "pageSize": 3,
  "totalItems": 12,
  "totalPages": 4,
  "hasPrevious": false,
  "hasNext": true,
  "items": [
    {
      "donoId": "31d04533-5722-490c-b2af-5f2a425bb83b",
      "nome": "Ana Beatriz",
      "telefone": "11900000001",
      "email": "ana.beatriz@email.com"
    },
    {
      "donoId": "2ba64994-d559-49ea-971a-faa83c4705fc",
      "nome": "Bruno Costa",
      "telefone": "11900000002",
      "email": "bruno.costa@email.com"
    },
    {
      "donoId": "b26d3eef-5948-4c48-8b0e-1c9dbd02d669",
      "nome": "Carla Mendes",
      "telefone": "11900000003",
      "email": "carla.mendes@email.com"
    }
  ]
}
```

#### Versão explícita por header

```http
$ curl -i -H "X-Api-Version: 2.0" "http://localhost:5091/api/donos?pageSize=3"
HTTP/1.1 200 OK
Content-Type: application/json; charset=utf-8
api-supported-versions: 2.0
api-deprecated-versions: 1.0
X-RateLimit-Limit: 60
X-RateLimit-Remaining: 57
X-RateLimit-Reset: 57

{
  "page": 1,
  "pageSize": 3,
  "totalItems": 12,
  "totalPages": 4,
  "hasPrevious": false,
  "hasNext": true,
  "items": [
    {
      "donoId": "31d04533-5722-490c-b2af-5f2a425bb83b",
      "nome": "Ana Beatriz",
      "telefone": "11900000001",
      "email": "ana.beatriz@email.com"
    },
    {
      "donoId": "2ba64994-d559-49ea-971a-faa83c4705fc",
      "nome": "Bruno Costa",
      "telefone": "11900000002",
      "email": "bruno.costa@email.com"
    },
    {
      "donoId": "b26d3eef-5948-4c48-8b0e-1c9dbd02d669",
      "nome": "Carla Mendes",
      "telefone": "11900000003",
      "email": "carla.mendes@email.com"
    }
  ]
}
```

#### Versão por segmento de URL (opcional)

```http
$ curl -i "http://localhost:5091/api/v2/donos?pageSize=3"
HTTP/1.1 200 OK
Content-Type: application/json; charset=utf-8
api-supported-versions: 2.0
api-deprecated-versions: 1.0
X-RateLimit-Limit: 60
X-RateLimit-Remaining: 56
X-RateLimit-Reset: 56

{
  "page": 1,
  "pageSize": 3,
  "totalItems": 12,
  "totalPages": 4,
  "hasPrevious": false,
  "hasNext": true,
  "items": [
    {
      "donoId": "31d04533-5722-490c-b2af-5f2a425bb83b",
      "nome": "Ana Beatriz",
      "telefone": "11900000001",
      "email": "ana.beatriz@email.com"
    },
    {
      "donoId": "2ba64994-d559-49ea-971a-faa83c4705fc",
      "nome": "Bruno Costa",
      "telefone": "11900000002",
      "email": "bruno.costa@email.com"
    },
    {
      "donoId": "b26d3eef-5948-4c48-8b0e-1c9dbd02d669",
      "nome": "Carla Mendes",
      "telefone": "11900000003",
      "email": "carla.mendes@email.com"
    }
  ]
}
```

## Versão não suportada

#### api-version=3.0

```http
$ curl -i "http://localhost:5091/api/donos?api-version=3.0"
HTTP/1.1 400 Bad Request
Content-Type: application/problem+json
api-supported-versions: 2.0
api-deprecated-versions: 1.0

{
  "type": "https://docs.api-versioning.org/problems#unsupported",
  "title": "Unsupported API version",
  "status": 400,
  "detail": "The HTTP resource that matches the request URI 'http://localhost:5091/api/donos' does not support the API version '3.0'.",
  "code": "UnsupportedApiVersion",
  "traceId": "00-68da721cc589b02480c0e21e22589b6a-861e6db6f7ce0f28-00"
}
```

