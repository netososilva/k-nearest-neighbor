using System.Collections.Generic;
using System.Linq;
using Knn.BaseDeDados;
using Knn.Helper;

namespace Knn.Estatistica.Formulas
{
    public class Variancia : Desempate
    {
        #region Métodos

        internal List<KeyValuePair<string, double>> Calcular(List<Voto> distancias, Dictionary<string, int> votos)
        {
            var variancias = new List<KeyValuePair<string, double>>();
            var media = new Media();
            var medias = media.Calcular(distancias, votos);

            foreach (var classe in votos.Keys)
            {
                variancias.Add(new KeyValuePair<string, double>
                (
                    classe,
                    media.Calcular(SomatorioDistancias(ref distancias, classe), medias.First(x => x.Key == classe).Value)
                ));
            }

            return variancias;
        }

        internal override string Desempata(List<Voto> distancias, Dictionary<string, int> votos)
        {
            var variancias = Calcular(distancias, votos);

            return variancias.OrderBy(distancia => distancia.Value).First().Key;
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