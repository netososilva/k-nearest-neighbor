using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Knn.Estatistica.Formulas;
using Knn.Helper;

namespace Knn.BaseDeDados
{
    public class Base
    {
        #region Propriedades

        public List<Entrada> Dados { get; set; }
        public Dictionary<string, int> Classes { get; set; }

        #endregion

        #region Construtor

        public Base(List<string[]> atributos, List<string> classes)
        {
            if (atributos.Count != classes.Count)
                throw new Exception("Quantidade de registros divergente");

            Classes = new Dictionary<string, int>();
            Dados = new List<Entrada>(atributos.Count);

            for (var i = 0; i < atributos.Count; i++)
            {
                AtribuiEntrada(atributos[i], classes[i]);
            }
        }

        #endregion

        #region Métodos Públicos

        private bool ClasseNaoEstaBalanceada(Base baseDeTestes, int tamanhoBaseDeTestes, string classe, Dictionary<string, int> classesBaseDeTreinamento)
        {
            return baseDeTestes.Classes[classe] < classesBaseDeTreinamento[classe] * (tamanhoBaseDeTestes / 100.0m);
        }

        /// <summary>
        /// Embaralha dados através do algoritmo de Fisher-Yales (solução de Durstenfeld)
        /// </summary>
        public void Embaralha()
        {
            var rdn = new Random();
            var tamanho = Dados.Count;
            int randomico;

            while (tamanho > 0)
            {
                tamanho--;

                randomico = rdn.Next(0, tamanho);

                var entradaTemporaria = Dados[randomico];
                Dados[randomico] = Dados[tamanho];
                Dados[tamanho] = entradaTemporaria;
            }
        }

        public void Normalizar()
        {
            var quantidadeDeAtributos = Dados.First().Atributos.Length;
            
            for (var atributo = 0; atributo < quantidadeDeAtributos; atributo++)
            {
                var menorAtributo = MenorAtributo(atributo);
                var maiorAtributo = MaiorAtributo(atributo);
                
                for (var dado = 0; dado < Dados.Count; dado++)
                {
                    if (!Helper.Calculo.VerificaSeAtributoEhNumerico(Dados[dado].Atributos[atributo]))
                        continue;

                    Dados[dado].Atributos[atributo] = NormalizaAtributo(dado, atributo, menorAtributo, maiorAtributo);
                }
            }
        }

        public void RetirarElementosRepetidos()
        {
            foreach (var elemento in Dados)
            {
                if (Dados.Count(x => x.Equals(elemento)) > 1)
                    Dados.Remove(elemento);
            }
        }

        public Base Split(decimal porcentagemBaseDeTestes)
        {
            var atributos = new List<string[]>();
            var classes = new List<string>();

            for (var i = TamanhoBaseDeTestes(porcentagemBaseDeTestes); i > 0 ; i--)
            {
                atributos.Add(Dados[i].Atributos);
                classes.Add(Dados[i].Classe);
                Dados.RemoveAt(i);
            }

            return new Base(atributos, classes);
        }

        public Base SplitComClassesBalanceadas(decimal porcentagemBaseDeTestes)
        {
            var atributos = new List<string[]>();
            var classes = new List<string>();
            var baseDeTeste = new Base(atributos, classes);
            var classesBaseDeTreinamento = Classes;
            var classesJaAtribuidas = TamanhoBaseDeTestes(porcentagemBaseDeTestes);

            for (var i = Dados.Count; i > 0; i--)
            {
                if (ClasseNaoEstaBalanceada(baseDeTeste, classesJaAtribuidas, Dados[i].Classe, classesBaseDeTreinamento))
                {
                    baseDeTeste.AtribuiEntrada(Dados[i].Atributos, Dados[i].Classe);
                    RemoveEntrada(i);
                }
            }

            return baseDeTeste;
        }
        
        public void SubstituirValoresFaltantes()
        {
            var quantidadeDeAtributos = Dados.First().Atributos.Length;
            var media = new Media();

            for (var atributo = 0; atributo < quantidadeDeAtributos; atributo++)
            {
                if (!Helper.Calculo.VerificaSeAtributoEhNumerico(Dados.First().Atributos[atributo]))
                    continue;

                var atributoFaltante = ConvertHelper.ToString(media.Calcular(SomatorioAtributo(atributo), quantidadeDeAtributos));

                for (var dado = 0; dado < Dados.Count; dado++)
                {
                    if (EhAtributoFaltante(atributo, dado))
                        Dados[dado].Atributos[atributo] = atributoFaltante;
                }
            }
        }

        public Base Zip(Base baseDeDados)
        {
            var novaBase = new Base(Dados.Select(x => x.Atributos).ToList(), Dados.Select(x => x.Classe).ToList());

            foreach (var entrada in baseDeDados.Dados)
            {
                novaBase.AtribuiEntrada(entrada.Atributos, entrada.Classe);
            }

            return novaBase;
        }

        #endregion

        #region Métodos Privados

        private void AtribuiEntrada(string[] atributo, string classe)
        {
            var entrada = new Entrada
            {
                Atributos = atributo,
                Classe = classe
            };

            Dados.Add(entrada);

            if (!Classes.ContainsKey(classe))
                Classes.Add(classe, 0);

            Classes[classe] += 1;
        }
        
        private bool EhAtributoFaltante(int atributo, int dado)
        {
            var atributoFaltante = Dados[dado].Atributos[atributo].Trim();

            return atributoFaltante.Equals("<null>") || atributoFaltante.Equals("?");
        }
        
        private double MenorAtributo(int indiceAtributo)
        {
            return Dados.Min(dado => Helper.Calculo.VerificaSeAtributoEhNumerico(dado.Atributos[indiceAtributo]) ? 
                                     dado.Atributos[indiceAtributo].ToDouble() :
                                     double.MaxValue);
        }

        private double MaiorAtributo(int indiceAtributo)
        {
            return Dados.Min(dado => Helper.Calculo.VerificaSeAtributoEhNumerico(dado.Atributos[indiceAtributo]) ?
                                     dado.Atributos[indiceAtributo].ToDouble() :
                                     double.MinValue);
        }

        private string NormalizaAtributo(int dado, int atributo, double menorAtributo, double maiorAtributo)
        {
            return ((Dados[dado].Atributos[atributo].ToDouble() - menorAtributo) / 
                   (maiorAtributo - menorAtributo)).ToString("G", CultureInfo.InvariantCulture);
        }

        private void RemoveEntrada(int indice)
        {
            var classe = Dados[indice].Classe;

            if (Classes[classe] == 1)
                Classes.Remove(classe);

            Classes[classe] -= 1;

            Dados.RemoveAt(indice);
        }

        private double SomatorioAtributo(int atributo)
        {
            return Dados.Sum(x => x.Atributos[atributo].ToDouble());
        }

        private int TamanhoBaseDeTestes(decimal porcentagemDeBaseDeTestes)
        {
            return (int) (Dados.Count * (porcentagemDeBaseDeTestes / 100.0m));
        }

        #endregion
    }
}