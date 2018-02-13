using System;
using Knn.BaseDeDados;

namespace Knn.Calculo.Formulas
{
    public class DistanciaCorrelacional : Distancia
    {
        #region Métodos

        internal override double Calcular(Entrada novaEntrada, Entrada entradaBaseDeDados)
        {
            return Math.Sqrt(2 * (1 - new Correlacional().Calcular(novaEntrada, entradaBaseDeDados)));
        }
        
        #endregion
    }
}