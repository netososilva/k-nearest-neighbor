using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Knn.BaseDeDados
{
    public class Arquivo
    {
        #region Métodos Públicos

        public void LerArquivo(string path, ref List<string[]> atributos, ref List<string> classes)
        {
            string[] dadosArquivo;

            using (var arquivo = new StreamReader(path))
            {
                dadosArquivo = arquivo.ReadToEnd().Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
            }

            var quantidadeEntradasArquivo = dadosArquivo.Length;

            if (quantidadeEntradasArquivo == 0)
                throw new Exception("Arquivo vazio");

            var quantidadeDeAtributos = QuantidadeAtributos(ref dadosArquivo);

            for (var i = 0; i < dadosArquivo.Length; i++)
            {
                atributos.Add(ObterAtributos(ref dadosArquivo, i, quantidadeDeAtributos));
                classes.Add(ObterClasse(ref dadosArquivo, i));
            }
        }

        #endregion

        #region Métodos Privados

        private string[] ObterAtributos(ref string[] arquivo, int indice, int quantidadeDeAtributos)
        {
            return arquivo[indice].Split(',').Take(quantidadeDeAtributos - 1).ToArray();
        }

        private string ObterClasse(ref string[] arquivo, int indice)
        {
            return arquivo[indice].Split(',').Last();
        }

        private int QuantidadeAtributos(ref string[] arquivo)
        {
            return arquivo.First().Split(',').Length - 1;
        }

        #endregion
    }
}