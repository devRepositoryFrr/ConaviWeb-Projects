using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConaviWeb.Model.Expedientes
{
    public class SerieDocumental
    {
        public int Id { get; set; }
        public string Codigo { get; set; }
        public string Descripcion { get; set; }
        public string VigDocA { get; set; }
        public string VigDocL { get; set; }
        public string VigDocFC { get; set; }
        public string PlazoConAT { get; set; }
        public string PlazoConAC { get; set; }
        public string PlazoConTot { get; set; }
        public string TecSelE { get; set; }
        public string TecSelC { get; set; }
        public string TecSelM { get; set; }
        public string Observaciones { get; set; }
        public string estatus { get; set; }
    }
}
