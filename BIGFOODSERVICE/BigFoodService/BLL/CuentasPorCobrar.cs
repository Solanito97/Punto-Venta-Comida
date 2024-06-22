using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class CuentasPorCobrar
    {
        [DisplayName("Número de Factura")]
        public int NumFactura { get; set; }
        [DisplayName("Codigo de Cliente")]
        public string CodCliente { get; set; }
        [DisplayName("Fecha de Factura")]
        public DateTime FechaFactura { get; set; }
        [DisplayName("Fecha de Registro")]
        public DateTime FechaRegistro { get; set; }
        [DisplayName("Monto de Factura")]
        public double MontoFactura { get; set; }
        public string Usuario { get; set; }
        public char Estado { get; set; }

    }
}
