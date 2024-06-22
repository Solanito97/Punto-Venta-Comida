using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class DetFactura
    {
        [DisplayName("Número de Factura")]
        public int NumFactura { get; set; }
        [DisplayName("Código Interno")]
        public string CodInterno { get; set; }
        public int Cantidad { get; set; }
        [DisplayName("Precio Unitario")]
        public double PrecioUnitario { get; set; }
        public double SubTotal
        {
            get
            {
                return PrecioUnitario * Cantidad;
            }
        
        }
        [DisplayName("Porcentaje Impuesto")]
        public double PorcImp { get;set; }
        [DisplayName("Porcentaje Descuento")]
        public double PorDescuento { get; set; }

    }
}
