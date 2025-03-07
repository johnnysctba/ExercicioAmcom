using MediatR;
using Questao5.Application.Queries.Responses;
using Questao5.Model;
using System.ComponentModel.DataAnnotations;

namespace Questao5.Application.Queries.Requests
{
    /// <summary>
    /// Query para realizar uma consulta de saldo de uma conta corrente.
    /// </summary>
    public class ConsultarSaldoQuery : IRequest<Result<SaldoResponse>>
    {
        /// <summary>
        /// Identificação única da conta corrente.
        /// </summary>
        /// <example>B6BAFC09-6967-ED11-A567-055DFA4A16C9</example>
        [Required]
        public string IdContaCorrente { get; set; } 
    }
}
