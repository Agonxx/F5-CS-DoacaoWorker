using DoacaoWorker.Worker.Extensions;
using Prometheus;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDatabase(builder.Configuration)
                .AddApplicationServices(builder.Configuration)
                .AddMessaging(builder.Configuration);

var app = builder.Build();

app.MapGet("/health", () => Results.Ok(new { status = "Healthy" }));
app.MapMetrics();

app.Run();
