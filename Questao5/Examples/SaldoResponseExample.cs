using Questao5.Application.Queries.Responses;
using Swashbuckle.AspNetCore.Filters;

namespace Questao5.Examples
{
    public class SaldoResponseExample : IExamplesProvider<SaldoResponse>
    {
        public SaldoResponse GetExamples()
        {
            return new SaldoResponse
            {
                NumeroConta = 123,
                NomeTitular = "Katherine Sanchez",
                DataHoraConsulta = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"),
                Saldo = 1500.75m
            };
        }
    }
}
