using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConaviWeb.Model.Expedientes
{
    public class Expediente
    {
        public int IdUser { get; set; }
        public int NoProg { get; set; }
        public int Id { get; set; }
        public int IdExpediente { get; set; }
        public string Codigo { get; set; }
        public string Nombre { get; set; }
        public string Periodo { get; set; }
        public int AniosResguardo { get; set; }
        public int Legajos { get; set; }
        public int Fojas { get; set; }
        public string Observaciones { get; set; }
        public DateTime FechaRegistro { get; set; }
        public DateTime FechaPrimeroAntiguo { get; set; }
        public DateTime FechaUltimoReciente { get; set; }
        public int IdInventario { get; set; }
        public string VigDocValA { get; set; }
        public string VigDocValL { get; set; }
        public string VigDocValFC { get; set; }
        public string VigDocPlaConAT { get; set; }
        public string VigDocPlaConAC { get; set; }
        public string VigDocPlaConTot { get; set; }
        public string TecSelE { get; set; }
        public string TecSelC { get; set; }
        public string TecSelM { get; set; }
        public string Area { get; set; }
        public string EsEditable { get; set; }
        public string Estatus { get; set; }
    }
}