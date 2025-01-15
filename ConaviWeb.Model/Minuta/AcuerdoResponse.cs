using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConaviWeb.Model.Minuta
{
    public class AcuerdoResponse
    {
        public int Id { get; set; }
        public int IdReunion { get; set; }
        public string Interno { get; set; }
        public string Externo { get; set; }
        public string FechaTermino { get; set; }
        public string Descripcion { get; set; }
        public string Gestion { get; set; }
        public string Area { get; set; }
        public string Estatus { get; set; }
    }
}
