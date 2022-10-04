using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConaviWeb.Model.Reporteador
{
    public class PevCartaBA
    {
        public string Id_unico { get; set; }
        public string Curp { get; set; }
        public string Nombre_curp { get; set; }
        public string Nombre_ine { get; set; }
        public string Referencia { get; set; }
        public string Tipo_apoyo { get; set; }
        public string Monto { get; set; }
        public string Direccion { get; set; }
        public string Telefonos { get; set; }
        public string Telefono { get; set; }
        public string Sucursal { get; set; }
        public string Colonia_sucursal { get; set; }
        public string Fecha { get; set; }
        public string Estado { get; set; }
        public string Municipio { get; set; }
        public string Edompo { get; set; }
        public string Fecha_atencion { get; set; }
        public string Hora_atencion_inicio { get; set; }
        public string Hora_atencion_fin { get; set; }
        public string Path_carta { get; set; }
        public string Emisora { get; set; }
    }
}
