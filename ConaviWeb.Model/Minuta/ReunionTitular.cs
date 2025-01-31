using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConaviWeb.Model.Minuta
{
    public class ReunionTitular
    {
        public int Id { get; set; }
        public string Tema { get; set; }
        public string Responsable { get; set; }
        public string Modalidad { get; set; }
        public string Liga { get; set; }
        public string Asunto { get; set; }
        public string Sector { get; set; }
        public string NombreSol { get; set; }
        public string CargoSol { get; set; }
        public string InstSol { get; set; }
        public string Objetivo { get; set; }
        public string Lugar { get; set; }
        public string Sala { get; set; }
        public string Tiempo { get; set; }
        public string RadioCel { get; set; }
        public string PSedatu { get; set; }
        public string PExterno { get; set; }
        public string RadioAD { get; set; }
        public string AccesoD { get; set; }
        public string Orden { get; set; }
        public string Insumos { get; set; }
        public DateTime FchCarga { get; set; }
    }
}
