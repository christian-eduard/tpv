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
using System.Windows.Navigation;
using System.Windows.Resources;
using System.Windows.Shapes;
using System.IO;
using MaterialDesignThemes.Wpf;
using Microsoft.Win32;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Runtime.InteropServices;
using BespokeFusion;
using System.Collections;
using System.Data;
using AmRoMessageDialog;

namespace ProyectoTPV
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        #region Import para teclado virtual de windows 
        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool Wow64DisableWow64FsRedirection(ref IntPtr ptr);
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool Wow64RevertWow64FsRedirection(IntPtr ptr);

        private const UInt32 WM_SYSCOMMAND = 0x112;
        private const UInt32 SC_RESTORE = 0xf120;
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        static extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, IntPtr wParam, IntPtr lParam);

        private string OnScreenKeyboadApplication = "osk.exe";
        #endregion

        public MainWindow()
        {
            InitializeComponent();

            grid_tpvSI.Visibility = Visibility.Hidden;
            grid_adminSI.Visibility = Visibility.Hidden;
            grid_pedidosSI.Visibility = Visibility.Hidden;

            Directory.CreateDirectory(Environment.CurrentDirectory + "\\imagenes\\usuarios\\");
            Directory.CreateDirectory(Environment.CurrentDirectory + "\\imagenes\\categorias\\");
            Directory.CreateDirectory(Environment.CurrentDirectory + "\\imagenes\\productos\\");
            Directory.CreateDirectory(Environment.CurrentDirectory + "\\Pedidos\\");

            Load_Usuarios();
            Load_Categorias();

            refreshDatagrids();
        }

        private void btn_closeApp_Click(object sender, RoutedEventArgs e)
        {
            cerrarCaja();
            this.Close();
        }

        #region Custom MessageBox
        public bool MsgPregunta(string Mensaje, string Titulo)
        {
            AmRoMessageBox messageBox = new AmRoMessageBox
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

            return (messageBox.Show(Mensaje, Titulo, AmRoMessageBoxButton.OkCancel).Equals(AmRoMessageBoxResult.Ok));
        }

        #endregion

        #region Declaraciones Global
        UnitOfWork u = new UnitOfWork();
        List<Producto> listProductoProveedor;
        LineaVenta lv;
        TicketVenta tv;
        Producto prod = new Producto();
        Usuario ActiveUsr;
        Usuario usr = new Usuario();
        Categoria cat = new Categoria();
        Caja caja = new Caja();
        SubCategoria subCat = new SubCategoria();
        VarianteProducto varProd = new VarianteProducto();
        Proveedor prov = new Proveedor();
        Mesa mesa = new Mesa();
        LineaPedidoProveedor lPedidoProv = new LineaPedidoProveedor();
        PedidoProveedor pedidoProv = new PedidoProveedor();
        List<PedidoProveedor> listaPedidos = new List<PedidoProveedor>();
        List<TicketVenta> listaTickets = new List<TicketVenta>();

        string idUsuario;
        string unidades = null;
        decimal total = 0;
        decimal cambio = 0;
        string imagen;
        bool create;
        #endregion

        #region Zona TPV

        void Select_usuario(string usuario)
        {
            ActiveUsr = new Usuario();
            PasswordBox.Clear();

            string[] usrContent = usuario.Split(' ');
            idUsuario = usrContent[0];
            textblockusuarioSeleccionado.Text = usrContent[1] + " " + usrContent[2];
            Console.WriteLine("id usuario seleccionado" + idUsuario);
        }

        void Load_Usuarios()
        {
            StackPanel_Usuarios.Children.Clear();

            foreach (Usuario item in u.VendedorRepository.GetAll())
            {
                //cargar usuarios en pantalla de login
                MaterialDesignThemes.Wpf.Chip chip = new MaterialDesignThemes.Wpf.Chip();

                string rutaImagen = Environment.CurrentDirectory + "\\imagenes\\usuarios\\" + item.RutaImagen;
                if (File.Exists(rutaImagen))
                {
                    chip.IconForeground = Brushes.Transparent;
                    chip.Icon = new ImageBrush(new BitmapImage(new Uri(rutaImagen, UriKind.Relative)));
                    chip.IconBackground = new ImageBrush(new BitmapImage(new Uri(rutaImagen, UriKind.Relative)));
                }
                chip.FontSize = 20;
                chip.Content = item.UsuarioId + " " + item.Nombre + " " + item.Apellidos;
                Thickness margin = chip.Margin;
                margin.Top = 10;
                margin.Bottom = 10;
                margin.Left = 10;
                margin.Right = 10;
                chip.Margin = margin;


                //Evento clic
                chip.Click += (s, e) =>
                {
                    Select_usuario(chip.Content.ToString());
                };

                StackPanel_Usuarios.Children.Add(chip);

            }
        }

        void Load_Categorias()
        {
            StackPanel_Categorias.Children.Clear();
            WrapPanel_productos.Children.Clear();

            foreach (Categoria item in u.CategoriaRepository.GetAll())
            {
                Button b = new Button();
                StackPanel sp = new StackPanel();
                Image img = new Image();
                TextBlock tb = new TextBlock();

                #region formato botton
                string rutaImagen = Environment.CurrentDirectory + "\\imagenes\\categorias\\" + item.RutaImagen;
                if (File.Exists(rutaImagen))
                {
                    b.Background = new ImageBrush(new BitmapImage(new Uri(rutaImagen, UriKind.Relative)));
                }
                b.Foreground = Brushes.Black;
                tb.HorizontalAlignment = HorizontalAlignment.Stretch;
                tb.Background = Brushes.White;
                tb.VerticalAlignment = VerticalAlignment.Bottom;
                tb.Text = item.Nombre;
                tb.Opacity = 0.6;
                if (Math.Round((double)(190 / item.Nombre.Count()), 0) < 20)
                {
                    tb.FontSize = Math.Round((double)(190 / item.Nombre.Count()), 0);
                }
                else
                {
                    tb.FontSize = 20;
                }
                b.Padding = new Thickness(1);
                b.Content = tb;
                b.VerticalContentAlignment = VerticalAlignment.Bottom;
                b.HorizontalContentAlignment = HorizontalAlignment.Center;

                b.Margin = new Thickness(10);
                b.Height = 100;
                b.Width = 100;

                #endregion

                //Evento clic

                if (item.SubCategoria.Count > 1)
                {
                    b.Click += (s, e) => { Load_SubCategorias(item); };
                }
                else
                {
                    b.Click += (s, e) => { Load_Productos(item.SubCategoria.First(c => c.Nombre.Equals(item.Nombre))); };
                }

                StackPanel_Categorias.Children.Add(b);
            }
        }

        void Load_Mesas()
        {
            WrapPanel_productos.Children.Clear();
            try
            {
                foreach (Mesa ms in u.MesaRepository.GetAll())
                {


                    #region formato boton

                    Button b = new Button();
                    StackPanel sp = new StackPanel();
                    Image img = new Image();
                    TextBlock tb = new TextBlock();
                    string rutaImagen = Environment.CurrentDirectory + "\\imagenes\\mesabar.jpg";
                    if (File.Exists(rutaImagen))
                    {
                        b.Background = new ImageBrush(new BitmapImage(new Uri(rutaImagen, UriKind.Relative)));
                    }
                    b.Foreground = Brushes.Black;
                    tb.HorizontalAlignment = HorizontalAlignment.Stretch;
                    tb.Background = Brushes.White;
                    tb.VerticalAlignment = VerticalAlignment.Bottom;
                    tb.Text = ms.NombreMesa;
                    tb.Opacity = 0.6;
                    if (Math.Round((double)(190 / ms.NombreMesa.Count()), 0) < 20)
                    {
                        tb.FontSize = Math.Round((double)(190 / ms.NombreMesa.Count()), 0);
                    }
                    else
                    {
                        tb.FontSize = 20;
                    }
                    b.Padding = new Thickness(1);
                    b.Content = tb;
                    b.VerticalContentAlignment = VerticalAlignment.Bottom;
                    b.HorizontalContentAlignment = HorizontalAlignment.Center;
                    b.Tag = ms;
                    b.Margin = new Thickness(10);
                    b.Height = 100;
                    b.Width = 100;
                    #endregion

                    //Evento clic
                    b.Click += (s, e2) =>
                    {
                        tv.Mesa = (Mesa)b.Tag;
                        txtblock_numMesa.Text = ms.NombreMesa;
                    };
                    WrapPanel_productos.Children.Add(b);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        void cerrarCaja()
        {
            if (MsgPregunta("Quieres Cerrar la caja?", "Cierre de caja"))
            {
                try
                {
                    caja.FechaHoraRecuento = DateTime.Now;
                    u.CajaRepository.Update(caja);

                    string message = "Cierre de caja por " + ActiveUsr.Nombre + Environment.NewLine +
                    "Dinero inicial " + caja.DineroInicial + Environment.NewLine +
                    "Recuento de caja " + caja.DineroFinal + Environment.NewLine +
                    "Beneficios: " + (caja.DineroFinal - caja.DineroInicial);

                    string caption = "Recuento";

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

                    caja = new Caja();
                }
                catch (Exception)
                {
                    AmRoMessageBox.ShowDialog("Ya se realizó el recuento");
                }
            }
        }

        void cerrarSession()
        {
            grid_tpvSI.Visibility = Visibility.Hidden;
            grid_tpvNO.Visibility = Visibility.Visible;
            grid_pedidosSI.Visibility = Visibility.Hidden;
            grid_pedidosNO.Visibility = Visibility.Visible;
            grid_adminNO.Visibility = Visibility.Visible;
            grid_adminSI.Visibility = Visibility.Hidden;

            listaTickets.Clear();
            chip_usuarioVenta.Content = "Session cerrada";
            string rutaImagen = Environment.CurrentDirectory + "\\imagenes\\usuarios\\admin.jpg";
            if (File.Exists(rutaImagen))
            {
                chip_usuarioVenta.IconForeground = Brushes.Transparent;
                chip_usuarioVenta.Icon = new ImageBrush(new BitmapImage(new Uri(rutaImagen, UriKind.Relative)));
                chip_usuarioVenta.IconBackground = new ImageBrush(new BitmapImage(new Uri(rutaImagen, UriKind.Relative)));
            }

            tabControl_Main.SelectedIndex = 0;
        }

        void Load_SubCategorias(Categoria objCat)
        {
            StackPanel_Categorias.Children.Clear();
            WrapPanel_productos.Children.Clear();

            #region boton volver
            Button b2 = new Button();
            StackPanel sp2 = new StackPanel();
            Image img2 = new Image();
            TextBlock tb2 = new TextBlock();
            #region formato botton
            b2.Foreground = Brushes.Black;
            b2.FontSize = 20;
            b2.FontWeight = FontWeights.Light;
            PackIcon icono = new PackIcon();
            icono.Kind = PackIconKind.ArrowLeftDropCircleOutline;
            icono.Width = 40;
            icono.Height = 40;
            icono.Foreground = Brushes.WhiteSmoke;
            b2.Content = icono;
            b2.VerticalContentAlignment = VerticalAlignment.Center;
            b2.HorizontalContentAlignment = HorizontalAlignment.Center;
            Thickness margin = b2.Margin;
            margin.Top = 10;
            margin.Bottom = 10;
            margin.Left = 10;
            margin.Right = 10;
            b2.Margin = margin;
            b2.Height = 50;
            b2.Width = 80;
            b2.Margin = margin;
            #endregion
            //Evento clic
            b2.Click += (s, e) =>
            {
                Load_Categorias();
            };

            StackPanel_Categorias.Children.Add(b2);

            #endregion

            foreach (SubCategoria sCat in objCat.SubCategoria)
            {
                if (sCat.Nombre != objCat.Nombre)
                {
                    Button b = new Button();
                    StackPanel sp = new StackPanel();
                    Image img = new Image();
                    TextBlock tb = new TextBlock();

                    #region formato botton
                    string rutaImagen = Environment.CurrentDirectory + "\\imagenes\\categorias\\" + sCat.RutaImagen;
                    if (File.Exists(rutaImagen))
                    {
                        b.Background = new ImageBrush(new BitmapImage(new Uri(rutaImagen, UriKind.Relative)));
                    }
                    b.Foreground = Brushes.Black;
                    tb.HorizontalAlignment = HorizontalAlignment.Stretch;
                    tb.Background = Brushes.White;
                    tb.VerticalAlignment = VerticalAlignment.Bottom;
                    tb.Text = sCat.Nombre;
                    tb.Opacity = 0.6;
                    if (Math.Round((double)(190 / sCat.Nombre.Count()), 0) < 20)
                    {
                        tb.FontSize = Math.Round((double)(190 / sCat.Nombre.Count()), 0);
                    }
                    else
                    {
                        tb.FontSize = 20;
                    }
                    b.Padding = new Thickness(1);
                    b.Content = tb;
                    b.VerticalContentAlignment = VerticalAlignment.Bottom;
                    b.HorizontalContentAlignment = HorizontalAlignment.Center;

                    b.Margin = new Thickness(10);
                    b.Height = 100;
                    b.Width = 100;
                    #endregion

                    //Evento clic
                    b.Click += (s, e) =>
                    {
                        Load_Productos(sCat);
                    };

                    StackPanel_Categorias.Children.Add(b);
                }
            }

        }

        void Load_Productos(SubCategoria sCat)
        {
            WrapPanel_productos.Children.Clear();
            try
            {
                foreach (var pr in sCat.Producto)
                {


                    #region formato boton

                    Button b = new Button();
                    StackPanel sp = new StackPanel();
                    Image img = new Image();
                    TextBlock tb = new TextBlock();
                    string rutaImagen = Environment.CurrentDirectory + "\\imagenes\\productos\\" + pr.RutaImagen;
                    if (File.Exists(rutaImagen))
                    {
                        b.Background = new ImageBrush(new BitmapImage(new Uri(rutaImagen, UriKind.Relative)));
                    }
                    b.Foreground = Brushes.Black;
                    tb.HorizontalAlignment = HorizontalAlignment.Stretch;
                    tb.Background = Brushes.White;
                    tb.VerticalAlignment = VerticalAlignment.Bottom;
                    tb.Text = pr.Nombre;
                    tb.Opacity = 0.6;
                    if (Math.Round((double)(190 / pr.Nombre.Count()), 0) < 20)
                    {
                        tb.FontSize = Math.Round((double)(190 / pr.Nombre.Count()), 0);
                    }
                    else
                    {
                        tb.FontSize = 20;
                    }
                    b.Padding = new Thickness(1);
                    b.Content = tb;
                    b.VerticalContentAlignment = VerticalAlignment.Bottom;
                    b.HorizontalContentAlignment = HorizontalAlignment.Center;
                    b.Tag = pr.Nombre;
                    b.Margin = new Thickness(10);
                    b.Height = 100;
                    b.Width = 100;
                    #endregion

                    //Evento clic
                    b.Click += (s, e) =>
                    {
                        Button aux = new Button();
                        aux = (Button)e.Source;
                        if (pr.VarianteProducto.Count > 1)
                        {
                            Load_VarianteProducto(pr);
                        }
                        else
                        {
                            Load_LineaVenta(pr.VarianteProducto.First(c => c.Nombre.Equals(aux.Tag)));
                        }

                    };
                    WrapPanel_productos.Children.Add(b);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        void Load_VarianteProducto(Producto prod)
        {
            WrapPanel_productos.Children.Clear();

            try
            {
                foreach (var vp in prod.VarianteProducto.Where(c => u.ProductoRepository.GetAll().All(x => x.Nombre != c.Nombre)))
                {
                    #region formato boton
                    Button b = new Button();
                    StackPanel sp = new StackPanel();
                    Image img = new Image();
                    TextBlock tb = new TextBlock();
                    string rutaImagen = Environment.CurrentDirectory + "\\imagenes\\productos\\" + vp.RutaImagen;
                    if (File.Exists(rutaImagen))
                    {
                        b.Background = new ImageBrush(new BitmapImage(new Uri(rutaImagen, UriKind.Relative)));
                    }
                    b.Foreground = Brushes.Black;
                    tb.HorizontalAlignment = HorizontalAlignment.Stretch;
                    tb.Background = Brushes.White;
                    tb.VerticalAlignment = VerticalAlignment.Bottom;
                    tb.Text = vp.Nombre;
                    tb.Opacity = 0.6;
                    if (Math.Round((double)(190 / vp.Nombre.Count()), 0) < 20)
                    {
                        tb.FontSize = Math.Round((double)(190 / vp.Nombre.Count()), 0);
                    }
                    else
                    {
                        tb.FontSize = 20;
                    }

                    b.Padding = new Thickness(1);
                    b.Content = tb;
                    b.VerticalContentAlignment = VerticalAlignment.Bottom;
                    b.HorizontalContentAlignment = HorizontalAlignment.Center;
                    b.Tag = vp.Nombre;
                    b.Margin = new Thickness(10);
                    b.Height = 100;
                    b.Width = 100;
                    #endregion

                    //Evento clic
                    b.Click += (s, e) =>
                    {
                        Load_LineaVenta(vp);
                    };

                    WrapPanel_productos.Children.Add(b);
                }
            }
            catch (Exception er)
            { Console.WriteLine(er); }

        }

        void Load_LineaVenta(VarianteProducto varianteProducto)
        {

            if (mesa != null)
            {
                lv = new LineaVenta();

                lv.VarianteProducto = varianteProducto;

                if (unidades != null)
                {
                    lv.Unidades = Convert.ToInt32(unidades);
                }
                else
                {
                    unidades = 1.ToString();
                    lv.Unidades = Convert.ToInt32(unidades);
                }

                tv.LineaVenta.Add(lv);
                total += varianteProducto.Precio * lv.Unidades;
                txtblock_Total.Text = total.ToString();
                unidades = 1.ToString();
                txtblock_unidades.Text = unidades.ToString();
                RefrescarDatagridLineaVenta(varianteProducto);
                unidades = null;
            }
            else
            {
                Load_Mesas();
            }
        }

        void crear_Ticket()
        {
            if (tv.LineaVenta.Count > 0)
            {
                decimal total = 0;
                try
                {
                    total = (Convert.ToDecimal(txtblock_Total.Text) + tv.Mesa.IncrementoMesa);
                }
                catch (Exception)
                {
                    total = (Convert.ToDecimal(txtblock_Total.Text));
                }

                VentanaCambio vCambio = new VentanaCambio(total);
                vCambio.Owner = this;
                vCambio.Cambio += value => cambio = value;
                vCambio.ShowDialog();

                tv.LineaVenta.ToList().ForEach(i => i.VarianteProducto.Stock -= i.Unidades);
                tv.Usuario = ActiveUsr;
                tv.FechaHora = DateTime.Now;
                u.TicketVentaRepository.Create(tv);
                caja.DineroFinal += total;
                ticket ventanaTicket = new ticket(tv);
                if (listaTickets.Contains(tv))
                {
                    listaTickets.Remove(tv);
                }
                WrapPanel_productos.Children.Clear();
                resetCaja();
                string rutaImagen = Environment.CurrentDirectory + "/imagenes/ticketBackground.jpg";
                ventanaTicket.Background = new ImageBrush(new BitmapImage(new Uri(rutaImagen, UriKind.Relative)));
                ventanaTicket.ShowDialog();
            }
            else
            {
                AmRoMessageBox.ShowDialog("No tienes lineas de venta");
            }
        }

        private void button_nuevoTicket_Click(object sender, RoutedEventArgs e)
        {
            //TODO nuevo tickets
            if (!listaTickets.Contains(tv))
            {
                listaTickets.Add(tv);
            }
            WrapPanel_productos.Children.Clear();
            resetCaja();
        }

        private void button_select_mesa_Click(object sender, RoutedEventArgs e)
        {
            //TODO mostrar mesas
            Load_Mesas();
        }

        private void button_ticketsAbiertos_Click(object sender, RoutedEventArgs e)
        {
            //TODO implementar tickets sin cerrar


            WrapPanel_productos.Children.Clear();
            try
            {
                foreach (TicketVenta Tventa in listaTickets)
                {

                    #region formato boton

                    Button b = new Button();
                    StackPanel sp = new StackPanel();
                    Image img = new Image();
                    TextBlock tb = new TextBlock();
                    //b.Background = Brushes.WhiteSmoke;
                    tb.HorizontalAlignment = HorizontalAlignment.Stretch;
                    tb.Foreground = Brushes.White;
                    tb.VerticalAlignment = VerticalAlignment.Center;
                    tb.Text = "Ticket " + (listaTickets.ToList().IndexOf(Tventa) + 1);
                    if (Math.Round((double)(190 / tb.Text.Count()), 0) < 20)
                    {
                        tb.FontSize = Math.Round((double)(190 / tb.Text.Count()), 0);
                    }
                    else
                    {
                        tb.FontSize = 20;
                    }
                    b.Padding = new Thickness(1);
                    b.Content = tb;
                    b.VerticalContentAlignment = VerticalAlignment.Center;
                    b.HorizontalContentAlignment = HorizontalAlignment.Center;
                    b.Tag = tb.Text;
                    b.Margin = new Thickness(10);
                    b.Height = 100;
                    b.Width = 100;
                    #endregion

                    //Evento clic
                    b.Click += (s, e2) =>
                    {
                        //TODO mostrar lineas datagrid total
                        datagridLineaVenta.ItemsSource = Tventa.LineaVenta;
                        txtblock_Total.Text = Tventa.LineaVenta.Sum(c => (c.VarianteProducto.Precio * c.Unidades)).ToString();
                        tv = Tventa;
                    };
                    WrapPanel_productos.Children.Add(b);
                }
            }
            catch (Exception)
            {
                //throw;
            }
        }

        private void button_verClientes_Click(object sender, RoutedEventArgs e)
        {
            //Mostrar y buscar clientes
        }

        private void resetCaja()
        {
            tv = new TicketVenta();
            txtblock_numMesa.Text = "Sin mesa";
            txt_precioProducto.Text = 0.ToString();
            total = 0;
            txtblock_unidades.Text = 1.ToString();
            unidades = null;
            PasswordBox.Clear();
            RefrescarDatagridLineaVenta();
        }

        private void button_caja_click(object sender, RoutedEventArgs e)
        {
            crear_Ticket();
        }

        void habilitarPaneles(string tipoUsuario)
        {
            if (tipoUsuario.Equals("usuario") || tipoUsuario.Equals("admin") || tipoUsuario.Equals("gerente"))
            {

                grid_tpvSI.Visibility = Visibility.Visible;
                grid_tpvNO.Visibility = Visibility.Hidden;

                if (tipoUsuario.Equals("usuario") || tipoUsuario.Equals("gerente"))
                {
                    //TODO arqueo de caja
                    VentanaArqueoDeCaja vCambio = new VentanaArqueoDeCaja(u, ActiveUsr);
                    vCambio.Owner = this;
                    vCambio.ShowDialog();
                    caja = u.CajaRepository.GetAll().Last();
                }
            }
            else
            {
                grid_tpvSI.Visibility = Visibility.Hidden;
                grid_tpvNO.Visibility = Visibility.Visible;
            }

            if (tipoUsuario.Equals("admin"))
            {

                //Logueado como admin
                grid_adminNO.Visibility = Visibility.Hidden;
                grid_adminSI.Visibility = Visibility.Visible;

                datagridCategorias.ItemsSource = u.CategoriaRepository.GetAll();
                datagridProducto.ItemsSource = u.ProductoRepository.GetAll();
                datagridUsuarios.ItemsSource = u.VendedorRepository.GetAll();
                listProductoProveedor = new List<Producto>();

                cargarcomboboxImagenes();
                refreshComboBoxCategorias();
                refreshComboBoxCategoriasPadre();
                refreshComboBoxProductoPadre();
                refreshComboboxCatalogoProveedor();
                deshabilitarCamposCategorias();
                deshabilitarCamposProductos();
                deshabilitarCamposVarianteProductos();
                deshabilitarCamposUsuario();
                deshabilitarCamposSubCategorias();
                deshabilitarCamposProveedor();
                deshabilitarCamposMesa();
            }
            else
            {
                grid_adminNO.Visibility = Visibility.Visible;
                grid_adminSI.Visibility = Visibility.Hidden;
            }

            if (tipoUsuario.Equals("admin") || tipoUsuario.Equals("gerente"))
            {
                grid_pedidosSI.Visibility = Visibility.Visible;
                grid_pedidosNO.Visibility = Visibility.Hidden;
            }
            else
            {
                grid_pedidosSI.Visibility = Visibility.Hidden;
                grid_pedidosNO.Visibility = Visibility.Visible;
            }

        }

        #region Teclado Login

        #region numerosTeclado
        private void btn_1_Click(object sender, RoutedEventArgs e)
        {
            PasswordBox.Password += 1;
        }

        private void btn_2_Click(object sender, RoutedEventArgs e)
        {
            PasswordBox.Password += 2;
        }

        private void btn_3_Click(object sender, RoutedEventArgs e)
        {
            PasswordBox.Password += 3;
        }

        private void btn_4_Click(object sender, RoutedEventArgs e)
        {
            PasswordBox.Password += 4;
        }

        private void btn_5_Click(object sender, RoutedEventArgs e)
        {
            PasswordBox.Password += 5;
        }

        private void btn_6_Click(object sender, RoutedEventArgs e)
        {
            PasswordBox.Password += 6;
        }

        private void btn_7_Click(object sender, RoutedEventArgs e)
        {
            PasswordBox.Password += 7;
        }

        private void btn_8_Click(object sender, RoutedEventArgs e)
        {
            PasswordBox.Password += 8;
        }

        private void btn_9_Click(object sender, RoutedEventArgs e)
        {
            PasswordBox.Password += 9;
        }

        private void btn_0_Click(object sender, RoutedEventArgs e)
        {
            PasswordBox.Password += 0;
        }

        private void btn_c_Click(object sender, RoutedEventArgs e)
        {
            PasswordBox.Clear();
        }
        #endregion

        private void btn_log_Click(object sender, RoutedEventArgs e)
        {
            //logear usuario
            Console.WriteLine("Pin: " + PasswordBox.Password);
            bool valida = u.VendedorRepository.GetAll().Any(c => c.UsuarioId.ToString().Equals(idUsuario) && c.Pin.Equals(PasswordBox.Password));
            if (valida)
            {
                ActiveUsr = u.VendedorRepository.Get().SingleOrDefault(c => c.UsuarioId.ToString().Equals(idUsuario));
                habilitarPaneles(ActiveUsr.TipoUsuario);
                tab_tpv.IsSelected = true;
                resetCaja();
                chip_usuarioVenta.Content = ActiveUsr.Nombre + " " + ActiveUsr.Apellidos;
                string rutaImagen = Environment.CurrentDirectory + "\\imagenes\\usuarios\\" + ActiveUsr.RutaImagen;
                if (File.Exists(rutaImagen))
                {
                    chip_usuarioVenta.IconForeground = Brushes.Transparent;
                    chip_usuarioVenta.Icon = new ImageBrush(new BitmapImage(new Uri(rutaImagen, UriKind.Relative)));
                    chip_usuarioVenta.IconBackground = new ImageBrush(new BitmapImage(new Uri(rutaImagen, UriKind.Relative)));
                }
            }
            else
            {
                AmRoMessageBox.ShowDialog("Pin incorrecto");
            }

        }

        #endregion

        private void button_anularLineaVenta_Click(object sender, RoutedEventArgs e)
        {
            if (datagridLineaVenta.SelectedIndex > -1)
            {
                tv.LineaVenta.Remove((LineaVenta)datagridLineaVenta.SelectedItem);
            }
            else
            {
                try
                {
                    tv.LineaVenta.Remove(tv.LineaVenta.Last());
                }
                catch (Exception er)
                {
                    Console.WriteLine(er);
                }
            }
            try
            {

            }
            catch (Exception)
            {
                try
                {

                }
                catch (Exception er)
                {
                    Console.WriteLine(er);
                }

            }
            RefrescarDatagridLineaVenta();
        }

        private void chip_usuarioVenta_Click(object sender, RoutedEventArgs e)
        {
            //TODO cerrar caja
            cerrarCaja();
            cerrarSession();
        }

        private void RefrescarDatagridLineaVenta(VarianteProducto varprod = null)
        {
            datagridLineaVenta.ItemsSource = null;
            datagridLineaVenta.ItemsSource = tv.LineaVenta.Reverse().ToList();

            if (tv.LineaVenta.Count > 0)
            {
                if (varprod == null)
                {
                    total = tv.LineaVenta.Sum(c => c.VarianteProducto.Precio * c.Unidades);
                }
                else
                {
                    total = tv.LineaVenta.Sum(c => c.VarianteProducto.Precio * c.Unidades);
                }
            }
            else
            {
                total = 0;
            }
            txtblock_Total.Text = total.ToString();
        }

        private void limiparCamposProductos()
        {
            txt_descripcionProducto.Clear();
            txt_nombreProducto.Clear();
            txt_precioProducto.Clear();
            txt_StockInicial.Clear();
            comboboxIVA.SelectedIndex = -1;
            comboBox_CategoriasProducto.SelectedIndex = -1;
            comboBox_SubCategoriasProducto.SelectedIndex = -1;
        }

        void recibirCambio_EventHandler(object sender, RoutedEventArgs e)
        {
            //cambio;
        }

        #region teclado unidades

        private void btn_1c_Click(object sender, RoutedEventArgs e)
        {
            unidades += 1;
            txtblock_unidades.Text = unidades.ToString();
        }

        private void btn_2c_Click(object sender, RoutedEventArgs e)
        {
            unidades += 2;
            txtblock_unidades.Text = unidades.ToString();
        }

        private void btn_3c_Click(object sender, RoutedEventArgs e)
        {
            unidades += 3;
            txtblock_unidades.Text = unidades.ToString();
        }

        private void btn_4c_Click(object sender, RoutedEventArgs e)
        {
            unidades += 4;
            txtblock_unidades.Text = unidades.ToString();
        }

        private void btn_5c_Click(object sender, RoutedEventArgs e)
        {
            unidades += 5;
            txtblock_unidades.Text = unidades.ToString();
        }

        private void btn_6c_Click(object sender, RoutedEventArgs e)
        {
            unidades += 6;
            txtblock_unidades.Text = unidades.ToString();
        }

        private void btn_7c_Click(object sender, RoutedEventArgs e)
        {
            unidades += 7;
            txtblock_unidades.Text = unidades.ToString();
        }

        private void btn_8c_Click(object sender, RoutedEventArgs e)
        {
            unidades += 8;
            txtblock_unidades.Text = unidades.ToString();
        }

        private void btn_9c_Click(object sender, RoutedEventArgs e)
        {
            unidades += 9;
            txtblock_unidades.Text = unidades.ToString();
        }

        private void btn_0c_Click(object sender, RoutedEventArgs e)
        {
            unidades += 0;
            txtblock_unidades.Text = unidades.ToString();
        }

        private void btn_cc_Click(object sender, RoutedEventArgs e)
        {
            unidades = null;
            txtblock_unidades.Text = 1.ToString();
        }
        #endregion

        #endregion

        #region Zona Admin

        public void refreshDatagrids()
        {
            datagridmesas.ItemsSource = null;
            datagridmesas.ItemsSource = u.MesaRepository.GetAll();

            datagridpedidosRealizados.ItemsSource = null;
            datagridpedidosRealizados.ItemsSource = u.PedidoProveedorRepository.GetAll().Where(c => c.Recibido == false);

            datagridPedidos.ItemsSource = null;
            datagridPedidos.ItemsSource = u.VarianteProductoRepository.GetAll();

            datagridProveedores.ItemsSource = null;
            datagridProveedores.ItemsSource = u.ProveedorRepository.GetAll().Where(c => c.ProveedorId != 1);

            datagridCategorias.ItemsSource = null;
            datagridCategorias.ItemsSource = u.CategoriaRepository.GetAll();

            datagridProducto.ItemsSource = null;
            datagridProducto.ItemsSource = u.ProductoRepository.GetAll();

            datagridVarianteProducto.ItemsSource = null;
            datagridVarianteProducto.ItemsSource = u.VarianteProductoRepository.Get().Where(c => u.ProductoRepository.GetAll().All(x => x.Nombre != c.Nombre));

            datagridUsuarios.ItemsSource = null;
            datagridUsuarios.ItemsSource = u.VendedorRepository.GetAll();

            datagridSubCategorias.ItemsSource = null;
            datagridSubCategorias.ItemsSource = u.SubCategoriaRepository.Get().Where(c => u.CategoriaRepository.GetAll().All(x => x.Nombre != c.Nombre));

        }

        private void changeDataContext()
        {

            if (tabAdmin_Usuarios.IsSelected)
            {
                DataContext = usr;
            }

            if (tabAdmin_Productos.IsSelected)
            {
                //TODO diferenciar tab productos de variantes.
                DataContext = prod;
            }

            if (tabAdmin_Categorias.IsSelected)
            {
                //TODO diferenciar tab categorias de subcategorias
                DataContext = cat;
            }
        }

        private void cargarcomboboxImagenes()
        {
            combobox_imagenesCategorias.Items.Clear();
            combobox_imagenesSubCategorias.Items.Clear();
            foreach (var item in Directory.GetFiles(Environment.CurrentDirectory + "\\imagenes\\categorias\\").Select(System.IO.Path.GetFileName).ToList())
            {
                combobox_imagenesCategorias.Items.Add(item);
                combobox_imagenesSubCategorias.Items.Add(item);
            }
            comboBoxImagenProductos.Items.Clear();
            comboboxImagenUsuario.Items.Clear();
            foreach (var item in Directory.GetFiles(Environment.CurrentDirectory + "\\imagenes\\productos\\").Select(System.IO.Path.GetFileName).ToList())
            {
                comboBoxImagenProductos.Items.Add(item);
                comboBoxImagenVarianteProductos.Items.Add(item);
            }

            foreach (var item in Directory.GetFiles(Environment.CurrentDirectory + "\\imagenes\\usuarios\\").Select(System.IO.Path.GetFileName).ToList())
            {
                comboboxImagenUsuario.Items.Add(item);
            }

        }

        private void tabControl_Main_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (tab_admin.IsSelected)
            {
                changeDataContext();
            }

        }

        private void tabcontrol_administracion_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            changeDataContext();
        }

        private Boolean validado(Object obj)
        {

            ValidationContext validationContext = new ValidationContext(obj, null, null);
            List<System.ComponentModel.DataAnnotations.ValidationResult> errors = new List<System.ComponentModel.DataAnnotations.ValidationResult>();
            Validator.TryValidateObject(obj, validationContext, errors, true);
            if (errors.Count() > 0)
            {

                string mensageErrores = string.Empty;
                foreach (var error in errors)
                {
                    error.MemberNames.First();

                    mensageErrores += error.ErrorMessage + Environment.NewLine;
                }
                AmRoMessageBox.ShowDialog(mensageErrores, @"/!\ERROR/!\"); return false;
            }
            else
            {
                return true;
            }
        }

        private void btn_virtualKeyboard_Click(object sender, RoutedEventArgs e)
        {
            //ejecucion del teclado virtual de windows en sistema 64 bits
            string processName = System.IO.Path.GetFileNameWithoutExtension(OnScreenKeyboadApplication);
            var query = from process in Process.GetProcesses()
                        where process.ProcessName == processName
                        select process;

            var keyboardProcess = query.FirstOrDefault();
            if (keyboardProcess == null)
            {
                IntPtr ptr = new IntPtr(); ;
                bool sucessfullyDisabledWow64Redirect = false;
                if (System.Environment.Is64BitOperatingSystem)
                {
                    sucessfullyDisabledWow64Redirect = Wow64DisableWow64FsRedirection(ref ptr);
                }
                using (Process osk = new Process())
                {
                    osk.StartInfo.FileName = OnScreenKeyboadApplication;
                    osk.Start();
                    //  osk.WaitForInputIdle(2000);
                }
                if (System.Environment.Is64BitOperatingSystem)
                    if (sucessfullyDisabledWow64Redirect)
                        Wow64RevertWow64FsRedirection(ptr);
            }
            else
            {
                var windowHandle = keyboardProcess.MainWindowHandle;
                SendMessage(windowHandle, WM_SYSCOMMAND, new IntPtr(SC_RESTORE), new IntPtr(0));
            }
        }

        #region Gestion Categorias

        private void btn_ImagenCategoria_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "Tipo imágenes|*.bmp;*.jpg;*.jpeg;*.png;";
            openFileDialog1.FilterIndex = 1;
            bool? userClickedOK = openFileDialog1.ShowDialog();

            if (userClickedOK == true)
            {
                string rutaArchivo = openFileDialog1.FileName;
                string rutaDestino = Environment.CurrentDirectory + "\\imagenes\\categorias\\" + openFileDialog1.SafeFileName;
                if (!File.Exists(rutaDestino))
                {

                }
                if (File.Exists(rutaDestino))
                {

                    if (MsgPregunta("Ya hay un archivo con este nombre, Sobre escribir?", "Sobre Escribir archivo"))
                    {
                        File.Delete(rutaDestino);
                        File.Copy(rutaArchivo, rutaDestino);
                    }
                }
                else
                {
                    File.Copy(rutaArchivo, rutaDestino);
                }

                cargarcomboboxImagenes();
                BitmapImage logo = new BitmapImage();
                logo.BeginInit();
                logo.UriSource = new Uri(rutaDestino);
                logo.EndInit();
                img_categoria.Source = logo;
                imagen = openFileDialog1.SafeFileName;
                combobox_imagenesCategorias.Text = imagen;
            }
        }

        private void btn_guardarCategoria_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(imagen))
            {
                imagen = "default.jpg";
            }
            try
            {
                cat.Nombre = txtboxNombreCategoria.Text;
                cat.Descripcion = txtboxDescripcionCategoria.Text;
                cat.RutaImagen = imagen;

                subCat.Nombre = txtboxNombreCategoria.Text;
                subCat.Descripcion = txtboxDescripcionCategoria.Text;
                subCat.RutaImagen = imagen;
                subCat.Categoria = cat;

            }
            catch (Exception)
            {
            }

            if (validado(cat))
            {
                if (create)
                {
                    cat.SubCategoria.Add(subCat); //añadimos la subcategoria a categoria
                    if (!u.CategoriaRepository.GetAll().Any(c => c.Nombre.Equals(cat.Nombre)))
                    {
                        u.CategoriaRepository.Create(cat);

                        AmRoMessageBox.ShowDialog("Guardado correctamente");
                        deshabilitarCamposCategorias();
                        refreshComboBoxCategorias();
                        refreshComboBoxProductoPadre();
                        refreshDatagrids();
                        borrarCamposCategorias();
                        Load_Categorias();
                    }
                    else
                    {
                        AmRoMessageBox.ShowDialog("Ya existe una categoría con el mismo nombre");
                    }

                }
                else
                {
                    u.CategoriaRepository.Update(cat);
                    u.SubCategoriaRepository.Update(subCat);
                    AmRoMessageBox.ShowDialog("Guardado correctamente");
                    deshabilitarCamposCategorias();
                    refreshComboBoxCategorias();
                    refreshDatagrids();
                    borrarCamposCategorias();
                    Load_Categorias();
                }
                refreshComboBoxCategoriasPadre();
            }
        }

        private void btn_borrarCategoria_Click(object sender, RoutedEventArgs e)
        {
            if (datagridCategorias.SelectedItem != null)
            {
                if (MsgPregunta("¿Seguro que quieres eliminar esta categoría?", "Eliminar"))
                {
                    try
                    {
                        string rutaImagen = Environment.CurrentDirectory + "\\imagenes\\categorias\\" + cat.RutaImagen;

                        u.CategoriaRepository.Delete(cat);
                        refreshDatagrids();
                        Load_Categorias();
                        refreshComboBoxCategorias();
                        borrarCamposCategorias();

                    }
                    catch (Exception er)
                    {
                        Console.WriteLine(er);
                        AmRoMessageBox.ShowDialog("Error al intentar borrar");
                    }
                }
            }
            else
            {
                AmRoMessageBox.ShowDialog("Selecciona una categoría a borrar");
            }
        }

        private void datagridCategorias_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (datagridCategorias.SelectedIndex > -1)
            {
                create = false;
                cat = (Categoria)datagridCategorias.SelectedItem;
                subCat = u.SubCategoriaRepository.Get().Where(c => cat.Nombre.Equals(c.Nombre)).FirstOrDefault();
                txtboxDescripcionCategoria.Text = cat.Descripcion;
                txtboxNombreCategoria.Text = cat.Nombre;
                combobox_imagenesCategorias.Text = cat.RutaImagen;
                string rutaDestino = Environment.CurrentDirectory + "\\imagenes\\categorias\\" + cat.RutaImagen;
                try
                {
                    BitmapImage logo = new BitmapImage();
                    logo.BeginInit();
                    logo.UriSource = new Uri(rutaDestino);
                    logo.EndInit();
                    img_categoria.Source = logo;
                }
                catch (Exception er)
                {
                    Console.WriteLine(er);
                    img_categoria.Source = null;
                }
                habilitarCamposCategorias();
            }
        }

        private void btn_nuevaCategoria_Click(object sender, RoutedEventArgs e)
        {
            create = true;
            cat = new Categoria();
            subCat = new SubCategoria(); // crear primera subcategoria con los mismos datos que su padre.
            habilitarCamposCategorias();
            btn_borrarCategoria.IsEnabled = false;
            borrarCamposCategorias();
        }

        private void deshabilitarCamposCategorias()
        {
            txtboxDescripcionCategoria.IsEnabled = false;
            txtboxNombreCategoria.IsEnabled = false;
            btn_borrarCategoria.IsEnabled = false;
            btn_guardarCategoria.IsEnabled = false;
            btn_ImagenCategoria.IsEnabled = false;
            combobox_imagenesCategorias.IsEnabled = false;
        }

        private void habilitarCamposCategorias()
        {
            txtboxDescripcionCategoria.IsEnabled = true;
            txtboxNombreCategoria.IsEnabled = true;
            btn_borrarCategoria.IsEnabled = true;
            btn_guardarCategoria.IsEnabled = true;
            btn_ImagenCategoria.IsEnabled = true;
            combobox_imagenesCategorias.IsEnabled = true;
        }

        public void refreshComboBoxCategorias()
        {
            comboBox_CategoriasProducto.ItemsSource = null;
            comboBox_CategoriasProducto.ItemsSource = u.CategoriaRepository.GetAll();
            comboBox_CategoriasProducto.SelectedValuePath = "CategoriaId";
            comboBox_CategoriasProducto.DisplayMemberPath = "Nombre";
        }

        public void refreshComboBoxSubCategoriasProducto()
        {
            comboBox_SubCategoriasProducto.ItemsSource = null;
            List<SubCategoria> aux = new List<SubCategoria>();
            aux = u.SubCategoriaRepository.Get().Where(c => c.Categoria.Equals((Categoria)comboBox_CategoriasProducto.SelectedItem)).ToList();
            aux.RemoveAt(0);
            comboBox_SubCategoriasProducto.ItemsSource = aux;
            comboBox_SubCategoriasProducto.SelectedValuePath = "SubCategoriaId";
            comboBox_SubCategoriasProducto.DisplayMemberPath = "Nombre";
            comboBox_SubCategoriasProducto.IsEnabled = true;
        }

        private void borrarCamposCategorias()
        {
            combobox_imagenesCategorias.SelectedIndex = -1;
            imagen = null;
            txtboxDescripcionCategoria.Clear();
            txtboxNombreCategoria.Clear();
            img_categoria.Source = null;
        }

        private void combobox_imagenesCategorias_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (combobox_imagenesCategorias.SelectedIndex > -1)
            {
                imagen = null;
                try
                {
                    BitmapImage logo = new BitmapImage();
                    logo.BeginInit();
                    logo.UriSource = new Uri(Environment.CurrentDirectory + "\\imagenes\\categorias\\" + combobox_imagenesCategorias.SelectedItem.ToString());
                    logo.EndInit();
                    img_categoria.Source = logo;
                    imagen = combobox_imagenesCategorias.SelectedItem.ToString();
                }
                catch (Exception er)
                {
                    Console.WriteLine(er);
                }
            }
        }

        #endregion

        #region Gestion SubCategoria

        private void btn_ImagenSubCategoria_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "Tipo imágenes|*.bmp;*.jpg;*.jpeg;*.png;";
            openFileDialog1.FilterIndex = 1;
            bool? userClickedOK = openFileDialog1.ShowDialog();

            if (userClickedOK == true)
            {
                string rutaArchivo = openFileDialog1.FileName;
                string rutaDestino = Environment.CurrentDirectory + "\\imagenes\\categorias\\" + openFileDialog1.SafeFileName;
                if (!File.Exists(rutaDestino))
                {

                }
                if (File.Exists(rutaDestino))
                {

                    if (MsgPregunta("Ya hay un archivo con este nombre, Sobre escribir?", "Sobre Escribir archivo"))
                    {
                        File.Delete(rutaDestino);
                        File.Copy(rutaArchivo, rutaDestino);
                    }
                }
                else
                {
                    File.Copy(rutaArchivo, rutaDestino);
                }

                cargarcomboboxImagenes();
                BitmapImage logo = new BitmapImage();
                logo.BeginInit();
                logo.UriSource = new Uri(rutaDestino);
                logo.EndInit();
                img_Subcategoria.Source = logo;
                imagen = openFileDialog1.SafeFileName;
                combobox_imagenesSubCategorias.Text = imagen;
            }
        }

        private void btn_guardarSubCategoria_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(imagen))
            {
                imagen = "default.jpg";
            }
            try
            {
                subCat.Nombre = txtboxNombreSubCategoria.Text;
                subCat.Descripcion = txtboxDescripcionSubCategoria.Text;
                subCat.Categoria = (Categoria)combobox_CategoriaPadre.SelectedItem;
                subCat.RutaImagen = imagen;
            }
            catch (Exception er)
            {
                Console.WriteLine(er);
            }

            if (validado(subCat))
            {
                if (create)
                {
                    u.SubCategoriaRepository.Create(subCat);
                    AmRoMessageBox.ShowDialog("Guardado correctamente");
                    deshabilitarCamposSubCategorias();
                    refreshComboBoxCategoriasPadre();
                    refreshDatagrids();
                    borrarCamposSubCategorias();
                    Load_Categorias();
                }
                else
                {
                    u.SubCategoriaRepository.Update(subCat);

                    AmRoMessageBox.ShowDialog("Guardado correctamente");
                    deshabilitarCamposSubCategorias();
                    refreshDatagrids();
                    borrarCamposSubCategorias();
                    Load_Categorias();
                }
            }
        }

        private void btn_borrarSubCategoria_Click(object sender, RoutedEventArgs e)
        {
            if (MsgPregunta("¿Seguro que quieres eliminar esta subcategoría?", "Eliminar"))
            {
                try
                {
                    string rutaImagen = Environment.CurrentDirectory + "\\imagenes\\categorias\\" + subCat.RutaImagen;

                    u.SubCategoriaRepository.Delete(subCat);
                    refreshDatagrids();
                    refreshComboBoxCategoriasPadre();
                    borrarCamposSubCategorias();

                }
                catch (Exception er)
                {
                    Console.WriteLine(er);
                    AmRoMessageBox.ShowDialog("Error al intentar borrar");
                }
            }
            else
            {
                AmRoMessageBox.ShowDialog("Selecciona una subcategoría a borrar");
            }
        }

        private void btn_nuevaSubCategoria_Click(object sender, RoutedEventArgs e)
        {
            create = true;
            cat = new Categoria();
            habilitarCamposSubCategorias();
            btn_borrarSubCategoria.IsEnabled = false;
            borrarCamposSubCategorias();
        }

        private void refreshComboBoxCategoriasPadre()
        {
            combobox_CategoriaPadre.ItemsSource = null;
            combobox_CategoriaPadre.ItemsSource = u.CategoriaRepository.GetAll();
            combobox_CategoriaPadre.DisplayMemberPath = "Nombre";
        }

        private void deshabilitarCamposSubCategorias()
        {
            txtboxDescripcionSubCategoria.IsEnabled = false;
            txtboxNombreSubCategoria.IsEnabled = false;
            btn_borrarSubCategoria.IsEnabled = false;
            btn_guardarSubCategoria.IsEnabled = false;
            btn_ImagenSubCategoria.IsEnabled = false;
            combobox_imagenesSubCategorias.IsEnabled = false;
            combobox_CategoriaPadre.IsEnabled = false;
        }

        private void habilitarCamposSubCategorias()
        {
            txtboxDescripcionSubCategoria.IsEnabled = true;
            txtboxNombreSubCategoria.IsEnabled = true;
            btn_borrarSubCategoria.IsEnabled = true;
            btn_guardarSubCategoria.IsEnabled = true;
            btn_ImagenSubCategoria.IsEnabled = true;
            combobox_imagenesSubCategorias.IsEnabled = true;
            combobox_CategoriaPadre.IsEnabled = true;
        }

        private void borrarCamposSubCategorias()
        {
            combobox_imagenesSubCategorias.SelectedIndex = -1;
            combobox_CategoriaPadre.SelectedIndex = -1;
            imagen = null;
            txtboxDescripcionSubCategoria.Clear();
            txtboxNombreSubCategoria.Clear();
            img_Subcategoria.Source = null;
        }

        private void datagridSubCategorias_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            if (datagridSubCategorias.SelectedIndex > -1)
            {
                create = false;
                subCat = (SubCategoria)datagridSubCategorias.SelectedItem;
                txtboxDescripcionSubCategoria.Text = subCat.Descripcion;
                txtboxNombreSubCategoria.Text = subCat.Nombre;
                combobox_imagenesSubCategorias.Text = subCat.RutaImagen;
                combobox_CategoriaPadre.SelectedItem = subCat.Categoria;
                string rutaDestino = Environment.CurrentDirectory + "\\imagenes\\categorias\\" + subCat.RutaImagen;
                try
                {
                    BitmapImage logo = new BitmapImage();
                    logo.BeginInit();
                    logo.UriSource = new Uri(rutaDestino);
                    logo.EndInit();
                    img_Subcategoria.Source = logo;
                }
                catch (Exception er)
                {
                    Console.WriteLine(er);
                    img_Subcategoria.Source = null;
                }
                habilitarCamposSubCategorias();
            }
        }

        private void combobox_imagenesSubCategorias_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (combobox_imagenesSubCategorias.SelectedIndex > -1)
            {
                imagen = null;
                try
                {
                    BitmapImage logo = new BitmapImage();
                    logo.BeginInit();
                    logo.UriSource = new Uri(Environment.CurrentDirectory + "\\imagenes\\categorias\\" + combobox_imagenesSubCategorias.SelectedItem.ToString());
                    logo.EndInit();
                    img_Subcategoria.Source = logo;
                    imagen = combobox_imagenesSubCategorias.SelectedItem.ToString();
                }
                catch (Exception er)
                {
                    Console.WriteLine(er);
                }
            }
        }

        #endregion

        #region Gestion Productos 

        private void btn_ImagenProducto_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "Tipo imágenes|*.bmp;*.jpg;*.jpeg;*.png;";
            openFileDialog1.FilterIndex = 1;
            bool? userClickedOK = openFileDialog1.ShowDialog();

            if (userClickedOK == true)
            {
                // Open the selected file to read.
                string rutaArchivo = openFileDialog1.FileName;
                string rutaDestino = Environment.CurrentDirectory + "\\imagenes\\productos\\" + openFileDialog1.SafeFileName;
                if (File.Exists(rutaDestino))
                {
                    if (MsgPregunta("Ya hay un archivo con este nombre, Sobre escribir?", "Sobre Escribir archivo"))
                    {
                        try
                        {
                            File.Delete(rutaDestino);
                            File.Copy(rutaArchivo, rutaDestino);
                        }
                        catch (Exception er)
                        {
                            Console.WriteLine(er);
                        }
                    }

                }
                else
                {
                    File.Copy(rutaArchivo, rutaDestino);
                }

                cargarcomboboxImagenes();
                BitmapImage logo = new BitmapImage();
                logo.BeginInit();
                logo.UriSource = new Uri(rutaDestino);
                logo.EndInit();
                img_Producto.Source = logo;
                imagen = openFileDialog1.SafeFileName;
                comboBoxImagenProductos.Text = imagen;
            }
        }

        private void btn_guardarProducto_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(imagen))
            {
                imagen = "default.jpg";
            }

            try
            {
                int iva = 0;
                decimal price = 0;
                int stock = 0;
                prod.Nombre = txt_nombreProducto.Text;
                prod.Descripcion = txt_descripcionProducto.Text;
                prod.RutaImagen = imagen;
                prod.Proveedor = u.ProveedorRepository.GetAll().Find(c => c.ProveedorId == 1); //proveedor por defecto
                Int32.TryParse(comboboxIVA.Text, out iva);
                Decimal.TryParse(txt_precioProducto.Text.Replace('.', ','), out price);
                Int32.TryParse(txt_StockInicial.Text, out stock);
                prod.Iva = iva;
                prod.Precio = price;
                prod.Stock = stock;

                //creacion de la variante producto padre.
                varProd.Nombre = txt_nombreProducto.Text;
                varProd.Descripcion = txt_descripcionProducto.Text;
                varProd.RutaImagen = imagen;
                varProd.Precio = price;
                varProd.Stock = stock;


                if (comboBox_SubCategoriasProducto.IsEnabled)
                {
                    prod.SubCategoria = (SubCategoria)comboBox_SubCategoriasProducto.SelectedItem;
                }
                else
                {
                    prod.SubCategoria = u.SubCategoriaRepository.Get().First(c => c.Nombre.Equals(comboBox_CategoriasProducto.Text));
                }
            }
            catch (Exception er) { Console.WriteLine(er); }

            if (validado(prod))
            {

                if (create)
                {
                    prod.VarianteProducto.Add(varProd);
                    if (!u.ProductoRepository.GetAll().Any(c => c.Nombre.Equals(prod.Nombre)))
                    {
                        u.ProductoRepository.Create(prod);
                        AmRoMessageBox.ShowDialog("Guardado correctamente");
                        deshabilitarCamposProductos();
                        limiparCamposProductos();
                        img_Producto.Source = null;
                        refreshComboBoxProductoPadre();
                        Load_Categorias();
                        refreshDatagrids();
                    }
                    else
                    {
                        AmRoMessageBox.ShowDialog("Ya existe un producto con el mismo nombre");
                    }
                }
                else
                {
                    u.ProductoRepository.Update(prod);
                    u.VarianteProductoRepository.Update(varProd);
                    AmRoMessageBox.ShowDialog("Guardado correctamente");
                    deshabilitarCamposProductos();
                    limiparCamposProductos();
                    img_Producto.Source = null;
                    refreshComboBoxProductoPadre();
                    Load_Categorias();
                    refreshDatagrids();
                }

                refreshComboboxCatalogoProveedor();
            }
        }

        private void btn_borrarProducto_Click(object sender, RoutedEventArgs e)
        {
            if (datagridProducto.SelectedItem != null)
            {
                if (MsgPregunta("¿Seguro que quieres eliminar este producto?", "Eliminar"))
                {
                    try
                    {
                        string rutaImagen = Environment.CurrentDirectory + "\\imagenes\\productos\\" + prod.RutaImagen;
                        u.ProductoRepository.Delete(prod);
                        refreshComboBoxProductoPadre();
                        refreshDatagrids();
                        Load_Categorias();
                    }
                    catch (Exception er)
                    {
                        Console.WriteLine(er);
                        AmRoMessageBox.ShowDialog("Error al intentar borrar");
                    }
                }
            }
            else
            {
                AmRoMessageBox.ShowDialog("Selecciona un producto a borrar");
            }
        }

        private void btn_nuevoProducto_Click(object sender, RoutedEventArgs e)
        {
            create = true;
            prod = new Producto();
            varProd = new VarianteProducto();
            habilitarCamposProductos();
            btn_borrarProducto.IsEnabled = false;
            comboBox_SubCategoriasProducto.IsEnabled = false;
            limiparCamposProductos();
            img_Producto.Source = null;
            datagridProducto.SelectedIndex = -1;
        }

        private void datagridProducto_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (datagridProducto.SelectedIndex > -1)
            {
                create = false;
                prod = (Producto)datagridProducto.SelectedItem;
                varProd = u.VarianteProductoRepository.Get().FirstOrDefault(c => prod.Nombre.Equals(c.Nombre));
                txt_descripcionProducto.Text = prod.Descripcion;
                txt_nombreProducto.Text = prod.Nombre;
                txt_precioProducto.Text = prod.Precio.ToString();
                txt_StockInicial.Text = prod.Stock.ToString();
                comboBoxImagenProductos.Text = prod.RutaImagen;
                comboboxIVA.Text = prod.Iva.ToString();
                imagen = prod.RutaImagen;
                try
                {
                    comboBox_CategoriasProducto.Text = prod.SubCategoria.Categoria.Nombre;
                    if (prod.SubCategoria.Categoria.Nombre != prod.SubCategoria.Nombre)
                    {
                        comboBox_SubCategoriasProducto.IsEnabled = true;
                        comboBox_SubCategoriasProducto.Text = prod.SubCategoria.Nombre;
                    }
                    else
                    {
                        comboBox_SubCategoriasProducto.IsEnabled = false;
                    }

                }
                catch (Exception er)
                {
                    Console.WriteLine(er);
                    comboBox_CategoriasProducto.Text = "";
                }
                string rutaDestino = Environment.CurrentDirectory + "\\imagenes\\productos\\" + prod.RutaImagen;
                try
                {
                    BitmapImage logo = new BitmapImage();
                    logo.BeginInit();
                    logo.UriSource = new Uri(rutaDestino);
                    logo.EndInit();
                    img_Producto.Source = logo;
                }
                catch (Exception er)
                {
                    Console.WriteLine(er);
                    img_Producto.Source = null;
                }

                habilitarCamposProductos();
            }
        }

        private void deshabilitarCamposProductos()
        {
            btn_ImagenProducto.IsEnabled = false;
            btn_guardarProducto.IsEnabled = false;
            btn_borrarProducto.IsEnabled = false;
            txt_descripcionProducto.IsEnabled = false;
            txt_nombreProducto.IsEnabled = false;
            txt_precioProducto.IsEnabled = false;
            txt_StockInicial.IsEnabled = false;
            comboboxIVA.IsEnabled = false;
            comboBox_CategoriasProducto.IsEnabled = false;
            comboBox_SubCategoriasProducto.IsEnabled = false;
            comboBoxImagenProductos.IsEnabled = false;
        }

        private void habilitarCamposProductos()
        {
            btn_ImagenProducto.IsEnabled = true;
            btn_guardarProducto.IsEnabled = true;
            btn_borrarProducto.IsEnabled = true;
            txt_descripcionProducto.IsEnabled = true;
            txt_nombreProducto.IsEnabled = true;
            txt_precioProducto.IsEnabled = true;
            txt_StockInicial.IsEnabled = true;
            comboboxIVA.IsEnabled = true;
            comboBox_CategoriasProducto.IsEnabled = true;
            comboBoxImagenProductos.IsEnabled = true;
        }

        private void comboBoxImagenProductos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (comboBoxImagenProductos.SelectedIndex > -1)
            {
                imagen = null;
                try
                {
                    BitmapImage logo = new BitmapImage();
                    logo.BeginInit();
                    logo.UriSource = new Uri(Environment.CurrentDirectory + "\\imagenes\\productos\\" + comboBoxImagenProductos.SelectedItem.ToString());
                    logo.EndInit();
                    img_Producto.Source = logo;
                    imagen = comboBoxImagenProductos.SelectedItem.ToString();
                }
                catch (Exception er)
                {
                    Console.WriteLine(er);
                }
            }
        }


        #endregion

        #region VarianteProductos

        private void datagridVarianteProducto_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (datagridVarianteProducto.SelectedIndex > -1)
            {
                create = false;
                varProd = (VarianteProducto)datagridVarianteProducto.SelectedItem;
                comboBox_ProductoPadre.Text = varProd.Producto.Nombre;
                txt_descripcionVarianteProducto.Text = varProd.Descripcion;
                txt_nombreVarianteProducto.Text = varProd.Nombre;
                txt_precioVarianteProducto.Text = varProd.Precio.ToString();
                txt_StockInicialVariante.Text = varProd.Stock.ToString();
                comboBoxImagenVarianteProductos.Text = varProd.RutaImagen;
                imagen = varProd.RutaImagen;

                string rutaDestino = Environment.CurrentDirectory + "\\imagenes\\productos\\" + varProd.RutaImagen;
                try
                {
                    BitmapImage logo = new BitmapImage();
                    logo.BeginInit();
                    logo.UriSource = new Uri(rutaDestino);
                    logo.EndInit();
                    img_VarianteProducto.Source = logo;
                }
                catch (Exception er)
                {
                    Console.WriteLine(er);
                    img_VarianteProducto.Source = null;
                }

                habilitarCamposVarianteProductos();
            }
        }

        private void refreshComboBoxProductoPadre()
        {
            comboBox_ProductoPadre.ItemsSource = null;
            comboBox_ProductoPadre.ItemsSource = u.ProductoRepository.GetAll();
            comboBox_ProductoPadre.SelectedValuePath = "ProductoId";
            comboBox_ProductoPadre.DisplayMemberPath = "Nombre";
        }

        private void btn_ImagenVarianteProducto_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "Tipo imágenes|*.bmp;*.jpg;*.jpeg;*.png;";
            openFileDialog1.FilterIndex = 1;
            bool? userClickedOK = openFileDialog1.ShowDialog();

            if (userClickedOK == true)
            {
                // Open the selected file to read.
                string rutaArchivo = openFileDialog1.FileName;
                string rutaDestino = Environment.CurrentDirectory + "\\imagenes\\productos\\" + openFileDialog1.SafeFileName;
                if (File.Exists(rutaDestino))
                {
                    if (MsgPregunta("Ya hay un archivo con este nombre, Sobre escribir?", "Sobre Escribir archivo"))
                    {
                        try
                        {
                            File.Delete(rutaDestino);
                            File.Copy(rutaArchivo, rutaDestino);
                        }
                        catch (Exception er)
                        {
                            Console.WriteLine(er);
                        }
                    }
                }
                else
                {
                    File.Copy(rutaArchivo, rutaDestino);
                }

                cargarcomboboxImagenes();
                BitmapImage logo = new BitmapImage();
                logo.BeginInit();
                logo.UriSource = new Uri(rutaDestino);
                logo.EndInit();
                img_VarianteProducto.Source = logo;
                imagen = openFileDialog1.SafeFileName;
                comboBoxImagenVarianteProductos.Text = imagen;
            }
        }

        private void comboBoxImagenVarianteProductos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (comboBoxImagenVarianteProductos.SelectedIndex > -1)
            {
                imagen = null;
                try
                {
                    BitmapImage logo = new BitmapImage();
                    logo.BeginInit();
                    logo.UriSource = new Uri(Environment.CurrentDirectory + "\\imagenes\\productos\\" + comboBoxImagenVarianteProductos.SelectedItem.ToString());
                    logo.EndInit();
                    img_VarianteProducto.Source = logo;
                    imagen = comboBoxImagenVarianteProductos.SelectedItem.ToString();
                }
                catch (Exception er)
                {
                    Console.WriteLine(er);
                }
            }
        }

        private void limiparCamposVarianteProductos()
        {
            txt_descripcionVarianteProducto.Clear();
            txt_nombreVarianteProducto.Clear();
            txt_precioVarianteProducto.Clear();
            txt_StockInicialVariante.Clear();
            comboBox_ProductoPadre.SelectedIndex = -1;
            comboBoxImagenVarianteProductos.SelectedIndex = -1;
        }

        private void btn_guardarVarianteProducto_Click(object sender, RoutedEventArgs e)
        {
            string nombreVariante = null;
            if (String.IsNullOrEmpty(imagen))
            {
                imagen = "default.jpg";
            }

            try
            {
                decimal price = 0;
                int stock = 0;

                varProd.Descripcion = txt_descripcionVarianteProducto.Text;
                varProd.RutaImagen = imagen;
                varProd.Producto = (Producto)comboBox_ProductoPadre.SelectedItem;
                varProd.Nombre = txt_nombreVarianteProducto.Text;
                Decimal.TryParse(txt_precioVarianteProducto.Text.Replace('.', ','), out price);
                Int32.TryParse(txt_StockInicialVariante.Text, out stock);
                varProd.Precio = price;
                varProd.Stock = stock;
            }
            catch (Exception er) { Console.WriteLine(er); }

            //TODO FODY VALIDACIONES VARIANTES
            //if (validado(prod))
            // {
            if (create)
            {
                if (!u.VarianteProductoRepository.GetAll().Any(c => c.Nombre.Equals(nombreVariante)))
                {
                    u.VarianteProductoRepository.Create(varProd);
                    AmRoMessageBox.ShowDialog("Guardado correctamente");
                    deshabilitarCamposVarianteProductos();
                    limiparCamposVarianteProductos();
                    img_VarianteProducto.Source = null;
                    Load_Categorias();
                    refreshDatagrids();
                    refreshComboBoxProductoPadre();
                }
                else
                {
                    AmRoMessageBox.ShowDialog("Ya existe una variante con el mismo nombre");
                }
            }
            else
            {
                u.VarianteProductoRepository.Update(varProd);
                AmRoMessageBox.ShowDialog("Guardado correctamente");
                deshabilitarCamposVarianteProductos();
                limiparCamposVarianteProductos();
                img_VarianteProducto.Source = null;
                Load_Categorias();
                refreshDatagrids();
                refreshComboBoxProductoPadre();
            }
            //   }
        }

        private void btn_borrarVarianteProducto_Click(object sender, RoutedEventArgs e)
        {
            if (datagridVarianteProducto.SelectedItem != null)
            {
                if (MsgPregunta("¿Seguro que quieres eliminar este producto?", "Eliminar"))
                {
                    try
                    {
                        string rutaImagen = Environment.CurrentDirectory + "\\imagenes\\productos\\" + varProd.RutaImagen;
                        u.VarianteProductoRepository.Delete(varProd);
                        refreshDatagrids();
                        Load_Categorias();
                    }
                    catch (Exception er)
                    {
                        Console.WriteLine(er);
                        AmRoMessageBox.ShowDialog("Error al intentar borrar");
                    }
                }
            }
            else
            {
                AmRoMessageBox.ShowDialog("Selecciona un producto a borrar");
            }
        }

        private void btn_nuevoVarianteProducto_Click(object sender, RoutedEventArgs e)
        {
            create = true;
            varProd = new VarianteProducto();
            habilitarCamposVarianteProductos();
            btn_borrarVarianteProducto.IsEnabled = false;
            limiparCamposVarianteProductos();
            img_VarianteProducto.Source = null;
            datagridVarianteProducto.SelectedIndex = -1;
        }

        private void deshabilitarCamposVarianteProductos()
        {
            btn_ImagenVarianteProducto.IsEnabled = false;
            btn_guardarVarianteProducto.IsEnabled = false;
            btn_borrarVarianteProducto.IsEnabled = false;
            txt_descripcionVarianteProducto.IsEnabled = false;
            txt_nombreVarianteProducto.IsEnabled = false;
            txt_precioVarianteProducto.IsEnabled = false;
            txt_StockInicialVariante.IsEnabled = false;
            comboBox_ProductoPadre.IsEnabled = false;
            comboBoxImagenVarianteProductos.IsEnabled = false;
        }

        private void habilitarCamposVarianteProductos()
        {
            btn_ImagenVarianteProducto.IsEnabled = true;
            btn_guardarVarianteProducto.IsEnabled = true;
            btn_borrarVarianteProducto.IsEnabled = true;
            txt_descripcionVarianteProducto.IsEnabled = true;
            txt_nombreVarianteProducto.IsEnabled = true;
            txt_precioVarianteProducto.IsEnabled = true;
            txt_StockInicialVariante.IsEnabled = true;
            comboBox_ProductoPadre.IsEnabled = true;
            comboBoxImagenVarianteProductos.IsEnabled = true;
        }

        private void comboBox_CategoriasProducto_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (comboBox_CategoriasProducto.SelectedIndex > -1)
            {
                cat = (Categoria)comboBox_CategoriasProducto.SelectedItem;
                if (cat.SubCategoria.Count > 1)
                {
                    refreshComboBoxSubCategoriasProducto();
                }
                else
                {
                    comboBox_SubCategoriasProducto.SelectedIndex = -1;
                    comboBox_SubCategoriasProducto.IsEnabled = false;
                }
            }
        }

        #endregion

        #region Gestion Usuarios

        private void btn_ImagenUsuario_Click(object sender, RoutedEventArgs e)
        {
            {
                OpenFileDialog openFileDialog1 = new OpenFileDialog();
                openFileDialog1.Filter = "Tipo imágenes|*.bmp;*.jpg;*.jpeg;*.png;";
                openFileDialog1.FilterIndex = 1;
                bool? userClickedOK = openFileDialog1.ShowDialog();

                if (userClickedOK == true)
                {
                    // Open the selected file to read.
                    string rutaArchivo = openFileDialog1.FileName;
                    string rutaDestino = Environment.CurrentDirectory + "\\imagenes\\usuarios\\" + openFileDialog1.SafeFileName;
                    if (!File.Exists(rutaDestino))
                    {

                    }
                    if (File.Exists(rutaDestino))
                    {
                        if (MsgPregunta("Ya hay un archivo con este nombre, Sobre escribir?", "Sobre Escribir archivo"))
                        {
                            File.Delete(rutaDestino);
                            File.Copy(rutaArchivo, rutaDestino);
                        }

                    }
                    else
                    {
                        File.Copy(rutaArchivo, rutaDestino);
                    }

                    cargarcomboboxImagenes();
                    BitmapImage logo = new BitmapImage();
                    logo.BeginInit();
                    logo.UriSource = new Uri(rutaDestino);
                    logo.EndInit();
                    img_Usuario.Source = logo;
                    imagen = openFileDialog1.SafeFileName;
                    comboboxImagenUsuario.Text = imagen;
                }
            }
        }

        private void btn_guardarUsuario_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(imagen))
            {
                imagen = "default.jpg";
            }

            try
            {
                usr.Nombre = txt_nombreUsuario.Text;
                usr.Apellidos = txt_apellidosUsuario.Text;
                usr.RutaImagen = imagen;
                usr.Login = txt_loginUsuario.Text;
                usr.Pin = txt_pinUsuario.Text;
                usr.Email = txt_emailUsuario.Text;
                usr.TipoUsuario = comboBox_TipoUsuario.Text;
            }
            catch (Exception er) { Console.WriteLine(er); }

            if (validado(usr))
            {

                if (create)
                {
                    if (!u.VendedorRepository.GetAll().Any(c => c.Login.Equals(usr.Login)))
                    {
                        u.VendedorRepository.Create(usr);
                        AmRoMessageBox.ShowDialog("Guardado correctamente");
                        deshabilitarCamposUsuario();
                        limpiarCamposUsuario();
                        Load_Usuarios();
                        refreshDatagrids();
                    }
                    else
                    {
                        AmRoMessageBox.ShowDialog("Ya existe un usuario con el mismo Login");
                    }
                }
                else
                {
                    u.VendedorRepository.Update(usr);
                    AmRoMessageBox.ShowDialog("Guardado correctamente");
                    deshabilitarCamposUsuario();
                    limpiarCamposUsuario();
                    Load_Usuarios();
                    refreshDatagrids();
                }
            }
        }

        private void btn_borrarUsuario_Click(object sender, RoutedEventArgs e)
        {
            if (datagridUsuarios.SelectedItem != null)
            {
                if ((Usuario)datagridUsuarios.SelectedItem != ActiveUsr)
                {
                    if (MsgPregunta("¿Seguro que quieres eliminar este usuario?", "Eliminar"))
                    {
                        try
                        {
                            u.VendedorRepository.Delete(usr);
                            refreshDatagrids();
                            Load_Usuarios();
                            limpiarCamposUsuario();
                        }
                        catch (Exception er)
                        {
                            Console.WriteLine(er);
                            AmRoMessageBox.ShowDialog("Error al intentar borrar", "Error");
                        }
                    }
                }
                else { AmRoMessageBox.ShowDialog("No puede borrar el usuario, está actualmente activo", "Error"); }
            }
            else
            {
                AmRoMessageBox.ShowDialog("Selecciona un usuario a borrar", "Error");
            }
        }

        private void btn_nuevoUsuario_Click(object sender, RoutedEventArgs e)
        {
            create = true;
            usr = new Usuario();
            habilitarCamposUsuario();
            btn_borrarUsuario.IsEnabled = false;
            limpiarCamposUsuario();
            img_Usuario.Source = null;
            datagridUsuarios.SelectedIndex = -1;
            comboBox_TipoUsuario.SelectedIndex = 0;
        }

        private void limpiarCamposUsuario()
        {
            txt_emailUsuario.Clear();
            txt_loginUsuario.Clear();
            txt_nombreUsuario.Clear();
            txt_apellidosUsuario.Clear();
            txt_pinUsuario.Clear();
            comboBox_TipoUsuario.SelectedIndex = -1;
            comboboxImagenUsuario.SelectedIndex = -1;
            //imagen = null;
            //img_Usuario = null;

        }

        private void deshabilitarCamposUsuario()
        {
            btn_ImagenUsuario.IsEnabled = false;
            btn_guardarUsuario.IsEnabled = false;
            btn_borrarUsuario.IsEnabled = false;
            txt_emailUsuario.IsEnabled = false;
            txt_loginUsuario.IsEnabled = false;
            txt_nombreUsuario.IsEnabled = false;
            txt_apellidosUsuario.IsEnabled = false;
            txt_pinUsuario.IsEnabled = false;
            comboBox_TipoUsuario.IsEnabled = false;
            comboboxImagenUsuario.IsEnabled = false;
        }

        private void habilitarCamposUsuario()
        {
            btn_ImagenUsuario.IsEnabled = true;
            btn_guardarUsuario.IsEnabled = true;
            btn_borrarUsuario.IsEnabled = true;
            txt_emailUsuario.IsEnabled = true;
            txt_loginUsuario.IsEnabled = true;
            txt_nombreUsuario.IsEnabled = true;
            txt_apellidosUsuario.IsEnabled = true;
            txt_pinUsuario.IsEnabled = true;
            comboBox_TipoUsuario.IsEnabled = true;
            comboboxImagenUsuario.IsEnabled = true;
        }

        private void datagridUsuarios_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (datagridUsuarios.SelectedIndex > -1)
            {
                create = false;
                usr = (Usuario)datagridUsuarios.SelectedItem;
                txt_nombreUsuario.Text = usr.Nombre;
                txt_apellidosUsuario.Text = usr.Apellidos;
                txt_loginUsuario.Text = usr.Login;
                txt_pinUsuario.Text = usr.Pin;
                comboboxImagenUsuario.Text = usr.RutaImagen;
                comboBox_TipoUsuario.Text = usr.TipoUsuario;
                imagen = usr.RutaImagen;
                try
                {
                    txt_emailUsuario.Text = usr.Email;
                }
                catch (Exception er)
                {
                    Console.WriteLine(er);
                    txt_emailUsuario.Text = "";
                }

                string rutaDestino = Environment.CurrentDirectory + "\\imagenes\\usuarios\\" + usr.RutaImagen;
                try
                {
                    BitmapImage logo = new BitmapImage();
                    logo.BeginInit();
                    logo.UriSource = new Uri(rutaDestino);
                    logo.EndInit();
                    img_Usuario.Source = logo;
                }
                catch (Exception er)
                {
                    Console.WriteLine(er);
                    img_Usuario.Source = null;
                }

                habilitarCamposUsuario();
            }
        }

        private void comboboxImagenUsuario_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if (comboboxImagenUsuario.SelectedIndex > -1)
            {
                imagen = null;
                try
                {
                    BitmapImage logo = new BitmapImage();
                    logo.BeginInit();
                    logo.UriSource = new Uri(Environment.CurrentDirectory + "\\imagenes\\usuarios\\" + comboboxImagenUsuario.SelectedItem.ToString());
                    logo.EndInit();
                    img_Usuario.Source = logo;
                    imagen = comboboxImagenUsuario.SelectedItem.ToString();
                }
                catch (Exception er)
                {
                    Console.WriteLine(er);
                }
            }

        }







        #endregion

        #region Gestion Proveedores

        #region pestaña proveedor
        private void datagridProveedores_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (datagridProveedores.SelectedIndex > -1)
            {
                create = false;
                prov = (Proveedor)datagridProveedores.SelectedItem;
                txt_cifProveedor.Text = prov.Cif;
                txt_direccionProveedor.Text = prov.Direccion;
                txt_emailProveedor.Text = prov.Email;
                txt_nombreProveedor.Text = prov.Nombre;
                txt_nombreContactoProveedor.Text = prov.NombreContacto;
                txt_telefonoProveedor.Text = prov.Telefono;
                habilitarCamposProveedor();
                btn_borrarProveedor.IsEnabled = true;
            }
        }

        private void btn_guardarProveedor_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                prov.Cif = txt_cifProveedor.Text;
                prov.Direccion = txt_direccionProveedor.Text;
                prov.Email = txt_emailProveedor.Text;
                prov.Nombre = txt_nombreProveedor.Text;
                prov.NombreContacto = txt_nombreContactoProveedor.Text;
                prov.Telefono = txt_telefonoProveedor.Text;
            }
            catch (Exception er) { Console.WriteLine(er); }


            if (create)
            {
                if (!u.ProveedorRepository.GetAll().Any(c => c.Nombre.Equals(prov.Nombre)))
                {
                    try
                    {
                        u.ProveedorRepository.Create(prov);
                        AmRoMessageBox.ShowDialog("Guardado correctamente");
                        deshabilitarCamposProveedor();
                        limpiarCamposProveedor();
                        refreshDatagrids();
                    }
                    catch (Exception)
                    {
                        AmRoMessageBox.ShowDialog("Error al guardar");
                    }
                }
                else
                {
                    AmRoMessageBox.ShowDialog("Ya existe un proveedor con el mismo nombre");
                }
            }
            else
            {
                try
                {
                    u.ProveedorRepository.Update(prov);
                    AmRoMessageBox.ShowDialog("Guardado correctamente");
                    deshabilitarCamposProveedor();
                    limpiarCamposProveedor();
                    refreshDatagrids();
                }
                catch (Exception)
                {
                    AmRoMessageBox.ShowDialog("Error al actualizar");
                }
            }
            refreshComboboxCatalogoProveedor();

        }

        private void btn_borrarProveedor_Click(object sender, RoutedEventArgs e)
        {
            if (datagridProveedores.SelectedItem != null)
            {

                if (MsgPregunta("¿Seguro que quieres eliminar este proveedor?", "Eliminar"))
                {
                    try
                    {
                        u.ProveedorRepository.Delete(prov);
                        refreshDatagrids();
                        limpiarCamposProveedor();
                        deshabilitarCamposProveedor();
                    }
                    catch (Exception er)
                    {
                        Console.WriteLine(er);
                        AmRoMessageBox.ShowDialog("Error al intentar borrar", "Error");
                    }
                }
                refreshComboboxCatalogoProveedor();

            }
            else
            {
                AmRoMessageBox.ShowDialog("Selecciona un proveedor a borrar", "Error");
            }
        }

        private void btn_nuevoProveedor_Click(object sender, RoutedEventArgs e)
        {
            create = true;
            prov = new Proveedor();
            limpiarCamposProveedor();
            btn_borrarProveedor.IsEnabled = false;
            habilitarCamposProveedor();
            datagridProveedores.SelectedIndex = -1;
        }

        private void limpiarCamposProveedor()
        {
            txt_cifProveedor.Clear();
            txt_direccionProveedor.Clear();
            txt_emailProveedor.Clear();
            txt_nombreProveedor.Clear();
            txt_nombreContactoProveedor.Clear();
            txt_telefonoProveedor.Clear();
        }

        private void deshabilitarCamposProveedor()
        {
            txt_cifProveedor.IsEnabled = false;
            txt_direccionProveedor.IsEnabled = false;
            txt_emailProveedor.IsEnabled = false;
            txt_nombreProveedor.IsEnabled = false;
            txt_nombreContactoProveedor.IsEnabled = false;
            txt_telefonoProveedor.IsEnabled = false;
        }

        private void habilitarCamposProveedor()
        {
            txt_cifProveedor.IsEnabled = true;
            txt_direccionProveedor.IsEnabled = true;
            txt_emailProveedor.IsEnabled = true;
            txt_nombreProveedor.IsEnabled = true;
            txt_nombreContactoProveedor.IsEnabled = true;
            txt_telefonoProveedor.IsEnabled = true;
        }
        #endregion

        #region Pestaña CatalogoProveedor


        private void checkboxProveedor_Checked(object sender, RoutedEventArgs e)
        {
            listProductoProveedor.Add((Producto)datagridCatalogoProveedorProductos.SelectedItem);
        }
        private void checkboxProveedor_Unchecked(object sender, RoutedEventArgs e)
        {
            listProductoProveedor.Remove((Producto)datagridCatalogoProveedorProductos.SelectedItem);
        }
        private void comboBox_proveedorCatalogo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                datagridCatalogoProveedor.ItemsSource = null;
                datagridCatalogoProveedor.ItemsSource = u.ProductoRepository.GetAll().Where(c => c.Proveedor.ProveedorId.Equals(comboBox_proveedorCatalogo.SelectedValue));
            }
            catch (Exception)
            {
            }
        }
        private void btn_proveedorCatalogoMostrarTodo_Click(object sender, RoutedEventArgs e)
        {
            datagridCatalogoProveedorProductos.ItemsSource = u.ProductoRepository.GetAll();
        }
        private void datagridCatalogoProveedorProductos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        private void btn_ProveedorToProductos_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                prod = (Producto)datagridCatalogoProveedor.SelectedItem;
                prov = u.ProveedorRepository.GetAll().Find(c => c.ProveedorId.Equals(comboBox_proveedorCatalogo.SelectedValue));
                prod.Proveedor = u.ProveedorRepository.GetAll().Find(c => c.ProveedorId.Equals(1));

                u.ProveedorRepository.Update(prov);

                datagridCatalogoProveedor.ItemsSource = null;
                datagridCatalogoProveedor.ItemsSource = u.ProductoRepository.GetAll().Where(c => c.Proveedor.Equals(prov));

                datagridCatalogoProveedorProductos.ItemsSource = null;
                datagridCatalogoProveedorProductos.ItemsSource = u.ProductoRepository.GetAll();
            }
            catch (Exception)
            {
                AmRoMessageBox.ShowDialog("Selecciona un producto");
            }

        }
        public void refreshComboboxCatalogoProveedor()
        {
            try
            {
                comboBox_proveedorCatalogo.ItemsSource = u.ProveedorRepository.GetAll();
                comboBox_proveedorCatalogo.DisplayMemberPath = "Nombre";
                comboBox_proveedorCatalogo.SelectedValuePath = "ProveedorId";

                comboBox_proveedorCatalogoProductos.ItemsSource = u.ProductoRepository.GetAll();
                comboBox_proveedorCatalogoProductos.DisplayMemberPath = "Nombre";
                comboBox_proveedorCatalogoProductos.SelectedValuePath = "ProductoId";
            }
            catch (Exception)
            { }
        }
        private void btn_ProductosToProveedor_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                prov = u.ProveedorRepository.GetAll().Find(c => c.ProveedorId.Equals(comboBox_proveedorCatalogo.SelectedValue));
                foreach (Producto item in listProductoProveedor)
                {
                    prov.Producto.Add(item);
                }
                u.ProveedorRepository.Update(prov);
                datagridCatalogoProveedor.ItemsSource = null;
                datagridCatalogoProveedor.ItemsSource = u.ProductoRepository.GetAll().Where(c => c.Proveedor.ProveedorId.Equals(prov.ProveedorId));

                datagridCatalogoProveedorProductos.ItemsSource = null;
                datagridCatalogoProveedorProductos.ItemsSource = u.ProductoRepository.GetAll();

                listProductoProveedor.Clear();
            }
            catch (Exception)
            {
                AmRoMessageBox.ShowDialog("Selecciona un proveedor");
            }
        }
        private void comboBox_proveedorCatalogoProductos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            datagridCatalogoProveedorProductos.ItemsSource = null;
            datagridCatalogoProveedorProductos.ItemsSource = u.ProductoRepository.GetAll().Where(c => c.ProductoId.Equals(comboBox_proveedorCatalogoProductos.SelectedValue));
        }

        #endregion

        #endregion

        #region Gestion Mesa

        private void btn_guardarMesa_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int capacidad = 0;
                decimal incremento = 0;
                int.TryParse(txt_MesaCapacidad.Text, out capacidad);
                decimal.TryParse(txt_MesaPrecioIncremento.Text.Replace('.', ','), out incremento);
                mesa.IncrementoMesa = incremento;
                mesa.Capacidad = capacidad;
                mesa.NombreMesa = txt_MesaDescripcion.Text;

            }
            catch (Exception er) { Console.WriteLine(er); }

            if (create)
            {
                try
                {
                    u.MesaRepository.Create(mesa);
                    AmRoMessageBox.ShowDialog("Guardado correctamente");
                    refreshDatagrids();
                    limpiarCamposMesa();
                    deshabilitarCamposMesa();
                }
                catch (Exception)
                {
                    AmRoMessageBox.ShowDialog("Error al intentar guardar");
                }
            }
            else
            {
                try
                {
                    u.MesaRepository.Update(mesa);
                    AmRoMessageBox.ShowDialog("Guardado correctamente");
                    refreshDatagrids();
                    limpiarCamposMesa();
                    deshabilitarCamposMesa();
                }
                catch (Exception)
                {
                    AmRoMessageBox.ShowDialog("Error al intentar guardar los cambios");
                }
            }
        }

        private void btn_borrarMesa_Click(object sender, RoutedEventArgs e)
        {
            if (MsgPregunta("Seguro que quieres eliminar la mesa?", "Eliminar"))
            {
                try
                {
                    u.MesaRepository.Delete(mesa);
                    deshabilitarCamposMesa();
                    limpiarCamposMesa();
                    refreshDatagrids();
                }
                catch (Exception er)
                {
                    Console.WriteLine(er);
                    AmRoMessageBox.ShowDialog("Selecciona una mesa");
                }
            }
        }

        private void btn_nuevaMesa_Click(object sender, RoutedEventArgs e)
        {
            mesa = new Mesa();
            create = true;
            habilitarCamposMesa();
            limpiarCamposMesa();
        }

        private void datagridmesas_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (datagridmesas.SelectedIndex > -1)
            {
                create = false;
                mesa = (Mesa)datagridmesas.SelectedItem;

                txt_MesaPrecioIncremento.Text = mesa.IncrementoMesa.ToString();
                txt_MesaDescripcion.Text = mesa.NombreMesa;
                txt_MesaCapacidad.Text = mesa.Capacidad.ToString();
                habilitarCamposMesa();
            }
        }

        private void limpiarCamposMesa()
        {
            txt_MesaCapacidad.Clear();
            txt_MesaDescripcion.Clear();
            txt_MesaPrecioIncremento.Clear();
        }

        private void deshabilitarCamposMesa()
        {
            txt_MesaCapacidad.IsEnabled = false;
            txt_MesaDescripcion.IsEnabled = false;
            txt_MesaPrecioIncremento.IsEnabled = false;
        }

        private void habilitarCamposMesa()
        {
            txt_MesaCapacidad.IsEnabled = true;
            txt_MesaDescripcion.IsEnabled = true;
            txt_MesaPrecioIncremento.IsEnabled = true;
        }

        #endregion

        #endregion

        #region Pedidos

        #region realizar pedidos

        private void btn_addLineaPedido_Click(object sender, RoutedEventArgs e)
        {
            if (varProd != null)
            {
                try
                {
                    lPedidoProv = new LineaPedidoProveedor();
                    lPedidoProv.VarianteProducto = varProd;
                    lPedidoProv.Cantidad = Convert.ToInt32(numericUpDownPedido.Value);
                    lPedidoProv.Comentario = txt_PedidoComentarios.Text;
                    numericUpDownPedido.Text = "1";
                    if (listaPedidos.Count < 1)
                    {
                        listaPedidos.Add(new PedidoProveedor { Proveedor = lPedidoProv.VarianteProducto.Producto.Proveedor });
                    }

                    bool nuevopedido = false;
                    foreach (PedidoProveedor p in listaPedidos)
                    {
                        if (lPedidoProv.VarianteProducto.Producto.Proveedor == p.Proveedor)
                        {
                            p.LineaPedidoProveedor.Add(lPedidoProv);
                            nuevopedido = false;
                            break;
                        }
                        else
                        {
                            nuevopedido = true;
                        }
                    }
                    if (nuevopedido)
                    {
                        pedidoProv = new PedidoProveedor();
                        pedidoProv.Proveedor = lPedidoProv.VarianteProducto.Producto.Proveedor;
                        pedidoProv.LineaPedidoProveedor.Add(lPedidoProv);
                        listaPedidos.Add(pedidoProv);
                    }
                    datagridLineaspedidos.Items.Clear();
                    foreach (PedidoProveedor pp in listaPedidos)
                    {
                        foreach (LineaPedidoProveedor lp in pp.LineaPedidoProveedor)
                        {
                            datagridLineaspedidos.Items.Add(lp);
                        }
                    }

                    txt_NumArticulosCarrito.Text = "( " + datagridLineaspedidos.Items.Count + " )";


                }
                catch (Exception er)
                {
                    Console.WriteLine("error carrito: " + er);
                    AmRoMessageBox.ShowDialog("No se pudo añadir al carrito, comprueba los datos");
                }

            }
        }
        private void btn_PedidoBuscarProd_Click(object sender, RoutedEventArgs e)
        {
            datagridPedidos.ItemsSource = null;
            datagridPedidos.ItemsSource = u.VarianteProductoRepository.GetAll().Where(c => c.Producto.Nombre.ToUpper().Contains(txt_PedidoBuscarProd.Text.ToUpper()));
        }
        private void datagridPedidos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (datagridPedidos.SelectedIndex > -1)
            {
                varProd = (VarianteProducto)datagridPedidos.SelectedItem;
                txt_PedidoNombreProducto.Text = varProd.Producto.Nombre;
                txt_PedidoNombreVariante.Text = varProd.Nombre;
            }
        }
        private void btn_PedidoBuscarProdporStock_Click(object sender, RoutedEventArgs e)
        {
            datagridPedidos.ItemsSource = null;
            datagridPedidos.ItemsSource = u.VarianteProductoRepository.GetAll().Where(c => c.Stock <= numericUpDownStock.Value);
        }
        private void btn_RealizarPedido_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                foreach (PedidoProveedor pp in listaPedidos)
                {
                    pp.FechaPedido = DateTime.Now;
                    pp.Recibido = false;
                    PdfGenerator pdf = new PdfGenerator();
                    u.PedidoProveedorRepository.Create(pp);
                    pdf.NewPdf(ActiveUsr, pp);
                }
                datagridLineaspedidos.Items.Clear();
                listaPedidos.Clear();
                txt_NumArticulosCarrito.Text = "( " + datagridLineaspedidos.Items.Count + " )";
                AmRoMessageBox.ShowDialog("Pedido confirmado");
                tab_PedidosRealizados.IsSelected = true;
                refreshDatagrids();
            }
            catch (Exception)
            {
                AmRoMessageBox.ShowDialog("No se pudo imprimir el pedido");
            }
        }
        private void datagridLineaspedidos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //TODO selecionar linea de lista para borrar
            lPedidoProv = (LineaPedidoProveedor)datagridLineaspedidos.SelectedItem;
        }
        private void btn_deleteLineaPedido_Click(object sender, RoutedEventArgs e)
        {


            foreach (PedidoProveedor pp in listaPedidos)
            {
                pp.LineaPedidoProveedor.Remove(lPedidoProv);
            }
            datagridLineaspedidos.Items.Remove(lPedidoProv);
            txt_NumArticulosCarrito.Text = "( " + datagridLineaspedidos.Items.Count + " )";
        }


        #endregion

        #region confirmar pedidos
        private void btn_PedidoconfirmarRecepcion_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!pedidoProv.Recibido)
                {
                    foreach (LineaPedidoProveedor lp in pedidoProv.LineaPedidoProveedor)
                    {
                        lp.VarianteProducto.Stock += lp.Cantidad;
                        lp.VarianteProducto.Producto.Stock += lp.Cantidad;
                    }
                    u.PedidoProveedorRepository.GetAll().Where(c => c.PedidoProveedorId.Equals(pedidoProv.PedidoProveedorId)).First().Recibido = true;
                    u.PedidoProveedorRepository.Update(pedidoProv);
                    refreshDatagrids();

                    AmRoMessageBox.ShowDialog("Recepción de pedido confirmada!" + Environment.NewLine + "Stock incrementado.");
                }
                else
                {
                    AmRoMessageBox.ShowDialog("Ya recibiste ese pedido");
                }
            }
            catch
            {
                AmRoMessageBox.ShowDialog("Selecciona un pedido");
            }
        }

        private void datagridpedidosRealizados_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            pedidoProv = (PedidoProveedor)datagridpedidosRealizados.SelectedItem;
        }

        private void datagridpedidosRealizados_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                string directorio = Environment.CurrentDirectory + @"\Pedidos\" + pedidoProv.Proveedor.Nombre + @"\";
                Process.Start(directorio + "Num" + pedidoProv.PedidoProveedorId + " " + String.Format("{0:dd-MM-yyyy}", pedidoProv.FechaPedido.Date) + ".pdf");
            }
            catch (Exception er)
            {
                Console.WriteLine("Error abrir pdf: " + er);
            }
        }

        private void btn_PedidosVerTodos_Click(object sender, RoutedEventArgs e)
        {
            datagridpedidosRealizados.ItemsSource = null;
            datagridpedidosRealizados.ItemsSource = u.PedidoProveedorRepository.GetAll();
        }

        private void btn_PedidosVerSinConfirmar_Click(object sender, RoutedEventArgs e)
        {
            datagridpedidosRealizados.ItemsSource = null;
            datagridpedidosRealizados.ItemsSource = u.PedidoProveedorRepository.GetAll().Where(c => c.Recibido == false);
        }

        #endregion

        #endregion


        private void btn_MaxMinApp_Click(object sender, RoutedEventArgs e)
        {
            if (Application.Current.MainWindow.WindowState == WindowState.Maximized)
            {
                Application.Current.MainWindow.WindowState = WindowState.Normal;
                icon_MaxMinApp.Kind = PackIconKind.WindowMaximize;
            }
            else
            {
                Application.Current.MainWindow.WindowState = WindowState.Maximized;
                icon_MaxMinApp.Kind = PackIconKind.WindowRestore;
            }
        }

        private void btn_MinimizeApp_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.WindowState = WindowState.Minimized;
        }
    }
}
//PROYECTO ADAT 2017
//OSCAR SUAREZ PAYO