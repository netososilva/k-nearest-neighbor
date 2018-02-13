using System;
using Knn.BaseDeDados;

namespace Knn.Calculo.Formulas
{
    public class Minkowski : Distancia
    {
        #region Atributos

        private readonly int _q;

        #endregion

        #region Construtor

        public Minkowski(int q)
        {
            _q = q;
        }

        #endregion

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

            return Math.Pow(distancia, 1.0 / _q);
        }

        private double Formula(Entrada novaEntrada, Entrada entradaBaseDeDados, int indice)
        {
            return Math.Pow(Math.Abs(Helper.Calculo.Diferenca(novaEntrada.Atributos[indice], entradaBaseDeDados.Atributos[indice])), _q);
        }

        #endregion
    }
}
