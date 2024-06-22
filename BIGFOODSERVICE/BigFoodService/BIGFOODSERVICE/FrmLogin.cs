using BLL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
//USAMOS LA REFERENCIA DE BLL
using BLL;
using DAL;

namespace BIGFOODSERVICE
{
    public partial class FrmLogin : Form
    {
        //variable control 
        private bool autenticado = false;

        //variable objeto usuario
        private Usuario varObjUser = null;
        private Conexion Conexion = null;

        public FrmLogin()
        {
            InitializeComponent();
            this.Conexion = new Conexion();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            //cerrar el programa
            Application.Exit();
        }
        private void FrmLogin_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (autenticado == false)
            {
                Application.Exit(); //cerrar aplicación
            }

        }
        private void IntentoAutenticacion()
        {
            try
            {
                //se instancia el objeto
                this.varObjUser = new Usuario();

                //se rellena con los datos del front-end
                this.varObjUser.Login = this.txtUsuario.Text.Trim();
                this.varObjUser.Password = this.txtPassword.Text.Trim();

                //se valida el password
                if (!(this.varObjUser.Login.Equals("admin") & this.varObjUser.Password.Equals("admin")))
                {
                    throw new Exception("Usuario o contraseña incorrecto...");
                }
                else
                {
                    // esta correcto 
                    this.autenticado = true;
                    this.Close();
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        // ---------------------------------------------------------------------------------------------------------------------------
        private void IntentoAutenticacion(string pLogin, string pPassword)
        {
            try
            {
                this.varObjUser = this.Conexion.AuntenticarUsuario(pLogin, pPassword);

                if (this.varObjUser.Login == null)
                {
                    throw new Exception("Usuario o contraseña incorrecto");
                }
                else
                {
                    this.autenticado = true;
                    this.Close();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        // ---------------------------------------------------------------------------------------------------------------------------
        private void btnAceptar_Click(object sender, EventArgs e)
        {
            try
            {
                //ojo  estamos utilizando el método encargado de realizar la autenticación
                this.IntentoAutenticacion(this.txtUsuario.Text.Trim(),this.txtPassword.Text.Trim());

            }
            catch (Exception ex)
            {
                //se muestra el mensaje al usuario a nivel del front-end
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }//CIERE DE CLASE 
}//CIERRE DE NAMESPACE
