﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class Bitacora
    {
        public string Tabla { get;set; }

        public string Usuario { get;set; }
        public string  Maquina { get;set; }
        public DateTime Fecha { get;set; }

        [DisplayName("Tipo de Movimiento")]
        public string TipoMov { get;set; }
        public string Registro { get;set; }

    }
}
