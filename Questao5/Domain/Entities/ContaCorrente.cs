using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Questao5.Domain.Entities
{
    [Table("contacorrente")]
    public class ContaCorrente
    {
        [Key]
        [Column("idcontacorrente")]
        public string IdContaCorrente { get; set; } = string.Empty;

        [Column("numero")]
        public int Numero { get; set; }

        [Column("nome")]
        public string Nome { get; set; } = string.Empty;

        [Column("ativo")]
        public int Ativo { get; set; }
    }


}
