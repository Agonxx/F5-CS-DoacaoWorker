namespace DoacaoWorker.Domain.Interfaces.Repositories
{
    public interface ICampanhaRepository
    {
        // Recalcula ValorArrecadado como a soma das doações da campanha. False se a campanha não existe.
        Task<bool> RecalcularValorArrecadado(int campanhaId);
    }
}
