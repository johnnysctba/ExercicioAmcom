using MediatR;
using Newtonsoft.Json;
using Questao5.Application.Commands.Requests;
using Questao5.Application.Commands.Responses;
using Questao5.Domain.Entities;
using Questao5.Infrastructure.IRepositories;
using Questao5.Model;
public class MovimentarContaCorrenteHandler : IRequestHandler<MovimentarContaCorrenteCommand, Result<MovimentacaoResponse>>
{
    private readonly IContaCorrenteRepository _contaCorrenteRepository;
    private readonly IMovimentoRepository _movimentoRepository;
    private readonly IIdempotenciaRepository _idempotenciaRepository;

    public MovimentarContaCorrenteHandler(IContaCorrenteRepository contaCorrenteRepository, IMovimentoRepository movimentoRepository, IIdempotenciaRepository idempotenciaRepository)
    {
        _contaCorrenteRepository = contaCorrenteRepository;
        _movimentoRepository = movimentoRepository;
        _idempotenciaRepository = idempotenciaRepository;
    }

    public async Task<Result<MovimentacaoResponse>> Handle(MovimentarContaCorrenteCommand request, CancellationToken cancellationToken)
    {
        var idempotencia = await _idempotenciaRepository.ObterPorChaveAsync(request.IdRequisicao);
        if (idempotencia != null)
        {
            return Result<MovimentacaoResponse>.Success(JsonConvert.DeserializeObject<MovimentacaoResponse>(idempotencia.Resultado));
        }

        var contaCorrente = await _contaCorrenteRepository.ObterPorIdAsync(request.IdContaCorrente);
        if (contaCorrente == null)
        {
            return Result<MovimentacaoResponse>.Fail("Conta corrente não encontrada.", "INVALID_ACCOUNT");
        }

        if (contaCorrente.Ativo == 0)
        {
            return Result<MovimentacaoResponse>.Fail("Conta corrente inativa.", "INACTIVE_ACCOUNT");
        }

        if (request.Valor <= 0)
        {
            return Result<MovimentacaoResponse>.Fail("Valor inválido.", "INVALID_VALUE");
        }

        if (request.TipoMovimento != "C" && request.TipoMovimento != "D")
        {
            return Result<MovimentacaoResponse>.Fail("Tipo de movimento inválido.", "INVALID_TYPE");
        }

        var movimento = new Movimento
        {
            IdMovimento = Guid.NewGuid().ToString(),
            IdContaCorrente = request.IdContaCorrente,
            DataMovimento = DateTime.Now.ToString("dd/MM/yyyy"),
            TipoMovimento = request.TipoMovimento,
            Valor = request.Valor
        };

        await _movimentoRepository.AdicionarAsync(movimento);

        var idempotenciaResponse = new MovimentacaoResponse { IdMovimento = movimento.IdMovimento };
        await _idempotenciaRepository.AdicionarAsync(new Idempotencia
        {
            ChaveIdempotencia = request.IdRequisicao,
            Requisicao = JsonConvert.SerializeObject(request),
            Resultado = JsonConvert.SerializeObject(idempotenciaResponse)
        });

        return Result<MovimentacaoResponse>.Success(idempotenciaResponse);
    }
}