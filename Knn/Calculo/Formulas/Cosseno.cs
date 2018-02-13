using System;
using Knn.BaseDeDados;
using Knn.Helper;

namespace Knn.Calculo.Formulas
{
    public class Cosseno : Distancia
    {
        #region Métodos

        internal override double Calcular(Entrada novaEntrada, Entrada entradaBaseDeDados)
        {
            var distancia = 0.0;
            var denominador = 0.0;
            var divisorNovaEntrada = 0.0;
            var divisorEntradaBaseDeDados = 0.0;

            var quantidadeDeAtributos = entradaBaseDeDados.Atributos.Length;

            for (var indice = 0; indice < quantidadeDeAtributos; indice++)
            {
                if (AtributoSaoNumericos(novaEntrada, entradaBaseDeDados, indice))
                {
                    denominador += CalculaDenominador(novaEntrada, entradaBaseDeDados, indice);
                    divisorNovaEntrada += CalculaDivisor(novaEntrada, indice);
                    divisorEntradaBaseDeDados += CalculaDivisor(entradaBaseDeDados, indice);
                }
                else
                    distancia += Hamming.Formula(novaEntrada, entradaBaseDeDados, indice);
            }

            return Formula(denominador, divisorEntradaBaseDeDados, divisorNovaEntrada, distancia);
        }

        private double Formula(double denominador, double divisorEntradaBaseDeDados, double divisorNovaEntrada, double distancia)
        {
            return denominador / (Math.Sqrt(divisorEntradaBaseDeDados) * Math.Sqrt(divisorNovaEntrada)) + distancia;
        }

        private double CalculaDenominador(Entrada novaEntrada, Entrada entradaBaseDeDados, int indice)
        {
            return novaEntrada.Atributos[indice].ToDouble() * entradaBaseDeDados.Atributos[indice].ToDouble();
        }

        private double CalculaDivisor(Entrada entrada, int indice)
        {
            return Math.Pow(entrada.Atributos[indice].ToDouble(), 2);
        }

        #endregion
    }
}