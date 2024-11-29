using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConaviWeb.Model.Minuta
{
    public class Reunion
    {
        public int Id { get; set; }
        public int Sector { get; set; }
        public int EntidadFed { get; set; }
        public int Municipio { get; set; }
        public string Asunto { get; set; }
        public DateTime Fecha_sesion { get; set; }
        public string Solicitante { get; set; }
        public string Contacto { get; set; }
        public string Antecedentes { get; set; }
        public int Responsable { get; set; }
        public DateTime Fecha_atencion { get; set; }
        public string Observaciones { get; set; }
        public int Gestion { get; set; }
    }
}
