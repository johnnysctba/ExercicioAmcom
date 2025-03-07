using System;
using System.Globalization;

namespace Questao1
{
    class ContaBancaria
    {

        public int Numero { get; private set; }
        public string Titular { get; set; }
        public double Saldo { get; private set; }

        public ContaBancaria(int numero, string titular, double depositoInicial)
        {
            Numero = numero;
            Titular = titular;
            Deposito(depositoInicial);
        }

        public ContaBancaria(int numero, string titular)
        {
            Numero = numero;
            Titular = titular;
            Saldo = 0.0;
        }

        public void Deposito(double quantia)
        {
            if (quantia > 0)
            {
                Saldo += quantia;
            }
            else
            {
                throw new ArgumentException("A quantia para depósito deve ser maior que zero.");
            }
        }

        public void Saque(double quantia)
        {
            double taxaSaque = 3.50;
            if (quantia > 0)
            {
                Saldo -= (quantia + taxaSaque);
            }
            else
            {
                throw new ArgumentException("A quantia para saque deve ser maior que zero.");
            }
        }

        public override string ToString()
        {
            return $"Conta {Numero}, Titular: {Titular}, Saldo: $ {Saldo.ToString("F2", CultureInfo.InvariantCulture)}";
        }

    }
}
