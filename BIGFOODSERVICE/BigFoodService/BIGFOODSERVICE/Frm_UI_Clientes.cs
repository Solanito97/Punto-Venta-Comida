using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

//se utilizan los demas proyectos 
using BLL;
using DAL;
using Newtonsoft.Json;

namespace BIGFOODSERVICE
{
    public partial class Frm_UI_Clientes : Form
    {
        private Clientes varObjClientes = null;
        private Conexion varObjConexion = null;
        /// <summary>
        /// constructor por omision del formulario
        /// </summary>
        public int funcion = 0;
        public Frm_UI_Clientes()
        {
            InitializeComponent();
            this.varObjConexion = new Conexion();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private async Task<bool> RegistrarClientes()
        {
            try
            {
                var client = new HttpClient();
                Clientes clientes = new Clientes();
                ClienteGometa clienteAPI = new ClienteGometa();

                clientes.CedulaLegal = this.txtCedula.Text.Trim();
                clientes.Email = this.txtEmail.Text.Trim();
                clientes.Usuario = this.txtUsuario.Text.Trim();
                clientes.FechaRegistro = DateTime.Now;
                clientes.Estado = 'A';
                clientes.NombreCompleto = txtNombreCompleto.Text.Trim();
                clientes.TipoCedula = txtTipoCed.Text.Trim();

                if (txtTipoCed.ReadOnly == true)
                    clienteAPI = await this.InformacionCliente(clientes.CedulaLegal);

                // Asignar los valores obtenidos desde clienteAPI si están disponibles
                if (clienteAPI != null && clienteAPI.nombre != null)
                {
                    clientes.NombreCompleto = clienteAPI.nombre;
                    clientes.TipoCedula = clienteAPI.tipoIdentificacion;
                }

                // Transformar el valor de TipoCedula
                if (clientes.TipoCedula == "01")
                {
                    clientes.TipoCedula = "Nacional";
                }
                else
                {
                    clientes.TipoCedula = "Extranjero";
                }

                string url = $"http://ApiBigFoodService.somee.com/api/Cliente";
                var json = JsonConvert.SerializeObject(clientes);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await client.PostAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    var varCliente = JsonConvert.DeserializeObject<Clientes>(result);
                    return true;
                }
                else
                {
                    MessageBox.Show("Error al obtener datos", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Se produjo un error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        private async Task<ClienteGometa> InformacionCliente(string cedula)
        {
            try
            {
                var client = new HttpClient();

                string url = $"https://apis.gometa.org/cedulas/{cedula}";

                var response = await client.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    ClienteGometa varCliente = JsonConvert.DeserializeObject<ClienteGometa>(result);
                    if (varCliente.nombre == null)
                    {
                        MessageBox.Show("Credenciales inválidas, Escriba su tipo de cedula y su nombre completo", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        txtTipoCed.ReadOnly = false;
                        txtNombreCompleto.ReadOnly = false;
                        return null;
                    }
                    return varCliente;
                }
                return null;
            }
            catch (Exception ex)
            {

                throw ex;
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
                    if (await this.RegistrarClientes())
                    {
                        MessageBox.Show("Cliente Registrado Correctamente", "Confirmado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Error al registrar Cliente", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    if (await this.Modificar())
                    {
                        //modificar los datos del usuario
                        MessageBox.Show("Los datos del Cliente se modificaron correctamente", "Proceso Realizado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Error al modificar Cliente", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                this.Close();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }//cierre de btn

        private void Frm_UI_Clientes_Load(object sender, EventArgs e)
        {
            try
            {
                if (this.funcion == 1)
                {
                    this.btnRegistrar.Text = "Modificar";
                    this.txtCedula.ReadOnly = true;
                }
                else
                {
                    this.btnRegistrar.Text = "Registrar";
                    this.txtCedula.ReadOnly = false;
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
                Clientes clientes = new Clientes();
                clientes.CedulaLegal = this.txtCedula.Text.Trim() ;
                txtCedula.ReadOnly = true;
                clientes.Email = this.txtEmail.Text.Trim();
                clientes.Usuario = this.txtUsuario.Text.Trim();
                clientes.FechaRegistro = DateTime.Now;
                clientes.Estado = 'A';
                clientes.NombreCompleto = txtNombreCompleto.Text.Trim();
                clientes.TipoCedula = txtTipoCed.Text.Trim();
                var client = new HttpClient();

                string url = $"http://ApiBigFoodService.somee.com/api/Cliente";
                var json = JsonConvert.SerializeObject(clientes);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await client.PutAsync(url, content);

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
        public void PasarDatos(Clientes temp)
        {
            try
            {
                this.txtCedula.Text = temp.CedulaLegal;
                txtCedula.ReadOnly = true;
                this.txtNombreCompleto.Text = temp.NombreCompleto;
                this.txtEmail.Text = temp.Email;
                this.txtTipoCed.Text = temp.TipoCedula;
                this.txtUsuario.Text = temp.Usuario;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }//CIERRRE DE CLASE
    }//CIERRE DE NAMESPACE
}//Metodo encargado de verificar las credenciales del usuario


        
        