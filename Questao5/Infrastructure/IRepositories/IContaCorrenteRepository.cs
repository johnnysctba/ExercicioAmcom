using Questao5.Domain.Entities;

namespace Questao5.Infrastructure.IRepositories
{
    public interface IContaCorrenteRepository
    {
        Task<ContaCorrente> ObterPorIdAsync(string id);
        Task<IEnumerable<ContaCorrente>> ObterTodosAsync();
    }
}
