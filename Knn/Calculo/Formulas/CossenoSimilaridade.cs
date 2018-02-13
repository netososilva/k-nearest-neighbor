using System;
using System.Collections.Generic;
using System.Linq;
using Knn.BaseDeDados;
using Knn.Helper;

namespace Knn.Calculo.Formulas
{
    public class CossenoSimilaridade : Distancia
    {
        #region Métodos

        internal override double Calcular(Entrada novaEntrada, Entrada entradaBaseDeDados)
        {
            var interseccao = novaEntrada.Atributos.Intersect(entradaBaseDeDados.Atributos).ToList();

            return Formula(novaEntrada, entradaBaseDeDados, interseccao);
        }

        private double Formula(Entrada novaEntrada, Entrada entradaBaseDeDados, List<string> interseccao)
        {
            var quantidadeElementosInterseccao = interseccao.Count.ToDouble();

            return quantidadeElementosInterseccao /
                   Math.Sqrt(novaEntrada.Atributos.Length + entradaBaseDeDados.Atributos.Length).ToDouble();
        }

        #endregion
    }
}