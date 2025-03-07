using Dapper;
using Microsoft.Data.Sqlite;
using Questao5.Domain.Entities;
using Questao5.Infrastructure.IRepositories;
using Questao5.Infrastructure.Sqlite;
using System.Data;

namespace Questao5.Infrastructure.Repositories
{
    public class IdempotenciaRepository : IIdempotenciaRepository
    {
        private readonly string _connectionString;

        public IdempotenciaRepository(DatabaseConfig databaseConfig)
        {
            _connectionString = databaseConfig.Name;
        }

        public async Task<Idempotencia> ObterPorChaveAsync(string chave)
        {
            using IDbConnection connection = new SqliteConnection(_connectionString);
            var query = "SELECT * FROM idempotencia WHERE chave_idempotencia = @Chave";
            return await connection.QueryFirstOrDefaultAsync<Idempotencia>(query, new { Chave = chave });
        }

        public async Task AdicionarAsync(Idempotencia idempotencia)
        {
            using IDbConnection connection = new SqliteConnection(_connectionString);
            var query = "INSERT INTO idempotencia (chave_idempotencia, requisicao, resultado) " +
                        "VALUES (@ChaveIdempotencia, @Requisicao, @Resultado)";
            await connection.ExecuteAsync(query, idempotencia);
        }
    }
}
