using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConaviWeb.Model.PrediosAdquisicion
{
    public class Predio
    {
        //Datos del Predio
        public int Id { get; set; }
        public int IdSedatu { get; set; }
        public string NombrePredio { get; set; }
        public string FechaRecorrido { get; set; }
        public string Domicilio { get; set; }
        public string CoordN { get; set; }
        public string CoordO { get; set; }
        public string Kmz { get; set; }
        public string CveEstado { get; set; }
        public string Estado { get; set; }
        public string CveMunicipio { get; set; }
        public string Municipio { get; set; }
        public string CveLocalidad { get; set; }
        public string Localidad { get; set; }
        //Características
        public string UsoSuelo { get; set; }
        public int CveTipoPredio { get; set; }
        public string TipoPredio { get; set; }
        public int CveAcceso { get; set; }
        public string Acceso { get; set; }
        public int CveLocalizacion { get; set; }
        public string Localizacion { get; set; }
        public string DistanciaLocalizacion { get; set; }
        public string CveOtrasCaracteristicas { get; set; }
        public string CaracPredioOtro { get; set; }
        public int CveElectricidad { get; set; }
        public string Electricidad { get; set; }
        public string DistElectricidad { get; set; }
        public int CveDrenaje { get; set; }
        public string Drenaje { get; set; }
        public string DistDrenaje { get; set; }
        public int CveAguaPotable { get; set; }
        public string AguaPotable { get; set; }
        public string DistAguaPotable { get; set; }
        //Elementos del Entorno(500m)
        public int? NivelesConstruidos { get; set; }
        public string MaterialesNaturales { get; set; }
        public string MaterialesIndustrializados { get; set; }
        //Vialidades y accesos
        public YesOrNo TieneConexionVialidad { get; set; }
        public int? CveTipoVialidadConexion { get; set; }
        public string TipoVialidadConexion { get; set; }
        public string DistanciaVialidadConexion { get; set; }
        public int? CveCondicionVialidadConexion { get; set; } 
        public string CondicionVialidadConexion { get; set; } 
        public string MaterialesVialidadConexion { get; set; }
        public int? CveTipoVialidadNivel { get; set; }
        public string TipoVialidadNivel { get; set; }
        public string DistanciaVialidadNivel { get; set; }
        public int? CveCondicionVialidadNivel { get; set; } 
        public string CondicionVialidadNivel { get; set; } 
        public string MaterialesVialidadNivel { get; set; }
        public YesOrNo TieneAccesoPredio { get; set; }
        public int? CveTipoAccesoPredio { get; set; }
        public string TipoAccesoPredio { get; set; }
        public string DistanciaAccesoPredio { get; set; }
        public int? CveCondicionAccesoPredio { get; set; }
        public string CondicionAccesoPredio { get; set; }
        public string MaterialesAccesoPredio { get; set; }
        public int? CveTipoAccesoPredioNivel { get; set; }
        public string TipoAccesoPredioNivel { get; set; }
        public string DistanciaAccesoPredioNivel { get; set; }
        public int? CveCondicionAccesoPredioNivel { get; set; }
        public string CondicionAccesoPredioNivel { get; set; }
        public string MaterialesAccesoPredioNivel { get; set; }
        //Transporte público
        public string DistanciaTranPubAccesoInmediato { get; set; }
        public int? CveCondicionTranPubAccesoInmediato { get; set; }
        public string CondicionTranPubAccesoInmediato { get; set; }
        public string DistanciaTranPubComoda { get; set; }
        public int? CveCondicionTranPubComoda { get; set; }
        public string CondicionTranPubComoda { get; set; }
        public string DistanciaTranPubLejano { get; set; }
        public int? CveCondicionTranPubLejano { get; set; }
        public string CondicionTranPubLejano { get; set; }
        public string DistanciaTranPubColectivo { get; set; }
        public int? CveCondicionTranPubColectivo { get; set; }
        public string CondicionTranPubColectivo { get; set; }
        public string DistanciaTranPubCombi { get; set; }
        public int? CveCondicionTranPubCombi { get; set; }
        public string CondicionTranPubCombi { get; set; }
        public string DistanciaTranPubMototaxi { get; set; }
        public int? CveCondicionTranPubMototaxi { get; set; }
        public string CondicionTranPubMototaxi { get; set; }
        public string DistanciaTranPubCentral { get; set; }
        public int? CveCondicionTranPubCentral { get; set; }
        public string CondicionTranPubCentral { get; set; }
        public string TranPubOtro { get; set; }
        public string DistanciaTranPubOtro { get; set; }
        public int? CveCondicionTranPubOtro { get; set; }
        public string CondicionTranPubOtro { get; set; }
        //Equipamiento Urbano y Servicios
        public string EqUrbEducacionNivel { get; set; }
        public string EqUrbEducacionDistancia { get; set; }
        public string EqUrbSaludNivel { get; set; }
        public string EqUrbSaludDistancia { get; set; }
        public string EqUrbServUrbanosNivel { get; set; }
        public string EqUrbServUrbanosDistancia { get; set; }
        public string EqUrbRecreacionNivel { get; set; }
        public string EqUrbRecreacionDistancia { get; set; }
        public string EqUrbCulturaNivel { get; set; }
        public string EqUrbCulturaDistancia { get; set; }
        public string EqUrbAsistenciaSocialNivel { get; set; }
        public string EqUrbAsistenciaSocialDistancia { get; set; }
        public string EqUrbOtro { get; set; }
        public string EqUrbOtroNivel { get; set; }
        public string EqUrbOtroDistancia { get; set; }
        public string ServUrbEducacionNivel { get; set; }
        public string ServUrbEducacionDistancia { get; set; }
        public string ServUrbSaludNivel { get; set; }
        public string ServUrbSaludDistancia { get; set; }
        public string ServUrbRecreacionNivel { get; set; }
        public string ServUrbRecreacionDistancia { get; set; }
        public string ServUrbCulturaNivel { get; set; }
        public string ServUrbCulturaDistancia { get; set; }
        public string ServUrbOtro { get; set; }
        public string ServUrbOtroNivel { get; set; }
        public string ServUrbOtroDistancia { get; set; }
        //Ubicación y Descripción del entorno físico del predio
        public string CveTopografiaTerreno { get; set; }
        public string TopografiaTerreno { get; set; }
        public string PendientesSuaves { get; set; }
        public string PendientesInclinadas { get; set; }
        public string PendientesAbruptas { get; set; }
        public string Planicies { get; set; }
        public string CortesdeSuelo { get; set; }
        public string VegetacionInterior { get; set; }
        public string EstratoArboreo { get; set; }
        public string EstratoArbustivo { get; set; }
        public string CveHidrografia { get; set; }
        public string Hidrografia { get; set; }
        public string HRios { get; set; }
        public string Escurrimientos { get; set; }
        public string Arroyos { get; set; }
        public string Lagos { get; set; }
        public string Lagunas { get; set; }
        public string Manantiales { get; set; }
        public string OjoAgua { get; set; }
        public string Humedales { get; set; }
        public string Acueducto { get; set; }
        public string Canal { get; set; }
        public string CveRestriccionesFederales { get; set; }
        public string RestriccionesFederales { get; set; }
        public string DistanciaRios { get; set; }
        public string Rios { get; set; }
        public string DistanciaCorrientes { get; set; }
        public string CorrientesIntermitentes { get; set; }
        public string DistanciaCuerposAgua { get; set; }
        public string CuerposdeAgua { get; set; }
        public string DistanciaLineasAltaTension { get; set; }
        public string LineasAltaTension { get; set; }
        public string DistanciaViasFerreas { get; set; }
        public string ViasFerreas { get; set; }
        public string DistanciaCarreteras { get; set; }
        public string Carreteras { get; set; }
        public string DistanciaDuctos { get; set; }
        public string Ductos { get; set; }
        public string DistanciaGaseras { get; set; }
        public string Gaseras { get; set; }
        public string DistanciaBarrancas { get; set; }
        public string Barrancas { get; set; }
        public string CveCaracteristicasSuelo { get; set; }
        public string CaracteristicasSuelo { get; set; }
        public string Rocas { get; set; }
        public string ArenasGravas { get; set; }
        public string LimosArcillas { get; set; }
        public string RellenoEscombro { get; set; }
        public string MateriaOrganica { get; set; }
        public string CveOtrasObservaciones { get; set; }
        public string OtrasObservaciones { get; set; }
        public string OtraObservacion { get; set; }
        public string FracturasTerreno { get; set; }
        public string Hundimientos { get; set; }
        public string InestabilidadLaderas { get; set; }
        public string ZonasInundables { get; set; }
        public YesOrNo PredioAptoVivienda { get;set; }
        public string ObservacionesPredioApto { get; set; }
        public string PorcentajeSuperficieUtillizable { get; set; }
        //Observaciones adicionales
        public string ObservacionesAdicionales { get; set; }
        public string NombreRealizoRecorrido { get; set; }
        public string CorreoElectronico { get; set; }
        public string ReporteFotografico { get; set; }
        public int IdUser { get; set; }
        public int Calificacion { get; set; }
        public string FechaRegistro { get; set; }
        public string FechaUltimaActualizacion { get; set; }
    }
    public enum YesOrNo { Si = 1, No = 2 }
}
