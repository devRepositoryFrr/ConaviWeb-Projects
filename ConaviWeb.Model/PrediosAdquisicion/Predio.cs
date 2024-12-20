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
        public string FechaRecorrido { get; set; }
        public string Domicilio { get; set; }
        public string CoordN { get; set; }
        public string CoordO { get; set; }
        public string CveEstado { get; set; }
        public string Estado { get; set; }
        public string CveMunicipio { get; set; }
        public string Municipio { get; set; }
        public string CveLocalidad { get; set; }
        public string Localidad { get; set; }
        //Características
        public int CveTipoPredio { get; set; }
        public string TipoPredio { get; set; }
        public int CveAcceso { get; set; }
        public string Acceso { get; set; }
        public int CveLocalizacion { get; set; }
        public string Localizacion { get; set; }
        public string DistanciaLocalizacion { get; set; }
        public int CveElectricidad { get; set; }
        public string Electricidad { get; set; }
        public string DistElectricidad { get; set; }
        public int CveDrenaje { get; set; }
        public string Drenaje { get; set; }
        public string DistDrenaje { get; set; }
        public int CveAguaPotable { get; set; }
        public string AguaPotable { get; set; }
        public string DistAguaPotable { get; set; }
        public string CveOtrasCaracteristicas { get; set; }
        public string CaracPredioOtro { get; set; }
        //Elementos del Entorno(500m)
        public int? NivelesConstruidos { get; set; }
        public string MaterialesNaturales { get; set; }
        public string MaterialesIndustrializados { get; set; }
        //Vialidades y accesos
        public YesOrNo TieneConexionVialidad { get; set; }
        public int? TipoVialidadConexion { get; set; }
        public string DistanciaVialidadConexion { get; set; }
        public int? CondicionesVialidadConexion { get; set; } 
        public string MaterialesVialidadConexion { get; set; }
        public YesOrNo TieneAccesoPredio { get; set; }
        public int? TipoAccesoPredio { get; set; }
        public string DistanciaAccesoPredio { get; set; }
        public int? CondicionesAccesoPredio { get; set; }
        public string MaterialesAccesoPredio { get; set; }
        //Transporte público
        public string DistanciaTranPubAccesoInmediato { get; set; }
        public int? CondicionesTranPubAccesoInmediato { get; set; }
        public string DistanciaTranPubComoda { get; set; }
        public int? CondicionesTranPubComoda { get; set; }
        public string DistanciaTranPubLejano { get; set; }
        public int? CondicionesTranPubLejano { get; set; }
        public string DistanciaTranPubParada { get; set; }
        public int? CondicionesTranPubParada { get; set; }
        public string DistanciaTranPubEstacionCetram { get; set; }
        public int? CondicionesTranPubEstacionCetram { get; set; }
        public string DistanciaTranPubSitioTaxis { get; set; }
        public int? CondicionesTranPubSitioTaxis { get; set; }
        public string TranPubOtro { get; set; }
        public string DistanciaTranPubOtro { get; set; }
        public int? CondicionesTranPubOtro { get; set; }
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
        public string EstratoArboreo { get; set; }
        public string EstratoArbustivo { get; set; }
        public string CveHidrografia { get; set; }
        public string Hidrografia { get; set; }
        public string CveRestriccionesFederales { get; set; }
        public string RestriccionesFederales { get; set; }
        public string DistanciaRios { get; set; }
        public string DistanciaCorrientes { get; set; }
        public string DistanciaCuerposAgua { get; set; }
        public string DistanciaLineasAltaTension { get; set; }
        public string DistanciaViasFerreas { get; set; }
        public string DistanciaCarreteras { get; set; }
        public string DistanciaDuctos { get; set; }
        public string DistanciaBarrancas { get; set; }
        public string CveCaracteristicasSuelo { get; set; }
        public string CaracteristicasSuelo { get; set; }
        public string CveOtrasObservaciones { get; set; }
        public string OtrasObservaciones { get; set; }
        public YesOrNo PredioAptoVivienda { get;set; }
        public string CondicionesPredioApto { get; set; }
        public string PorcentajeSuperficieUtillizable { get; set; }
        //Observaciones adicionales
        public string ObservacionesAdicionales { get; set; }
        public string NombreRealizoRecorrido { get; set; }
        public int IdUser { get; set; }
    }
    public enum YesOrNo { Si = 1, No = 2 }
}
