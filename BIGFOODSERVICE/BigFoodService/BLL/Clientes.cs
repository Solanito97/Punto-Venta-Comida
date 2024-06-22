using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class Clientes
    {
        [Key, DisplayName("Cedula")]
        public string  CedulaLegal { get; set; }
        [DisplayName("Tipo de Cedula")]
        public string TipoCedula { get; set; }
        [DisplayName("Nombre Completo")]
        public string NombreCompleto { get; set; }
        public string Email { get; set; }
        [DisplayName("Fecha de Registro")]
        public DateTime FechaRegistro { get; set; }
        public char Estado { get; set; }
        public string Usuario { get; set;}
    }
}
