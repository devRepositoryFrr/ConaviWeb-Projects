using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConaviWeb.Model.Minuta
{
    public class ReunionResponse
    {
        public int Id { get; set; }
        public string Sector { get; set; }
        public string EntidadFed { get; set; }
        public string Municipio { get; set; }
        public string FechaSolicitud { get; set; }
        public string FechaAtencion { get; set; }
        public string Asunto { get; set; }
        public string NombreSol { get; set; }
        public string CargoSol { get; set; }
        public string DependenciaSol { get; set; }
        public string NombreCont { get; set; }
        public string TelefonoCont { get; set; }
        public string EmailCont { get; set; }
        public string Antecedentes { get; set; }
        public string Observaciones { get; set; }
        public string Responsable { get; set; }
        public string Gestion { get; set; }
        public string Estatus { get; set; }
        public List<Participante> Participantes { get; set; }
        public List<AcuerdoResponse> Acuerdos { get; set; }
    }
}
