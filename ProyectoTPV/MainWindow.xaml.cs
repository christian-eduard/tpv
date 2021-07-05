using OpenPOS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.IO;
using Microsoft.Win32;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Runtime.InteropServices;
using AmRoMessageDialog;

namespace OpenPOS
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

            Directory.CreateDirectory(Environment.CurrentDirectory + "\\imagenes\\usuarios\\");
            Directory.CreateDirectory(Environment.CurrentDirectory + "\\imagenes\\categorias\\");
            Directory.CreateDirectory(Environment.CurrentDirectory + "\\imagenes\\productos\\");

            refreshComboBoxCategorias();
            deshabilitarCamposCategorias();
            deshabilitarCamposProductos();
            deshabilitarCamposUsuario();

            Load_Usuarios();
            Load_Categorias();
        }

        #region Custom MessageBox
        public bool MsgPregunta(string Mensaje, string Titulo)
        {

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
            var result = messageBox.Show(Mensaje, Titulo, AmRoMessageBoxButton.OkCancel);

            return result == AmRoMessageBoxResult.Ok;
        }

        #endregion

        #region Declaraciones Global
        UnitOfWork u = new UnitOfWork();
        SalesLine lv;
        Invoice tv;
        Item prod = new Item();
        User ActiveUsr;
        User usr = new User();
        Group cat = new Group();
        string idUsuario;
        string unidades = null;
        decimal total = 0;
        string imagen;
        bool create;
        #endregion

        private void btn_closeApp_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBoxResult.Yes == MessageBox.Show("Seguro que quieres Cerrar el TPV?\nSe perderán los cambios sin guardar.", "Cerrar TPV", MessageBoxButton.YesNo))
            {
                this.Close();
            }
        }

        #region Zona Usuario

        void Select_usuario(string usuario)
        {
            ActiveUsr = new User();
            PasswordBox.Clear();

            string[] usrContent = usuario.Split(' ');
            idUsuario = usrContent[0];
            textblockusuarioSeleccionado.Text = usrContent[1] + " " + usrContent[2];
            Console.WriteLine("id usuario seleccionado" + idUsuario);
        }

        void Load_Usuarios()
        {
            StackPanel_Usuarios.Children.Clear();

            foreach (User item in u.UserRepository.GetAll())
            {
                //cargar usuarios en pantalla de login
                MaterialDesignThemes.Wpf.Chip chip = new MaterialDesignThemes.Wpf.Chip();

                string rutaImagen = Environment.CurrentDirectory + "\\imagenes\\usuarios\\" + item.ImagePath;
                if (File.Exists(rutaImagen))
                {
                    chip.IconForeground = Brushes.Transparent;
                    chip.Icon = new ImageBrush(new BitmapImage(new Uri(rutaImagen, UriKind.Relative)));
                    chip.IconBackground = new ImageBrush(new BitmapImage(new Uri(rutaImagen, UriKind.Relative)));
                }
                chip.FontSize = 20;
                chip.Content = item.UserId + " " + item.Name + " " + item.LastName;
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

            foreach (Group item in u.GroupRepository.GetAll())
            {
                Button b = new Button();
                StackPanel sp = new StackPanel();
                Image img = new Image();
                TextBlock tb = new TextBlock();

                #region formato botton
                string rutaImagen = Environment.CurrentDirectory + "\\imagenes\\categorias\\" + item.ImagePath;
                if (File.Exists(rutaImagen))
                {
                    b.Background = new ImageBrush(new BitmapImage(new Uri(rutaImagen, UriKind.Relative)));
                }
                b.Foreground = Brushes.Black;
                b.FontSize = 25;
                b.FontWeight = FontWeights.Light;
                b.Content = item.Name;
                b.VerticalContentAlignment = VerticalAlignment.Bottom;
                b.HorizontalContentAlignment = HorizontalAlignment.Center;
                Thickness margin = b.Margin;
                margin.Top = 10;
                margin.Bottom = 10;
                margin.Left = 10;
                margin.Right = 10;
                b.Margin = margin;
                b.Height = 150;
                b.Width = 150;
                b.Margin = margin;
                #endregion

                //Evento clic
                b.Click += (s, e) =>
                {
                    Load_Productos(b.Content.ToString());
                };

                StackPanel_Categorias.Children.Add(b);
            }
        }

        void Load_Productos(string categoria)
        {
            WrapPanel_productos.Children.Clear();

            try
            {
                foreach (var item in u.ItemRepository.Get().Where(c => c.Group.Name.Equals(categoria)))
                {
                    Button b = new Button();

                    #region formato boton
                    string rutaImagen = Environment.CurrentDirectory + "\\imagenes\\productos\\" + item.ImagePath;

                    if (File.Exists(rutaImagen))
                    {
                        b.Background = new ImageBrush(new BitmapImage(new Uri(rutaImagen, UriKind.Relative)));
                    }

                    b.Foreground = Brushes.Black;
                    b.FontSize = 25;
                    b.FontWeight = FontWeights.Light;
                    b.Content = item.Name;
                    Thickness margin = b.Margin;
                    b.ToolTip = b.Content;
                    b.VerticalContentAlignment = VerticalAlignment.Bottom;
                    b.HorizontalContentAlignment = HorizontalAlignment.Center;
                    margin.Top = 10;
                    margin.Bottom = 10;
                    margin.Left = 10;
                    margin.Right = 10;
                    b.Margin = margin;
                    b.Height = 150;
                    b.Width = 150;
                    b.Margin = margin;
                    #endregion

                    //Evento clic
                    b.Click += (s, e) =>
                    {
                        Load_LineaVenta(b.Content.ToString());
                    };


                    WrapPanel_productos.Children.Add(b);
                }
            }
            catch (Exception)
            { }

        }

        void Load_LineaVenta(string NombreProducto)
        {
            lv = new SalesLine();
            lv.Item = u.ItemRepository.Get().FirstOrDefault(c => c.Name.Equals(NombreProducto));

            if (unidades != null)
            {
                lv.Unit = Convert.ToInt32(unidades);
            }
            else
            {
                unidades = 1.ToString();
                lv.Unit = Convert.ToInt32(unidades);
            }
            tv.SalesLine.Add(lv);
            total += u.ItemRepository.Get().FirstOrDefault(c => c.Name.Equals(NombreProducto)).Price * lv.Unit;
            txtblock_Total.Text = total.ToString();
            unidades = 1.ToString();
            txtblock_unidades.Text = unidades.ToString();
            RefrescarDatagridLineaVenta();
            unidades = null;
        }

        void New_Ticket()
        {
            if (tv.SalesLine.Count > 0)
            {
                tv.SalesLine.ToList().ForEach(i => i.Item.Stock -= i.Unit);
                tv.User = ActiveUsr;
                tv.Timestamp = DateTime.Now;
                u.InvoiceRepository.Create(tv);
                invoice ventanaTicket = new invoice(tv);
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

        private void resetCaja()
        {
            tv = new Invoice();
            txt_precioProducto.Text = 0.ToString();
            total = 0;
            txtblock_unidades.Text = 1.ToString();
            unidades = null;
            PasswordBox.Clear();
            RefrescarDatagridLineaVenta();
        }

        private void button_caja_click(object sender, RoutedEventArgs e)
        {
            New_Ticket();
        }

        void habilitarPaneles(string tipoUsuario)
        {
            if (tipoUsuario.Equals("usuario") || tipoUsuario.Equals("admin"))
            {
                grid_tpvSI.Visibility = Visibility.Visible;
                grid_tpvNO.Visibility = Visibility.Hidden;
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

                datagridCategorias.ItemsSource = u.GroupRepository.GetAll();
                datagridProducto.ItemsSource = u.ItemRepository.GetAll();
                datagridUsuarios.ItemsSource = u.UserRepository.GetAll();

                cargarcomboboxImagenes();

            }
            else
            {
                grid_adminNO.Visibility = Visibility.Visible;
                grid_adminSI.Visibility = Visibility.Hidden;
            }

        }

        #region Teclado Login

        #region numerosTeclado

        private void writePasswordNumber(object sender, RoutedEventArgs e)
        {
            if (PasswordBox.Password.Length < 4)
            {
                int number = Convert.ToInt32((sender as Button).Content.ToString());
                PasswordBox.Password += number;
            }
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
            bool valida = u.UserRepository.GetAll().Any(c => c.UserId.ToString().Equals(idUsuario) && c.Pin.Equals(PasswordBox.Password));
            if (valida)
            {
                ActiveUsr = u.UserRepository.Get().SingleOrDefault(c => c.UserId.ToString().Equals(idUsuario));
                habilitarPaneles(ActiveUsr.UserType);
                tab_tpv.IsSelected = true;
                resetCaja();
                chip_usuarioVenta.Content = ActiveUsr.Name + " " + ActiveUsr.LastName;
                string rutaImagen = Environment.CurrentDirectory + "\\imagenes\\usuarios\\" + ActiveUsr.ImagePath;
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
                tv.SalesLine.Remove((SalesLine)datagridLineaVenta.SelectedItem);
            }
            else
            {
                try
                {
                    tv.SalesLine.Remove(tv.SalesLine.Last());
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
            tabControl_Main.SelectedIndex = 0;
        }

        private void RefrescarDatagridLineaVenta()
        {
            datagridLineaVenta.ItemsSource = null;
            datagridLineaVenta.ItemsSource = tv.SalesLine.Reverse().ToList();

            if (tv.SalesLine.Count > 0)
            {
                total = tv.SalesLine.Sum(c => c.Item.Price * c.Unit);
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
            datagridCategorias.ItemsSource = null;
            datagridCategorias.ItemsSource = u.GroupRepository.GetAll();

            datagridProducto.ItemsSource = null;
            datagridProducto.ItemsSource = u.ItemRepository.GetAll();

            datagridUsuarios.ItemsSource = null;
            datagridUsuarios.ItemsSource = u.UserRepository.GetAll();
        }

        private void changeDataContext()
        {

            if (tabAdmin_Usuarios.IsSelected)
            {
                DataContext = usr;
            }

            if (tabAdmin_Productos.IsSelected)
            {
                DataContext = prod;
            }

            if (tabAdmin_Categorias.IsSelected)
            {
                DataContext = cat;
            }
        }

        private void cargarcomboboxImagenes()
        {
            combobox_imagenesCategorias.Items.Clear();
            foreach (var item in Directory.GetFiles(Environment.CurrentDirectory + "\\imagenes\\categorias\\").Select(System.IO.Path.GetFileName).ToList())
            {
                combobox_imagenesCategorias.Items.Add(item);
            }
            comboBoxImagenProductos.Items.Clear();
            foreach (var item in Directory.GetFiles(Environment.CurrentDirectory + "\\imagenes\\productos\\").Select(System.IO.Path.GetFileName).ToList())
            {
                comboBoxImagenProductos.Items.Add(item);
            }
            comboboxImagenUsuario.Items.Clear();
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
                AmRoMessageBox.ShowDialog(mensageErrores, @"/!\ERROR/!\");
                return false;
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
            openFileDialog1.Filter = "BMP|*.bmp|GIF|*.gif|JPG|*.jpg;*.jpeg|PNG|*.png|TIFF|*.tif;*.tiff|"
       + "Imágenes|*.bmp;*.jpg;*.jpeg;*.png;*.tif;*.tiff";
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
                cat.Name = txtboxNombreCategoria.Text;
                cat.Description = txtboxDescripcionCategoria.Text;
                cat.ImagePath = imagen;
            }
            catch (Exception)
            {
            }

            if (validado(cat))
            {
                if (create)
                {
                    if (!u.GroupRepository.GetAll().Any(c => c.Name.Equals(cat.Name)))
                    {
                        u.GroupRepository.Create(cat);

                        AmRoMessageBox.ShowDialog("Guardado correctamente");
                        deshabilitarCamposCategorias();
                        refreshComboBoxCategorias();
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
                    u.GroupRepository.Update(cat);

                    AmRoMessageBox.ShowDialog("Guardado correctamente");
                    deshabilitarCamposCategorias();
                    refreshComboBoxCategorias();
                    refreshDatagrids();
                    borrarCamposCategorias();
                    Load_Categorias();
                }
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
                        string rutaImagen = Environment.CurrentDirectory + "\\imagenes\\categorias\\" + cat.ImagePath;

                        u.GroupRepository.Delete(cat);
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
                cat = (Group)datagridCategorias.SelectedItem;
                txtboxDescripcionCategoria.Text = cat.Description;
                txtboxNombreCategoria.Text = cat.Name;
                combobox_imagenesCategorias.Text = cat.ImagePath;
                string rutaDestino = Environment.CurrentDirectory + "\\imagenes\\categorias\\" + cat.ImagePath;
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
            cat = new Group();
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
            comboBox_CategoriasProducto.ItemsSource = u.GroupRepository.GetAll();
            comboBox_CategoriasProducto.SelectedValuePath = "CategoriaId";
            comboBox_CategoriasProducto.DisplayMemberPath = "Nombre";
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

        #region Gestion Productos 

        private void btn_ImagenProducto_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "BMP|*.bmp|GIF|*.gif|JPG|*.jpg;*.jpeg|PNG|*.png|TIFF|*.tif;*.tiff|"
       + "All Graphics Types|*.bmp;*.jpg;*.jpeg;*.png;*.tif;*.tiff";
            openFileDialog1.FilterIndex = 1;
            bool? userClickedOK = openFileDialog1.ShowDialog();

            if (userClickedOK == true)
            {
                // Open the selected file to read.
                string rutaArchivo = openFileDialog1.FileName;
                string rutaDestino = Environment.CurrentDirectory + "\\imagenes\\productos\\" + openFileDialog1.SafeFileName;
                if (!File.Exists(rutaDestino))
                {

                }
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
                prod.Name = txt_nombreProducto.Text;
                prod.Description = txt_descripcionProducto.Text;
                prod.ImagePath = imagen;
                Int32.TryParse(comboboxIVA.Text, out iva);
                Decimal.TryParse(txt_precioProducto.Text.Replace('.', ','), out price);
                Int32.TryParse(txt_StockInicial.Text, out stock);
                prod.Tax = iva;
                prod.Price = price;
                prod.Stock = stock;
                prod.Group = (Group)comboBox_CategoriasProducto.SelectedItem;
            }
            catch (Exception er) { Console.WriteLine(er); }

            if (validado(prod))
            {

                if (create)
                {
                    if (!u.ItemRepository.GetAll().Any(c => c.Name.Equals(prod.Name)))
                    {
                        u.ItemRepository.Create(prod);
                        AmRoMessageBox.ShowDialog("Guardado correctamente");
                        deshabilitarCamposProductos();
                        limiparCamposProductos();
                        img_Producto.Source = null;
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
                    u.ItemRepository.Update(prod);
                    AmRoMessageBox.ShowDialog("Guardado correctamente");
                    deshabilitarCamposProductos();
                    limiparCamposProductos();
                    img_Producto.Source = null;
                    Load_Categorias();
                    refreshDatagrids();
                }
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
                        string rutaImagen = Environment.CurrentDirectory + "\\imagenes\\productos\\" + prod.ImagePath;
                        u.ItemRepository.Delete(prod);
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
            prod = new Item();
            habilitarCamposProductos();
            btn_borrarProducto.IsEnabled = false;
            limiparCamposProductos();
            img_categoria.Source = null;
            datagridCategorias.SelectedIndex = -1;
        }

        private void datagridProducto_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (datagridProducto.SelectedIndex > -1)
            {
                create = false;
                prod = (Item)datagridProducto.SelectedItem;
                txt_descripcionProducto.Text = prod.Description;
                txt_nombreProducto.Text = prod.Name;
                txt_precioProducto.Text = prod.Price.ToString();
                txt_StockInicial.Text = prod.Stock.ToString();
                comboBoxImagenProductos.Text = prod.ImagePath;
                comboboxIVA.Text = prod.Tax.ToString();
                imagen = prod.ImagePath;
                try
                {
                    comboBox_CategoriasProducto.Text = prod.Group.Name;
                }
                catch (Exception er)
                {
                    Console.WriteLine(er);
                    comboBox_CategoriasProducto.Text = "";
                }
                string rutaDestino = Environment.CurrentDirectory + "\\imagenes\\productos\\" + prod.ImagePath;
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

        #region Gestion Usuarios

        private void btn_ImagenUsuario_Click(object sender, RoutedEventArgs e)
        {
            {
                OpenFileDialog openFileDialog1 = new OpenFileDialog();
                openFileDialog1.Filter = "BMP|*.bmp|GIF|*.gif|JPG|*.jpg;*.jpeg|PNG|*.png|TIFF|*.tif;*.tiff|"
           + "Todas las Imágenes|*.bmp;*.jpg;*.jpeg;*.png;*.tif;*.tiff";
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
                usr.Name = txt_nombreUsuario.Text;
                usr.LastName = txt_apellidosUsuario.Text;
                usr.ImagePath = imagen;
                usr.Login = txt_loginUsuario.Text;
                usr.Pin = txt_pinUsuario.Text;
                usr.Email = txt_emailUsuario.Text;
                usr.UserType = comboBox_TipoUsuario.Text;
            }
            catch (Exception er) { Console.WriteLine(er); }

            if (validado(usr))
            {

                if (create)
                {
                    if (!u.UserRepository.GetAll().Any(c => c.Login.Equals(usr.Login)))
                    {
                        u.UserRepository.Create(usr);
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
                    u.UserRepository.Update(usr);
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
                if ((User)datagridUsuarios.SelectedItem != ActiveUsr)
                {
                    if (MsgPregunta("¿Seguro que quieres eliminar este usuario?", "Eliminar"))
                    {
                        try
                        {
                            u.UserRepository.Delete(usr);
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
            usr = new User();
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
                usr = (User)datagridUsuarios.SelectedItem;
                txt_nombreUsuario.Text = usr.Name;
                txt_apellidosUsuario.Text = usr.LastName;
                txt_loginUsuario.Text = usr.Login;
                txt_pinUsuario.Text = usr.Pin;
                comboboxImagenUsuario.Text = usr.ImagePath;
                comboBox_TipoUsuario.Text = usr.UserType;
                imagen = usr.ImagePath;
                try
                {
                    txt_emailUsuario.Text = usr.Email;
                }
                catch (Exception er)
                {
                    Console.WriteLine(er);
                    txt_emailUsuario.Text = "";
                }

                string rutaDestino = Environment.CurrentDirectory + "\\imagenes\\usuarios\\" + usr.ImagePath;
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

        #endregion


    }
}




