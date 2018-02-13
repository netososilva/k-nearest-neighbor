using Knn.BaseDeDados;

namespace Knn.Calculo.Formulas
{
    public class Hamming : Distancia
    {
        #region Métodos

        internal override double Calcular(Entrada novaEntrada, Entrada entradaBaseDeDados)
        {
            double distancia = 0;
            var quantidadeDeAtributos = entradaBaseDeDados.Atributos.Length;

            for (var indice = 0; indice < quantidadeDeAtributos; indice++)
            {
                distancia += Formula(novaEntrada, entradaBaseDeDados, indice);
            }

            return distancia;
        }

        internal static double Formula(Entrada novaEntrada, Entrada entradaBaseDeDados, int indice)
        {
            return novaEntrada.Atributos[indice].Equals(entradaBaseDeDados.Atributos[indice]) ? 0 : 1;
        }

        #endregion
    }
}