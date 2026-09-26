namespace DoacaoWorker.Domain.Interfaces.Services
{
    public interface IArrecadacaoService
    {
        Task<bool> AtualizarValorArrecadado(int campanhaId);
    }
}
