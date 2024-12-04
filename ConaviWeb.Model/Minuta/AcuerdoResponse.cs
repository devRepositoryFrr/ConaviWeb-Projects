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
        public string Responsable { get; set; }
        public DateTime Fecha_termino { get; set; }
        public string Descripcion { get; set; }
        public string Gestion { get; set; }
        public string Estatus { get; set; }
    }
}
