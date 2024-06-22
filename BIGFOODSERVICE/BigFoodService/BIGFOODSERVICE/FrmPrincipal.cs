using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
//referenciamos a los demas proyectos
using BLL;
using DAL; 

namespace BIGFOODSERVICE
{
    public partial class FrmPrincipal : Form
    {
        public FrmPrincipal()
        {
            InitializeComponent();
        }


        /// <summary>
        /// Metodo encargado de ejecutar una aplicacion
        /// </summary>
        /// <param name="appName"></param>

        private void EjecutarAplicaciones(string appName)
        {
            System.Diagnostics.Process.Start(appName);
        }
        /// <summary>
        /// SE ENCARGA DE EJECUTAR EL CLIC
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void documentoDeTextoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                this.EjecutarAplicaciones("winword.exe");
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void hojaDeCálculoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                this.EjecutarAplicaciones("excel.exe");
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void presentadorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                this.EjecutarAplicaciones("simpress");
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void calculadoraToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                this.EjecutarAplicaciones("calc");
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            try
            {
                this.EjecutarAplicaciones("excel.exe");
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            try
            {
                this.EjecutarAplicaciones("winword.exe");
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            try
            {
                this.EjecutarAplicaciones("calc");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        //--------------------------------------------------------------------------------------------------
        /// <summary>
        /// Metodo para mostrar el login
        /// </summary>
        private void MostrarLogin()
        {
            try
            {//variable formulario 
                FrmLogin frm = new FrmLogin();

                //se muestra de forma exclusiva el formulario
                frm.ShowDialog();

                //Se libera la memoria utilizada
                frm.Dispose();

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }//Cierre método


        private void cerrarSesiónToolStripMenuItem1_Click_1(object sender, EventArgs e)
        {
            this.MostrarLogin();
        }

        private void cerrarSesiónToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.MostrarLogin();
        }
        //-------------------------------------------------------------------------------------------------
        private void FrmPrincipal_Load(object sender, EventArgs e)
        {
            try
            {
                //se muestra el mensaje en el area de las notificaciones por 25 seg
                this.notifyIcon1.ShowBalloonTip(20);

                this.MostrarLogin();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void usuariosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                this.MostrarBuscarUsuarios();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); ;
            }
        }

        private void MostrarBuscarUsuarios()
        {
            try
            {
                FrmBuscarUsuarios frm = new FrmBuscarUsuarios();
                frm.Show();
                //frm.Dispose();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void clientesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                this.MostrarClientes();
            }
            catch (Exception EX)
            {

                MessageBox.Show(EX.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        private void MostrarClientes()
        {
            try
            {
                FrmBuscarClientes frm = new FrmBuscarClientes();
                frm.ShowDialog();

                //Se libera la memoria utilizada
                frm.Dispose();

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void productosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                this.MostrarProductos();
            }
            catch (Exception EX)
            {

                MessageBox.Show(EX.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void MostrarProductos()
        {
            try
            {
                FrmBuscarProductos frm = new FrmBuscarProductos();
                frm.ShowDialog();

                //Se libera la memoria utilizada
                frm.Dispose();

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnCliente_Click(object sender, EventArgs e)
        {
            try
            {
                FrmBuscarClientes frm = new FrmBuscarClientes();
                frm.ShowDialog();

                //Se libera la memoria utilizada
                frm.Dispose();

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void btnProductos_Click(object sender, EventArgs e)
        {
            try
            {
                FrmBuscarProductos frm = new FrmBuscarProductos();
                frm.ShowDialog();

                //Se libera la memoria utilizada
                frm.Dispose();

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void clientesPorNombreToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                FrmBuscarClientes frm = new FrmBuscarClientes();
                frm.ShowDialog();

                //Se libera la memoria utilizada
                frm.Dispose();

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void clientesPorNombreToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            try
            {
                FrmBuscarProductos frm = new FrmBuscarProductos();
                frm.ShowDialog();

                //Se libera la memoria utilizada
                frm.Dispose();

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void facturaciónToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                this.mostraFacturacion();
            }
            catch (Exception EX)
            {

                MessageBox.Show(EX.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        public void mostraFacturacion()
        {
            try { 
               FrmFacturas facturas=new FrmFacturas();
                facturas.ShowDialog();
                facturas.Dispose();
             
            }catch(Exception ex) 
            { throw ex; }
        }
        public void mostraBitacora()
        {
            try
            {
                Frm_UI_Bitacora Bitacora = new Frm_UI_Bitacora();
                Bitacora.ShowDialog();
                Bitacora.Dispose();

            }
            catch (Exception ex)
            { throw ex; }
        }
        private void bitacoraToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                this.mostraBitacora();
            }
            catch (Exception EX)
            {

                MessageBox.Show(EX.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void procesosToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
        public void mostraCobrar()
        {
            try
            {
               Frm Bitacora = new Frm();
                Bitacora.ShowDialog();
                Bitacora.Dispose();

            }
            catch (Exception ex)
            { throw ex; }
        }
        private void cuentasPorCobrarToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            try
            {
                this.mostraCobrar();
            }
            catch (Exception EX)
            {

                MessageBox.Show(EX.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnFacturar_Click(object sender, EventArgs e)
        {
            try
            {
                this.mostraFacturacion();
            }
            catch (Exception EX)
            {

                MessageBox.Show(EX.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }//cierre formilario
}//cierre namespace
