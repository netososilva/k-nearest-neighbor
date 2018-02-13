using System.Collections.Generic;
using System.Linq;
using Knn.BaseDeDados;
using Knn.Helper;

namespace Knn.Estatistica.Formulas
{
    public class Media : Desempate
    {
        #region Métodos

        internal double Calcular(double somatorio, int quantidade)
        {
            return somatorio / quantidade;
        }

        internal double Calcular(double somatorio, double quantidade)
        {
            return somatorio / quantidade;
        }

        internal List<KeyValuePair<string, double>> Calcular(List<Voto> distancias, Dictionary<string, int> votos)
        {
            var medias = new List<KeyValuePair<string, double>>();

            foreach (var classe in votos.Keys)
            {
                medias.Add(new KeyValuePair<string, double>
                (
                    classe,
                    Calcular(SomatorioDistancias(ref distancias, classe), votos[classe])
                ));
            }

            return medias;
        }

        internal override string Desempata(List<Voto> distancias, Dictionary<string, int> votos)
        {
            var medias = Calcular(distancias, votos);

            return medias.OrderBy(distancia => distancia.Value).First().Key;
        }

        #endregion

        #region Métodos Privados

        private double SomatorioDistancias(ref List<Voto> distancias, string classe)
        {
            return distancias.Where(distancia => distancia.Classe == classe).Sum(distancia => distancia.Distancia).ToDouble();
        }

        #endregion
    }
}