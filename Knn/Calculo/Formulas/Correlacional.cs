using System;
using System.Linq;
using Knn.BaseDeDados;
using Knn.Helper;

namespace Knn.Calculo.Formulas
{
    public class Correlacional : Distancia
    {
        #region Métodos

        internal override double Calcular(Entrada novaEntrada, Entrada entradaBaseDeDados)
        {
            var distancia = 0.0;
            var denominador = 0.0;
            var divisorNovaEntrada = 0.0;
            var divisorEntradaBaseDeDados = 0.0;
            var mediaNovaEntrada = CalculaMediaDeAtributos(novaEntrada);
            var mediaEntradaBaseDeDados = CalculaMediaDeAtributos(entradaBaseDeDados);

            var quantidadeDeAtributos = entradaBaseDeDados.Atributos.Length;

            for (var indice = 0; indice < quantidadeDeAtributos; indice++)
            {
                if (AtributoSaoNumericos(novaEntrada, entradaBaseDeDados, indice))
                {
                    denominador += CalculaDenominador(novaEntrada, entradaBaseDeDados, mediaNovaEntrada, mediaEntradaBaseDeDados, indice);
                    divisorNovaEntrada += CalculaDivisor(novaEntrada, mediaNovaEntrada, indice);
                    divisorEntradaBaseDeDados += CalculaDivisor(entradaBaseDeDados, mediaEntradaBaseDeDados, indice);
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

        private double CalculaDenominador(Entrada novaEntrada, Entrada entradaBaseDeDados, 
                                          double mediaNovaEntrada, double mediaEntradaBaseDeDados, int indice)
        {
            return Helper.Calculo.Diferenca(novaEntrada.Atributos[indice], mediaNovaEntrada) * 
                   Helper.Calculo.Diferenca(entradaBaseDeDados.Atributos[indice], mediaEntradaBaseDeDados);
        }

        private double CalculaDivisor(Entrada entrada, double mediaAtributos, int indice)
        {
            return Math.Pow(Helper.Calculo.Diferenca(entrada.Atributos[indice], mediaAtributos) , 2);
        }

        private double CalculaMediaDeAtributos(Entrada entrada)
        {
            return entrada.Atributos.Sum(atributo => Helper.Calculo.VerificaSeAtributoEhNumerico(atributo) ? atributo.ToDouble() : 0) / 
                   entrada.Atributos.Count(Helper.Calculo.VerificaSeAtributoEhNumerico);
        }

        #endregion
    }
}