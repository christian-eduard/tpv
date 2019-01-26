using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace ProyectoTPV
{
    public class DataClass
    {
        public string Producto { get; set; }

        public string Variante { get; set; }
    }

    public class LineaVentaMultiValueConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values[0].ToString().Equals(values[1].ToString()))
            {
                return String.Format("{0}", values[0], values[1]);
            }
            else
            {
                return String.Format("{0} {1}", values[0], values[1]);
            }

        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

}
