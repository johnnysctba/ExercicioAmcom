using Questao5.Domain.Entities;

namespace Questao5.Infrastructure.IRepositories
{
    public interface IIdempotenciaRepository
    {
        Task<Idempotencia> ObterPorChaveAsync(string chave);
        Task AdicionarAsync(Idempotencia idempotencia);
    }
}
