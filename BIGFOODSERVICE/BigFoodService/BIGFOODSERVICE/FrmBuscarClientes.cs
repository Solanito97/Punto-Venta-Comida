using BLL;
using DAL;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BIGFOODSERVICE
{
    public partial class FrmBuscarClientes : Form
    {
        private Conexion varObjConexion = null;
        public FrmBuscarClientes()
        {
            InitializeComponent();

        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            try
            {
                this.MostrarRegistrarClientes(0);
                //se actualiza la lista de forma automatica, el espacio en blanco lo actualiza automaticamente
                this.ConsultaPorNombre("");
            }
            catch (Exception)
            {

                MessageBox.Show("Error no se pudo mostar la ventana", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }
        private async Task ConsultaPorNombre(string nombre)
        {
            try
            {
                var client = new HttpClient();
                string url;

                if (!string.IsNullOrWhiteSpace(nombre))
                {
                    //url = $"http://ApiBigFoodService.somee.com/BuscarNombre/{nombre}";
                    url = $"http://www.ApiBigFoodService.somee.com/BuscarNombre/{nombre}";
                }
                else
                {
                    url = "http://www.ApiBigFoodService.somee.com/API/Cliente";
                }

                var response = await client.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    var varCliente = JsonConvert.DeserializeObject<List<Clientes>>(result);
                    this.dtgDatos.DataSource = varCliente;
                    this.dtgDatos.AutoResizeColumns();
                    this.dtgDatos.ReadOnly = true;
                }
                else
                {
                    MessageBox.Show("Error al obterner datos", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                // Manejo de excepciones de manera más elegante
                MessageBox.Show($"Se produjo un error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtNombre_TextChanged(object sender, EventArgs e)
        {
            try
            {
                this.ConsultaPorNombre(this.txtNombre.Text.Trim());
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void modificarToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.dtgDatos.SelectedRows.Count <= 0)
                {
                    throw new Exception("Seleccione la fila del Cliente que desea Modificar");
                }
                //pantalla multifuncional
                this.MostrarRegistrarClientes(1);
                //se actualizan los usuarios 
                this.ConsultaPorNombre(" ");
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void MostrarRegistrarClientes( int funcion)
        {
            //se instancia la pantalla para registrar usuarios
            Frm_UI_Clientes frm = new Frm_UI_Clientes();

            //funciona realizar 0= Registrar y 1 = modificar
            frm.funcion = funcion;
            if(funcion == 1)
                {
                //se crea la instancia del objeto usuario
                Clientes temp = new Clientes();
                //se rellenan los datos del objeto con la fila seleccionada
                temp.CedulaLegal = this.dtgDatos.SelectedRows[0].Cells[0].Value.ToString();;
                temp.TipoCedula = this.dtgDatos.SelectedRows[0].Cells[1].Value.ToString();
                temp.NombreCompleto = this.dtgDatos.SelectedRows[0].Cells[2].Value.ToString();
                temp.Email = this.dtgDatos.SelectedRows[0].Cells[3].Value.ToString();
                temp.Usuario = this.dtgDatos.SelectedRows[0].Cells[6].Value.ToString();
                //se pasa el objeto al formulario para mostrar los datos en pantalla
                frm.PasarDatos(temp);

            }
            //se muestra la pantalla de forma exclusiva 
            frm.ShowDialog();
            //se liberan recursos
            frm.Dispose();
        }

        private async void eliminarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                //se valida que usuario tenga una fila seleccionada
                if (this.dtgDatos.SelectedRows.Count <= 0)
                {
                    throw new Exception("Seleccione la fila del Cliente que desea eliminar");
                }
                //solicitamos confirmacion para eliminar 
                if (MessageBox.Show("Desea Eliminar el Cliente seleccionado", "Confirmar", MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    //se toma la fila seleccionada y se extrae el login en a celda 0
                    if(await this.EjecutarEliminar(this.dtgDatos.SelectedRows[0].Cells[0].Value.ToString()))
                    {
                        MessageBox.Show("El cliente Fue eliminado correctamente", "Confirmar", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                    else
                    {
                        MessageBox.Show("Error al eliminar el cliente", "Error", MessageBoxButtons.OK,MessageBoxIcon.Error);
                    }
                    //se actualia la lista
                    this.ConsultaPorNombre(" ");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task<bool> EjecutarEliminar(string cedula)
        {
            try
            {
                var client = new HttpClient();

                // Asegúrate de que la URL es correcta y el servidor de la API está en funcionamiento
                string url = $"http://www.ApiBigFoodService.somee.com/api/Cliente/{cedula}";

                var response = await client.DeleteAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    // Mostrar detalles del error si la respuesta no es exitosa
                    var errorContent = await response.Content.ReadAsStringAsync();
                    MessageBox.Show($"Error al eliminar el cliente: {response.StatusCode} - {errorContent}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
            catch (HttpRequestException httpEx)
            {
                // Manejo específico para errores de HTTP
                MessageBox.Show(httpEx.Message, "Error de HTTP", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            catch (Exception ex)
            {
                // Muestra el MessageBox con el mensaje de error
                MessageBox.Show(ex.Message, "Error, El usuario tiene cuentas pendientes", MessageBoxButtons.OK, MessageBoxIcon.Error);
                // Devuelve false para indicar que la eliminación no fue exitosa
                return false;
            }
        }

        private void panelParametros_Paint(object sender, PaintEventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }//cierra clase
}//cierra namespace
