using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class Facturas
    {
        public int Numero { get; set; }
        public DateTime Fecha { get; set; }
        [DisplayName("Código Cliente")]
        public string CodCliente { get; set; }
        public double SubTotal { get; set; }
        [DisplayName("Monto Descuento")]
        public double MontoDescuento{ get; set; }
        [DisplayName("Monto Impuesto")]
        public double MontoImpuesto { get; set; }
        public double Total {  get; set; }
        
            
        public char Estado { get; set; }
        public string Usuario { get; set; }
        [DisplayName("Tipo de Pago")]
        public string TipoPago { get; set; }
        public string Condicion { get; set; }  



    }
}
