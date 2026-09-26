# F5-CS-DoacaoWorker

Worker da plataforma **Conexão Solidária** (Hackathon FIAP Pós Tech, Fase 5). Repositório-irmão de [conexao-solidaria](https://github.com/Agonxx/conexao-solidaria), que concentra a documentação e as decisões do projeto.

## Responsabilidade

Consome o `DoacaoRecebidaEvent` publicado pelo [F5-CS-CampanhasApi](https://github.com/Agonxx/F5-CS-CampanhasApi) e atualiza o `ValorArrecadado` da campanha no `CampanhasDB`. A API de doação não escreve esse valor: é o Worker que o mantém.

- O valor é **recalculado** como a soma das doações da campanha (um único `UPDATE` com subquery), e não incrementado com o valor do evento. Assim, reentrega da mesma mensagem não duplica o valor e doações simultâneas não se sobrescrevem.
- Campanha inexistente: a mensagem é ignorada com aviso (retry não resolveria).
- Falha transitória (ex.: banco fora do ar): 3 novas tentativas a cada 5 s; depois a mensagem vai para a fila `_error`.
- O schema é da CampanhasApi (`EnsureCreated` roda lá). O Worker só mapeia as colunas de que precisa e não cria tabelas.

## Stack

.NET 9, MassTransit + RabbitMQ, EF Core + SQL Server, Prometheus (`/metrics`), `/health`.

## Como rodar localmente

Pré-requisito: Docker Desktop. O Worker depende do banco criado pela CampanhasApi, então o fluxo completo sobe os dois. Os compose dos dois repos usam o container `sqlserver` e a porta 1433: **suba um de cada vez**.

```
cd ../F5-CS-CampanhasApi
docker compose up -d --build
cd ../F5-CS-DoacaoWorker
docker build -t doacaoworker .
docker run -d --name doacaoworker --network f5-cs-campanhasapi_conexao-solidaria-network -p 5003:8080 -e "ConnectionStrings__CampanhasDB=Server=sqlserver;Database=CampanhasDB;User Id=sa;Password=Sa12345678!;TrustServerCertificate=True" -e RabbitMQ__Username=guest -e RabbitMQ__Password=guest doacaoworker
```

Depois, crie uma campanha e doe pela CampanhasApi (Swagger em `http://localhost:5002/swagger`); a transparência (`GET /api/Campanha/Transparencia`) passa a mostrar o valor somado. O `docker-compose.yml` deste repo sobe o Worker com SQL Server e RabbitMQ próprios, útil só quando o `CampanhasDB` já existe.

Endpoints do Worker (porta 5003): `GET /health` e `GET /metrics`, com os contadores `doacoes_processadas_total` e `doacoes_ignoradas_total`.

## Contrato do evento

`DoacaoRecebidaEvent` (`namespace Shared.Contracts.Events`), classe duplicada em cada repo. O MassTransit roteia pelo nome completo do tipo; há teste de `FullName`.

## Testes

```
dotnet test DoacaoWorker.Tests/DoacaoWorker.Tests.csproj
```
