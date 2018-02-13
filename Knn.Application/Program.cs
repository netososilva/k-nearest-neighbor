using System;
using System.Collections.Generic;
using System.Globalization;
using Knn.BaseDeDados;
using Knn.Calculo.Formulas;
using Knn.Estatistica.Formulas;

namespace Knn.Application
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Lendo arquivo...");

            const string pathArquivo = @"C:\Users\netos\Google Drive\Fatec\TCC\Projeto\Bases\Padrão\titanic.dat";
            var arquivo = new Arquivo();
            var atributos = new List<string[]>();
            var classes = new List<string>();

            arquivo.LerArquivo(pathArquivo, ref atributos, ref classes);
            
            var baseDeTreinamento = new Base(atributos, classes);
            var baseDeTestes = baseDeTreinamento.Split(50);

            var knn = new Classificador.Knn();

            Console.WriteLine("Calculando...");

            knn.Treina(baseDeTreinamento, baseDeTestes, new Euclidiana(), new Media(), 3);

            ExibeResultados(knn);

            Console.Read();
        }

        public static void ExibeResultados(Classificador.Knn knn)
        {
            Console.Clear();
            Console.WriteLine("Métricas");
            Console.WriteLine();
            Console.WriteLine("Base de dados: ");
            Console.WriteLine();

            foreach (KeyValuePair<string, int> classes in knn.BaseDeDados.Classes)
            {
                Console.WriteLine("Classe/Registros: " + classes.Key + " / " + classes.Value);
            }

            Console.WriteLine();

            foreach (var classe in knn.BaseDeDados.Classes.Keys)
            {
                Console.WriteLine("Classe: " + classe);
                Console.WriteLine();
                int[,] matrizDeConfusao = knn.MatrizDeConfusao.MatrizPorClasse(classe);

                for (int i = 0; i < 2; i++)
                {
                    for (int j = 0; j < 2; j++)
                    {
                        Console.Write(matrizDeConfusao[i, j] + " ");
                    }
                    Console.Write("\n");
                }

                Console.WriteLine();
                Console.WriteLine("VP: " + knn.MatrizDeConfusao.VerdadeirosPositivos(classe));
                Console.WriteLine("FP: " + knn.MatrizDeConfusao.FalsosPositivos(classe));
                Console.WriteLine("FN: " + knn.MatrizDeConfusao.FalsosNegativos(classe));
                Console.WriteLine("VN: " + knn.MatrizDeConfusao.VerdadeirosNegativos(classe));
                Console.WriteLine("F-measure: " + knn.MatrizDeConfusao.FMeasure(classe).ToString("P", CultureInfo.InvariantCulture));
                Console.WriteLine("G-measure: " + knn.MatrizDeConfusao.GMeasure(classe).ToString("P", CultureInfo.InvariantCulture));
                Console.WriteLine();
            }

            Console.WriteLine("Acúracia: " + knn.MatrizDeConfusao.Acuracia().ToString("P", CultureInfo.InvariantCulture));
            Console.WriteLine("Melhor K: " + knn.MelhorK);
            Console.WriteLine("Tempo de execução: " + (knn.TempoDeExecucao.TotalMilliseconds / 1000.0).ToString(CultureInfo.InvariantCulture) + "s");
            Console.WriteLine("Quantidade de previsões corretas: " + knn.MatrizDeConfusao.TotalAcertos());
            Console.WriteLine("Tamanho da base de testes: " + knn.MatrizDeConfusao.TotalElementos());
        }
    }
}