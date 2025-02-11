using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConaviWeb.Model.PrediosAdquisicion
{
    public class PropuestaConceptual
    {
        public int Id { get; set; }
        public int IdPredio { get; set; }
        public string SuperficieTerreno { get; set; }
        public string SuperficieDesplante { get; set; }
        public string SuperficieDonacion { get; set; }
        public string SuperficieEquipamiento { get; set; }
        public string SuperficieConstruccion { get; set; }
        public string SuperficieRestriccion { get; set; }
        public string SuperficieRecreativa { get; set; }
        public string SuperficieAraesVerdes { get; set; }
        public string SuperficieCirculacionesVehiculares { get; set; }
        public string SuperficieCirculacionesPeatonales { get; set; }
        public int CajonesEstacionamiento { get; set; }
        public string SuperficieCajonesEstacionamiento { get; set; }
        public int TotalViviendas  { get; set; }
        public int ViviendasUnifamiliar { get; set; }
        public string SuperficieViviendasUnifamiliar { get; set; }
        public string NivelesViviendasUnifamiliar { get; set; }
        public int ViviendasMultifamiliar { get; set; }
        public string SuperficieViviendasMultifamiliar { get; set; }
        public string NivelesViviendasMultifamiliar { get; set; }
        public int ViviendasRenta { get; set; }
        public string SuperficieViviendasRenta { get; set; }
        public int NivelesViviendasRenta { get; set; }
        public string Estatus { get; set; }
        public int ClaveEstatus { get; set; }
        public string ObservacionesEstatus { get; set; }
        public string EstatusPropuesta { get; set; }
        public int ClaveEstatusPropuesta { get; set; }
        public string ArchivoPropuesta { get; set; }
        public string ObservacionesPropuesta { get; set; }
        public string ArchivoConciliacion { get; set; }
        public int TotalViviendasFinal { get; set; }
        public int TotalCajonesEstacionamiento { get; set; }
        public int NivelesVivienda { get; set; }
    }
}
