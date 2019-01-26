using BespokeFusion;
using ProyectoTPV.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ProyectoTPV
{
    /// <summary>
    /// Lógica de interacción para VentanaArqueoDeCaja.xaml
    /// </summary>
    public partial class VentanaArqueoDeCaja : Window
    {
        public VentanaArqueoDeCaja(UnitOfWork u, Usuario usr)
        {
            InitializeComponent();
            this.u = u;
            this.usr = usr;
        }
        decimal total;
        decimal cantidadInit;
        UnitOfWork u;
        Usuario usr;
        Caja c = new Caja();
        public event Action<decimal> Cambio;

        private void button_cambio_50euros_Click(object sender, RoutedEventArgs e)
        {
            cantidadInit += 50;
            txtblock_cantidad.Text = cantidadInit.ToString();
        }

        private void button_cambio_20euros_Click(object sender, RoutedEventArgs e)
        {
            cantidadInit += 20;
            txtblock_cantidad.Text = cantidadInit.ToString();
        }

        private void button_cambio_10euros_Click(object sender, RoutedEventArgs e)
        {
            cantidadInit += 10;
            txtblock_cantidad.Text = cantidadInit.ToString();
        }

        private void button_cambio_5euros_Click(object sender, RoutedEventArgs e)
        {
            cantidadInit += 5;
            txtblock_cantidad.Text = cantidadInit.ToString();
        }

        private void button_cambio_2euros_Click(object sender, RoutedEventArgs e)
        {
            cantidadInit += 2;
            txtblock_cantidad.Text = cantidadInit.ToString();
        }

        private void button_cambio_1euros_Click(object sender, RoutedEventArgs e)
        {
            cantidadInit += 1;
            txtblock_cantidad.Text = cantidadInit.ToString();
        }

        private void button_cambio_50centimos_Click(object sender, RoutedEventArgs e)
        {
            cantidadInit += 0.5m;
            txtblock_cantidad.Text = cantidadInit.ToString();
        }

        private void button_cambio_20centimos_Click(object sender, RoutedEventArgs e)
        {
            cantidadInit += 0.2m;
            txtblock_cantidad.Text = cantidadInit.ToString();
        }

        private void button_cambio_10centimos_Click(object sender, RoutedEventArgs e)
        {
            cantidadInit += 0.1m;
            txtblock_cantidad.Text = cantidadInit.ToString();
        }

        private void button_cambio_5centimos_Click(object sender, RoutedEventArgs e)
        {
            cantidadInit += 0.05m;
            txtblock_cantidad.Text = cantidadInit.ToString();
        }

        private void button_cambio_2centimos_Click(object sender, RoutedEventArgs e)
        {
            cantidadInit += 0.02m;
            txtblock_cantidad.Text = cantidadInit.ToString();
        }

        private void button_cambio_1centimos_Click(object sender, RoutedEventArgs e)
        {
            cantidadInit += 0.01m;
            txtblock_cantidad.Text = cantidadInit.ToString();
        }

        private void btn_validar_Click(object sender, RoutedEventArgs e)
        {
            c.DineroInicial = cantidadInit;
            c.FechaHoraApertura = DateTime.Now;
            c.DineroFinal = cantidadInit;
            c.Vendedor = usr;
          //  c.FechaHoraRecuento = DateTime.Now;
            u.CajaRepository.Create(c);
            var msg = new CustomMaterialMessageBox
            {
                TxtMessage = { Text = "Hola " + usr.Nombre + Environment.NewLine + "Caja inicial " + cantidadInit + " €", Foreground = Brushes.Black, FontSize = 40, HorizontalAlignment = HorizontalAlignment.Center },
                TxtTitle = { Text = "Inicio de caja", Foreground = Brushes.White },
                MainContentControl = { Background = Brushes.White },
                BtnCopyMessage = { Visibility = Visibility.Hidden },
                BorderBrush = { Opacity = 0 },
                BtnCancel = { Visibility = Visibility.Hidden },
                BtnOk = { Content = "Acceso al TPV", HorizontalAlignment = HorizontalAlignment.Center, Foreground = Brushes.White, Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#795548")), Width = 200 },
                TitleBackgroundPanel = { Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#795548")) },
            };

            msg.Show();
            this.Close();
        }

        private void btn_borrar_Click(object sender, RoutedEventArgs e)
        {
            cantidadInit = 0;
            txtblock_cantidad.Text = cantidadInit.ToString();
        }

        private void btn_cerrar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
