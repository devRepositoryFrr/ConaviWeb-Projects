using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConaviWeb.Model.Expedientes
{
    public class Area
    {
        public int Id { get; set; }
        public string Descripcion { get; set; }
        public int IdResponsable { get; set; }
        public string Responsable { get; set; }
        public string estatus { get; set; }
    }
}