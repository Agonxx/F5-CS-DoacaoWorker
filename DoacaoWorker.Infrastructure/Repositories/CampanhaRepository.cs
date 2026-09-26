using DoacaoWorker.Domain.Interfaces.Repositories;
using DoacaoWorker.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DoacaoWorker.Infrastructure.Repositories
{
    public class CampanhaRepository : ICampanhaRepository
    {
        private readonly DoacaoWorkerDbContext _db;

        public CampanhaRepository(DoacaoWorkerDbContext db)
        {
            _db = db;
        }

        public async Task<bool> RecalcularValorArrecadado(int campanhaId)
        {
            // Um único UPDATE com subquery: sem read-modify-write, então doações simultâneas não se atropelam
            var changes = await _db.Campanhas
                .Where(c => c.Id == campanhaId)
                .ExecuteUpdateAsync(s => s.SetProperty(
                    c => c.ValorArrecadado,
                    c => _db.Doacoes.Where(d => d.IdCampanha == c.Id).Sum(d => (decimal?)d.ValorDoacao) ?? 0m));

            return changes > 0;
        }
    }
}
