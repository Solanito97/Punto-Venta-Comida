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

namespace BIGFOODSERVICE
{
    public partial class FrmBitacora : Form
    {
        public FrmBitacora()
        {
            InitializeComponent();
        }

        private void MostrarRegistrarUsuarios(int funcion)
        {
            try
            {
                //se instancia la pantalla para registrar usuarios
                Frm_UI_Bitacora frm = new Frm_UI_Bitacora();

                //funciona realizar 0= Registrar y 1 = modificar
               // frm.funcion = funcion;

                //proceso de modificar
                if (funcion == 1)
                {
                    //se crea la instancia del objeto usuario
                    Bitacora temp = new Bitacora();
                    //se rellenan los datos del objeto con la fila seleccionada
                    temp.Tabla = this.DtgDatos.SelectedRows[0].Cells[0].Value.ToString();
                    temp.Usuario = this.DtgDatos.SelectedRows[0].Cells[1].Value.ToString();
                    //temp.Maquina = this.DtgDatos.SelectedRows[0].Cells[2].Value.ToString();
                    //temp.Fecha = this.DtgDatos.SelectedRows[0].Cells[3].Value.ToString();
                    temp.TipoMov = this.DtgDatos.SelectedRows[0].Cells[4].Value.ToString();
                    temp.Registro = this.DtgDatos.SelectedRows[0].Cells[5].Value.ToString();


                    //se pasa el objeto al formulario para mostrar los datos en pantalla
                  //  frm.PasarDatos(temp);

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

        private void FrmBitacora_Load(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
