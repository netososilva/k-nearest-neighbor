using System;
using Knn.BaseDeDados;

namespace Knn.Calculo.Formulas
{
    public class Euclidiana : Distancia
    {
        #region Métodos

        internal override double Calcular(Entrada novaEntrada, Entrada entradaBaseDeDados)
        {
            var distancia = 0.0;
            var quantidadeDeAtributos = entradaBaseDeDados.Atributos.Length;

            for (var indice = 0; indice < quantidadeDeAtributos; indice++)
            {
                if (AtributoSaoNumericos(novaEntrada, entradaBaseDeDados, indice))
                    distancia += Formula(novaEntrada, entradaBaseDeDados, indice);
                else
                    distancia += Hamming.Formula(novaEntrada, entradaBaseDeDados, indice);
            }

            return Math.Sqrt(distancia);
        }

        private double Formula(Entrada novaEntrada, Entrada entradaBaseDeDados, int indice)
        {
            return Math.Pow(Helper.Calculo.Diferenca(novaEntrada.Atributos[indice], entradaBaseDeDados.Atributos[indice]), 2);
        }

        #endregion
    }
}