using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoTPV.Model
{
    class PdfGenerator
    {
        public void NewPdf(Usuario ActiveUsr, PedidoProveedor pedido)
        {
            string directorio = Environment.CurrentDirectory + @"\Pedidos\" + pedido.Proveedor.Nombre + @"\";

            Directory.CreateDirectory(directorio);
            // Creamos el documento con el tamaño de página tradicional
            Document doc = new Document(PageSize.LETTER);
            // Indicamos donde vamos a guardar el documento
            PdfWriter writer = PdfWriter.GetInstance(doc,
                                        new FileStream(directorio + "Num" + pedido.PedidoProveedorId + " " + String.Format("{0:dd-MM-yyyy}", pedido.FechaPedido.Date) + ".pdf", FileMode.Create));

            // Le colocamos el título y el autor
            // **Nota: Esto no será visible en el documento
            doc.AddTitle("Pedido Nº " + pedido.PedidoProveedorId + " - " + pedido.Proveedor.Nombre + " - " + pedido.FechaPedido.ToShortDateString());
            doc.AddCreator(ActiveUsr.Nombre + " " + ActiveUsr.Apellidos);
            // Abrimos el archivo
            doc.Open();

            // Creamos el tipo de Font que vamos utilizar
            iTextSharp.text.Font _standardFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 12, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
            iTextSharp.text.Font _largeFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 15, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
            iTextSharp.text.Font _extralargeFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.COURIER, 30, iTextSharp.text.Font.BOLD, BaseColor.DARK_GRAY);
            // Escribimos el encabezamiento en el documento
            Paragraph par = new Paragraph(" - SOLICITUD DE PRESUPUESTO - ", _extralargeFont);
            par.Alignment = 1;
            doc.Add(new Paragraph(par));
            doc.Add(Chunk.NEWLINE);
            doc.Add(Chunk.NEWLINE);
            doc.Add(new Paragraph("Pedido Nº " + pedido.PedidoProveedorId + " - " + pedido.FechaPedido.ToShortDateString() + Environment.NewLine +
                pedido.Proveedor.Nombre + " - " + pedido.Proveedor.NombreContacto + Environment.NewLine +
               "Dirección: " + pedido.Proveedor.Direccion + Environment.NewLine +
               "Email: " + pedido.Proveedor.Email + Environment.NewLine +
               "Teléfono: " + pedido.Proveedor.Telefono, _largeFont));
            doc.Add(Chunk.NEWLINE);
            doc.Add(Chunk.NEWLINE);
            // Creamos una tabla que contendrá el nombre, apellido y país
            // de nuestros visitante.
            PdfPTable tabla = new PdfPTable(3);
            tabla.WidthPercentage = 80;
            tabla.HorizontalAlignment = 1;
            // Configuramos el título de las columnas de la tabla
            PdfPCell clUnidades = new PdfPCell(new Phrase("Unidades", _standardFont));
            clUnidades.BorderWidth = 0;
            clUnidades.BorderWidthBottom = 0.75f;

            PdfPCell clProductos = new PdfPCell(new Phrase("Producto", _standardFont));
            clProductos.BorderWidth = 0;
            clProductos.BorderWidthBottom = 0.75f;

            PdfPCell clComentarios = new PdfPCell(new Phrase("Comentario", _standardFont));
            clComentarios.BorderWidth = 0;
            clComentarios.BorderWidthBottom = 0.75f;

            // Añadimos las celdas a la tabla
            tabla.AddCell(clUnidades);
            tabla.AddCell(clProductos);
            tabla.AddCell(clComentarios);

            // Llenamos la tabla con información
            foreach (LineaPedidoProveedor lp in pedido.LineaPedidoProveedor)
            {
                clUnidades = new PdfPCell(new Phrase(lp.Cantidad.ToString(), _standardFont));
                clUnidades.BorderWidth = 0;


                if (lp.VarianteProducto.Nombre.Equals(lp.VarianteProducto.Producto.Nombre))
                {
                    clProductos = new PdfPCell(new Phrase(lp.VarianteProducto.Nombre, _standardFont));
                }
                else
                {
                    clProductos = new PdfPCell(new Phrase(lp.VarianteProducto.Producto.Nombre + " - " + lp.VarianteProducto.Nombre, _standardFont));
                }
                clProductos.BorderWidth = 0;

                clComentarios = new PdfPCell(new Phrase(lp.Comentario, _standardFont));
                clComentarios.BorderWidth = 0;

                // Añadimos las celdas a la tabla
                tabla.AddCell(clUnidades);
                tabla.AddCell(clProductos);
                tabla.AddCell(clComentarios);
            }


            // Finalmente, añadimos la tabla al documento PDF y cerramos el documento
            doc.Add(tabla);

            doc.Close();
            writer.Close();
        }
    }
}
