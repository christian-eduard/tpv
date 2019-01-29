using ProyectoTPV.Model;
using System;
using System.Windows;
using AmRoMessageDialog;

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
          // c.FechaHoraRecuento = DateTime.Now;
            u.CajaRepository.Create(c);

            string message = "Hola " + usr.Nombre + Environment.NewLine + "Caja inicial " + cantidadInit + " €";
            string caption = "Acceso al TPV";

            var messageBox = new AmRoMessageBox
            {
                Background = "#333333",
                TextColor = "#ffffff",
                IconColor = "#3399ff",
                RippleEffectColor = "#000000",
                ClickEffectColor = "#1F2023",
                ShowMessageWithEffect = true,
                EffectArea = this,
                ParentWindow = this
            };

            messageBox.Show(message, caption, AmRoMessageBoxButton.Ok);
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
