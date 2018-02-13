namespace Knn.BaseDeDados
{
    public class Entrada
    {
        public string[] Atributos { get; set; }
        public string Classe { get; set; }

        public bool Equals(Entrada entrada)
        {
            if (entrada.Atributos.Length != Atributos.Length)
                return false;

            for (var i = 0; i < entrada.Atributos.Length; i++)
            {
                if (entrada.Atributos[i] != Atributos[i])
                    return false;
            }

            return true;
        }
    }
}