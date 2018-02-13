using System.Globalization;

namespace Knn.Helper
{
    public static class ConvertHelper
    {
        public static double ToDouble(this object valor)
        {
            return System.Convert.ToDouble(valor, CultureInfo.InvariantCulture);
        }

        public static string ToString(this object valor)
        {
            return System.Convert.ToString(valor, CultureInfo.InvariantCulture);
        }
    }
}