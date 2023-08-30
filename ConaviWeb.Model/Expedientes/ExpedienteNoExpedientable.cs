using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConaviWeb.Model.Expedientes
{
    public class ExpedienteNoExpedientable
    {
        public int NoProg { get; set; }
        public int Id { get; set; }
        public int IdTipoSoporte { get; set; }
        public string Clave { get; set; }
        public string Soporte { get; set; }
        public string Titulo { get; set; }
        public string ClaveInterna { get; set; }
        public int Partes { get; set; }
        public string Observaciones { get; set; }
        public DateTime FechaElaboracion { get; set; }
        public DateTime FechaRegistro { get; set; }
        public int IdInventario { get; set; }
        public int IdUser { get; set; }
        public string EsEditable { get; set; }
    }
}