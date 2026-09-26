using System.ComponentModel.DataAnnotations;

namespace DoacaoWorker.Domain.Entities
{
    // Visão parcial da tabela Doacoes (dona do schema: F5-CS-CampanhasApi)
    public class Doacao
    {
        [Key] public int Id { get; set; }
        [Required] public int IdCampanha { get; set; }
        [Required] public decimal ValorDoacao { get; set; }
    }
}
