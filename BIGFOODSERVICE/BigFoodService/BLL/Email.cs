using BLL;
using PdfSharp.Pdf;
using PdfSharp.Drawing;
using System;
using System.IO;
using MimeKit;
using MailKit.Net.Smtp;

public class Email
{
    public void GenerarYEnviarFactura(Clientes cliente, Facturas factura)
    {
        // Generar la factura en formato PDF
        byte[] facturaPdf = GenerarFacturaPDF(cliente, factura);

        // Enviar la factura por correo electrónico
        EnviarFacturaPorCorreo(cliente.Email, facturaPdf);
    }

    private byte[] GenerarFacturaPDF(Clientes cliente, Facturas factura)
    {
        using (MemoryStream stream = new MemoryStream())
        {
            PdfDocument document = new PdfDocument();
            PdfPage page = document.AddPage();
            XGraphics gfx = XGraphics.FromPdfPage(page);
            XFont font = new XFont("Verdana", 12);

            // Encabezado de la factura
            gfx.DrawString("Factura N°: " + factura.Numero, font, XBrushes.Black, new XRect(40, 40, page.Width - 80, 20), XStringFormats.TopLeft);
            gfx.DrawString("Fecha: " + factura.Fecha.ToShortDateString(), font, XBrushes.Black, new XRect(40, 60, page.Width - 80, 20), XStringFormats.TopLeft);
            gfx.DrawString("Cliente: " + cliente.NombreCompleto, font, XBrushes.Black, new XRect(40, 80, page.Width - 80, 20), XStringFormats.TopLeft);

            // Detalles de la factura
            int yPoint = 120;
            gfx.DrawString("Producto", font, XBrushes.Black, new XRect(40, yPoint, page.Width - 80, 20), XStringFormats.TopLeft);
            gfx.DrawString("Cantidad", font, XBrushes.Black, new XRect(200, yPoint, page.Width - 80, 20), XStringFormats.TopLeft);
            gfx.DrawString("Precio", font, XBrushes.Black, new XRect(400, yPoint, page.Width - 80, 20), XStringFormats.TopLeft);

            // Aquí deberías agregar la lógica para iterar sobre los productos comprados y agregarlos a la tabla.
            // Ejemplo:
            // yPoint += 20;
            // foreach (var producto in factura.Productos)
            // {
            //     gfx.DrawString(producto.Nombre, font, XBrushes.Black, new XRect(40, yPoint, page.Width - 80, 20), XStringFormats.TopLeft);
            //     gfx.DrawString(producto.Cantidad.ToString(), font, XBrushes.Black, new XRect(200, yPoint, page.Width - 80, 20), XStringFormats.TopLeft);
            //     gfx.DrawString(producto.Precio.ToString("C"), font, XBrushes.Black, new XRect(400, yPoint, page.Width - 80, 20), XStringFormats.TopLeft);
            //     yPoint += 20;
            // }

            // Total de la factura
            yPoint += 20;
            gfx.DrawString("Subtotal: " + factura.SubTotal.ToString("C"), font, XBrushes.Black, new XRect(40, yPoint, page.Width - 80, 20), XStringFormats.TopLeft);
            gfx.DrawString("Descuento: " + factura.MontoDescuento.ToString("C"), font, XBrushes.Black, new XRect(40, yPoint + 20, page.Width - 80, 20), XStringFormats.TopLeft);
            gfx.DrawString("Impuesto: " + factura.MontoImpuesto.ToString("C"), font, XBrushes.Black, new XRect(40, yPoint + 40, page.Width - 80, 20), XStringFormats.TopLeft);
            gfx.DrawString("Total: " + factura.Total.ToString("C"), font, XBrushes.Black, new XRect(40, yPoint + 60, page.Width - 80, 20), XStringFormats.TopLeft);

            document.Save(stream, false);
            return stream.ToArray();
        }
    }

    private void EnviarFacturaPorCorreo(string emailCliente, byte[] facturaPdf)
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress("Tu Empresa", "pruebaweb0415@Outlook.com"));
        message.To.Add(new MailboxAddress(emailCliente, emailCliente));
        message.Subject = "Factura de compra";

        var body = new TextPart("plain")
        {
            Text = "Adjuntamos la factura de su compra."
        };

        var attachment = new MimePart("application", "pdf")
        {
            Content = new MimeContent(new MemoryStream(facturaPdf), ContentEncoding.Default),
            ContentDisposition = new ContentDisposition(ContentDisposition.Attachment),
            ContentTransferEncoding = ContentEncoding.Base64,
            FileName = "Factura.pdf"
        };

        var multipart = new Multipart("mixed");
        multipart.Add(body);
        multipart.Add(attachment);

        message.Body = multipart;

        using (var client = new MailKit.Net.Smtp.SmtpClient())
        {
            client.Connect("smtp-mail.outlook.com", 587, false);
            client.Authenticate("pruebaweb0415@Outlook.com", "@Ucr2024");

            client.Send(message);
            client.Disconnect(true);
        }
    }
}
