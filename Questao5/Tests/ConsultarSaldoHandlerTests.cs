using Moq;
using Questao5.Application.Handlers;
using Questao5.Application.Queries.Requests;
using Questao5.Domain.Entities;
using Questao5.Infrastructure.IRepositories;
using Xunit;

namespace Questao5.Tests
{
    public class ConsultarSaldoHandlerTests
    {
        private readonly Mock<IContaCorrenteRepository> _contaCorrenteRepositoryMock;
        private readonly Mock<IMovimentoRepository> _movimentoRepositoryMock;
        private readonly ConsultarSaldoHandler _handler;

        public ConsultarSaldoHandlerTests()
        {
            _contaCorrenteRepositoryMock = new Mock<IContaCorrenteRepository>();
            _movimentoRepositoryMock = new Mock<IMovimentoRepository>();
            _handler = new ConsultarSaldoHandler(_contaCorrenteRepositoryMock.Object, _movimentoRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_DeveRetornarSaldoCorreto_QuandoContaExisteEAtiva()
        {
            var idContaCorrente = "B6BAFC09-6967-ED11-A567-055DFA4A16C9";
            var contaCorrente = new ContaCorrente
            {
                IdContaCorrente = idContaCorrente,
                Numero = 123,
                Nome = "Katherine Sanchez",
                Ativo = 1
            };

            var movimentos = new List<Movimento>
            {
                new Movimento { IdMovimento = Guid.NewGuid().ToString(), IdContaCorrente = idContaCorrente, TipoMovimento = "C", Valor = 1000 },
                new Movimento { IdMovimento = Guid.NewGuid().ToString(), IdContaCorrente = idContaCorrente, TipoMovimento = "D", Valor = 500 }
            };

            _contaCorrenteRepositoryMock
                .Setup(repo => repo.ObterPorIdAsync(idContaCorrente))
                .ReturnsAsync(contaCorrente);

            _movimentoRepositoryMock
                .Setup(repo => repo.ObterPorContaCorrenteAsync(idContaCorrente))
                .ReturnsAsync(movimentos);

            var query = new ConsultarSaldoQuery { IdContaCorrente = idContaCorrente };

            var result = await _handler.Handle(query, CancellationToken.None);

            Assert.True(result.IsSuccess);
            Assert.Equal(500, result.Value.Saldo);
            Assert.Equal(123, result.Value.NumeroConta);
            Assert.Equal("Katherine Sanchez", result.Value.NomeTitular);
        }

        [Fact]
        public async Task Handle_DeveRetornarErro_QuandoContaNaoExiste()
        {
            var idContaCorrente = "ID_INEXISTENTE";

            _contaCorrenteRepositoryMock
                .Setup(repo => repo.ObterPorIdAsync(idContaCorrente))
                .ReturnsAsync((ContaCorrente)null);

            var query = new ConsultarSaldoQuery { IdContaCorrente = idContaCorrente };

            var result = await _handler.Handle(query, CancellationToken.None);

            Assert.False(result.IsSuccess);
            Assert.Equal("Conta corrente não encontrada.", result.Error);
            Assert.Equal("INVALID_ACCOUNT", result.ErrorType);
        }

        [Fact]
        public async Task Handle_DeveRetornarErro_QuandoContaInativa()
        {
            var idContaCorrente = "B6BAFC09-6967-ED11-A567-055DFA4A16C9";
            var contaCorrente = new ContaCorrente
            {
                IdContaCorrente = idContaCorrente,
                Numero = 123,
                Nome = "Katherine Sanchez",
                Ativo = 0
            };

            _contaCorrenteRepositoryMock
                .Setup(repo => repo.ObterPorIdAsync(idContaCorrente))
                .ReturnsAsync(contaCorrente);

            var query = new ConsultarSaldoQuery { IdContaCorrente = idContaCorrente };

            var result = await _handler.Handle(query, CancellationToken.None);

            Assert.False(result.IsSuccess);
            Assert.Equal("Conta corrente inativa.", result.Error);
            Assert.Equal("INACTIVE_ACCOUNT", result.ErrorType);
        }

        [Fact]
        public async Task Handle_DeveRetornarSaldoZero_QuandoContaNaoTemMovimentos()
        {
            var idContaCorrente = "B6BAFC09-6967-ED11-A567-055DFA4A16C9";
            var contaCorrente = new ContaCorrente
            {
                IdContaCorrente = idContaCorrente,
                Numero = 123,
                Nome = "Katherine Sanchez",
                Ativo = 1
            };

            _contaCorrenteRepositoryMock
                .Setup(repo => repo.ObterPorIdAsync(idContaCorrente))
                .ReturnsAsync(contaCorrente);

            _movimentoRepositoryMock
                .Setup(repo => repo.ObterPorContaCorrenteAsync(idContaCorrente))
                .ReturnsAsync(new List<Movimento>());

            var query = new ConsultarSaldoQuery { IdContaCorrente = idContaCorrente };

            var result = await _handler.Handle(query, CancellationToken.None);

            Assert.True(result.IsSuccess);
            Assert.Equal(0, result.Value.Saldo);
        }

        [Fact]
        public async Task Handle_DeveRetornarSaldoCorreto_QuandoMovimentosTemValoresDecimais()
        {
            var idContaCorrente = "B6BAFC09-6967-ED11-A567-055DFA4A16C9";
            var contaCorrente = new ContaCorrente
            {
                IdContaCorrente = idContaCorrente,
                Numero = 123,
                Nome = "Katherine Sanchez",
                Ativo = 1
            };

            var movimentos = new List<Movimento>
    {
        new Movimento { IdMovimento = Guid.NewGuid().ToString(), IdContaCorrente = idContaCorrente, TipoMovimento = "C", Valor = 1000.75m },
        new Movimento { IdMovimento = Guid.NewGuid().ToString(), IdContaCorrente = idContaCorrente, TipoMovimento = "D", Valor = 500.25m }
    };

            _contaCorrenteRepositoryMock
                .Setup(repo => repo.ObterPorIdAsync(idContaCorrente))
                .ReturnsAsync(contaCorrente);

            _movimentoRepositoryMock
                .Setup(repo => repo.ObterPorContaCorrenteAsync(idContaCorrente))
                .ReturnsAsync(movimentos);

            var query = new ConsultarSaldoQuery { IdContaCorrente = idContaCorrente };

            var result = await _handler.Handle(query, CancellationToken.None);

            Assert.True(result.IsSuccess);
            Assert.Equal(500.50m, result.Value.Saldo);
        }

        [Fact]
        public async Task Handle_DeveRetornarErro_QuandoRepositorioLancaExcecao()
        {
            var idContaCorrente = "B6BAFC09-6967-ED11-A567-055DFA4A16C9";

            _contaCorrenteRepositoryMock
                .Setup(repo => repo.ObterPorIdAsync(idContaCorrente))
                .ThrowsAsync(new Exception("Erro de conexão com o banco de dados."));

            var query = new ConsultarSaldoQuery { IdContaCorrente = idContaCorrente };

            var result = await _handler.Handle(query, CancellationToken.None);

            Assert.False(result.IsSuccess);
            Assert.Equal("Erro ao consultar saldo.", result.Error);
            Assert.Equal("INVALID_PROCESS", result.ErrorType);
        }

        [Fact]
        public async Task Handle_DeveRetornarDataHoraNoFormatoCorreto()
        {
            var idContaCorrente = "B6BAFC09-6967-ED11-A567-055DFA4A16C9";
            var contaCorrente = new ContaCorrente
            {
                IdContaCorrente = idContaCorrente,
                Numero = 123,
                Nome = "Katherine Sanchez",
                Ativo = 1
            };

            _contaCorrenteRepositoryMock
                .Setup(repo => repo.ObterPorIdAsync(idContaCorrente))
                .ReturnsAsync(contaCorrente);

            _movimentoRepositoryMock
                .Setup(repo => repo.ObterPorContaCorrenteAsync(idContaCorrente))
                .ReturnsAsync(new List<Movimento>());

            var query = new ConsultarSaldoQuery { IdContaCorrente = idContaCorrente };

            var result = await _handler.Handle(query, CancellationToken.None);

            Assert.True(result.IsSuccess);
            Assert.Matches(@"\d{2}/\d{2}/\d{4} \d{2}:\d{2}:\d{2}", result.Value.DataHoraConsulta);
        }


    }
}
