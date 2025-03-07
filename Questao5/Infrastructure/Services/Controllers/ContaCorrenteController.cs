using MediatR;
using Microsoft.AspNetCore.Mvc;
using Questao5.Application.Commands.Requests;
using Questao5.Application.Commands.Responses;
using Questao5.Application.Queries.Requests;
using Questao5.Application.Queries.Responses;
using Questao5.Domain.Responses;
using Questao5.Examples;
using Swashbuckle.AspNetCore.Filters;
namespace Questao5.Infrastructure.Services.Controllers
{
    /// <summary>
    /// Controlador para operações de conta corrente.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class ContaCorrenteController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ContaCorrenteController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Consulta o saldo de uma conta corrente.
        /// </summary>
        /// <param name="query">Dados da consulta.</param>
        /// <returns>Saldo da conta corrente.</returns>
        /// <response code="200">Saldo consultado com sucesso.</response>
        /// <response code="400">Requisição inválida.</response>
        [HttpGet("saldo")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SaldoResponse))]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(SaldoResponseExample))]

        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponse))]
        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(ErrorResponseExample))]

        public async Task<IActionResult> ConsultarSaldo([FromQuery] ConsultarSaldoQuery query)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _mediator.Send(query);
            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }
            return BadRequest(result.GetError());
        }

        /// <summary>
        /// Realiza uma movimentação (crédito ou débito) em uma conta corrente.
        /// </summary>
        /// <param name="command">Dados da movimentação.</param>
        /// <returns>ID do movimento gerado.</returns>
        /// <response code="200">Movimentação realizada com sucesso.</response>
        /// <response code="400">Requisição inválida.</response>
        [HttpPost("movimentar")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(MovimentacaoResponse))]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(MovimentacaoResponseExample))]

        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponse))]
        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(ErrorResponseExample))]
        public async Task<IActionResult> Movimentar([FromBody] MovimentarContaCorrenteCommand command)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _mediator.Send(command);
            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }
            return BadRequest(result.GetError());
        }
    }
}
