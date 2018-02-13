using System.Collections.Generic;
using System.Linq;
using Knn.BaseDeDados;
using Knn.Helper;

namespace Knn.Calculo.Formulas
{
    public class Jaccard : Distancia
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
                   (novaEntrada.Atributos.Length + entradaBaseDeDados.Atributos.Length - quantidadeElementosInterseccao)
                   .ToDouble();
        }

        #endregion
    }
}