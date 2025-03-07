using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Questao5.Domain.Entities
{
    [Table("movimento")]
    public class Movimento
    {
        [Key]
        [Column("idmovimento")]
        public string IdMovimento { get; set; } = string.Empty;

        [Column("idcontacorrente")]
        public string IdContaCorrente { get; set; } = string.Empty;

        [Column("datamovimento")]
        public string DataMovimento { get; set; } = string.Empty;

        [Column("tipomovimento")]
        public string TipoMovimento { get; set; } = string.Empty;

        [Column("valor")]
        public decimal Valor { get; set; }
    }
}
