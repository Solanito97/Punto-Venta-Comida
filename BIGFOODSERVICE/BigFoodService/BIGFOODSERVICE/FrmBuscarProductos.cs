using BLL;
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
    public partial class FrmBuscarProductos : Form
    {
        int funcion = 0;
        public FrmBuscarProductos()
        {
            InitializeComponent();
        }

        private void txtCodigo_TextChanged(object sender, EventArgs e)
        {
            try
            {
                this.ConsultaPorCodigo(this.txtCodigo.Text.Trim());
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        private void btnNuevo_Click(object sender, EventArgs e)
        {
            try
            {
                this.MostrarRegistrarProductos(0);
                //se actualiza la lista de forma automatica, el espacio en blanco lo actualiza automaticamente
                this.ConsultaPorCodigo("");
            }
            catch (Exception)
            {

                MessageBox.Show("Error no se pudo mostar la ventana", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

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

        private void MostrarRegistrarProductos(int funcion)
        {
            // Se instancia el formulario para registrar o modificar productos
            Frm_UI_Productos frm = new Frm_UI_Productos();

            // Se asigna la función (0 = Registrar, 1 = Modificar)
            frm.funcion = funcion;

            if (funcion == 1 && this.dtgDatos.SelectedRows.Count > 0)
            {
                // Se crea una instancia del objeto Productos
                Productos temp = new Productos();

                // Se obtienen los datos de la fila seleccionada y se asignan al objeto Productos
                temp.CodigoInterno = this.dtgDatos.SelectedRows[0].Cells["CodigoInterno"].Value.ToString();
                temp.CodBarra = Convert.ToInt32(this.dtgDatos.SelectedRows[0].Cells["CodBarra"].Value);
                temp.Descripcion = this.dtgDatos.SelectedRows[0].Cells["Descripcion"].Value.ToString();

                // PrecioVenta es int, por lo tanto se usa Convert.ToInt32
                temp.PrecioVenta = Convert.ToInt32(this.dtgDatos.SelectedRows[0].Cells["PrecioVenta"].Value);

                // Descuento e Impuesto son Double
                temp.Descuento = Convert.ToInt32(this.dtgDatos.SelectedRows[0].Cells["Descuento"].Value);
                temp.Impuesto = Convert.ToInt32(this.dtgDatos.SelectedRows[0].Cells["Impuesto"].Value);

                // UnidadMedida, PrecioCompra, Usuario son strings
                temp.UnidadMedida = this.dtgDatos.SelectedRows[0].Cells["UnidadMedida"].Value.ToString();
                temp.PrecioCompra = Convert.ToInt32(this.dtgDatos.SelectedRows[0].Cells["PrecioCompra"].Value);
                temp.Usuario = this.dtgDatos.SelectedRows[0].Cells["Usuario"].Value.ToString();

                // Existencia es int
                temp.Existencia = Convert.ToInt32(this.dtgDatos.SelectedRows[0].Cells["Existencia"].Value);

                // Se pasa el objeto al formulario para mostrar los datos en pantalla
                frm.PasarDatos(temp);
            }

            // Se muestra el formulario de forma modal
            frm.ShowDialog();

            // Se liberan recursos
            frm.Dispose();
        }

        private async Task<bool> EjecutarEliminar(string codigo)
        {
            try
            {
                var client = new HttpClient();
                int CodigoInterno = int.Parse(codigo);
                string url = $"http://ApiBigFoodService.somee.com/api/Producto/{CodigoInterno}";
                var response = await client.DeleteAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                return false;

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private async void eliminarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                //se valida que usuario tenga una fila seleccionada
                if (this.dtgDatos.SelectedRows.Count <= 0)
                {
                    throw new Exception("Seleccione la fila del Producto que desea eliminar");
                }
                //solicitamos confirmacion para eliminar 
                if (MessageBox.Show("Desea Eliminar el Producto seleccionado", "Confirmar", MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    //se toma la fila seleccionada y se extrae el login en a celda 0
                    if (await this.EjecutarEliminar(this.dtgDatos.SelectedRows[0].Cells[0].Value.ToString()))
                    {
                        MessageBox.Show("El cliente Fue eliminado correctamente", "Confirmar", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                    else
                    {
                        MessageBox.Show("Error al eliminar el producto", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    //se actualia la lista
                    this.ConsultaPorCodigo(" ");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void modificarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.dtgDatos.SelectedRows.Count <= 0)
                {
                    throw new Exception("Seleccione la fila del Producto que desea Modificar");
                }
                //pantalla multifuncional
                this.MostrarRegistrarProductos(1);
                //se actualizan los usuarios 
                this.ConsultaPorCodigo(" ");
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void dtgDatos_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void panelHeader_Paint(object sender, PaintEventArgs e)
        {

        }

        private void FrmBuscarProductos_Load(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void pictureBox1_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }
    }//cierre de Clase
}//cierre de namespace
