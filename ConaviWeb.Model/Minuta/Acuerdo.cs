using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConaviWeb.Model.Minuta
{
    public class Acuerdo
    {
        public int IdReunion { get; set; }
        public string Responsable { get; set; }
        public DateTime FechaTermino { get; set; }
        public string Descripcion { get; set; }
        public string Interno { get; set; }
        public string Externo { get; set; }
        public int IdGestion { get; set; }
        public int IdEstatus { get; set; }
    }
}
