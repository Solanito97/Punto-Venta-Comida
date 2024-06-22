using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

//se utilizan los demas proyectos 
using BLL;
using DAL;


namespace BIGFOODSERVICE
{
    public partial class Frm_UI_Users : Form
    {
        private Usuario varObjUsuario = null;
        private Conexion varObjConexion = null;
        /// <summary>
        /// constructor por omision del formulario
        /// </summary>
        public int funcion = 0;
        public Frm_UI_Users()
        {
            InitializeComponent();
            this.varObjConexion = new Conexion();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void CrearUsuario()
        {
            try
            {
                // se instancia el usuario
                this.varObjUsuario = new Usuario();
                // se rellena los datos del usuario con los valorees digitados a nivel de front end
                this.varObjUsuario.Login = this.txtLogin.Text.Trim();
                this.varObjUsuario.Password = this.txtPassword.Text.Trim();
                if (! this.varObjUsuario.ConfirmarPassword(this.txtConfirmar.Text.Trim()))
                {
                    throw new Exception("La confirmacion es incorrecta debe verificar el Password");
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }//cierre de crear usuario

        private void Registrar()
        {
            try
            {
                //por medio de la Capa DAL se envia el objeto uduario para almacenar sus datos en la BD 
                this.varObjConexion.RegistrarUsuarios(this.varObjUsuario);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }//cierre de registrar

        private void btnRegistrar_Click(object sender, EventArgs e)
        {
            try
            {
                // se crea el usuario
                this.CrearUsuario();
                //0 reporesenta proceso de registrar
                if (this.funcion == 0)
                {
                    // se registra los datos del usuario
                    this.Registrar();
                    MessageBox.Show("Usuario Registrado Correctamente", "Confirmado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    //modificar los datos del usuario
                    this.Modificar();
                    MessageBox.Show("Los datos del usuario se modificaron", "Proceso Realizado", MessageBoxButtons.OK, MessageBoxIcon.Information);

                }
                this.Close();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }//cierre de btn

        private void Frm_UI_Users_Load(object sender, EventArgs e)
        {
            try
            {
                if (this.funcion == 1)
                {
                    this.btnRegistrar.Text = "Modificar";
                    this.txtLogin.ReadOnly = true;
                }
                else
                {
                    this.btnRegistrar.Text = "Registrar";
                    this.txtLogin.ReadOnly = false;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void Modificar()
        {
            try
            {
                this.varObjConexion.ModificarUsuario(this.varObjUsuario);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public void PasarDatos(Usuario temp)
        {
            try
            {
                this.txtLogin.Text = temp.Login;
                this.txtPassword.Text = temp.Password;
                this.txtConfirmar.Text = temp.Password;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

    }//CIERRRE DE CLASE
}//CIERRE DE NAMESPACE
