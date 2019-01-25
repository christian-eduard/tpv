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
using OpenPOS.Model;

namespace OpenPOS
{
    /// <summary>
    /// Lógica de interacción para invoice.xaml
    /// </summary>
    public partial class invoice : Window
    {
        public invoice(Invoice ticket)
        {

            InitializeComponent();
            tv = ticket;
            drawInvoice();
        }
        Invoice tv;
        UnitOfWork u = new UnitOfWork();


        public void drawInvoice()
        {
            foreach (SalesLine lv in tv.SalesLine.ToList())
            {
                Label lb = new Label();
                lb.HorizontalAlignment = HorizontalAlignment.Center;
                lb.Content = lv.Unit + " - " + lv.Item.Name + " - " + lv.Item.Price + "€" + " - " + lv.Unit * lv.Item.Price + "€";
                stackpanel_invoice.Children.Add(lb);
            }
            stackpanel_invoice.Children.Add(new Separator());

            Label lbBaseImponible = new Label();
            decimal total = tv.SalesLine.Sum(c => c.Unit * c.Item.Price);
            lbBaseImponible.HorizontalAlignment = HorizontalAlignment.Stretch;
            lbBaseImponible.FontSize = 14;
            lbBaseImponible.Content = "Base imponible " + Math.Round((total / 1.1m), 2) + "€";
            stackpanel_invoice.Children.Add(lbBaseImponible);

            Label iva = new Label();
            iva.Content = "IVA (10%)" + Math.Round((total / 1.1m) * 0.1m, 2) + "€";
            iva.HorizontalAlignment = HorizontalAlignment.Stretch;

            stackpanel_invoice.Children.Add(iva);

            Label lbtotal = new Label();
            lbtotal.FontSize = 15;
            lbtotal.FontWeight = FontWeights.Bold;
            lbtotal.HorizontalAlignment = HorizontalAlignment.Stretch;
            lbtotal.Content = "TOTAL: " + total + "€";
            stackpanel_invoice.Children.Add(lbtotal);

            Label lbAtendido = new Label();
            lbAtendido.FontSize = 12;
            lbAtendido.FontWeight = FontWeights.Bold;
            lbAtendido.HorizontalAlignment = HorizontalAlignment.Stretch;
            lbAtendido.Content = "Le atendió " + tv.User.Name + " " + tv.User.LastName;
            stackpanel_invoice.Children.Add(lbAtendido);
        }

    }
}
