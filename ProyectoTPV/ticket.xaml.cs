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
using ProyectoTPV.Model;

namespace ProyectoTPV
{
    /// <summary>
    /// Lógica de interacción para ticket.xaml
    /// </summary>
    public partial class ticket : Window
    {
        public ticket(TicketVenta ticket)
        {

            InitializeComponent();
            tv = ticket;
            rellenarTicket();
        }
        TicketVenta tv;
        UnitOfWork u = new UnitOfWork();


        public void rellenarTicket()
        {
            foreach (LineaVenta lv in tv.LineaVenta.ToList())
            {
                Label lb = new Label();
                lb.HorizontalAlignment = HorizontalAlignment.Center;
                if (lv.VarianteProducto.Producto.Nombre != lv.VarianteProducto.Nombre)
                {
                    lb.Content = lv.Unidades + " - " + lv.VarianteProducto.Producto.Nombre + " - " + lv.VarianteProducto.Nombre + " - " + lv.VarianteProducto.Precio + "€" + " total " + lv.Unidades * lv.VarianteProducto.Precio + "€";
                }
                else
                {
                    lb.Content = lv.Unidades + " - " + lv.VarianteProducto.Producto.Nombre + " - " + lv.VarianteProducto.Precio + "€" + " total " + lv.Unidades * lv.VarianteProducto.Precio + "€";
                }
                stackpanel_ticket.Children.Add(lb);
            }
            stackpanel_ticket.Children.Add(new Separator());
            try
            {
                Label lbmesa = new Label();
                lbmesa.HorizontalAlignment = HorizontalAlignment.Left;
                lbmesa.FontSize = 12;
                lbmesa.Content = tv.Mesa.NombreMesa + " incremento " + tv.Mesa.IncrementoMesa + " €";
                stackpanel_ticket.Children.Add(lbmesa);

            }
            catch (Exception er)
            {
                Console.WriteLine(er);
            }

            Label lbBaseImponible = new Label();
            decimal total = tv.LineaVenta.Sum(c => c.Unidades * c.VarianteProducto.Precio);
            try
            {
                total += tv.Mesa.IncrementoMesa;
            }
            catch (Exception er)
            {
                Console.WriteLine(er);
            }
            lbBaseImponible.HorizontalAlignment = HorizontalAlignment.Stretch;
            lbBaseImponible.FontSize = 14;
            lbBaseImponible.Content = "Base imponible " + Math.Round((total / 1.1m), 2) + "€";
            stackpanel_ticket.Children.Add(lbBaseImponible);

            Label iva = new Label();
            iva.Content = "IVA (10%)" + Math.Round((total / 1.1m) * 0.1m, 2) + "€";
            iva.HorizontalAlignment = HorizontalAlignment.Stretch;

            stackpanel_ticket.Children.Add(iva);

            Label lbtotal = new Label();
            lbtotal.FontSize = 15;
            lbtotal.FontWeight = FontWeights.Bold;
            lbtotal.HorizontalAlignment = HorizontalAlignment.Stretch;
            lbtotal.Content = "TOTAL: " + total + "€";
            stackpanel_ticket.Children.Add(lbtotal);

            Label lbAtendido = new Label();
            lbAtendido.FontSize = 12;
            lbAtendido.FontWeight = FontWeights.Bold;
            lbAtendido.HorizontalAlignment = HorizontalAlignment.Stretch;
            lbAtendido.Content = "Le atendió " + tv.Usuario.Nombre + " " + tv.Usuario.Apellidos;
            stackpanel_ticket.Children.Add(lbAtendido);
        }

    }
}
