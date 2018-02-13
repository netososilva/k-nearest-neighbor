using System;

namespace Knn.Helper
{
    internal static class Calculo
    {
        #region Métodos

        internal static double Diferenca(object atributo1, object atributo2)
        {
            return atributo1.ToDouble() - atributo2.ToDouble();
        }

        internal static double Soma(object atributo1, object atributo2)
        {
            return atributo1.ToDouble() + atributo2.ToDouble();
        }

        internal static double SomaComModulo(object atributo1, object atributo2)
        {
            return Math.Abs(atributo1.ToDouble()) + Math.Abs(atributo2.ToDouble());
        }

        internal static bool VerificaSeAtributoEhNumerico(object atributo)
        {
            return double.TryParse(atributo.ToString(), out var numeroTemporario);
        }

        #endregion
    }
}