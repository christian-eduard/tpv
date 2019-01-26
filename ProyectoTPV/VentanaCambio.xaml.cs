using BespokeFusion;
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
    /// Lógica de interacción para VentanaCambio.xaml
    /// </summary>
    public partial class VentanaCambio : Window
    {
        public VentanaCambio(decimal totalcaja)
        {
            InitializeComponent();
            total = totalcaja;
        }
        decimal total;
        decimal cambio;

        public event Action<decimal> Cambio;

        private void button_cambio_50euros_Click(object sender, RoutedEventArgs e)
        {
            cambio += 50;
            txtblock_cambioRecibido.Text = cambio.ToString();
        }

        private void button_cambio_20euros_Click(object sender, RoutedEventArgs e)
        {
            cambio += 20;
            txtblock_cambioRecibido.Text = cambio.ToString();
        }

        private void button_cambio_10euros_Click(object sender, RoutedEventArgs e)
        {
            cambio += 10;
            txtblock_cambioRecibido.Text = cambio.ToString();
        }

        private void button_cambio_5euros_Click(object sender, RoutedEventArgs e)
        {
            cambio += 5;
            txtblock_cambioRecibido.Text = cambio.ToString();
        }

        private void button_cambio_2euros_Click(object sender, RoutedEventArgs e)
        {
            cambio += 2;
            txtblock_cambioRecibido.Text = cambio.ToString();
        }

        private void button_cambio_1euros_Click(object sender, RoutedEventArgs e)
        {
            cambio += 1;
            txtblock_cambioRecibido.Text = cambio.ToString();
        }

        private void button_cambio_50centimos_Click(object sender, RoutedEventArgs e)
        {
            cambio += 0.5m;
            txtblock_cambioRecibido.Text = cambio.ToString();
        }

        private void button_cambio_20centimos_Click(object sender, RoutedEventArgs e)
        {
            cambio += 0.2m;
            txtblock_cambioRecibido.Text = cambio.ToString();
        }

        private void button_cambio_10centimos_Click(object sender, RoutedEventArgs e)
        {
            cambio += 0.1m;
            txtblock_cambioRecibido.Text = cambio.ToString();
        }

        private void button_cambio_5centimos_Click(object sender, RoutedEventArgs e)
        {
            cambio += 0.05m;
            txtblock_cambioRecibido.Text = cambio.ToString();
        }

        private void button_cambio_2centimos_Click(object sender, RoutedEventArgs e)
        {
            cambio += 0.02m;
            txtblock_cambioRecibido.Text = cambio.ToString();
        }

        private void button_cambio_1centimos_Click(object sender, RoutedEventArgs e)
        {
            cambio += 0.01m;
            txtblock_cambioRecibido.Text = cambio.ToString();
        }

        private void btn_validar_Click(object sender, RoutedEventArgs e)
        {
            if (total <= cambio)
            {
                Cambio(cambio);
                var msg = new CustomMaterialMessageBox
                {
                    TxtMessage = { Text = "A devolver: " + Convert.ToString(cambio - total), Foreground = Brushes.Black, FontSize = 50, HorizontalAlignment = HorizontalAlignment.Center },
                    TxtTitle = { Text = "Devolución", Foreground = Brushes.White },
                    MainContentControl = { Background = Brushes.White },
                    BtnCopyMessage = { Visibility = Visibility.Hidden },
                    BtnCancel = { Visibility = Visibility.Hidden },
                    BorderBrush = { Opacity = 0 },
                   //BorderThickness = new Thickness(1, 0, 1, 1),
                    BtnOk = { Content = "Cambio entregado", HorizontalAlignment = HorizontalAlignment.Center, Foreground = Brushes.White, Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#795548")), Width = 200 },

                    TitleBackgroundPanel = { Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#795548")) },
                };

                msg.Show();

                this.Close();
            }
            else
            {
                var msg = new CustomMaterialMessageBox
                {
                    TxtMessage = { Text = "¡ Falta cambio !", Foreground = Brushes.Black, FontSize = 50, HorizontalAlignment = HorizontalAlignment.Center },
                    TxtTitle = { Text = "Error", Foreground = Brushes.Black },
                    MainContentControl = { Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FAFAFA")) },
                    BtnCopyMessage = { Visibility = Visibility.Hidden },
                    BtnCancel = { Visibility = Visibility.Hidden },
                    BorderBrush = { Opacity = 1 },
                    //BorderThickness = new Thickness(1, 0, 1, 1),
                    BtnOk = { Content = "Volver a comprobar", HorizontalAlignment = HorizontalAlignment.Center, Foreground = Brushes.White, Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#795548")), Width = 200 },
                    TitleBackgroundPanel = { Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#f44336")) },
                };

                msg.Show();
            }

        }

        private void btn_borrar_Click(object sender, RoutedEventArgs e)
        {
            cambio = 0;
            txtblock_cambioRecibido.Text = cambio.ToString();
        }

        private void btn_cerrar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
