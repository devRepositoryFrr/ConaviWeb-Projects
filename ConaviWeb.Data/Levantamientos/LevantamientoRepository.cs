using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConaviWeb.Model;
using ConaviWeb.Model.PrediosAdquisicion;
using Dapper;
using MySql.Data.MySqlClient;

namespace ConaviWeb.Data.Levantamientos
{
    public class LevantamientoRepository : ILevantamientoRepository
    {
        private readonly MySQLConfiguration _connectionString;
        public LevantamientoRepository(MySQLConfiguration connectionString)
        {
            _connectionString = connectionString;
        }

        protected MySqlConnection DbConnection()
        {
            return new MySqlConnection(_connectionString.ExpConnectionString);
        }
        public async Task<IEnumerable<Catalogo>> GetEstados()
        {
            var db = DbConnection();
            var sql = @"
                        select clave Clave,descripcion Descripcion from prod_predios.c_entidad_federativa where clave != '00';
                       ";
            return await db.QueryAsync<Catalogo>(sql, new { });
        }
        public async Task<IEnumerable<Catalogo>> GetMunicipios(string cveedo)
        {
            var db = DbConnection();
            var sql = @"
                        select clave Clave,descripcion Descripcion from prod_predios.c_municipio where clave_entidad_federativa = @CveEdo and clave != '000';
                       ";
            return await db.QueryAsync<Catalogo>(sql, new { CveEdo = cveedo });
        }
        public async Task<IEnumerable<Catalogo>> GetLocalidades(string cveedo, string cvemun)
        {
            var db = DbConnection();
            var sql = @"
                        select clave Clave,descripcion Descripcion from prod_predios.c_localidad where clave_entidad_federativa = @CveEdo and clave_municipio = @CveMun and clave != '0000';
                       ";
            return await db.QueryAsync<Catalogo>(sql, new { CveEdo = cveedo, CveMun = cvemun });
        }
        public async Task<bool> InsertFormatoLevantamiento(Predio predio)
        {
            var db = DbConnection();
            var sql = @"
                        INSERT INTO prod_predios.formato_predio
                        (`id`, `id_sedatu`, `nombre_predio`,`fecha_recorrido`,`domicilio`,`cve_edo`,`cve_mun`,`cve_loc`,`coord_n`,`coord_o`,`tipo_predio`,`localizacion`,`distancia_localizacion`,`otra_caracteristica`,`otra_caracteristica_desc`,`electricidad`,`electricidad_distancia`,`drenaje`,`drenaje_distancia`,`agua_potable`,`agua_potable_distancia`,`niveles_construidos`,`materiales_naturales`,`materiales_industrializados`,`vialidad_conexion_predio`,`tipo_vialidad_conexion_predio`,`distancia_vialidad_conexion_predio`,`condicion_vialidad_conexion_predio`,`material_vialidad_conexion_predio`,`tipo_vialidad_conexion_predio_nivel`,`distancia_vialidad_conexion_predio_nivel`,`condicion_vialidad_conexion_predio_nivel`,`material_vialidad_conexion_predio_nivel`,`acceso_predio`,`tipo_acceso_predio`,`distancia_acceso_predio`,`condicion_acceso_predio`,`material_acceso_predio`,`tipo_acceso_predio_nivel`,`distancia_acceso_predio_nivel`,`condicion_acceso_predio_nivel`,`material_acceso_predio_nivel`,`distancia_transporte_publico_inmediato`,`condiciones_transporte_publico_inmediato`,`distancia_transporte_publico_comoda`,`condiciones_transporte_publico_comoda`,`distancia_transporte_publico_lejano`,`condiciones_transporte_publico_lejano`,`distancia_parada_transporte`,`condiciones_parada_transporte`,`otro_estructura_transporte`,`distancia_otro_estructura_transporte`,`condiciones_otro_estructura_transporte`,`nivel_equrban_educacion`,`distancia_equrban_educacion`,`nivel_equrban_salud`,`distancia_equrban_salud`,`nivel_equrban_recreacion`,`distancia_equrban_recreacion`,`nivel_equrban_cultura`,`distancia_equrban_cultura`,`nivel_equrban_servurbanos`,`distancia_equrban_servurbanos`,`nivel_equrban_asistencia_social`,`distancia_equrban_asistencia_social`,`otro_equrban`,`nivel_equran_otro`,`distancia_equrban_otro`,`nivel_servurbanos_educacion`,`distancia_servurbanos_educacion`,`nivel_servurbanos_salud`,`distancia_servurbanos_salud`,`nivel_servurbanos_recreacion`,`distancia_servurbanos_recreacion`,`nivel_servurbanos_cultura`,`distancia_servurbanos_cultura`,`otro_servurbanos`,`nivel_servurbanos_otro`,`distancia_servurbanos_otro`,`topografia_terreno`,`vegetacion_interior_est_arboreo`,`vegetacion_interior_est_arbustivo`,`hidrografia`,`restricciones_federales`,`distancia_rios`,`distancia_corrientes_intermitentes`,`distancia_cuerpos_agua`,`distancia_lineas_alta_tension`,`distancia_vias_ferreas`,`distancia_carreteras`,`distancia_ductos`,`distancia_barrancas`,`caracteristicas_suelo`,`otras_observaciones`,`predio_apto_desarrollo_vivienda`,`observaciones_predio_apto_desarrollo`,`porcentaje_superficie_utilizable`,`observaciones_adicionales`,`nombre_realizo_recorrido`,`correo_electronico`)
                        VALUES (@id,@id_sedatu,@nombre_predio,@fecha_recorrido,@domicilio,@cve_edo,@cve_mun,@cve_loc,@coord_n,@coord_o,@tipo_predio,@localizacion,@distancia_localizacion,@otra_caracteristica,@otra_caracteristica_desc,@electricidad,@electricidad_distancia,@drenaje,@drenaje_distancia,@agua_potable,@agua_potable_distancia,@niveles_construidos,@materiales_naturales,@materiales_industrializados,@vialidad_conexion_predio,@tipo_vialidad_conexion_predio,@distancia_vialidad_conexion_predio,@condicion_vialidad_conexion_predio,@material_vialidad_conexion_predio,@tipo_vialidad_conexion_predio_nivel,@distancia_vialidad_conexion_predio_nivel,@condicion_vialidad_conexion_predio_nivel,@material_vialidad_conexion_predio_nivel,@acceso_predio,@tipo_acceso_predio,@distancia_acceso_predio,@condicion_acceso_predio,@material_acceso_predio,@tipo_acceso_predio_nivel,@distancia_acceso_predio_nivel,@condicion_acceso_predio_nivel,@material_acceso_predio_nivel,@distancia_transporte_publico_inmediato,@condiciones_transporte_publico_inmediato,@distancia_transporte_publico_comoda,@condiciones_transporte_publico_comoda,@distancia_transporte_publico_lejano,@condiciones_transporte_publico_lejano,@distancia_parada_transporte,@condiciones_parada_transporte,@otro_estructura_transporte,@distancia_otro_estructura_transporte,@condiciones_otro_estructura_transporte,@nivel_equrban_educacion,@distancia_equrban_educacion,@nivel_equrban_salud,@distancia_equrban_salud,@nivel_equrban_recreacion,@distancia_equrban_recreacion,@nivel_equrban_cultura,@distancia_equrban_cultura,@nivel_equrban_servurbanos,@distancia_equrban_servurbanos,@nivel_equrban_asistencia_social,@distancia_equrban_asistencia_social,@otro_equrban,@nivel_equran_otro,@distancia_equrban_otro,@nivel_servurbanos_educacion,@distancia_servurbanos_educacion,@nivel_servurbanos_salud,@distancia_servurbanos_salud,@nivel_servurbanos_recreacion,@distancia_servurbanos_recreacion,@nivel_servurbanos_cultura,@distancia_servurbanos_cultura,@otro_servurbanos,@nivel_servurbanos_otro,@distancia_servurbanos_otro,@topografia_terreno,@vegetacion_interior_est_arboreo,@vegetacion_interior_est_arbustivo,@hidrografia,@restricciones_federales,@distancia_rios,@distancia_corrientes_intermitentes,@distancia_cuerpos_agua,@distancia_lineas_alta_tension,@distancia_vias_ferreas,@distancia_carreteras,@distancia_ductos,@distancia_barrancas,@caracteristicas_suelo,@otras_observaciones,@predio_apto_desarrollo_vivienda,@observaciones_predio_apto_desarrollo,@porcentaje_superficie_utilizable,@observaciones_adicionales,@nombre_realizo_recorrido,@correo)
                        ON DUPLICATE KEY UPDATE `fecha_recorrido` = @fecha_recorrido, `domicilio` = @domicilio, `cve_edo` = @cve_edo, `cve_mun` = @cve_mun, `cve_loc` = @cve_loc, `coord_n` = @coord_n, `coord_o` = @coord_o, `tipo_predio` = @tipo_predio, `localizacion` = @localizacion, `distancia_localizacion` = @distancia_localizacion, `otra_caracteristica` = @otra_caracteristica, `otra_caracteristica_desc` = @otra_caracteristica_desc, `electricidad` = @electricidad, `electricidad_distancia` = @electricidad_distancia, `drenaje` = @drenaje, `drenaje_distancia` = @drenaje_distancia, `agua_potable` = @agua_potable, `agua_potable_distancia` = @agua_potable_distancia, `niveles_construidos` = @niveles_construidos, `materiales_naturales` = @materiales_naturales, `materiales_industrializados` = @materiales_industrializados, `vialidad_conexion_predio` = @vialidad_conexion_predio, `tipo_vialidad_conexion_predio` = @tipo_vialidad_conexion_predio, `distancia_vialidad_conexion_predio` = @distancia_vialidad_conexion_predio, `condicion_vialidad_conexion_predio` = @condicion_vialidad_conexion_predio, `material_vialidad_conexion_predio` = @material_vialidad_conexion_predio, `tipo_vialidad_conexion_predio_nivel` = @tipo_vialidad_conexion_predio_nivel, `distancia_vialidad_conexion_predio_nivel` = @distancia_vialidad_conexion_predio_nivel, `condicion_vialidad_conexion_predio_nivel` = @condicion_vialidad_conexion_predio_nivel, `material_vialidad_conexion_predio_nivel` = @material_vialidad_conexion_predio_nivel, `acceso_predio` = @acceso_predio, `tipo_acceso_predio` = @tipo_acceso_predio, `distancia_acceso_predio` = @distancia_acceso_predio, `condicion_acceso_predio` = @condicion_acceso_predio, `material_acceso_predio` = @material_acceso_predio, `tipo_acceso_predio_nivel` = @tipo_acceso_predio_nivel, `distancia_acceso_predio_nivel` = @distancia_acceso_predio_nivel, `condicion_acceso_predio_nivel` = @condicion_acceso_predio_nivel, `material_acceso_predio_nivel` = @material_acceso_predio_nivel, `distancia_transporte_publico_inmediato` = @distancia_transporte_publico_inmediato, `condiciones_transporte_publico_inmediato` = @condiciones_transporte_publico_inmediato, `distancia_transporte_publico_comoda` = @distancia_transporte_publico_comoda, `condiciones_transporte_publico_comoda` = @condiciones_transporte_publico_comoda, `distancia_transporte_publico_lejano` = @distancia_transporte_publico_lejano, `condiciones_transporte_publico_lejano` = @condiciones_transporte_publico_lejano, `distancia_parada_transporte` = @distancia_parada_transporte, `condiciones_parada_transporte` = @condiciones_parada_transporte, `otro_estructura_transporte` = @otro_estructura_transporte, `distancia_otro_estructura_transporte` = @distancia_otro_estructura_transporte, `condiciones_otro_estructura_transporte` = @condiciones_otro_estructura_transporte, `nivel_equrban_educacion` = @nivel_equrban_educacion, `distancia_equrban_educacion` = @distancia_equrban_educacion, `nivel_equrban_salud` = @nivel_equrban_salud, `distancia_equrban_salud` = @distancia_equrban_salud, `nivel_equrban_recreacion` = @nivel_equrban_recreacion, `distancia_equrban_recreacion` = @distancia_equrban_recreacion, `nivel_equrban_cultura` = @nivel_equrban_cultura, `distancia_equrban_cultura` = @distancia_equrban_cultura, `nivel_equrban_servurbanos` = @nivel_equrban_servurbanos, `distancia_equrban_servurbanos` = @distancia_equrban_servurbanos, `nivel_equrban_asistencia_social` = @nivel_equrban_asistencia_social, `distancia_equrban_asistencia_social` = @distancia_equrban_asistencia_social, `otro_equrban` = @otro_equrban, `nivel_equran_otro` = @nivel_equran_otro, `distancia_equrban_otro` = @distancia_equrban_otro, `nivel_servurbanos_educacion` = @nivel_servurbanos_educacion, `distancia_servurbanos_educacion` = @distancia_servurbanos_educacion, `nivel_servurbanos_salud` = @nivel_servurbanos_salud, `distancia_servurbanos_salud` = @distancia_servurbanos_salud, `nivel_servurbanos_recreacion` = @nivel_servurbanos_recreacion, `distancia_servurbanos_recreacion` = @distancia_servurbanos_recreacion, `nivel_servurbanos_cultura` = @nivel_servurbanos_cultura, `distancia_servurbanos_cultura` = @distancia_servurbanos_cultura, `otro_servurbanos` = @otro_servurbanos, `nivel_servurbanos_otro` = @nivel_servurbanos_otro, `distancia_servurbanos_otro` = @distancia_servurbanos_otro, `topografia_terreno` = @topografia_terreno, `vegetacion_interior_est_arboreo` = @vegetacion_interior_est_arboreo, `vegetacion_interior_est_arbustivo` = @vegetacion_interior_est_arbustivo, `hidrografia` = @hidrografia, `restricciones_federales` = @restricciones_federales, `distancia_rios` = @distancia_rios, `distancia_corrientes_intermitentes` = @distancia_corrientes_intermitentes, `distancia_cuerpos_agua` = @distancia_cuerpos_agua, `distancia_lineas_alta_tension` = @distancia_lineas_alta_tension, `distancia_vias_ferreas` = @distancia_vias_ferreas, `distancia_carreteras` = @distancia_carreteras, `distancia_ductos` = @distancia_ductos, `distancia_barrancas` = @distancia_barrancas, `caracteristicas_suelo` = @caracteristicas_suelo, `otras_observaciones` = @otras_observaciones, `predio_apto_desarrollo_vivienda` = @predio_apto_desarrollo_vivienda, `observaciones_predio_apto_desarrollo` = @observaciones_predio_apto_desarrollo, `porcentaje_superficie_utilizable` = @porcentaje_superficie_utilizable, `observaciones_adicionales` = @observaciones_adicionales, `nombre_realizo_recorrido` = @nombre_realizo_recorrido, `correo_electronico` = @correo, `fecha_update` = NOW();";

            var result = await db.ExecuteAsync(sql, new
            {
                id = predio.Id,
                id_sedatu = predio.IdSedatu,
                nombre_predio = predio.NombrePredio,
                fecha_recorrido = predio.FechaRecorrido,
                domicilio = predio.Domicilio,
                cve_edo = predio.CveEstado,
                cve_mun = predio.CveMunicipio,
                cve_loc = predio.CveLocalidad,
                coord_n = predio.CoordN,
                coord_o = predio.CoordO,
                tipo_predio = predio.CveTipoPredio,
                localizacion = predio.CveLocalizacion,
                distancia_localizacion = predio.DistanciaLocalizacion,
                //acceso = predio.CveAcceso,
                otra_caracteristica = predio.CveOtrasCaracteristicas,
                otra_caracteristica_desc = predio.CaracPredioOtro,
                electricidad = predio.CveElectricidad,
                electricidad_distancia = predio.DistElectricidad,
                drenaje = predio.CveDrenaje,
                drenaje_distancia = predio.DistDrenaje,
                agua_potable = predio.CveAguaPotable,
                agua_potable_distancia = predio.DistAguaPotable,
                niveles_construidos = predio.NivelesConstruidos,
                materiales_naturales = predio.MaterialesNaturales,
                materiales_industrializados = predio.MaterialesIndustrializados,
                vialidad_conexion_predio = predio.TieneConexionVialidad,
                tipo_vialidad_conexion_predio = predio.TipoVialidadConexion,
                distancia_vialidad_conexion_predio = predio.DistanciaVialidadConexion,
                condicion_vialidad_conexion_predio = predio.CondicionVialidadConexion,
                material_vialidad_conexion_predio = predio.MaterialesVialidadConexion,
                tipo_vialidad_conexion_predio_nivel = predio.TipoVialidadNivel,
                distancia_vialidad_conexion_predio_nivel = predio.DistanciaVialidadNivel,
                condicion_vialidad_conexion_predio_nivel = predio.CondicionVialidadNivel,
                material_vialidad_conexion_predio_nivel = predio.MaterialesVialidadNivel,
                acceso_predio = predio.TieneAccesoPredio,
                tipo_acceso_predio = predio.TipoAccesoPredio,
                distancia_acceso_predio = predio.DistanciaAccesoPredio,
                condicion_acceso_predio = predio.CondicionAccesoPredio,
                material_acceso_predio = predio.MaterialesAccesoPredio,
                tipo_acceso_predio_nivel = predio.TipoAccesoPredioNivel,
                distancia_acceso_predio_nivel = predio.DistanciaAccesoPredioNivel,
                condicion_acceso_predio_nivel = predio.CondicionAccesoPredioNivel,
                material_acceso_predio_nivel = predio.MaterialesAccesoPredioNivel,
                distancia_transporte_publico_inmediato = predio.DistanciaTranPubAccesoInmediato,
                condiciones_transporte_publico_inmediato = predio.CondicionTranPubAccesoInmediato,
                distancia_transporte_publico_comoda = predio.DistanciaTranPubComoda,
                condiciones_transporte_publico_comoda = predio.CondicionTranPubComoda,
                distancia_transporte_publico_lejano = predio.DistanciaTranPubLejano,
                condiciones_transporte_publico_lejano = predio.CondicionTranPubLejano,
                distancia_parada_transporte = predio.DistanciaTranPubParada,
                condiciones_parada_transporte = predio.CondicionTranPubParada,
                //distancia_estacion_cetram = predio.DistanciaTranPubEstacionCetram,
                //condiciones_estacion_cetram = predio.CondicionTranPubEstacionCetram,
                //distancia_sitio_taxis = predio.DistanciaTranPubSitioTaxis,
                //condiciones_sitio_taxis = predio.CondicionTranPubSitioTaxis,
                otro_estructura_transporte = predio.TranPubOtro,
                distancia_otro_estructura_transporte = predio.DistanciaTranPubOtro,
                condiciones_otro_estructura_transporte = predio.CondicionTranPubOtro,
                nivel_equrban_educacion = predio.EqUrbEducacionNivel,
                distancia_equrban_educacion = predio.EqUrbEducacionDistancia,
                nivel_equrban_salud = predio.EqUrbSaludNivel,
                distancia_equrban_salud = predio.EqUrbSaludDistancia,
                nivel_equrban_recreacion = predio.EqUrbRecreacionNivel,
                distancia_equrban_recreacion = predio.EqUrbRecreacionDistancia,
                nivel_equrban_cultura = predio.EqUrbCulturaNivel,
                distancia_equrban_cultura = predio.EqUrbCulturaDistancia,
                nivel_equrban_servurbanos = predio.EqUrbServUrbanosNivel,
                distancia_equrban_servurbanos = predio.EqUrbServUrbanosDistancia,
                nivel_equrban_asistencia_social = predio.EqUrbAsistenciaSocialNivel,
                distancia_equrban_asistencia_social = predio.EqUrbAsistenciaSocialDistancia,
                otro_equrban = predio.EqUrbOtro,
                nivel_equran_otro = predio.EqUrbOtroNivel,
                distancia_equrban_otro = predio.EqUrbOtroDistancia,
                nivel_servurbanos_educacion = predio.ServUrbEducacionNivel,
                distancia_servurbanos_educacion = predio.ServUrbEducacionDistancia,
                nivel_servurbanos_salud = predio.ServUrbSaludNivel,
                distancia_servurbanos_salud = predio.ServUrbSaludDistancia,
                nivel_servurbanos_recreacion = predio.ServUrbRecreacionNivel,
                distancia_servurbanos_recreacion = predio.ServUrbRecreacionDistancia,
                nivel_servurbanos_cultura = predio.ServUrbCulturaNivel,
                distancia_servurbanos_cultura = predio.ServUrbCulturaDistancia,
                otro_servurbanos = predio.ServUrbOtro,
                nivel_servurbanos_otro = predio.ServUrbOtroNivel,
                distancia_servurbanos_otro = predio.ServUrbOtroDistancia,
                topografia_terreno = predio.TopografiaTerreno,
                vegetacion_interior_est_arboreo = predio.EstratoArboreo,
                vegetacion_interior_est_arbustivo = predio.EstratoArbustivo,
                hidrografia = predio.Hidrografia,
                restricciones_federales = predio.RestriccionesFederales,
                distancia_rios = predio.DistanciaRios,
                distancia_corrientes_intermitentes = predio.DistanciaCorrientes,
                distancia_cuerpos_agua = predio.DistanciaCuerposAgua,
                distancia_lineas_alta_tension = predio.DistanciaLineasAltaTension,
                distancia_vias_ferreas = predio.DistanciaViasFerreas,
                distancia_carreteras = predio.DistanciaCarreteras,
                distancia_ductos = predio.DistanciaDuctos,
                distancia_barrancas = predio.DistanciaBarrancas,
                caracteristicas_suelo = predio.CaracteristicasSuelo,
                otras_observaciones = predio.OtrasObservaciones,
                predio_apto_desarrollo_vivienda = predio.PredioAptoVivienda,
                observaciones_predio_apto_desarrollo = predio.ObservacionesPredioApto,
                porcentaje_superficie_utilizable = predio.PorcentajeSuperficieUtillizable,
                observaciones_adicionales = predio.ObservacionesAdicionales,
                nombre_realizo_recorrido = predio.NombreRealizoRecorrido,
                correo = predio.CorreoElectronico
            });
            return result > 0;
        }
        public async Task<Predio> GetFormatoLevantamiento(int id)
        {
            var db = DbConnection();
            var sql = @"
                        SELECT fp.id Id, fp.id_sedatu IdSedatu, fp.nombre_predio NombrePredio, fp.fecha_recorrido FechaRecorrido, fp.domicilio Domicilio, fp.cve_edo CveEstado, fp.cve_mun CveMunicipio, fp.cve_loc CveLocalidad, fp.kmz Kmz, fp.coord_n CoordN, fp.coord_o CoordO, fp.uso_suelo UsoSuelo, fp.tipo_predio CveTipoPredio, fp.localizacion CveLocalizacion, fp.distancia_localizacion DistanciaLocalizacion, fp.otra_caracteristica CveOtrasCaracteristicas, fp.otra_caracteristica_desc CaracPredioOtro, fp.electricidad CveElectricidad, fp.electricidad_distancia DistElectricidad, fp.drenaje CveDrenaje, fp.drenaje_distancia DistDrenaje, fp.agua_potable CveAguaPotable, fp.agua_potable_distancia DistAguaPotable, fp.niveles_construidos NivelesConstruidos, fp.materiales_naturales MaterialesNaturales, fp.materiales_industrializados MaterialesIndustrializados, fp.vialidad_conexion_predio TieneConexionVialidad, fp.tipo_vialidad_conexion_predio TipoVialidadConexion, fp.distancia_vialidad_conexion_predio DistanciaVialidadConexion, fp.condicion_vialidad_conexion_predio CondicionVialidadConexion, fp.material_vialidad_conexion_predio MaterialesVialidadConexion, fp.tipo_vialidad_conexion_predio_nivel TipoVialidadNivel, fp.distancia_vialidad_conexion_predio_nivel DistanciaVialidadNivel, fp.condicion_vialidad_conexion_predio_nivel CondicionVialidadNivel, fp.material_vialidad_conexion_predio_nivel MaterialesVialidadNivel, fp.acceso_predio TieneAccesoPredio, fp.tipo_acceso_predio TipoAccesoPredio, fp.distancia_acceso_predio DistanciaAccesoPredio, fp.condicion_acceso_predio CondicionAccesoPredio, fp.material_acceso_predio MaterialesAccesoPredio, fp.tipo_acceso_predio_nivel TipoAccesoPredioNivel, fp.distancia_acceso_predio_nivel DistanciaAccesoPredioNivel, fp.condicion_acceso_predio_nivel CondicionAccesoPredioNivel, fp.material_acceso_predio_nivel MaterialesAccesoPredioNivel, fp.distancia_transporte_publico_inmediato DistanciaTranPubAccesoInmediato, fp.condiciones_transporte_publico_inmediato CondicionTranPubAccesoInmediato, fp.distancia_transporte_publico_comoda DistanciaTranPubComoda, fp.condiciones_transporte_publico_comoda CondicionTranPubComoda, fp.distancia_transporte_publico_lejano DistanciaTranPubLejano, fp.condiciones_transporte_publico_lejano CondicionTranPubLejano, fp.distancia_parada_transporte DistanciaTranPubParada, fp.condiciones_parada_transporte CondicionTranPubParada, fp.otro_estructura_transporte TranPubOtro, fp.distancia_otro_estructura_transporte DistanciaTranPubOtro, fp.condiciones_otro_estructura_transporte CondicionTranPubOtro, fp.nivel_equrban_educacion EqUrbEducacionNivel, fp.distancia_equrban_educacion EqUrbEducacionDistancia, fp.nivel_equrban_salud EqUrbSaludNivel, fp.distancia_equrban_salud EqUrbSaludDistancia, fp.nivel_equrban_recreacion EqUrbRecreacionNivel, fp.distancia_equrban_recreacion EqUrbRecreacionDistancia, fp.nivel_equrban_cultura EqUrbCulturaNivel, fp.distancia_equrban_cultura EqUrbCulturaDistancia, fp.nivel_equrban_servurbanos EqUrbServUrbanosNivel, fp.distancia_equrban_servurbanos EqUrbServUrbanosDistancia, fp.nivel_equrban_asistencia_social EqUrbAsistenciaSocialNivel, fp.distancia_equrban_asistencia_social EqUrbAsistenciaSocialDistancia, fp.otro_equrban EqUrbOtro, fp.nivel_equran_otro EqUrbOtroNivel, fp.distancia_equrban_otro EqUrbOtroDistancia, fp.nivel_servurbanos_educacion ServUrbEducacionNivel, fp.distancia_servurbanos_educacion ServUrbEducacionDistancia, fp.nivel_servurbanos_salud ServUrbSaludNivel, fp.distancia_servurbanos_salud ServUrbSaludDistancia, fp.nivel_servurbanos_recreacion ServUrbRecreacionNivel, fp.distancia_servurbanos_recreacion ServUrbRecreacionDistancia, fp.nivel_servurbanos_cultura ServUrbCulturaNivel, fp.distancia_servurbanos_cultura ServUrbCulturaDistancia, fp.otro_servurbanos ServUrbOtro, fp.nivel_servurbanos_otro ServUrbOtroNivel, fp.distancia_servurbanos_otro ServUrbOtroDistancia, fp.topografia_terreno CveTopografiaTerreno, fp.vegetacion_interior_est_arboreo EstratoArboreo, fp.vegetacion_interior_est_arbustivo EstratoArbustivo, fp.hidrografia CveHidrografia, fp.restricciones_federales CveRestriccionesFederales, fp.distancia_rios DistanciaRios, fp.distancia_corrientes_intermitentes DistanciaCorrientes, fp.distancia_cuerpos_agua DistanciaCuerposAgua, fp.distancia_lineas_alta_tension DistanciaLineasAltaTension, fp.distancia_vias_ferreas DistanciaViasFerreas, fp.distancia_carreteras DistanciaCarreteras, fp.distancia_ductos DistanciaDuctos, fp.distancia_barrancas DistanciaBarrancas, fp.caracteristicas_suelo CveCaracteristicasSuelo, fp.otras_observaciones CveOtrasObservaciones, fp.predio_apto_desarrollo_vivienda PredioAptoVivienda, fp.observaciones_predio_apto_desarrollo ObservacionesPredioApto, fp.porcentaje_superficie_utilizable PorcentajeSuperficieUtillizable, fp.observaciones_adicionales ObservacionesAdicionales, fp.nombre_realizo_recorrido NombreRealizoRecorrido, fp.correo_electronico CorreoElectronico, fp.reporte_fotografico ReporteFotografico
                        FROM prod_predios.formato_predio fp
                        WHERE fp.id = @Id;";

            return await db.QueryFirstOrDefaultAsync<Predio>(sql, new { Id = id });
        }
        public async Task<IEnumerable<Predio>> GetPrediosAdquisicion()
        {
            var db = DbConnection();
            var sql = @"
                        SELECT fp.id Id, fp.fecha_recorrido FechaRecorrido, fp.domicilio Domicilio, ce.descripcion Estado, cm.descripcion Municipio, cl.descripcion Localidad, fp.nombre_realizo_recorrido NombreRealizoRecorrido, fp.predio_apto_desarrollo_vivienda PredioAptoVivienda, fp.observaciones_predio_apto_desarrollo CondicionesPredioApto, 0.0 calificacion
                        FROM prod_predios.formato_predio fp
                        JOIN prod_predios.c_entidad_federativa ce ON fp.cve_edo = ce.clave
                        JOIN prod_predios.c_municipio cm ON fp.cve_mun = cm.clave and cm.clave_entidad_federativa = fp.cve_edo
                        JOIN prod_predios.c_localidad cl ON fp.cve_loc = cl.clave and cl.clave_entidad_federativa = fp.cve_edo and cl.clave_municipio = fp.cve_mun;
                       ";
            return await db.QueryAsync<Predio>(sql, new {  });
        }
        public async Task<IEnumerable<Predio>> GetFullPrediosAdquisicion()
        {
            var db = DbConnection();
            var sql = @"
                        SELECT `formato_predio`.`id` Id,
                            `formato_predio`.`id_sedatu` IdSedatu,
                            `formato_predio`.`nombre_predio` NombrePredio,
                            `formato_predio`.`fecha_recorrido` FechaRecorrido,
                            `formato_predio`.`domicilio` Domicilio,
                            `formato_predio`.`cve_edo` CveEstado,
                            ce.`descripcion` estado,
                            `formato_predio`.`cve_mun` CveMunicipio,
                            cm.`descripcion` municipio,
                            `formato_predio`.`cve_loc` CveLocalidad,
                            cl.descripcion localidad,
                            `formato_predio`.`kmz` Kmz,
                            `formato_predio`.`uso_suelo` UsoSuelo,
                            `formato_predio`.`tipo_predio` CveTipoPredio,
                            ctp.descripcion TipoPredio,
                            `formato_predio`.`localizacion` CveLocalizacion,
                            clz.descripcion Localizacion,
                            `formato_predio`.`distancia_localizacion` DistanciaLocalizacion,
                            `formato_predio`.`otra_caracteristica` CveOtrasCaracteristicas,
                            `formato_predio`.`otra_caracteristica_desc` CaracPredioOtro,
                            `formato_predio`.`electricidad` CveElectricidad,
                            cet.`descripcion` Electricidad,
                            `formato_predio`.`electricidad_distancia` DistElectricidad,
                            `formato_predio`.`drenaje` CveDrenaje,
                            cd.`descripcion` Drenaje,
                            `formato_predio`.`drenaje_distancia` DistDrenaje,
                            `formato_predio`.`agua_potable` CveAguaPotable,
                            ca.`descripcion` AguaPotable,
                            `formato_predio`.`agua_potable_distancia` DistAguaPotable,
                            `formato_predio`.`niveles_construidos` NivelesConstruidos,
                            `formato_predio`.`materiales_naturales` MaterialesNaturales,
                            `formato_predio`.`materiales_industrializados` MaterialesIndustrializados,
                            `formato_predio`.`vialidad_conexion_predio` TieneConexionVialidad,
                            `formato_predio`.`tipo_vialidad_conexion_predio` CveTipoVialidadConexion,
                            cv.descripcion TipoVialidadConexion,
                            `formato_predio`.`distancia_vialidad_conexion_predio` DistanciaVialidadConexion,
                            `formato_predio`.`condicion_vialidad_conexion_predio` CveCondicionVialidadConexion,
                            ccc.`descripcion` CondicionVialidadConexion,
                            `formato_predio`.`material_vialidad_conexion_predio` MaterialesVialidadConexion,
                            `formato_predio`.`tipo_vialidad_conexion_predio_nivel` CveTipoVialidadNivel,
                            ctvn.descripcion TipoVialidadNivel,
                            `formato_predio`.`distancia_vialidad_conexion_predio_nivel` DistanciaVialidadNivel,
                            `formato_predio`.`condicion_vialidad_conexion_predio_nivel` CveCondicionVialidadNivel,
                            cccn.descripcion CondicionVialidadNivel,
                            `formato_predio`.`material_vialidad_conexion_predio_nivel` MaterialesVialidadNivel,
                            `formato_predio`.`acceso_predio` TieneAccesoPredio,
                            `formato_predio`.`tipo_acceso_predio` CveTipoAccesoPredio,
                            ctap.descripcion TipoAccesoPredio,
                            `formato_predio`.`distancia_acceso_predio` DistanciaAccesoPredio,
                            `formato_predio`.`condicion_acceso_predio` CveCondicionAccesoPredio,
                            ccap.descripcion CondicionAccesoPredio,
                            `formato_predio`.`material_acceso_predio` MaterialesAccesoPredio,
                            `formato_predio`.`tipo_acceso_predio_nivel` CveTipoAccesoPredioNivel,
                            ctapn.descripcion TipoAccesoPredioNivel,
                            `formato_predio`.`distancia_acceso_predio_nivel` DistanciaAccesoPredioNivel,
                            `formato_predio`.`condicion_acceso_predio_nivel` CveCondicionAccesoPredioNivel,
                            ccapn.descripcion CondicionAccesoPredioNivel,
                            `formato_predio`.`material_acceso_predio_nivel` MaterialesAccesoPredioNivel,
                            `formato_predio`.`distancia_transporte_publico_inmediato` DistanciaTranPubAccesoInmediato,
                            `formato_predio`.`condiciones_transporte_publico_inmediato` CveCondicionesTranPubAccesoInmediato,
                            cctpai.descripcion CondicionTranPubAccesoInmediato,
                            `formato_predio`.`distancia_transporte_publico_comoda` DistanciaTranPubComoda,
                            `formato_predio`.`condiciones_transporte_publico_comoda` CveCondicionesTranPubComoda,
                            cctbdc.descripcion CondicionTranPubComoda,
                            `formato_predio`.`distancia_transporte_publico_lejano` DistanciaTranPubLejano,
                            `formato_predio`.`condiciones_transporte_publico_lejano` CveCondicionesTranPubLejano,
                            cctpl.descripcion CondicionTranPubLejano,
                            `formato_predio`.`distancia_parada_transporte` DistanciaTranPubParada,
                            `formato_predio`.`condiciones_parada_transporte` CveCondicionesTranPubParada,
                            cctpp.descripcion CondicionTranPubParada,
                            `formato_predio`.`otro_estructura_transporte` TranPubOtro,
                            `formato_predio`.`distancia_otro_estructura_transporte` DistanciaTranPubOtro,
                            `formato_predio`.`condiciones_otro_estructura_transporte` CveCondicionesTranPubOtro,
                            cctpo.descripcion CondicionTranPubOtro,
                            `formato_predio`.`nivel_equrban_educacion` EqUrbEducacionNivel,
                            `formato_predio`.`distancia_equrban_educacion` EqUrbEducacionDistancia,
                            `formato_predio`.`nivel_equrban_salud` EqUrbSaludNivel,
                            `formato_predio`.`distancia_equrban_salud` EqUrbSaludDistancia,
                            `formato_predio`.`nivel_equrban_recreacion` EqUrbRecreacionNivel,
                            `formato_predio`.`distancia_equrban_recreacion` EqUrbRecreacionDistancia,
                            `formato_predio`.`nivel_equrban_cultura` EqUrbCulturaNivel,
                            `formato_predio`.`distancia_equrban_cultura` EqUrbCulturaDistancia,
                            `formato_predio`.`nivel_equrban_servurbanos` EqUrbServUrbanosNivel,
                            `formato_predio`.`distancia_equrban_servurbanos` EqUrbServUrbanosDistancia,
                            `formato_predio`.`nivel_equrban_asistencia_social` EqUrbAsistenciaSocialNivel,
                            `formato_predio`.`distancia_equrban_asistencia_social` EqUrbAsistenciaSocialDistancia,
                            `formato_predio`.`otro_equrban` EqUrbOtro,
                            `formato_predio`.`nivel_equran_otro` EqUrbOtroNivel,
                            `formato_predio`.`distancia_equrban_otro` EqUrbOtroDistancia,
                            `formato_predio`.`nivel_servurbanos_educacion` ServUrbEducacionNivel,
                            `formato_predio`.`distancia_servurbanos_educacion` ServUrbEducacionDistancia,
                            `formato_predio`.`nivel_servurbanos_salud` ServUrbSaludNivel,
                            `formato_predio`.`distancia_servurbanos_salud` ServUrbSaludDistancia,
                            `formato_predio`.`nivel_servurbanos_recreacion` ServUrbRecreacionNivel,
                            `formato_predio`.`distancia_servurbanos_recreacion` ServUrbRecreacionDistancia,
                            `formato_predio`.`nivel_servurbanos_cultura` ServUrbCulturaNivel,
                            `formato_predio`.`distancia_servurbanos_cultura` ServUrbCulturaDistancia,
                            `formato_predio`.`otro_servurbanos` ServUrbOtro,
                            `formato_predio`.`nivel_servurbanos_otro` ServUrbOtroNivel,
                            `formato_predio`.`distancia_servurbanos_otro` ServUrbOtroDistancia,
                            `formato_predio`.`topografia_terreno` CveTopografiaTerreno,
                            if(find_in_set(1,topografia_terreno) > 0,'SI','NO') PendientesSuaves,
                            if(find_in_set(2,topografia_terreno) > 0,'SI','NO') PendientesInclinadas,
                            if(find_in_set(3,topografia_terreno) > 0,'SI','NO') PendientesAbruptas,
                            if(find_in_set(4,topografia_terreno) > 0,'SI','NO') Planicies,
                            if(find_in_set(5,topografia_terreno) > 0,'SI','NO') CortesdeSuelo,
                            if(find_in_set(6,topografia_terreno) > 0,'SI','NO') VegetacionInterior,
                            `formato_predio`.`vegetacion_interior_est_arboreo` EstratoArboreo,
                            `formato_predio`.`vegetacion_interior_est_arbustivo` EstratoArbustivo,
                            `formato_predio`.`hidrografia` CveHidrografia,
                            if(find_in_set(1,hidrografia) > 0,'SI','NO') HRios,
                            if(find_in_set(2,hidrografia) > 0,'SI','NO') Escurrimientos,
                            if(find_in_set(3,hidrografia) > 0,'SI','NO') Arroyos,
                            if(find_in_set(4,hidrografia) > 0,'SI','NO') Lagos,
                            if(find_in_set(5,hidrografia) > 0,'SI','NO') Lagunas,
                            if(find_in_set(6,hidrografia) > 0,'SI','NO') Manantiales,
                            if(find_in_set(7,hidrografia) > 0,'SI','NO') OjoAgua,
                            if(find_in_set(8,hidrografia) > 0,'SI','NO') Humedales,
                            if(find_in_set(9,hidrografia) > 0,'SI','NO') Acueducto,
                            if(find_in_set(10,hidrografia) > 0,'SI','NO') Canal,
                            `formato_predio`.`restricciones_federales` CveRestriccionesFederales,
                            if(find_in_set(1,restricciones_federales) > 0,'SI','NO') Rios,
                            `formato_predio`.`distancia_rios` DistanciaRios,
                            if(find_in_set(2,restricciones_federales) > 0,'SI','NO') CorrientesIntermitentes,
                            `formato_predio`.`distancia_corrientes_intermitentes` DistanciaCorrientes,
                            if(find_in_set(3,restricciones_federales) > 0,'SI','NO') CuerposdeAgua,
                            `formato_predio`.`distancia_cuerpos_agua` DistanciaCuerposAgua,
                            if(find_in_set(4,restricciones_federales) > 0,'SI','NO') LineasALtaTension,
                            `formato_predio`.`distancia_lineas_alta_tension` DistanciaLineasAltaTension,
                            if(find_in_set(5,restricciones_federales) > 0,'SI','NO') ViasFerreas,
                            `formato_predio`.`distancia_vias_ferreas` DistanciaViasFerreas,
                            if(find_in_set(6,restricciones_federales) > 0,'SI','NO') Carreteras,
                            `formato_predio`.`distancia_carreteras` DistanciaCarreteras,
                            if(find_in_set(7,restricciones_federales) > 0,'SI','NO') Ductos,
                            `formato_predio`.`distancia_ductos` DistanciaDuctos,
                            if(find_in_set(8,restricciones_federales) > 0,'SI','NO') Gaseras,
                            `formato_predio`.`distancia_gaseras` DistanciaGaseras,
                            if(find_in_set(9,restricciones_federales) > 0,'SI','NO') Barrancas,
                            `formato_predio`.`distancia_barrancas` DistanciaBarrancas,
                            `formato_predio`.`caracteristicas_suelo` CveCaracteristicasSuelo,
                            if(find_in_set(1,caracteristicas_suelo) > 0,'SI','NO') Rocas,
                            if(find_in_set(2,caracteristicas_suelo) > 0,'SI','NO') ArenasGravas,
                            if(find_in_set(3,caracteristicas_suelo) > 0,'SI','NO') LimosArcillas,
                            if(find_in_set(4,caracteristicas_suelo) > 0,'SI','NO') RellenoEscombro,
                            if(find_in_set(5,caracteristicas_suelo) > 0,'SI','NO') MateriaOrganica,
                            `formato_predio`.`otras_observaciones` CveOtrasObservaciones,
                            if(find_in_set(1,otras_observaciones) > 0,'SI','NO') FracturasTerreno,
                            if(find_in_set(2,otras_observaciones) > 0,'SI','NO') Hundimientos,
                            if(find_in_set(3,otras_observaciones) > 0,'SI','NO') InestabilidadLaderas,
                            if(find_in_set(4,otras_observaciones) > 0,'SI','NO') ZonasInundables,
                            `formato_predio`.`predio_apto_desarrollo_vivienda` PredioAptoVivienda,
                            `formato_predio`.`observaciones_predio_apto_desarrollo` ObservacionesPredioApto,
                            `formato_predio`.`porcentaje_superficie_utilizable` PorcentajeSuperficieUtillizable,
                            `formato_predio`.`observaciones_adicionales` ObservacionesAdicionales,
                            `formato_predio`.`nombre_realizo_recorrido` NombreRealizoRecorrido,
                            `formato_predio`.`correo_electronico` CorreoElectronico,
                            `formato_predio`.`reporte_fotografico` ReporteFotografico,
                            `formato_predio`.`calificacion` Calificacion,
                            `formato_predio`.`fecha_registro` FechaRegistro,
                            `formato_predio`.`fecha_update` FechaUltimaActualizacion
                        FROM `prod_predios`.`formato_predio`
                        JOIN `prod_predios`.`c_entidad_federativa` ce ON `prod_predios`.`formato_predio`.cve_edo = ce.clave
                        JOIN `prod_predios`.`c_municipio` cm ON `prod_predios`.`formato_predio`.cve_edo = cm.clave_entidad_federativa and `prod_predios`.`formato_predio`.cve_mun = cm.clave
                        JOIN `prod_predios`.`c_localidad` cl ON `prod_predios`.`formato_predio`.cve_edo = cl.clave_entidad_federativa and `prod_predios`.`formato_predio`.cve_mun = cl.clave_municipio and `prod_predios`.`formato_predio`.cve_loc = cl.clave
                        LEFT JOIN `prod_predios`.`c_tipo_predio` ctp ON `prod_predios`.`formato_predio`.tipo_predio = ctp.id
                        LEFT JOIN `prod_predios`.`c_localizacion` clz ON `prod_predios`.`formato_predio`.localizacion = clz.id
                        LEFT JOIN `prod_predios`.`c_electricidad` cet ON `prod_predios`.`formato_predio`.electricidad = cet.id
                        LEFT JOIN `prod_predios`.`c_drenaje` cd ON `prod_predios`.`formato_predio`.drenaje = cd.id
                        LEFT JOIN `prod_predios`.`c_agua_potable` ca ON `prod_predios`.`formato_predio`.agua_potable = ca.id
                        LEFT JOIN `prod_predios`.`c_vialidad` cv ON `prod_predios`.`formato_predio`.tipo_vialidad_conexion_predio = cv.id
                        LEFT JOIN `prod_predios`.`c_condiciones` ccc ON `prod_predios`.`formato_predio`.condicion_vialidad_conexion_predio = ccc.id
                        LEFT JOIN `prod_predios`.`c_vialidad_nivel` ctvn ON `prod_predios`.`formato_predio`.tipo_vialidad_conexion_predio_nivel = ctvn.id
                        LEFT JOIN `prod_predios`.`c_condiciones` cccn ON `prod_predios`.`formato_predio`.condicion_vialidad_conexion_predio_nivel = cccn.id
                        LEFT JOIN `prod_predios`.`c_vialidad` ctap ON `prod_predios`.`formato_predio`.tipo_acceso_predio = ctap.id
                        LEFT JOIN `prod_predios`.`c_condiciones` ccap ON `prod_predios`.`formato_predio`.condicion_acceso_predio = ccap.id
                        LEFT JOIN `prod_predios`.`c_vialidad_nivel` ctapn ON `prod_predios`.`formato_predio`.tipo_acceso_predio_nivel = ctapn.id
                        LEFT JOIN `prod_predios`.`c_condiciones` ccapn ON `prod_predios`.`formato_predio`.condicion_acceso_predio_nivel = ccapn.id
                        LEFT JOIN `prod_predios`.`c_condiciones` cctpai ON `prod_predios`.`formato_predio`.condiciones_transporte_publico_inmediato = cctpai.id
                        LEFT JOIN `prod_predios`.`c_condiciones` cctbdc ON `prod_predios`.`formato_predio`.condiciones_transporte_publico_comoda = cctbdc.id
                        LEFT JOIN `prod_predios`.`c_condiciones` cctpl ON `prod_predios`.`formato_predio`.condiciones_transporte_publico_lejano = cctpl.id
                        LEFT JOIN `prod_predios`.`c_condiciones` cctpp ON `prod_predios`.`formato_predio`.condiciones_parada_transporte = cctpp.id
                        LEFT JOIN `prod_predios`.`c_condiciones` cctpo ON `prod_predios`.`formato_predio`.condiciones_otro_estructura_transporte = cctpo.id;
                       ";
            return await db.QueryAsync<Predio>(sql, new {  });
        }
        public async Task<bool> DropPredio(int id)
        {
            var db = DbConnection();
            var sql = @"
                        delete from prod_predios.formato_predio where id = @Id;";
            var result = await db.ExecuteAsync(sql, new { Id = id });
            return result > 0;
        }
        public async Task<bool> InsertFilePredio(string idPredio, string idFile, string filename, string extension)
        {
            var db = DbConnection();
            var sql = @"
                        INSERT INTO prod_predios.files_predios(nombre_archivo, extension, idFile, idPredio)
                        VALUES(@NomArchivo, @Ext, @IdFile, @IdPredio)
                        ON DUPLICATE KEY UPDATE nombre_archivo = @NomArchivo, extension = @Ext, fecha_update = NOW();
                       ";
            var result = await db.ExecuteAsync(sql, new {
                NomArchivo = filename,
                Ext = extension,
                IdFile = idFile,
                IdPredio = idPredio
            });
            return result > 0;
        }
        public async Task<bool> InsertRepFoto(string idPredio, string filename)
        {
            var db = DbConnection();
            var sql = @"
                        UPDATE prod_predios.formato_predio set reporte_fotografico = @NomArchivo WHERE id = @idPredio;
                       ";
            var result = await db.ExecuteAsync(sql, new {
                NomArchivo = filename,
                //Ext = extension,
                //IdFile = idSedatu,
                IdPredio = idPredio
            });
            return result > 0;
        }
        public async Task<Catalogo> GetFile(int idPredio, int idFile)
        {
            var db = DbConnection();
            var sql = @"
                        select idPredio Id, nombre_archivo Descripcion, estatus Clave from prod_predios.files_predios where idPredio = @IdPredio and idFile = @IdFile;
                       ";
            return await db.QueryFirstOrDefaultAsync<Catalogo>(sql, new { IdPredio = idPredio, IdFile = idFile });
        }
        public async Task<Catalogo> GetRepFoto(int idPredio)
        {
            var db = DbConnection();
            var sql = @"
                        select id_sedatu Clave,reporte_fotografico Descripcion from prod_predios.formato_predio where id = @Id;
                       ";
            return await db.QueryFirstOrDefaultAsync<Catalogo>(sql, new { Id = idPredio });
        }
        public async Task<bool> ValidarArchivo(string idPredio, int idFile)
        {
            var db = DbConnection();
            var sql = @"
                        UPDATE prod_predios.files_predios SET estatus = 1, fecha_cambio_estatus = NOW() WHERE idPredio = @Id and idFile = @IdFile;";
            var result = await db.ExecuteAsync(sql, new { Id = idPredio, IdFile = idFile });
            return result > 0;
        }
        public async Task<bool> RechazarArchivo(int idPredio, int idFile)
        {
            var db = DbConnection();
            var sql = @"
                        UPDATE prod_predios.files_predios SET estatus = 2, fecha_cambio_estatus = NOW() WHERE idPredio = @Id and idFile = @IdFile;";
            var result = await db.ExecuteAsync(sql, new { Id = idPredio, IdFile = idFile });
            return result > 0;
        }
    }
}
