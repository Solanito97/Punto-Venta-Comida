﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class Productos
    {
        [DisplayName("Codigo Interno")]
        public string CodigoInterno { get; set; }
        [DisplayName("Código de Barra")]
        public long CodBarra { get; set; } // Asegúrate de que el tipo es correcto
        public string Descripcion { get; set; }
        [DisplayName("Precio de Venta")]
        public int PrecioVenta { get; set; } // Cambiado a int
        public int Descuento { get; set; }
        public int Impuesto { get; set; }
        [DisplayName("Unidad de Medida")]
        public string UnidadMedida { get; set; }
        [DisplayName("Precio de Compra")]
        public int PrecioCompra { get; set; }
        public string Usuario { get; set; }
        public int Existencia { get; set; }
    }
}
