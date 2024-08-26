using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConaviWeb.Model.Expedientes
{
    public class Expediente
    {
        public int Consecutivo { get; set; }
        public int IdUser { get; set; }
        public string UserName { get; set; }
        public int NoProg { get; set; }
        public int Id { get; set; }
        public int IdExpediente { get; set; }
        public string Clave { get; set; }
        public string Codigo { get; set; }
        public int? IdTipoDocumental { get; set; }
        public string TipoDocumental { get; set; }
        public int? IdTipoSoporte { get; set; }
        public string Soporte { get; set; }
        public string Nombre { get; set; }
        public string Periodo { get; set; }
        public int? AniosResguardo { get; set; }
        public int Legajos { get; set; }
        public int? Fojas { get; set; }
        public int? NoPartes { get; set; }
        public string Observaciones { get; set; }
        public DateTime FechaRegistro { get; set; }
        //[DataType(DataType.Date)]
        public string FechaPrimeroAntiguo { get; set; }
        //[DataType(DataType.Date)]
        public string FechaUltimoReciente { get; set; }
        //[DataType(DataType.Date)]
        public string FechaElaboracion { get; set; }
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
        public int MigradoTP { get; set; }
        public int MigradoNE { get; set; }
        public string ObservacionesRevalidacion { get; set; }
    }
}