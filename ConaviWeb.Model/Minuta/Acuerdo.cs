using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConaviWeb.Model.Minuta
{
    public class Acuerdo
    {
        public int IdMinuta { get; set; }
        public string Responsable { get; set; }
        public string DAcuerdo { get; set; }
        public DateTime Fecha_termino { get; set; }
    }
}
