using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Knn.BaseDeDados;

namespace Knn.Calculo
{
    public abstract class Distancia
    {
        #region Métodos Abstratos

        internal abstract double Calcular(Entrada novaEntrada, Entrada entradaBaseDeDados);

        #endregion

        #region Métodos

        internal bool AtributoSaoNumericos(Entrada novaEntrada, Entrada entradaBaseDeDados, int indice)
        {
            return Helper.Calculo.VerificaSeAtributoEhNumerico(novaEntrada.Atributos[indice])
                   && Helper.Calculo.VerificaSeAtributoEhNumerico(entradaBaseDeDados.Atributos[indice]);
        }

        internal List<Voto> Calcular(Entrada entrada, Base baseDeTreinamento, int k)
        {
            var distancias = new Voto[baseDeTreinamento.Dados.Count];

            Parallel.For(0, baseDeTreinamento.Dados.Count, indice =>
            {
                distancias[indice] = new Voto
                {
                    Classe = baseDeTreinamento.Dados[indice].Classe,
                    Distancia = Calcular(entrada, baseDeTreinamento.Dados[indice])
                };
            });

            var kVizinhosMaisProximos = distancias.OrderBy(x => x.Distancia).Take(k).ToList();

            return kVizinhosMaisProximos;
        }

        #endregion
    }
}