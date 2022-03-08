using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConaviWeb.Model.RH
{
    public class Asistencia
    {
        public string Periodo { get; set; }
        public string NombreJefe { get; set; }
        public string PuestoJefe { get; set; }
        public List<string> Fechas { get; set; }
    }
}
