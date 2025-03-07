namespace Questao5.Application.Queries.Responses
{
    /// <summary>
    /// Representa a resposta de uma consulta de saldo.
    /// </summary>
    public class SaldoResponse
    {
        /// <summary>
        /// Número da conta corrente.
        /// </summary>
        /// <example>123</example>
        public int NumeroConta { get; set; }

        /// <summary>
        /// Nome do titular da conta corrente.
        /// </summary>
        /// <example>Katherine Sanchez</example>
        public string NomeTitular { get; set; }

        /// <summary>
        /// Data e hora da consulta no formato "dd/MM/yyyy HH:mm:ss".
        /// </summary>
        /// <example>25/10/2023 14:30:45</example>
        public string DataHoraConsulta { get; set; }

        /// <summary>
        /// Saldo atual da conta corrente.
        /// </summary>
        /// <example>1500.75</example>
        public decimal Saldo { get; set; }
    }

}
