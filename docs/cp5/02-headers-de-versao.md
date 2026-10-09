# CP5 — Headers api-supported-versions e api-deprecated-versions

> Gerado em 08/10/2026 22:53:39 contra `http://localhost:5091` (ambiente Development, SQLite local com 12 donos).

Com `ReportApiVersions = true`, toda resposta do recurso versionado informa as versões suportadas e as deprecadas.

```http
$ curl -sI "http://localhost:5091/api/donos"
HTTP/1.1 200 OK
api-supported-versions: 2.0
api-deprecated-versions: 1.0

$ curl -sI "http://localhost:5091/api/donos?api-version=1.0"
HTTP/1.1 200 OK
api-supported-versions: 2.0
api-deprecated-versions: 1.0

$ curl -sI -H "X-Api-Version: 1.0" "http://localhost:5091/api/donos"
HTTP/1.1 200 OK
api-supported-versions: 2.0
api-deprecated-versions: 1.0

```
