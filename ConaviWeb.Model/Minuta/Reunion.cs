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
        public int Modalidad { get; set; }
        public string EntidadFed { get; set; }
        public string Municipio { get; set; }
        public string Asunto { get; set; }
        public DateTime FechaSolicitud { get; set; }
        public string NombreSol { get; set; }
        public string CargoSol { get; set; }
        public string DependenciaSol { get; set; }
        public string NombreCont { get; set; }
        public string TelefonoCont { get; set; }
        public string EmailCont { get; set; }
        public string Antecedentes { get; set; }
        public int Responsable { get; set; }
        public DateTime FechaAtencion { get; set; }
        public string Observaciones { get; set; }
        public int Gestion { get; set; }
        public int IdEstatus { get; set; }
    }
}
