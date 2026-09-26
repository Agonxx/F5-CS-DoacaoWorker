using DoacaoWorker.Domain.Interfaces.Services;
using MassTransit;
using Prometheus;
using Shared.Contracts.Events;

namespace DoacaoWorker.Worker.Consumers
{
    public class DoacaoRecebidaConsumer : IConsumer<DoacaoRecebidaEvent>
    {
        private static readonly Counter Processadas = Metrics.CreateCounter(
            "doacoes_processadas_total", "Doações consumidas que atualizaram o valor arrecadado.");
        private static readonly Counter Ignoradas = Metrics.CreateCounter(
            "doacoes_ignoradas_total", "Doações consumidas cuja campanha não existe.");

        private readonly IArrecadacaoService _service;
        private readonly ILogger<DoacaoRecebidaConsumer> _logger;

        public DoacaoRecebidaConsumer(IArrecadacaoService service, ILogger<DoacaoRecebidaConsumer> logger)
        {
            _service = service;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<DoacaoRecebidaEvent> context)
        {
            var ev = context.Message;

            _logger.LogInformation(
                "DoacaoRecebidaEvent recebido: DoacaoId={DoacaoId} | CampanhaId={CampanhaId} | DoadorId={DoadorId} | Valor={Valor}",
                ev.DoacaoId, ev.CampanhaId, ev.DoadorId, ev.Valor);

            var atualizada = await _service.AtualizarValorArrecadado(ev.CampanhaId);

            if (!atualizada)
            {
                // Sem retry: reprocessar não faz a campanha aparecer
                Ignoradas.Inc();
                _logger.LogWarning("Campanha {CampanhaId} não encontrada; DoacaoId={DoacaoId} ignorada", ev.CampanhaId, ev.DoacaoId);
                return;
            }

            Processadas.Inc();
            _logger.LogInformation("ValorArrecadado da campanha {CampanhaId} atualizado (DoacaoId={DoacaoId})", ev.CampanhaId, ev.DoacaoId);
        }
    }
}
