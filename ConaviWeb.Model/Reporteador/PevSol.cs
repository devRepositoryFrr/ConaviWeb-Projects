using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConaviWeb.Model.Reporteador
{
    public class PevSol
    {
        public string Id_unico { get; set; }
        public string Nombre { get; set; }
        public string Curp { get; set; }
        public string Estado { get; set; }
        public string Municipio { get; set; }
        public string Localidad { get; set; }
        public string P1 { get; set; }
        public string Linea_apoyo { get; set; }
        public string Cuenta_ayude_trabajos { get; set; }
        public string Mes { get; set; }
        public byte[] Img1 { get; set; }
        public byte[] Img2 { get; set; }
    }
}
