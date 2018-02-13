using Knn.BaseDeDados;

namespace Knn.Calculo.Formulas
{
    public class DistanciaDeCossenoSimilaridade : Distancia
    {
        #region Métodos

        internal override double Calcular(Entrada novaEntrada, Entrada entradaBaseDeDados)
        {
            return 1 - new CossenoSimilaridade().Calcular(novaEntrada, entradaBaseDeDados);
        }

        #endregion
    }
}