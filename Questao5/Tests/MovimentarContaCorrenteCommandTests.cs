using Moq;
using Newtonsoft.Json;
using Questao5.Application.Commands.Requests;
using Questao5.Application.Commands.Responses;
using Questao5.Domain.Entities;
using Questao5.Infrastructure.IRepositories;
using Xunit;

namespace Questao5.Tests
{
    public class MovimentarContaCorrenteCommandTests
    {
        private readonly Mock<IContaCorrenteRepository> _contaCorrenteRepositoryMock;
        private readonly Mock<IMovimentoRepository> _movimentoRepositoryMock;
        private readonly Mock<IIdempotenciaRepository> _idempotenciaRepositoryMock;
        private readonly MovimentarContaCorrenteHandler _handler;

        public MovimentarContaCorrenteCommandTests()
        {
            _contaCorrenteRepositoryMock = new Mock<IContaCorrenteRepository>();
            _movimentoRepositoryMock = new Mock<IMovimentoRepository>();
            _idempotenciaRepositoryMock = new Mock<IIdempotenciaRepository>();
            _handler = new MovimentarContaCorrenteHandler(
                _contaCorrenteRepositoryMock.Object,
                _movimentoRepositoryMock.Object,
                _idempotenciaRepositoryMock.Object
            );
        }

        [Fact]
        public async Task Handle_ValidCommand_ReturnsSuccess()
        {
            var command = new MovimentarContaCorrenteCommand
            {
                IdRequisicao = Guid.NewGuid().ToString(),
                IdContaCorrente = "B6BAFC09-6967-ED11-A567-055DFA4A16C9",
                Valor = 100.00m,
                TipoMovimento = "C"
            };

            var contaCorrente = new ContaCorrente
            {
                IdContaCorrente = command.IdContaCorrente,
                Numero = 123,
                Nome = "João Silva",
                Ativo = 1
            };

            _idempotenciaRepositoryMock.Setup(repo => repo.ObterPorChaveAsync(command.IdRequisicao))
                .ReturnsAsync((Idempotencia)null);

            _contaCorrenteRepositoryMock.Setup(repo => repo.ObterPorIdAsync(command.IdContaCorrente))
                .ReturnsAsync(contaCorrente);

            _movimentoRepositoryMock.Setup(repo => repo.AdicionarAsync(It.IsAny<Movimento>()))
                .Returns(Task.CompletedTask);

            _idempotenciaRepositoryMock.Setup(repo => repo.AdicionarAsync(It.IsAny<Idempotencia>()))
                .Returns(Task.CompletedTask);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
        }

        [Fact]
        public async Task Handle_InvalidAccount_ReturnsFailure()
        {
            var command = new MovimentarContaCorrenteCommand
            {
                IdRequisicao = Guid.NewGuid().ToString(),
                IdContaCorrente = "INVALID_ACCOUNT_ID",
                Valor = 100.00m,
                TipoMovimento = "C"
            };

            _idempotenciaRepositoryMock.Setup(repo => repo.ObterPorChaveAsync(command.IdRequisicao))
                .ReturnsAsync((Idempotencia)null);

            _contaCorrenteRepositoryMock.Setup(repo => repo.ObterPorIdAsync(command.IdContaCorrente))
                .ReturnsAsync((ContaCorrente)null);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.False(result.IsSuccess);
            Assert.Equal("Conta corrente não encontrada.", result.Error);
            Assert.Equal("INVALID_ACCOUNT", result.ErrorType);
        }

