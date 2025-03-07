using MediatR;
using Questao5.Application.Commands.Responses;
using Questao5.Model;
using System.ComponentModel.DataAnnotations;

namespace Questao5.Application.Commands.Requests
{
    /// <summary>
    /// Comando para realizar uma movimentação (crédito ou débito) em uma conta corrente.
    /// </summary>
    public class MovimentarContaCorrenteCommand : IRequest<Result<MovimentacaoResponse>>
    {
        /// <summary>
        /// Identificação única da requisição (para garantir idempotência).
        /// </summary>
        /// <example>123e4567-e89b-12d3-a456-426614174000</example>
        [Required]
        public string IdRequisicao { get; set; }

        /// <summary>
        /// Identificação única da conta corrente.
        /// </summary>
        /// <example>B6BAFC09-6967-ED11-A567-055DFA4A16C9</example>
        [Required]
        public string IdContaCorrente { get; set; }

        /// <summary>
        /// Valor da movimentação. Deve ser um número positivo.
        /// </summary>
        /// <example>100.00</example>
        [Required]
        [RegularExpression(@"^\d+([.,]\d{1,2})?$", ErrorMessage = "O valor deve ter no máximo duas casas decimais.")]
        public decimal Valor { get; set; }

        /// <summary>
        /// Tipo de movimento. Valores aceitos: "C" (Crédito) ou "D" (Débito).
        /// </summary>
        /// <example>C</example>
        [Required]
        public string TipoMovimento { get; set; }
    }
}