using Questao5.Domain.Entities;

namespace Questao5.Infrastructure.IRepositories
{
    public interface IMovimentoRepository
    {
        Task AdicionarAsync(Movimento movimento);
        Task<IEnumerable<Movimento>> ObterPorContaCorrenteAsync(string idContaCorrente);
    }
}
