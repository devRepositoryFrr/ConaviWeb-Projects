using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConaviWeb.Model.Expedientes
{
    public class Inventario
    {
        public int Id { get; set; }
        public int IdArea { get; set; }
        public int IdUser { get; set; }
        public string NombreUnidadAdministrativa { get; set; }
        public DateTime FechaElaboracion { get; set; }
        public DateTime FechaTransferencia { get; set; }
        public string NombreResponsableAT { get; set; }
        public DateTime FechaRegistro { get; set; }
        public DateTime FechaEntrega { get; set; }
    }
}
