namespace Questao5.Domain.Responses
{
    /// <summary>
    /// Representa uma resposta de erro.
    /// </summary>
    public class ErrorResponse
    {
        /// <summary>
        /// Tipo do erro (ex: INVALID_ACCOUNT, INACTIVE_ACCOUNT).
        /// </summary>
        /// <example>INVALID_ACCOUNT</example>
        public string ErrorType { get; set; } = string.Empty;

        /// <summary>
        /// Mensagem descritiva do erro.
        /// </summary>
        /// <example>A conta corrente informada não foi encontrada.</example>
        public string Message { get; set; } = string.Empty;
    }
}
