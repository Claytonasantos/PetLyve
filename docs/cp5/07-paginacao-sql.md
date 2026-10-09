# CP5 — Paginação cortada no banco (SQL gerado pelo EF Core)

Log do EF Core (`Microsoft.EntityFrameworkCore.Database.Command`) durante `GET /api/donos?page=2&pageSize=5`.
O `Repository<T>.GetPagedAsync` executa `COUNT` + `ORDER BY` + `LIMIT/OFFSET` (Skip/Take) no SQLite e só então materializa com `ToListAsync`.

```text
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (0ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
      SELECT COUNT(*)
      FROM "Donos" AS "d"
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (0ms) [Parameters=[@p='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT "d"."DonoId", "d"."Email", "d"."Nome", "d"."Telefone"
      FROM "Donos" AS "d"
      ORDER BY "d"."Nome", "d"."DonoId"
      LIMIT @p OFFSET @p
```

> Página 2 com `pageSize=5`: Skip = (2 - 1) × 5 = 5 e Take = 5. Como os dois valores são iguais, o EF Core reaproveita o mesmo parâmetro `@p` em `LIMIT` e `OFFSET`.
