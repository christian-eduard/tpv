using BespokeFusion;
using System;
using System.Windows;
using System.Windows.Media;
using AmRoMessageDialog;

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
            string message = "A devolver: " + Convert.ToString(cambio - total);
            string caption = "Devolución";
            if (total <= cambio)
            {
                Cambio(cambio);

                AmRoMessageBox.ShowDialog(message, caption);
                this.Close();
            }
            else
            {
                message = "¡ Falta cambio !";
                caption = "Error";
                AmRoMessageBox.ShowDialog(message, caption);
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
