using MediatR;
using Questao5.Application.Queries.Requests;
using Questao5.Application.Queries.Responses;
using Questao5.Infrastructure.IRepositories;
using Questao5.Model;

namespace Questao5.Application.Handlers
{
    public class ConsultarSaldoHandler : IRequestHandler<ConsultarSaldoQuery, Result<SaldoResponse>>
    {
        private readonly IContaCorrenteRepository _contaCorrenteRepository;
        private readonly IMovimentoRepository _movimentoRepository;

        public ConsultarSaldoHandler(IContaCorrenteRepository contaCorrenteRepository, IMovimentoRepository movimentoRepository)
        {
            _contaCorrenteRepository = contaCorrenteRepository;
            _movimentoRepository = movimentoRepository;
        }

        public async Task<Result<SaldoResponse>> Handle(ConsultarSaldoQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var contaCorrente = await _contaCorrenteRepository.ObterPorIdAsync(request.IdContaCorrente);
                if (contaCorrente == null)
                {
                    return Result<SaldoResponse>.Fail("Conta corrente não encontrada.", "INVALID_ACCOUNT");
                }

                if (contaCorrente.Ativo == 0)
                {
                    return Result<SaldoResponse>.Fail("Conta corrente inativa.", "INACTIVE_ACCOUNT");
                }

                var movimentos = await _movimentoRepository.ObterPorContaCorrenteAsync(request.IdContaCorrente);
                var saldo = movimentos.Where(m => m.TipoMovimento == "C").Sum(m => m.Valor) -
                            movimentos.Where(m => m.TipoMovimento == "D").Sum(m => m.Valor);

                var response = new SaldoResponse
                {
                    NumeroConta = contaCorrente.Numero,
                    NomeTitular = contaCorrente.Nome,
                    DataHoraConsulta = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"),
                    Saldo = saldo
                };

                return Result<SaldoResponse>.Success(response);
            }
            catch (Exception ex)
            {
                return Result<SaldoResponse>.Fail("Erro ao consultar saldo.", "INVALID_PROCESS");

            }
        }
    }
}
