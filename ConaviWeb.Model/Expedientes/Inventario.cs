using System;
using System.ComponentModel.DataAnnotations;
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
        public int IdPuesto { get; set; }
        public int IdUser { get; set; }
        public string NombreUnidadAdministrativa { get; set; }
        //[DataType(DataType.Date)]
        public string FechaElaboracion { get; set; }
        //[DataType(DataType.Date)]
        public string FechaTransferencia { get; set; }
        public string NombreResponsableAT { get; set; }
        [DataType(DataType.Date)]
        public DateTime FechaRegistro { get; set; }
        //[DataType(DataType.Date)]
        public string FechaEntrega { get; set; }
    }
}
