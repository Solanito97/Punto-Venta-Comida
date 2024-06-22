using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BLL;

namespace DAL
{
    public class Conexion
    {
        private SqlConnection _connection; //establece conexion con el servidor
        private SqlCommand _command; //ejecutar transac-sql
        private SqlDataReader _reader; //lee los datos del servidor de BD
        private string stringConexion; //almacena los parametros requeridos para la conexion
        private SqlDataAdapter _adapter; //permite tabular un conjunto de datos
        private DataSet _dataSet; //estructura para lamacenar informacion en formato tabular
        public Conexion()
        {
            this.stringConexion = "data source=BigFoodServiceSA.mssql.somee.com;initial catalog=BigFoodServiceSA;user id=BaseDatosProyec_SQLLogin_1;password=nowem5s976;";
            

        }//cierre contsrutor 

        public Usuario AuntenticarUsuario(string pLogin, string pPw)
        {
            try
            {
                //se instancia la conexion 
                this._connection = new SqlConnection(this.stringConexion);

                //se intenta abrir la conexion
                this._connection.Open();

                //se instancia el comando 
                this._command = new SqlCommand();

                //se asigna la conexion
                this._command.Connection = this._connection;

                //se indica el tipo de comando
                this._command.CommandType = CommandType.StoredProcedure;

                //indicar cual es nombre del procedimiento almacenado
                this._command.CommandText = "[SP_Cns_Usuario]";

                //se asignan los valores a cada parametro del procemiento almacenado
                this._command.Parameters.AddWithValue("@Login", pLogin);
                this._command.Parameters.AddWithValue("@Passw", pPw);

                //se ejecuta el comando 
                this._reader = this._command.ExecuteReader();

                //se pregunta si existen datos
                Usuario temp = new Usuario();

                if (this._reader.Read())
                {
                    //se instancia el objeto usuario
                    temp = new Usuario();

                    //se rellena el objeto con los datos
                    temp.Login = this._reader.GetValue(0).ToString(); //el login esta en posicion 1
                    temp.Password = this._reader.GetValue(1).ToString(); //pww es la 2
                }

                //siempre se debe cerrar la conexion
                this._connection.Close();
                //despues se debe liberar los recursos 
                this._connection.Dispose();
                this._command.Dispose();
                this._reader.Dispose();

                //se retornan los datos del usuario
                return temp;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }//cierre de metodo AutenticaUsuarios

        public DataSet BuscarUsuarios(string plogin)
        {
            try
            {
                this._connection = new SqlConnection(this.stringConexion);
                this._connection.Open();

                this._command = new SqlCommand();
                this._command.Connection = this._connection;

                this._command.CommandType = CommandType.StoredProcedure;
                this._command.CommandText = "[SP_Cns_Usuario_Catalogo]";

                this._command.Parameters.AddWithValue("@login", plogin);

                this._adapter = new SqlDataAdapter();
                this._adapter.SelectCommand = this._command;

                this._dataSet = new DataSet();
                this._adapter.Fill(this._dataSet);

                this._connection.Close();
                this._connection.Dispose();
                this._command.Dispose();
                this._adapter.Dispose();

                return this._dataSet;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }//cierrede metodo Busacr Usuario

        public void RegistrarUsuarios(Usuario pUser)
        {
            try
            {
                //se intancia la conexion
                this._connection = new SqlConnection(this.stringConexion);
                //se intento abrir la conexion
                this._connection.Open();
                //se instancia el comando 
                this._command = new SqlCommand();
                //se asigna la conexion al comando
                this._command.Connection = this._connection;
                //se indica el tipo de comando en este cado un procedimiento almacenado
                this._command.CommandType = CommandType.StoredProcedure;
                //se indica el tipo de comando
                this._command.CommandText = "[Sp_Ins_Usuario]";
                //muy importante darle los valores a cada parametro
                this._command.Parameters.AddWithValue("@login", pUser.Login);
                this._command.Parameters.AddWithValue("@Password", pUser.Password);
                //muy importante el comando NoWuery() ya que es una transaccion de insercion
                this._command.ExecuteNonQuery();
                //se cierra la conaxion
                this._connection.Close();
                //se liberan los recursos 
                this._connection.Dispose();
                this._command.Dispose();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }//cierre de metodo RegistrarUsuarios

        public void Eliminarusario(string plogin)
        {
            try
            {
                this._connection = new SqlConnection(stringConexion);
                this._connection.Open();
                this._command = new SqlCommand();
                this._command.Connection = this._connection;

                this._command.CommandType = CommandType.StoredProcedure;
                this._command.CommandText = "[Sp_Del_Usuario]";
                this._command.Parameters.AddWithValue("@login", plogin);

                this._command.ExecuteNonQuery();//ejecuta como no consulta, no espere nada.
                //se cierra la conexion
                this._connection.Close();

                //se liberan los recursos
                this._connection.Dispose();
                this._command.Dispose();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }//cierre de metodo eliminar
        
        public void ModificarUsuario(Usuario usuario)
        {
            try
            {
                this._connection = new SqlConnection(this.stringConexion);
                this._connection.Open();

                this._command = new SqlCommand();
                this._command.Connection = this._connection;
                this._command.CommandType = CommandType.StoredProcedure;
                this._command.CommandText = "[Sp_Udp_Usuario]";

                this._command.Parameters.AddWithValue("@Log", usuario.Login);
                this._command.Parameters.AddWithValue("@Pass", usuario.Password);

                this._command.ExecuteNonQuery();
                this._connection.Close();
                this._connection.Dispose();
                this._command.Dispose();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

    }//cierre de clase
}//cierre de namespace
