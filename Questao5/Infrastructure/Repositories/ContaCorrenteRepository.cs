using Dapper;
using Microsoft.Data.Sqlite;
using Questao5.Domain.Entities;
using Questao5.Infrastructure.IRepositories;
using Questao5.Infrastructure.Sqlite;
using System.Data;
namespace Questao5.Infrastructure.Repositories
{
    public class ContaCorrenteRepository : IContaCorrenteRepository
    {
        private readonly string _connectionString;

        public ContaCorrenteRepository(DatabaseConfig databaseConfig)
        {
            _connectionString = databaseConfig.Name;
        }

        public async Task<ContaCorrente> ObterPorIdAsync(string id)
        {
            using IDbConnection connection = new SqliteConnection(_connectionString);
            var query = "SELECT * FROM contacorrente WHERE idcontacorrente = @Id";
            return await connection.QueryFirstOrDefaultAsync<ContaCorrente>(query, new { Id = id });
        }

        public async Task<IEnumerable<ContaCorrente>> ObterTodosAsync()
        {
            using IDbConnection connection = new SqliteConnection(_connectionString);
            var query = "SELECT * FROM contacorrente";
            return await connection.QueryAsync<ContaCorrente>(query);
        }
    }


}
