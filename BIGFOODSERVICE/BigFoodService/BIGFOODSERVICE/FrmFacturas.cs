using BLL;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RestSharp;
using RestSharp;
using Newtonsoft.Json;
using System;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace BIGFOODSERVICE
{
    public partial class FrmFacturas : Form
    {
        public FrmFacturas()
        {
            InitializeComponent();

        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            try
            {
                this.ConsultaPorCodigo(this.txtCodigo.Text.Trim());
                ActualizarValoresFacturaSegunCantidad();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void ConsultaPorCodigo(string codigo)
        {
            try
            {
                var client = new HttpClient();
                string url;
                if (!string.IsNullOrWhiteSpace(txtCodigo.Text))
                {
                    url = $"http://ApiBigFoodService.somee.com/api/Producto/{codigo}";
                }
                else
                {
                    url = $"http://ApiBigFoodService.somee.com/api/Producto";
                }
                var response = await client.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();

                    // Verifica si el JSON es un array o un objeto
                    if (result.TrimStart().StartsWith("["))
                    {
                        // Es un array
                        var productos = JsonConvert.DeserializeObject<List<Productos>>(result);
                        this.dtgDatos.DataSource = productos;
                    }
                    else
                    {
                        // Es un objeto
                        var producto = JsonConvert.DeserializeObject<Productos>(result);
                        var productos = new List<Productos> { producto }; // Crea una lista con el único objeto
                        this.dtgDatos.DataSource = productos;
                    }

                    this.dtgDatos.AutoResizeColumns();
                    this.dtgDatos.ReadOnly = true;
                }
                else
                {
                    MessageBox.Show("Error al obtener datos", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private async void modificar(Productos producto)
        {
            try
            {
                var client = new RestClient("http://ApiBigFoodService.somee.com");
                var request = new RestRequest($"api/Producto", Method.Put);

                // Convertir el objeto producto a JSON
                string jsonBody = JsonConvert.SerializeObject(producto);

                // Agregar el JSON como parámetro en el cuerpo de la solicitud
                request.AddParameter("application/json", jsonBody, ParameterType.RequestBody);

                // Ejecutar la solicitud de manera asíncrona
                var response = await client.ExecuteAsync(request);

                if (response.IsSuccessful)
                {
                    MessageBox.Show("Producto actualizado correctamente en la API.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show($"Error al actualizar el producto en la API. Código de respuesta: {response.StatusCode}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FrmFacturas_Load(object sender, EventArgs e)
        {
            // Aquí puedes inicializar otros componentes del formulario si es necesario
        }

        private void ActualizarValoresFactura(Productos producto, int cantidad)
        {
            double subtotal = producto.PrecioVenta * cantidad;
            double montoDescuento = producto.Descuento * cantidad;
            double montoImpuesto = producto.Impuesto * cantidad;
            double total = subtotal + montoImpuesto - montoDescuento;

            txtSubTotal.Text = subtotal.ToString("F2");
            textMontoDescuento.Text = montoDescuento.ToString("F2");
            textMontoIm.Text = montoImpuesto.ToString("F2");
            textTotall.Text = total.ToString("F2");

            txtFecha.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }

        private void txtCantidad_TextChanged(object sender, EventArgs e)
        {
            ActualizarValoresFacturaSegunCantidad();
        }

        private void ActualizarValoresFacturaSegunCantidad()
        {
            if (dtgDatos.SelectedRows.Count > 0 && !string.IsNullOrWhiteSpace(txtCantidad.Text))
            {
                var selectedRow = dtgDatos.SelectedRows[0];
                var producto = new Productos
                {
                    CodigoInterno = selectedRow.Cells["CodigoInterno"].Value.ToString(),
                    CodBarra = Convert.ToInt32(selectedRow.Cells["CodBarra"].Value),
                    Descripcion = selectedRow.Cells["Descripcion"].Value.ToString(),
                    PrecioVenta = Convert.ToInt32(selectedRow.Cells["PrecioVenta"].Value),
                    Descuento = Convert.ToInt32(selectedRow.Cells["Descuento"].Value),
                    Impuesto = Convert.ToInt32(selectedRow.Cells["Impuesto"].Value),
                    UnidadMedida = selectedRow.Cells["UnidadMedida"].Value.ToString(),
                    PrecioCompra = Convert.ToInt32(selectedRow.Cells["PrecioCompra"].Value),
                    Usuario = selectedRow.Cells["Usuario"].Value.ToString(),
                    Existencia = Convert.ToInt32(selectedRow.Cells["Existencia"].Value)
                };

                int cantidad;
                if (int.TryParse(txtCantidad.Text, out cantidad))
                {
                    if (cantidad > producto.Existencia)
                    {
                        MessageBox.Show("La cantidad ingresada excede la existencia del producto.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    else
                    {
                        // Actualizar existencia del producto específico
                        int nuevaExistencia = producto.Existencia - cantidad;
                        producto.Existencia = nuevaExistencia;

                        // Luego podrías actualizar los valores de la factura basándote en la cantidad comprada
                        ActualizarValoresFactura(producto, cantidad);
                    
                    }
                }
                else
                {
                    MessageBox.Show("Por favor, ingrese una cantidad válida.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void dtgDatos_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            ActualizarValoresFacturaSegunCantidad();
        }
       private async Task<bool> RegistrarFactura(Facturas factura, int cantidadComprada)
{
    try
    {
                
                    var selectedRow = dtgDatos.SelectedRows[0];
                    var producto = new Productos
                    {
                        CodigoInterno = selectedRow.Cells["CodigoInterno"].Value.ToString(),
                        CodBarra = Convert.ToInt32(selectedRow.Cells["CodBarra"].Value),
                        Descripcion = selectedRow.Cells["Descripcion"].Value.ToString(),
                        PrecioVenta = Convert.ToInt32(selectedRow.Cells["PrecioVenta"].Value),
                        Descuento = Convert.ToInt32(selectedRow.Cells["Descuento"].Value),
                        Impuesto = Convert.ToInt32(selectedRow.Cells["Impuesto"].Value),
                        UnidadMedida = selectedRow.Cells["UnidadMedida"].Value.ToString(),
                        PrecioCompra = Convert.ToInt32(selectedRow.Cells["PrecioCompra"].Value),
                        Usuario = selectedRow.Cells["Usuario"].Value.ToString(),
                        Existencia = Convert.ToInt32(selectedRow.Cells["Existencia"].Value)
                    };

                    int cantidad;
                    if (int.TryParse(txtCantidad.Text, out cantidad))
                    {
                        if (cantidad > producto.Existencia)
                        {
                            MessageBox.Show("La cantidad ingresada excede la existencia del producto.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return true;
                        }
                        else
                        {
                            // Actualizar existencia del producto específico
                            int nuevaExistencia = producto.Existencia - cantidad;
                            producto.Existencia = nuevaExistencia;

                        // Luego podrías actualizar los valores de la factura basándote en la cantidad comprada
                        modificar(producto);
                    }
                    }
                 

                

                using (var client = new HttpClient())

        {


                    int productosComprados = cantidad;
            // 1. Agregar la factura a la API de facturas
            string urlFactura = "http://ApiBigFoodService.somee.com/api/Factura";
            var jsonFactura = JsonConvert.SerializeObject(factura);
            var contentFactura = new StringContent(jsonFactura, Encoding.UTF8, "application/json");

            var responseFactura = await client.PostAsync(urlFactura, contentFactura);

            if (!responseFactura.IsSuccessStatusCode)
            {
                var errorContentFactura = await responseFactura.Content.ReadAsStringAsync();
                MessageBox.Show($"Error al registrar la factura: {errorContentFactura}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            // Obtener la factura registrada (número, etc.) desde la respuesta
            var resultFactura = await responseFactura.Content.ReadAsStringAsync();
            var facturaRegistrada = JsonConvert.DeserializeObject<Facturas>(resultFactura);

            // 2. Actualizar existencia en la API de Productos para cada producto comprado
          
            // 3. Registrar en la bitácora (log) de la aplicación
            string urlBitacora = "http://ApiBigFoodService.somee.com/api/Bitacora";
            var bitacora = new Bitacora
            {
                Tabla = "Factura",
                Usuario = factura.Usuario,
                Maquina = "9",
                Fecha = DateTime.Now,
                TipoMov = "Agregar",
                Registro = "exitoso" // Ajusta cómo obtienes el número de registro correcto
            };

            var jsonBitacora = JsonConvert.SerializeObject(bitacora);
            var contentBitacora = new StringContent(jsonBitacora, Encoding.UTF8, "application/json");

            var responseBitacora = await client.PostAsync(urlBitacora, contentBitacora);

            if (!responseBitacora.IsSuccessStatusCode)
            {
                var errorContentBitacora = await responseBitacora.Content.ReadAsStringAsync();
                MessageBox.Show($"Error al registrar en la bitácora: {errorContentBitacora}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                // Aquí podrías manejar cómo deshacer la operación si falla el registro en la bitácora
            }

            // 4. Registrar en cuentas por cobrar si el estado es "Deuda"
            if (factura.Estado == 'D')
            {
                string urlCuentasPorCobrar = "http://ApiBigFoodService.somee.com/api/CuentasPorCobrar";
                var cuentasPorCobrar = new CuentasPorCobrar
                {
                    NumFactura = factura.Numero,
                    CodCliente = factura.CodCliente,
                    FechaFactura = factura.Fecha,
                    FechaRegistro = factura.Fecha,
                    MontoFactura = factura.Total,
                    Usuario = factura.Usuario,
                    Estado = 'D' // Estado de Deuda
                };

                var jsonCuentasPorCobrar = JsonConvert.SerializeObject(cuentasPorCobrar);
                var contentCuentasPorCobrar = new StringContent(jsonCuentasPorCobrar, Encoding.UTF8, "application/json");

                var responseCuentasPorCobrar = await client.PostAsync(urlCuentasPorCobrar, contentCuentasPorCobrar);

                if (!responseCuentasPorCobrar.IsSuccessStatusCode)
                {
                    var errorContentCuentasPorCobrar = await responseCuentasPorCobrar.Content.ReadAsStringAsync();
                    MessageBox.Show($"Error al registrar en cuentas por cobrar: {errorContentCuentasPorCobrar}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    // Aquí podrías manejar cómo deshacer la operación si falla el registro en cuentas por cobrar
                }
            }

            return true; // Indicar que el registro fue exitoso
        }
    }
    catch (Exception ex)
    {
        MessageBox.Show($"Error al registrar la factura: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        return false; // Indicar que hubo un error al intentar registrar la factura
            }
        }




        private async void button2_Click(object sender, EventArgs e)
        {
            try
            {
                // Preparar los datos de la factura y los productos comprados
                Facturas factura = new Facturas
                {
                    Numero = int.Parse(txtFactura.Text),
                    Fecha = DateTime.Now,
                    CodCliente = txtCodCliente.Text,
                    SubTotal = Convert.ToDouble(txtSubTotal.Text),
                    MontoDescuento = Convert.ToDouble(textMontoDescuento.Text),
                    MontoImpuesto = Convert.ToDouble(textMontoIm.Text),
                    Total = Convert.ToDouble(textTotall.Text),
                    Estado = ObtenerEstado(), // Obtener estado desde el ComboBox
                    Usuario = txtUsuario.Text,
                    TipoPago = ObtenerTipoPago(), // Obtener tipo de pago desde el ComboBox
                    Condicion = ObtenerCondicion() // Obtener condición desde el ComboBox
                };

                List<Productos> productosComprados = ObtenerProductosComprados(); // Implementa este método según cómo obtienes los productos
                int cantidadComprada = ObtenerCantidadComprada(); // Implementa este método según cómo obtienes la cantidad comprada

                // Llama al método para registrar la factura y maneja el resultado
                bool registroExitoso = await RegistrarFactura(factura, cantidadComprada);

                if (registroExitoso)
                {
                    // Obtener el cliente para enviar la factura por correo
                    Clientes cliente = await ObtenerClientePorCodigo(factura.CodCliente);

                    // Generar y enviar la factura por correo electrónico
                    if (cliente != null)
                    {
                        try
                        {
                            Email emailHelper = new Email();
                            emailHelper.GenerarYEnviarFactura(cliente, factura);
                            MessageBox.Show("La factura se envió por correo electrónico.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error al enviar la factura por correo electrónico: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        MessageBox.Show("No se pudo obtener la información del cliente para enviar la factura por correo electrónico.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                    LimpiarFormulario(); // Implementa este método para limpiar el formulario después del registro
                }
                else
                {
                    MessageBox.Show("Hubo un problema al registrar la factura. Por favor, inténtelo nuevamente.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al registrar la factura: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        private async Task<Clientes> ObtenerClientePorCodigo(string codigoCliente)
        {
            try
            {
                var client = new HttpClient();
                string url = $"http://ApiBigFoodService.somee.com/api/Cliente/{codigoCliente}";

                var response = await client.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    var cliente = JsonConvert.DeserializeObject<Clientes>(result);
                    return cliente;
                }
                else
                {
                    MessageBox.Show("No se pudo obtener la información del cliente.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al obtener la información del cliente: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }
        private char ObtenerEstado()
        {
            // Implementa la lógica para obtener el estado desde el ComboBox
            if (cmbEstado.SelectedItem != null)
            {
                string estadoSeleccionado = cmbEstado.SelectedItem.ToString();

                // Aquí debes convertir el string a char. 
                // Esto puede variar dependiendo de cómo representas los estados en el ComboBox.
                // Asumiendo que los estados están representados por letras o caracteres específicos,
                // puedes hacer algo como esto:

                if (estadoSeleccionado.Length > 0)
                {
                    return estadoSeleccionado[0]; // Devuelve el primer carácter del string
                }
            }

            return '\0'; // Devuelve un valor por defecto, como char nulo, según tu lógica
        }

        private string ObtenerTipoPago()
        {
            // Implementa la lógica para obtener el tipo de pago desde el ComboBox
            if (cmbTipoPago.SelectedItem != null)
            {
                return cmbTipoPago.SelectedItem.ToString();
            }
            return ""; // Devuelve un valor por defecto o maneja el caso según tu lógica
        }

        private string ObtenerCondicion()
        {
            // Implementa la lógica para obtener la condición desde el ComboBox
            if (cmbCondicion.SelectedItem != null)
            {
                return cmbCondicion.SelectedItem.ToString();
            }
            return ""; // Devuelve un valor por defecto o maneja el caso según tu lógica
        }

        private List<Productos> ObtenerProductosComprados()
        {
            List<Productos> productosComprados = new List<Productos>();

            foreach (DataGridViewRow row in dtgDatos.SelectedRows)
            {
                var producto = new Productos
                {
                    CodigoInterno = row.Cells["CodigoInterno"].Value.ToString(),
                    CodBarra = Convert.ToInt32(row.Cells["CodBarra"].Value),
                    Descripcion = row.Cells["Descripcion"].Value.ToString(),
                    PrecioVenta = Convert.ToInt32(row.Cells["PrecioVenta"].Value),
                    Descuento = Convert.ToInt32(row.Cells["Descuento"].Value),
                    Impuesto = Convert.ToInt32(row.Cells["Impuesto"].Value),
                    UnidadMedida = row.Cells["UnidadMedida"].Value.ToString(),
                    PrecioCompra = Convert.ToInt32(row.Cells["PrecioCompra"].Value),
                    Usuario = row.Cells["Usuario"].Value.ToString(),
                    Existencia = Convert.ToInt32(row.Cells["Existencia"].Value) // Ajustado a int
                };

                productosComprados.Add(producto);
            }

            return productosComprados;
        }
        private int ObtenerCantidadComprada()
        {
            // Implementa la lógica para obtener la cantidad comprada
            return 1; // Ejemplo, ajusta según tu implementación
        }

        private void LimpiarFormulario()
        {
            // Implementa la lógica para limpiar el formulario después del registro exitoso
            // Puedes limpiar los campos de texto, DataGridView u otros controles según tu implementación
            txtCodCliente.Text = "";
            txtSubTotal.Text = "";
            textMontoDescuento.Text = "";
            textMontoIm.Text = "";
            textTotall.Text = "";
            txtUsuario.Text = "";
            // Limpia otros controles según sea necesario
        }

        private void groupBox6_Enter(object sender, EventArgs e)
        {

        }

        private void textMontoDescuento_TextChanged(object sender, EventArgs e)
        {

        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }

}