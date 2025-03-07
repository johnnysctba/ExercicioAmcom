using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Questao5.Domain.Entities
{
    [Table("idempotencia")]
    public class Idempotencia
    {
        [Key]
        [Column("chave_idempotencia")]
        public string ChaveIdempotencia { get; set; } = string.Empty;

        [Column("requisicao")]
        public string Requisicao { get; set; } = string.Empty;

        [Column("resultado")]
        public string Resultado { get; set; } = string.Empty;
    }
}
