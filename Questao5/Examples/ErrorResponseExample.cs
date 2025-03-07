using Questao5.Domain.Responses;
using Swashbuckle.AspNetCore.Filters;

namespace Questao5.Examples
{
    public class ErrorResponseExample : IExamplesProvider<ErrorResponse>
    {
        public ErrorResponse GetExamples()
        {
            return new ErrorResponse
            {
                ErrorType = "INVALID_ACCOUNT",
                Message = "A conta corrente informada não foi encontrada."
            };
        }
    }
}
