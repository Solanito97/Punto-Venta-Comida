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
    public partial class Frm : Form
    {
        public Frm()
        {
            InitializeComponent();

            // Configurar el DataGridView
            dtgDatos.AutoGenerateColumns = true; // Generar columnas automáticamente
            dtgDatos.ReadOnly = true; // DataGridView de solo lectura
        }
        private async void ConsultaBitacora()
        {
            try
            {
                var client = new HttpClient();
                string url = "http://ApiBigFoodService.somee.com/api/CuentasPorCobrar";

                var response = await client.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();

                    // Deserializar el JSON en una lista de Bitacora
                    var Consultas = JsonConvert.DeserializeObject<List<CuentasPorCobrar>>(result);

                    // Asignar la lista al DataSource del DataGridView
                    this.dtgDatos.DataSource = Consultas;

                    // Ajustar las columnas automáticamente
                    this.dtgDatos.AutoResizeColumns();
                }
                else
                {
                    MessageBox.Show("Error al obtener datos de la Bitacora", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FrmLoad(object sender, EventArgs e)
        {
            // Llamar a ConsultaBitacora al cargar el formulario para mostrar los datos
            ConsultaBitacora();
        }

        private void dtgDatos_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Aquí no necesitas llamar a ConsultaBitacora nuevamente, ya que se llama automáticamente al hacer clic
            // ConsultaBitacora();
        }
        private void Frm_Load(object sender, EventArgs e)
        {
            ConsultaBitacora();
        }

        private void dtgDatos_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
