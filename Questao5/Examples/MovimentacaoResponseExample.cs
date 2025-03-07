using Questao5.Application.Commands.Responses;
using Questao5.Application.Queries.Responses;
using Swashbuckle.AspNetCore.Filters;

namespace Questao5.Examples
{
    public class MovimentacaoResponseExample : IExamplesProvider<MovimentacaoResponse>
    {
        public MovimentacaoResponse GetExamples()
        {
            return new MovimentacaoResponse
            {
                IdMovimento = "a6157647-2be6-42be-aacc-ed0a59add34a"

            };
        }


    }
}
