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
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace BIGFOODSERVICE
{
    public partial class FrmBuscarUsuarios : Form
    {
        public int funcion = 0;
        private Conexion varObjConexion = null;

        public FrmBuscarUsuarios()
        {
            InitializeComponent();
            this.varObjConexion = new Conexion();
            //this.ConsultaPorNombre(" ");
        }
        private void ConsultaPorNombre(string nombre)
        {
            try
            {
                this.dtgDatos.DataSource = this.varObjConexion.BuscarUsuarios(nombre).Tables[0];
                this.dtgDatos.AutoResizeColumns();
                this.dtgDatos.ReadOnly = true;
                if (this.dtgDatos.Columns.Count <= 0)
                {
                    //se ocualta la columna de password
                    this.dtgDatos.Columns[1].Visible = false;
                }
            }
            catch (Exception ex)
            {

                throw ex;
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
        private void MostrarRegistrarUsuarios(int funcion)
        {
            try
            {
                //se instancia la pantalla para registrar usuarios
                Frm_UI_Users frm = new Frm_UI_Users();

                //funciona realizar 0= Registrar y 1 = modificar
                frm.funcion = funcion;

                //proceso de modificar
                if (funcion == 1)
                {
                    //se crea la instancia del objeto usuario
                    Usuario temp = new Usuario();
                    //se rellenan los datos del objeto con la fila seleccionada
                    temp.Login = this.dtgDatos.SelectedRows[0].Cells[0].Value.ToString();
                    temp.Password = this.dtgDatos.SelectedRows[0].Cells[1].Value.ToString();


                    //se pasa el objeto al formulario para mostrar los datos en pantalla
                    frm.PasarDatos(temp);

                }

                //se muestra la pantalla de forma exclusiva 
                frm.ShowDialog();
                //se liberan recursos
                frm.Dispose();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        private void btnNuevo_Click(object sender, EventArgs e)
        {
            try
            {
                this.MostrarRegistrarUsuarios(0);
                //se actualiza la lista de forma automatica, el espacio en blanco lo actualiza automaticamente

                this.ConsultaPorNombre("");
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
                    throw new Exception("Seleccione la fila del usuario que desea Modificar");
                }
                //pantalla multifuncional
                this.MostrarRegistrarUsuarios(1);
                //se actualizan los usuarios 
                this.ConsultaPorNombre("");
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void eliminarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                //se valida que usuario tenga una fila seleccionada
                if (this.dtgDatos.SelectedRows.Count <= 0)
                {
                    throw new Exception("Seleccione la fila del usuario que desea eliminar");
                }
                //solicitamos confirmacion para eliminar 
                if (MessageBox.Show("Desea Eliminar el usuario seleccionado", "Confirmar", MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    //se toma la fila seleccionada y se extrae el login en a celda 0
                    this.EjecutarEliminar(this.dtgDatos.SelectedRows[0].Cells[0].Value.ToString());
                    //se actualia la lista
                    this.ConsultaPorNombre(" ");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void EjecutarEliminar(string pLogin)
        {
            try
            {
                this.varObjConexion.Eliminarusario(pLogin);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }//cierre de class
}//ciere de namespace





