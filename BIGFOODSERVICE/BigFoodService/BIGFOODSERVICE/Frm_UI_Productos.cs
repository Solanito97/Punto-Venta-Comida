using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BLL;
using DAL;
using Newtonsoft.Json;

namespace BIGFOODSERVICE
{
    public partial class Frm_UI_Productos : Form
    {
        public int funcion = 0;
        public Frm_UI_Productos()
        {
            InitializeComponent();
        }

        private async Task<bool> RegistrarProductos()
        {
            try
            {
                var client = new HttpClient();
                Productos productos = new Productos();

                // Validación de datos
                productos.CodigoInterno = this.txtCodigoInt.Text.Trim();

                if (!long.TryParse(this.txtCodigoBarr.Text.Trim(), out long codBarra) || codBarra < 0)
                {
                    MessageBox.Show("El código de barras no tiene el formato correcto o es negativo.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                productos.CodBarra = codBarra;

                productos.Descripcion = this.txtDescripcion.Text.Trim();

                if (!int.TryParse(this.txtPrecioVenta.Text.Trim(), out int precioVenta) || precioVenta < 0)
                {
                    MessageBox.Show("El precio de venta no tiene el formato correcto o es negativo.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

                if (!int.TryParse(this.txtPrecioCompra.Text.Trim(), NumberStyles.Any, CultureInfo.InvariantCulture, out int precioCompra) || precioCompra < 0)
                {
                    MessageBox.Show("El precio de compra no tiene el formato correcto o es negativo.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

                // Validación adicional: El precio de venta no puede ser menor al precio de compra
                if (precioVenta < precioCompra)
                {
                    MessageBox.Show("El precio de venta no puede ser menor al precio de compra.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

                productos.PrecioVenta = precioVenta;
                productos.PrecioCompra = precioCompra;

                if (!int.TryParse(this.txtDescuento.Text.Trim(), NumberStyles.Any, CultureInfo.InvariantCulture, out int descuento) || descuento < 0)
                {
                    MessageBox.Show("El descuento no tiene el formato correcto o es negativo.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                productos.Descuento = descuento;

                if (!int.TryParse(this.txtImpuesto.Text.Trim(), NumberStyles.Any, CultureInfo.InvariantCulture, out int impuesto) || impuesto < 0)
                {
                    MessageBox.Show("El impuesto no tiene el formato correcto o es negativo.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                productos.Impuesto = impuesto;

                productos.UnidadMedida = this.txtUnidadMed.Text.Trim();

                productos.Usuario = this.txtUsuario.Text.Trim();

                if (!int.TryParse(this.txtExistencia.Text.Trim(), out int existencia) || existencia < 0)
                {
                    MessageBox.Show("La existencia no tiene el formato correcto o es negativa.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                productos.Existencia = existencia;

                string url = $"http://ApiBigFoodService.somee.com/api/Producto";
                var json = JsonConvert.SerializeObject(productos);

                // Imprimir el JSON en la consola
                Console.WriteLine(json);

                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await client.PostAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    var varProducto = JsonConvert.DeserializeObject<Productos>(result);
                    return true;
                }
                else
                {
                    // Captura el contenido de la respuesta en caso de error
                    var errorContent = await response.Content.ReadAsStringAsync();
                    MessageBox.Show($"Error al obtener datos: {errorContent}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                return false;
            }
            catch (Exception ex)
            {
                // Lanza una excepción más detallada
                throw new Exception("Error al registrar productos", ex);
            }
        }


        private async void btnRegistrar_Click(object sender, EventArgs e)
        {
            try
            {
                //0 reporesenta proceso de registrar
                if (this.funcion == 0)
                {
                    // se registra los datos del usuario
                    if (await this.RegistrarProductos())
                    {
                        MessageBox.Show("Producto Registrado Correctamente", "Confirmado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Error al registrar Producto", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    if (await this.Modificar())
                    {
                        //modificar los datos del usuario
                        MessageBox.Show("Los datos del Producto se modificaron correctamente", "Proceso Realizado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Error al modificar Producto", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                this.Close();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Frm_UI_Productos_Load(object sender, EventArgs e)
        {
            try
            {
                if (this.funcion == 1)
                {
                    this.btnRegistrar.Text = "Modificar";
                    this.txtCodigoInt.ReadOnly = true;
                }
                else
                {
                    this.btnRegistrar.Text = "Registrar";

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task<bool> Modificar()
        {
            try
            {
                Productos productos = new Productos();

                // Validación de datos
                productos.CodigoInterno = this.txtCodigoInt.Text.Trim();

                if (!long.TryParse(this.txtCodigoBarr.Text.Trim(), out long codBarra) || codBarra < 0)
                {
                    MessageBox.Show("El código de barras no tiene el formato correcto o es negativo.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                productos.CodBarra = codBarra;

                productos.Descripcion = this.txtDescripcion.Text.Trim();

                if (!int.TryParse(this.txtPrecioVenta.Text.Trim(), out int precioVenta) || precioVenta < 0)
                {
                    MessageBox.Show("El precio de venta no tiene el formato correcto o es negativo.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

                if (!int.TryParse(this.txtPrecioCompra.Text.Trim(), NumberStyles.Any, CultureInfo.InvariantCulture, out int precioCompra) || precioCompra < 0)
                {
                    MessageBox.Show("El precio de compra no tiene el formato correcto o es negativo.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

                // Validación adicional: El precio de venta no puede ser menor al precio de compra
                if (precioVenta < precioCompra)
                {
                    MessageBox.Show("El precio de venta no puede ser menor al precio de compra.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

                productos.PrecioVenta = precioVenta;
                productos.PrecioCompra = precioCompra;

                if (!int.TryParse(this.txtDescuento.Text.Trim(), NumberStyles.Any, CultureInfo.InvariantCulture, out int descuento) || descuento < 0)
                {
                    MessageBox.Show("El descuento no tiene el formato correcto o es negativo.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                productos.Descuento = descuento;

                if (!int.TryParse(this.txtImpuesto.Text.Trim(), NumberStyles.Any, CultureInfo.InvariantCulture, out int impuesto) || impuesto < 0)
                {
                    MessageBox.Show("El impuesto no tiene el formato correcto o es negativo.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                productos.Impuesto = impuesto;

                productos.UnidadMedida = this.txtUnidadMed.Text.Trim();

                productos.Usuario = this.txtUsuario.Text.Trim();

                if (!int.TryParse(this.txtExistencia.Text.Trim(), out int existencia) || existencia < 0)
                {
                    MessageBox.Show("La existencia no tiene el formato correcto o es negativa.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                productos.Existencia = existencia;

                var client = new HttpClient();

                string url = $"http://ApiBigFoodService.somee.com/api/Producto";
                var json = JsonConvert.SerializeObject(productos);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await client.PutAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    MessageBox.Show($"Error al obtener datos: {errorContent}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                return false;
            }
            catch (Exception ex)
            {
                // Lanza una excepción más detallada
                throw new Exception("Error al modificar productos", ex);
            }
        }


        public void PasarDatos(Productos temp)
        {
            try
            {
                this.txtCodigoInt.Text = temp.CodigoInterno.ToString();
                this.txtCodigoBarr.Text = temp.CodBarra.ToString();
                this.txtDescripcion.Text = temp.Descripcion;
                this.txtPrecioVenta.Text = temp.PrecioVenta.ToString();
                this.txtDescuento.Text = temp.Descuento.ToString();
                this.txtImpuesto.Text = temp.Impuesto.ToString();
                this.txtUnidadMed.Text = temp.UnidadMedida;
                this.txtPrecioCompra.Text = temp.PrecioCompra.ToString();
                this.txtUsuario.Text = temp.Usuario;
                this.txtExistencia.Text = temp.Existencia.ToString();

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }//cierre de clase
}//cierre de namespace
