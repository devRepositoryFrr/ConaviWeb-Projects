using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConaviWeb.Model.Expedientes
{
    public class Caratula
    {
        public int IdUser { get; set; }
        public string UserName { get; set; }
        public int Consecutivo { get; set; }
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
        public string FechaRegistro { get; set; }
        public string FechaPrimeroAntiguo { get; set; }
        public string FechaUltimoReciente { get; set; }
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
        public string Puesto { get; set; }
        public string EsEditable { get; set; }
        public string Estatus { get; set; }
        //los campos de la caratula
        //public int IdUser { get; set; }
        //public string UserName { get; set; }
        //public int NoProg { get; set; }
        //public int Id { get; set; }
        //public int IdExpediente { get; set; }
        //public string Codigo { get; set; }
        //public string Nombre { get; set; }
        //public string Periodo { get; set; }
        //public int AniosResguardo { get; set; }
        //public int Legajos { get; set; }
        //public int Fojas { get; set; }
        public int DocOriginales { get; set; }
        public int DocCopias { get; set; }
        public int Cds { get; set; }
        public string TecnicasSeleccion { get; set; }
        public string Publica { get; set; }
        public string Confidencial { get; set; }
        public string Reservada { get; set; }
        public string DescripcionAsunto { get; set; }
        public string FechaClasificacion { get; set; }
        public string PeriodoReserva { get; set; }
        public string FundamentoLegal { get; set; }
        public string AmpliacionPeriodo { get; set; }
        public string FechaDesclasificacion { get; set; }
        public string NombreDesclasifica { get; set; }
        public string CargoDesclasifica { get; set; }
        public string PartesReservando { get; set; }
        public string DatosTopograficos { get; set; }
        public string TipoSoporteDocumental { get; set; }
    }
}