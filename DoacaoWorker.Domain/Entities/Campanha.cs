using System.ComponentModel.DataAnnotations;

namespace DoacaoWorker.Domain.Entities
{
    // Visão parcial da tabela Campanhas (dona do schema: F5-CS-CampanhasApi). O Worker só escreve ValorArrecadado.
    public class Campanha
    {
        [Key] public int Id { get; set; }
        [Required] public decimal ValorArrecadado { get; set; }
    }
}
