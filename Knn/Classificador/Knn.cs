using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Knn.BaseDeDados;
using Knn.Calculo;
using Knn.Estatistica;

namespace Knn.Classificador
{
    public class Knn
    {
        #region Propriedades

        public Base BaseDeDados { get; set; }
        public MatrizDeConfusao MatrizDeConfusao { get; set; }
        public int MelhorK { get; set; }
        public TimeSpan TempoDeExecucao { get; set; }

        #endregion

        #region Métodos Públicos

        public string PreveClasse(Entrada entrada, Distancia metodoDeDistancia, Desempate metodoDeDesempate, int k)
        {
            var classePrevista = PreveClasse(entrada, metodoDeDistancia, metodoDeDesempate, BaseDeDados, k);

            return classePrevista;
        }

        public void Treina(Base treinamento, Base testes, Distancia metodoDeDistancia, Desempate metodoDeDesempate)
        {
            var kMaximo = Math.Log(10, 2);
            var melhorK = 0;

            BaseDeDados = treinamento.Zip(testes);

            for (var k = 3; k <= kMaximo; k++)
            {
                var tempo = new Stopwatch();
                tempo.Start();

                var matriz = ClassificaBaseDeDados(treinamento, testes, metodoDeDistancia, metodoDeDesempate, k);

                tempo.Stop();

                if (MatrizDeConfusao == null || matriz.Acuracia() > MatrizDeConfusao.Acuracia())
                {
                    MelhorK = k;
                    MatrizDeConfusao = matriz;
                    TempoDeExecucao = tempo.Elapsed;
                }
            }
        }

        public void Treina(Base treinamento, Base testes, Distancia metodoDeDistancia, Desempate metodoDeDesempate, int k)
        {
            BaseDeDados = treinamento.Zip(testes);
            MelhorK = k;

            var tempo = new Stopwatch();
            tempo.Start();

            MatrizDeConfusao = ClassificaBaseDeDados(treinamento, testes, metodoDeDistancia, metodoDeDesempate, k);

            tempo.Stop();
            TempoDeExecucao = tempo.Elapsed;
        }

        #endregion

        #region Métodos Privados

        private bool AcertouPrevisao(string classePrevista, string classe)
        {
            return classePrevista.Equals(classe);
        }

        private MatrizDeConfusao ClassificaBaseDeDados(Base treinamento, Base testes, Distancia metodoDeDistancia, Desempate metodoDeDesempate, int k)
        {
            var matriz = new MatrizDeConfusao(treinamento.Classes);
            
            foreach (var entrada in testes.Dados)
            {
                var classePrevista = PreveClasse(entrada, metodoDeDistancia, metodoDeDesempate, k);

                if (AcertouPrevisao(classePrevista, entrada.Classe))
                    matriz.AcertouPrevisao(classePrevista);
                else
                    matriz.ErrouPrevisao(classePrevista, entrada.Classe);
            }

            return matriz;
        }

        private bool Empate(List<KeyValuePair<string, int>> votos)
        {
            return votos[0].Value == votos[1].Value;
        }

        private bool MaisDeUmaClasseMaisProxima(Dictionary<string, int> votos)
        {
            return votos.Count > 1;
        }

        private string PreveClasse(Entrada entrada, Distancia distancia, Desempate desempate, Base baseDeTreinamento, int k)
        {
            var distancias = distancia.Calcular(entrada, baseDeTreinamento, k);
            var votos = new Dictionary<string, int>();

            foreach (var voto in distancias)
            {
                if (votos.ContainsKey(voto.Classe))
                    votos[voto.Classe] += 1;
                else
                    votos.Add(voto.Classe, 1);
            }

            if (!MaisDeUmaClasseMaisProxima(votos))
                return votos.First().Key;

            var votosOrdenados = votos.OrderByDescending(x => x.Value).ToList();

            if (!Empate(votosOrdenados))
                return votosOrdenados[0].Key;

            return desempate.Desempata(distancias, votos);
        }
       
        #endregion
    }
}