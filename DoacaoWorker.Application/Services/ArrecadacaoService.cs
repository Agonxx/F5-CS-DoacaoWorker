using DoacaoWorker.Domain.Interfaces.Repositories;
using DoacaoWorker.Domain.Interfaces.Services;

namespace DoacaoWorker.Application.Services
{
    public class ArrecadacaoService : IArrecadacaoService
    {
        private readonly ICampanhaRepository _campanhaRepository;

        public ArrecadacaoService(ICampanhaRepository campanhaRepository)
        {
            _campanhaRepository = campanhaRepository;
        }

        // Recalcula a partir das doações (e não soma o valor do evento): reentrega da mesma mensagem não duplica o valor
        public async Task<bool> AtualizarValorArrecadado(int campanhaId)
        {
            return await _campanhaRepository.RecalcularValorArrecadado(campanhaId);
        }
    }
}
