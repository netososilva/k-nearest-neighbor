using Knn.BaseDeDados;

namespace Knn.Calculo.Formulas
{
    public class DistanciaDeJaccard : Distancia
    {
        #region Métodos

        internal override double Calcular(Entrada novaEntrada, Entrada entradaBaseDeDados)
        {
            return 1 - new Jaccard().Calcular(novaEntrada, entradaBaseDeDados);
        }

        #endregion
    }
}