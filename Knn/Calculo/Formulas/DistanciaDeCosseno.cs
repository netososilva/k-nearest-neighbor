using Knn.BaseDeDados;

namespace Knn.Calculo.Formulas
{
    public class DistanciaDeCosseno : Distancia
    {
        #region Métodos

        internal override double Calcular(Entrada novaEntrada, Entrada entradaBaseDeDados)
        {
            return 1 - new Cosseno().Calcular(novaEntrada, entradaBaseDeDados);
        }

        #endregion
    }
}