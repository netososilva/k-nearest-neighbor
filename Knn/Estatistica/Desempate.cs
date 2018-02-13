using System.Collections.Generic;
using Knn.BaseDeDados;

namespace Knn.Estatistica
{
    public abstract class Desempate
    {
        internal abstract string Desempata(List<Voto> distancias, Dictionary<string, int> votos);
    }
}