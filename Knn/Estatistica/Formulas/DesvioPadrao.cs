using System;
using System.Collections.Generic;
using System.Linq;
using Knn.BaseDeDados;

namespace Knn.Estatistica.Formulas
{
    public class DesvioPadrao : Desempate
    {
        #region Métodos

        internal List<KeyValuePair<string, double>> Calcular(List<Voto> distancias, Dictionary<string, int> votos)
        {
            var desvioPadrao = new List<KeyValuePair<string, double>>();
            var variancia = new Variancia();
            var variancias = variancia.Calcular(distancias, votos);

            foreach (var classe in votos.Keys)
            {
                desvioPadrao.Add(new KeyValuePair<string, double>
                (
                    classe,
                    Math.Sqrt(variancias.First(x => x.Key == classe).Value)
                ));
            }

            return desvioPadrao;
        }

        internal override string Desempata(List<Voto> distancias, Dictionary<string, int> votos)
        {
            var desvioPadrao = Calcular(distancias, votos);

            return desvioPadrao.OrderBy(distancia => distancia.Value).First().Key;
        }

        #endregion
    }
}