using System;
using System.Collections.Generic;
using System.Linq;
using Knn.Helper;

namespace Knn
{
    public class MatrizDeConfusao
    {
        #region Propriedades

        private List<string> Classes { get; set; }
        private int[,] Classificacoes { get; set; }

        #endregion

        #region Construtor

        public MatrizDeConfusao(Dictionary<string, int> classes)
        {
            Classes = classes.Keys.ToList();
            Classificacoes = new int[classes.Count,classes.Count];
        }

        #endregion

        #region Métodos Públicos

        public void AcertouPrevisao(string classe)
        {
            var index = Classes.IndexOf(classe);

            Classificacoes[index, index] += 1;
        }
        public void ErrouPrevisao(string classePrevista, string classeCorreta)
        {
            var indexClassePrevista = Classes.IndexOf(classePrevista);
            var indexClasseCorreta = Classes.IndexOf(classeCorreta);

            Classificacoes[indexClasseCorreta, indexClassePrevista] += 1;
        }
        /// <summary>
        /// Matriz de confusão
        /// </summary>
        /// <returns></returns>
        public int[,] Matriz()
        {
            return Classificacoes;
        }
        /// <summary>
        /// Matriz de confusão analisando uma classe
        /// </summary>
        /// <param name="classe"></param>
        public int[,] MatrizPorClasse(string classe)
        {
            return new [,]
            {
                { VerdadeirosPositivos(classe), FalsosNegativos(classe) },
                { FalsosPositivos(classe), VerdadeirosNegativos(classe) }
            };
        }

        /// <summary>
        /// Pertencem a classe, mas foram previstos como pertencentes a outra classe
        /// </summary>
        /// <param name="classe"></param>
        /// <returns></returns>
        public int FalsosNegativos(string classe)
        {
            var classeIndex = Classes.IndexOf(classe);
            var falsosNegativos = 0;

            for (var i = 0; i < Classes.Count; i++)
            {
                if (i != classeIndex)
                    falsosNegativos += Classificacoes[classeIndex, i];
            }

            return falsosNegativos;
        }
        /// <summary>
        /// Foram previstos como pertencentes a uma classe que não pertencem
        /// </summary>
        /// <param name="classe"></param>
        /// <returns></returns>
        public int FalsosPositivos(string classe)
        {
            var classeIndex = Classes.IndexOf(classe);
            var falsosPositivos = 0;

            for (var i = 0; i < Classes.Count; i++)
            {
                if (i != classeIndex)
                    falsosPositivos += Classificacoes[classeIndex, i];
            }

            return falsosPositivos;
        }
        /// <summary>
        /// Foram previstos como não pertencentes a essa classe e realmente não pertencem
        /// </summary>
        /// <param name="classe"></param>
        /// <returns></returns>
        public int VerdadeirosNegativos(string classe)
        {
            var classeIndex = Classes.IndexOf(classe);
            var verdadeirosNegativos = 0;

            for (var i = 0; i < Classes.Count; i++)
            {
                for (var j = 0; j < Classes.Count; j++)
                {
                    if (i != classeIndex && j != classeIndex)
                        verdadeirosNegativos += Classificacoes[i, j];
                }
            }

            return verdadeirosNegativos;
        }
        /// <summary>
        /// Foram previstos como percentences a essa classe e realmente pertencem
        /// </summary>
        /// <returns></returns>
        public int VerdadeirosPositivos(string classe)
        {
            var classeIndex = Classes.IndexOf(classe);

            return Classificacoes[classeIndex, classeIndex];
        }
        public int TotalAcertos()
        {
            var acertos = 0;

            for (var i = 0; i < Classes.Count; i++)
            {
                acertos += Classificacoes[i, i];
            }

            return acertos;
        }
        public int TotalErros()
        {
            var erros = 0;

            for (var i = 0; i < Classes.Count; i++)
            {
                for (var j = 0; j < Classes.Count; j++)
                {
                    if (i != j)
                        erros += Classificacoes[i, j];
                }
            }

            return erros;
        }
        public int TotalElementos()
        {
            int contador = 0;

            for (int i = 0; i < Classes.Count; i++)
            {
                for (int j = 0; j < Classes.Count; j++)
                {
                    contador += Classificacoes[i, j];
                }
            }

            return contador;
        }

        #endregion

        #region Métricas

        /// <summary>
        /// Proporção de predições corretas sem leva em consideração o que é positivo e o que é negativo
        /// </summary>
        /// <returns></returns>
        public double Acuracia()
        {
            var acertos = System.Convert.ToDouble(TotalAcertos());
            var erros = System.Convert.ToDouble(TotalErros());

            return acertos / (acertos + erros);
        }
        /// <summary>
        /// Média harmônica de precisão e sensibilidade
        /// </summary>
        /// <param name="classe"></param>
        /// <returns></returns>
        public double FMeasure(string classe)
        {
            return 2 / (1 / Sensibilidade(classe) + 1 / Precisao(classe));
        }
        /// <summary>
        /// Média geométrica de precisão e sensibilidade
        /// </summary>
        /// <param name="classe"></param>
        /// <returns></returns>
        public double GMeasure(string classe)
        {
            return Math.Sqrt(Precisao(classe) * Sensibilidade(classe));
        }
        /// <summary>
        /// A proporção de verdadeiros positivos em relação a todas as predições positivas
        /// </summary>
        /// <param name="classe"></param>
        /// <returns></returns>
        public double Precisao(string classe)
        {
            var preditivoPositivo = VerdadeirosPositivos(classe).ToDouble() /
                                      (VerdadeirosPositivos(classe).ToDouble() + FalsosPositivos(classe).ToDouble());

            return preditivoPositivo.Equals(double.NaN) ? 0 : preditivoPositivo;
        }
        /// <summary>
        /// A proporção de verdadeiros positivos: a capacidade do sistema em predizer corretamente a 
        /// condição para casos que realmente a têm.
        /// </summary>
        /// <param name="classe"></param>
        /// <returns></returns>
        public double Sensibilidade(string classe)
        {
            var sensibilidade = VerdadeirosPositivos(classe).ToDouble() /
                                  (VerdadeirosPositivos(classe).ToDouble() + FalsosNegativos(classe).ToDouble());

            return sensibilidade.Equals(double.NaN) ? 0 : sensibilidade;
        }

        #endregion
    }
}