using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Questao5.Application.Commands.Requests;
using Questao5.Application.Commands.Responses;
using Questao5.Application.Queries.Requests;
using Questao5.Application.Queries.Responses;
using Questao5.Domain.Responses;
using Questao5.Infrastructure.Services.Controllers;
using Questao5.Model;
using Xunit;

namespace Questao5.Tests
{
    public class ContaCorrenteControllerTests
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly ContaCorrenteController _controller;

        public ContaCorrenteControllerTests()
        {
            _mediatorMock = new Mock<IMediator>();
            _controller = new ContaCorrenteController(_mediatorMock.Object);
        }

        [Fact]
        public async Task ConsultarSaldo_ComSucesso_DeveRetornarOk()
        {
            var query = new ConsultarSaldoQuery { IdContaCorrente = "123" };
            var result = Result<SaldoResponse>.Success(new SaldoResponse { Saldo = 100.00m });

            _mediatorMock.Setup(m => m.Send(query, It.IsAny<CancellationToken>())).ReturnsAsync(result);

            var response = await _controller.ConsultarSaldo(query);

            var okResult = Assert.IsType<OkObjectResult>(response);
            Assert.Equal(100.00m, ((SaldoResponse)okResult.Value).Saldo);
        }

        [Fact]
        public async Task ConsultarSaldo_ContaInvalida_DeveRetornarBadRequest()
        {
            var query = new ConsultarSaldoQuery { IdContaCorrente = "123" };
            var result = Result<SaldoResponse>.Fail("Conta corrente não encontrada.", "INVALID_ACCOUNT");

            _mediatorMock.Setup(m => m.Send(query, It.IsAny<CancellationToken>())).ReturnsAsync(result);

            var response = await _controller.ConsultarSaldo(query);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(response);
            var errorResponse = Assert.IsType<ErrorResponse>(badRequestResult.Value);
            Assert.Equal("INVALID_ACCOUNT", errorResponse.ErrorType);
            Assert.Equal("Conta corrente não encontrada.", errorResponse.Message);
        }

        [Fact]
        public async Task ConsultarSaldo_ContaInativa_DeveRetornarBadRequest()
        {
            var query = new ConsultarSaldoQuery { IdContaCorrente = "123" };
            var result = Result<SaldoResponse>.Fail("Conta corrente inativa.", "INACTIVE_ACCOUNT");

            _mediatorMock.Setup(m => m.Send(query, It.IsAny<CancellationToken>())).ReturnsAsync(result);

            var response = await _controller.ConsultarSaldo(query);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(response);
            var errorResponse = Assert.IsType<ErrorResponse>(badRequestResult.Value);
            Assert.Equal("INACTIVE_ACCOUNT", errorResponse.ErrorType);
            Assert.Equal("Conta corrente inativa.", errorResponse.Message);
        }

        [Fact]
        public async Task Movimentar_ComSucesso_DeveRetornarOk()
        {
            var command = new MovimentarContaCorrenteCommand
            {
                IdRequisicao = "123e4567-e89b-12d3-a456-426614174000",
                IdContaCorrente = "B6BAFC09-6967-ED11-A567-055DFA4A16C9",
                Valor = 100.00m,
                TipoMovimento = "C"
            };
            var result = Result<MovimentacaoResponse>.Success(new MovimentacaoResponse { IdMovimento = "456" });

            _mediatorMock.Setup(m => m.Send(command, It.IsAny<CancellationToken>())).ReturnsAsync(result);

            var response = await _controller.Movimentar(command);

            var okResult = Assert.IsType<OkObjectResult>(response);
            Assert.Equal("456", ((MovimentacaoResponse)okResult.Value).IdMovimento);
        }

        [Fact]
        public async Task Movimentar_ContaInvalida_DeveRetornarBadRequest()
        {
            var command = new MovimentarContaCorrenteCommand
            {
                IdRequisicao = "123e4567-e89b-12d3-a456-426614174000",
                IdContaCorrente = "B6BAFC09-6967-ED11-A567-055DFA4A16C9",
                Valor = 100.00m,
                TipoMovimento = "C"
            };
            var result = Result<MovimentacaoResponse>.Fail("Conta corrente não encontrada.", "INVALID_ACCOUNT");

            _mediatorMock.Setup(m => m.Send(command, It.IsAny<CancellationToken>())).ReturnsAsync(result);

            var response = await _controller.Movimentar(command);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(response);
            var errorResponse = Assert.IsType<ErrorResponse>(badRequestResult.Value);
            Assert.Equal("INVALID_ACCOUNT", errorResponse.ErrorType);
            Assert.Equal("Conta corrente não encontrada.", errorResponse.Message);
        }

        [Fact]
        public async Task Movimentar_ContaInativa_DeveRetornarBadRequest()
        {
            var command = new MovimentarContaCorrenteCommand
            {
                IdRequisicao = "123e4567-e89b-12d3-a456-426614174000",
                IdContaCorrente = "B6BAFC09-6967-ED11-A567-055DFA4A16C9",
                Valor = 100.00m,
                TipoMovimento = "C"
            };
            var result = Result<MovimentacaoResponse>.Fail("Conta corrente inativa.", "INACTIVE_ACCOUNT");

            _mediatorMock.Setup(m => m.Send(command, It.IsAny<CancellationToken>())).ReturnsAsync(result);

            var response = await _controller.Movimentar(command);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(response);
            var errorResponse = Assert.IsType<ErrorResponse>(badRequestResult.Value);
            Assert.Equal("INACTIVE_ACCOUNT", errorResponse.ErrorType);
            Assert.Equal("Conta corrente inativa.", errorResponse.Message);
        }

        [Fact]
        public async Task Movimentar_ValorInvalido_DeveRetornarBadRequest()
        {
            var command = new MovimentarContaCorrenteCommand
            {
                IdRequisicao = "123e4567-e89b-12d3-a456-426614174000",
                IdContaCorrente = "B6BAFC09-6967-ED11-A567-055DFA4A16C9",
                Valor = -100.00m,
                TipoMovimento = "C"
            };
            var result = Result<MovimentacaoResponse>.Fail("Valor inválido.", "INVALID_VALUE");

            _mediatorMock.Setup(m => m.Send(command, It.IsAny<CancellationToken>())).ReturnsAsync(result);

            var response = await _controller.Movimentar(command);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(response);
            var errorResponse = Assert.IsType<ErrorResponse>(badRequestResult.Value);
            Assert.Equal("INVALID_VALUE", errorResponse.ErrorType);
            Assert.Equal("Valor inválido.", errorResponse.Message);
        }

        [Fact]
        public async Task Movimentar_TipoMovimentoInvalido_DeveRetornarBadRequest()
        {
            var command = new MovimentarContaCorrenteCommand
            {
                IdRequisicao = "123e4567-e89b-12d3-a456-426614174000",
                IdContaCorrente = "B6BAFC09-6967-ED11-A567-055DFA4A16C9",
                Valor = 100.00m,
                TipoMovimento = "X"
            };
            var result = Result<MovimentacaoResponse>.Fail("Tipo de movimento inválido.", "INVALID_TYPE");

            _mediatorMock.Setup(m => m.Send(command, It.IsAny<CancellationToken>())).ReturnsAsync(result);

            var response = await _controller.Movimentar(command);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(response);
            var errorResponse = Assert.IsType<ErrorResponse>(badRequestResult.Value);
            Assert.Equal("INVALID_TYPE", errorResponse.ErrorType);
            Assert.Equal("Tipo de movimento inválido.", errorResponse.Message);
        }
    }
}