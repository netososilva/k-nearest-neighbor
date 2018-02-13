using System;
using Knn.BaseDeDados;

namespace Knn.Calculo.Formulas
{
    public class BrayCurtis : Distancia
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

            return distancia;
        }

        private double Formula(Entrada novaEntrada, Entrada entradaBaseDeDados, int indice)
        {
            return Math.Abs(Helper.Calculo.Diferenca(novaEntrada.Atributos[indice], entradaBaseDeDados.Atributos[indice])) /
                   Math.Abs(Helper.Calculo.Diferenca(novaEntrada.Atributos[indice], entradaBaseDeDados.Atributos[indice]));
        }

        #endregion
    }
}