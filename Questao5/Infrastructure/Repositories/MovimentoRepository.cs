using Dapper;
using Microsoft.Data.Sqlite;
using Questao5.Domain.Entities;
using Questao5.Infrastructure.IRepositories;
using Questao5.Infrastructure.Sqlite;
using System.Data;

namespace Questao5.Infrastructure.Repositories
{
    public class MovimentoRepository : IMovimentoRepository
    {
        private readonly string _connectionString;

        public MovimentoRepository(DatabaseConfig databaseConfig)
        {
            _connectionString = databaseConfig.Name;
        }

        public async Task AdicionarAsync(Movimento movimento)
        {
            using IDbConnection connection = new SqliteConnection(_connectionString);
            var query = "INSERT INTO movimento (idmovimento, idcontacorrente, datamovimento, tipomovimento, valor) " +
                        "VALUES (@IdMovimento, @IdContaCorrente, @DataMovimento, @TipoMovimento, @Valor)";
            await connection.ExecuteAsync(query, movimento);
        }

        public async Task<IEnumerable<Movimento>> ObterPorContaCorrenteAsync(string idContaCorrente)
        {
            using IDbConnection connection = new SqliteConnection(_connectionString);
            var query = "SELECT * FROM movimento WHERE idcontacorrente = @IdContaCorrente";
            return await connection.QueryAsync<Movimento>(query, new { IdContaCorrente = idContaCorrente });
        }
    }
}