        [Fact]
        public async Task Handle_InactiveAccount_ReturnsFailure()
        {
            var command = new MovimentarContaCorrenteCommand
            {
                IdRequisicao = Guid.NewGuid().ToString(),
                IdContaCorrente = "B6BAFC09-6967-ED11-A567-055DFA4A16C9",
                Valor = 100.00m,
                TipoMovimento = "C"
            };

            var contaCorrente = new ContaCorrente
            {
                IdContaCorrente = command.IdContaCorrente,
                Numero = 123,
                Nome = "João Silva",
                Ativo = 0
            };

            _idempotenciaRepositoryMock.Setup(repo => repo.ObterPorChaveAsync(command.IdRequisicao))
                .ReturnsAsync((Idempotencia)null);

            _contaCorrenteRepositoryMock.Setup(repo => repo.ObterPorIdAsync(command.IdContaCorrente))
                .ReturnsAsync(contaCorrente);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.False(result.IsSuccess);
            Assert.Equal("Conta corrente inativa.", result.Error);
            Assert.Equal("INACTIVE_ACCOUNT", result.ErrorType);
        }

        [Fact]
        public async Task Handle_InvalidValue_ReturnsFailure()
        {
            var command = new MovimentarContaCorrenteCommand
            {
                IdRequisicao = Guid.NewGuid().ToString(),
                IdContaCorrente = "B6BAFC09-6967-ED11-A567-055DFA4A16C9",
                Valor = 0,
                TipoMovimento = "C"
            };

            var contaCorrente = new ContaCorrente
            {
                IdContaCorrente = command.IdContaCorrente,
                Numero = 123,
                Nome = "João Silva",
                Ativo = 1
            };

            _idempotenciaRepositoryMock.Setup(repo => repo.ObterPorChaveAsync(command.IdRequisicao))
                .ReturnsAsync((Idempotencia)null);

            _contaCorrenteRepositoryMock.Setup(repo => repo.ObterPorIdAsync(command.IdContaCorrente))
                .ReturnsAsync(contaCorrente);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.False(result.IsSuccess);
            Assert.Equal("Valor inválido.", result.Error);
            Assert.Equal("INVALID_VALUE", result.ErrorType);
        }

        [Fact]
        public async Task Handle_InvalidMovementType_ReturnsFailure()
        {
            var command = new MovimentarContaCorrenteCommand
            {
                IdRequisicao = Guid.NewGuid().ToString(),
                IdContaCorrente = "B6BAFC09-6967-ED11-A567-055DFA4A16C9",
                Valor = 100.00m,
                TipoMovimento = "X"
            };

            var contaCorrente = new ContaCorrente
            {
                IdContaCorrente = command.IdContaCorrente,
                Numero = 123,
                Nome = "João Silva",
                Ativo = 1
            };

            _idempotenciaRepositoryMock.Setup(repo => repo.ObterPorChaveAsync(command.IdRequisicao))
                .ReturnsAsync((Idempotencia)null);

            _contaCorrenteRepositoryMock.Setup(repo => repo.ObterPorIdAsync(command.IdContaCorrente))
                .ReturnsAsync(contaCorrente);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.False(result.IsSuccess);
            Assert.Equal("Tipo de movimento inválido.", result.Error);
            Assert.Equal("INVALID_TYPE", result.ErrorType);
        }

        [Fact]
        public async Task Handle_Idempotency_ReturnsPreviousResult()
        {
            var command = new MovimentarContaCorrenteCommand
            {
                IdRequisicao = Guid.NewGuid().ToString(),
                IdContaCorrente = "B6BAFC09-6967-ED11-A567-055DFA4A16C9",
                Valor = 100.00m,
                TipoMovimento = "C"
            };

            var previousResponse = new MovimentacaoResponse { IdMovimento = "PREVIOUS_MOVEMENT_ID" };
            var idempotencia = new Idempotencia
            {
                ChaveIdempotencia = command.IdRequisicao,
                Requisicao = JsonConvert.SerializeObject(command),
                Resultado = JsonConvert.SerializeObject(previousResponse)
            };

            _idempotenciaRepositoryMock.Setup(repo => repo.ObterPorChaveAsync(command.IdRequisicao))
                .ReturnsAsync(idempotencia);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.True(result.IsSuccess);
            Assert.Equal(previousResponse.IdMovimento, result.Value.IdMovimento);
        }
    }
}
