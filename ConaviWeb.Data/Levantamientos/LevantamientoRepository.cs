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
                        (`id`, `fecha_recorrido`,`domicilio`,`cve_edo`,`cve_mun`,`cve_loc`,`coord_n`,`coord_o`,`tipo_predio`,`localizacion`,`distancia_localizacion`,`acceso`,`otra_caracteristica`,`otra_caracteristica_desc`,`electricidad`,`electricidad_distancia`,`drenaje`,`drenaje_distancia`,`agua_potable`,`agua_potable_distancia`,`niveles_construidos`,`materiales_naturales`,`materiales_industrializados`,`vialidad_conexion_predio`,`tipo_vialidad_conexion_predio`,`distancia_vialidad_conexion_predio`,`condicion_vialidad_conexion_predio`,`material_vialidad_conexion_predio`,`acceso_predio`,`tipo_acceso_predio`,`distancia_acceso_predio`,`condicion_acceso_predio`,`material_acceso_predio`,`distancia_transporte_publico_inmediato`,`condiciones_transporte_publico_inmediato`,`distancia_transporte_publico_comoda`,`condiciones_transporte_publico_comoda`,`distancia_transporte_publico_lejano`,`condiciones_transporte_publico_lejano`,`distancia_parada_transporte`,`condiciones_parada_transporte`,`distancia_estacion_cetram`,`condiciones_estacion_cetram`,`distancia_sitio_taxis`,`condiciones_sitio_taxis`,`otro_estructura_transporte`,`distancia_otro_estructura_transporte`,`condiciones_otro_estructura_transporte`,`nivel_equrban_educacion`,`distancia_equrban_educacion`,`nivel_equrban_salud`,`distancia_equrban_salud`,`nivel_equrban_recreacion`,`distancia_equrban_recreacion`,`nivel_equrban_cultura`,`distancia_equrban_cultura`,`nivel_equrban_servurbanos`,`distancia_equrban_servurbanos`,`nivel_equrban_asistencia_social`,`distancia_equrban_asistencia_social`,`otro_equrban`,`nivel_equran_otro`,`distancia_equrban_otro`,`nivel_servurbanos_educacion`,`distancia_servurbanos_educacion`,`nivel_servurbanos_salud`,`distancia_servurbanos_salud`,`nivel_servurbanos_recreacion`,`distancia_servurbanos_recreacion`,`nivel_servurbanos_cultura`,`distancia_servurbanos_cultura`,`otro_servurbanos`,`nivel_servurbanos_otro`,`distancia_servurbanos_otro`,`topografia_terreno`,`vegetacion_interior_est_arboreo`,`vegetacion_interior_est_arbustivo`,`hidrografia`,`restricciones_federales`,`distancia_rios`,`distancia_corrientes_intermitentes`,`distancia_cuerpos_agua`,`distancia_lineas_alta_tension`,`distancia_vias_ferreas`,`distancia_carreteras`,`distancia_ductos`,`distancia_barrancas`,`caracteristicas_suelo`,`otras_observaciones`,`predio_apto_desarrollo_vivienda`,`condiciones_predio_apto_desarrollo`,`porcentaje_superficie_utilizable`,`observaciones_adicionales`,`nombre_realizo_recorrido`)
                        VALUES (@id,@fecha_recorrido,@domicilio,@cve_edo,@cve_mun,@cve_loc,@coord_n,@coord_o,@tipo_predio,@localizacion,@distancia_localizacion,@acceso,@otra_caracteristica,@otra_caracteristica_desc,@electricidad,@electricidad_distancia,@drenaje,@drenaje_distancia,@agua_potable,@agua_potable_distancia,@niveles_construidos,@materiales_naturales,@materiales_industrializados,@vialidad_conexion_predio,@tipo_vialidad_conexion_predio,@distancia_vialidad_conexion_predio,@condicion_vialidad_conexion_predio,@material_vialidad_conexion_predio,@acceso_predio,@tipo_acceso_predio,@distancia_acceso_predio,@condicion_acceso_predio,@material_acceso_predio,@distancia_transporte_publico_inmediato,@condiciones_transporte_publico_inmediato,@distancia_transporte_publico_comoda,@condiciones_transporte_publico_comoda,@distancia_transporte_publico_lejano,@condiciones_transporte_publico_lejano,@distancia_parada_transporte,@condiciones_parada_transporte,@distancia_estacion_cetram,@condiciones_estacion_cetram,@distancia_sitio_taxis,@condiciones_sitio_taxis,@otro_estructura_transporte,@distancia_otro_estructura_transporte,@condiciones_otro_estructura_transporte,@nivel_equrban_educacion,@distancia_equrban_educacion,@nivel_equrban_salud,@distancia_equrban_salud,@nivel_equrban_recreacion,@distancia_equrban_recreacion,@nivel_equrban_cultura,@distancia_equrban_cultura,@nivel_equrban_servurbanos,@distancia_equrban_servurbanos,@nivel_equrban_asistencia_social,@distancia_equrban_asistencia_social,@otro_equrban,@nivel_equran_otro,@distancia_equrban_otro,@nivel_servurbanos_educacion,@distancia_servurbanos_educacion,@nivel_servurbanos_salud,@distancia_servurbanos_salud,@nivel_servurbanos_recreacion,@distancia_servurbanos_recreacion,@nivel_servurbanos_cultura,@distancia_servurbanos_cultura,@otro_servurbanos,@nivel_servurbanos_otro,@distancia_servurbanos_otro,@topografia_terreno,@vegetacion_interior_est_arboreo,@vegetacion_interior_est_arbustivo,@hidrografia,@restricciones_federales,@distancia_rios,@distancia_corrientes_intermitentes,@distancia_cuerpos_agua,@distancia_lineas_alta_tension,@distancia_vias_ferreas,@distancia_carreteras,@distancia_ductos,@distancia_barrancas,@caracteristicas_suelo,@otras_observaciones,@predio_apto_desarrollo_vivienda,@condiciones_predio_apto_desarrollo,@porcentaje_superficie_utilizable,@observaciones_adicionales,@nombre_realizo_recorrido)
                        ON DUPLICATE KEY UPDATE `fecha_recorrido` = @fecha_recorrido, `domicilio` = @domicilio, `cve_edo` = @cve_edo, `cve_mun` = @cve_mun, `cve_loc` = @cve_loc, `coord_n` = @coord_n, `coord_o` = @coord_o, `tipo_predio` = @tipo_predio, `localizacion` = @localizacion, `distancia_localizacion` = @distancia_localizacion, `acceso` = @acceso, `otra_caracteristica` = @otra_caracteristica, `otra_caracteristica_desc` = @otra_caracteristica_desc, `electricidad` = @electricidad, `electricidad_distancia` = @electricidad_distancia, `drenaje` = @drenaje, `drenaje_distancia` = @drenaje_distancia, `agua_potable` = @agua_potable, `agua_potable_distancia` = @agua_potable_distancia, `niveles_construidos` = @niveles_construidos, `materiales_naturales` = @materiales_naturales, `materiales_industrializados` = @materiales_industrializados, `vialidad_conexion_predio` = @vialidad_conexion_predio, `tipo_vialidad_conexion_predio` = @tipo_vialidad_conexion_predio, `distancia_vialidad_conexion_predio` = @distancia_vialidad_conexion_predio, `condicion_vialidad_conexion_predio` = @condicion_vialidad_conexion_predio, `material_vialidad_conexion_predio` = @material_vialidad_conexion_predio, `acceso_predio` = @acceso_predio, `tipo_acceso_predio` = @tipo_acceso_predio, `distancia_acceso_predio` = @distancia_acceso_predio, `condicion_acceso_predio` = @condicion_acceso_predio, `material_acceso_predio` = @material_acceso_predio, `distancia_transporte_publico_inmediato` = @distancia_transporte_publico_inmediato, `condiciones_transporte_publico_inmediato` = @condiciones_transporte_publico_inmediato, `distancia_transporte_publico_comoda` = @distancia_transporte_publico_comoda, `condiciones_transporte_publico_comoda` = @condiciones_transporte_publico_comoda, `distancia_transporte_publico_lejano` = @distancia_transporte_publico_lejano, `condiciones_transporte_publico_lejano` = @condiciones_transporte_publico_lejano, `distancia_parada_transporte` = @distancia_parada_transporte, `condiciones_parada_transporte` = @condiciones_parada_transporte, `distancia_estacion_cetram` = @distancia_estacion_cetram, `condiciones_estacion_cetram` = @condiciones_estacion_cetram, `distancia_sitio_taxis` = @distancia_sitio_taxis, `condiciones_sitio_taxis` = @condiciones_sitio_taxis, `otro_estructura_transporte` = @otro_estructura_transporte, `distancia_otro_estructura_transporte` = @distancia_otro_estructura_transporte, `condiciones_otro_estructura_transporte` = @condiciones_otro_estructura_transporte, `nivel_equrban_educacion` = @nivel_equrban_educacion, `distancia_equrban_educacion` = @distancia_equrban_educacion, `nivel_equrban_salud` = @nivel_equrban_salud, `distancia_equrban_salud` = @distancia_equrban_salud, `nivel_equrban_recreacion` = @nivel_equrban_recreacion, `distancia_equrban_recreacion` = @distancia_equrban_recreacion, `nivel_equrban_cultura` = @nivel_equrban_cultura, `distancia_equrban_cultura` = @distancia_equrban_cultura, `nivel_equrban_servurbanos` = @nivel_equrban_servurbanos, `distancia_equrban_servurbanos` = @distancia_equrban_servurbanos, `nivel_equrban_asistencia_social` = @nivel_equrban_asistencia_social, `distancia_equrban_asistencia_social` = @distancia_equrban_asistencia_social, `otro_equrban` = @otro_equrban, `nivel_equran_otro` = @nivel_equran_otro, `distancia_equrban_otro` = @distancia_equrban_otro, `nivel_servurbanos_educacion` = @nivel_servurbanos_educacion, `distancia_servurbanos_educacion` = @distancia_servurbanos_educacion, `nivel_servurbanos_salud` = @nivel_servurbanos_salud, `distancia_servurbanos_salud` = @distancia_servurbanos_salud, `nivel_servurbanos_recreacion` = @nivel_servurbanos_recreacion, `distancia_servurbanos_recreacion` = @distancia_servurbanos_recreacion, `nivel_servurbanos_cultura` = @nivel_servurbanos_cultura, `distancia_servurbanos_cultura` = @distancia_servurbanos_cultura, `otro_servurbanos` = @otro_servurbanos, `nivel_servurbanos_otro` = @nivel_servurbanos_otro, `distancia_servurbanos_otro` = @distancia_servurbanos_otro, `topografia_terreno` = @topografia_terreno, `vegetacion_interior_est_arboreo` = @vegetacion_interior_est_arboreo, `vegetacion_interior_est_arbustivo` = @vegetacion_interior_est_arbustivo, `hidrografia` = @hidrografia, `restricciones_federales` = @restricciones_federales, `distancia_rios` = @distancia_rios, `distancia_corrientes_intermitentes` = @distancia_corrientes_intermitentes, `distancia_cuerpos_agua` = @distancia_cuerpos_agua, `distancia_lineas_alta_tension` = @distancia_lineas_alta_tension, `distancia_vias_ferreas` = @distancia_vias_ferreas, `distancia_carreteras` = @distancia_carreteras, `distancia_ductos` = @distancia_ductos, `distancia_barrancas` = @distancia_barrancas, `caracteristicas_suelo` = @caracteristicas_suelo, `otras_observaciones` = @otras_observaciones, `predio_apto_desarrollo_vivienda` = @predio_apto_desarrollo_vivienda, `condiciones_predio_apto_desarrollo` = @condiciones_predio_apto_desarrollo, `porcentaje_superficie_utilizable` = @porcentaje_superficie_utilizable, `observaciones_adicionales` = @observaciones_adicionales, `nombre_realizo_recorrido` = @nombre_realizo_recorrido, `fecha_update` = NOW();";

            var result = await db.ExecuteAsync(sql, new
            {
                id = predio.Id,
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
                acceso = predio.CveAcceso,
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
                condicion_vialidad_conexion_predio = predio.CondicionesVialidadConexion,
                material_vialidad_conexion_predio = predio.MaterialesVialidadConexion,
                acceso_predio = predio.TieneAccesoPredio,
                tipo_acceso_predio = predio.TipoAccesoPredio,
                distancia_acceso_predio = predio.DistanciaAccesoPredio,
                condicion_acceso_predio = predio.CondicionesAccesoPredio,
                material_acceso_predio = predio.MaterialesAccesoPredio,
                distancia_transporte_publico_inmediato = predio.DistanciaTranPubAccesoInmediato,
                condiciones_transporte_publico_inmediato = predio.CondicionesTranPubAccesoInmediato,
                distancia_transporte_publico_comoda = predio.DistanciaTranPubComoda,
                condiciones_transporte_publico_comoda = predio.CondicionesTranPubComoda,
                distancia_transporte_publico_lejano = predio.DistanciaTranPubLejano,
                condiciones_transporte_publico_lejano = predio.CondicionesTranPubLejano,
                distancia_parada_transporte = predio.DistanciaTranPubParada,
                condiciones_parada_transporte = predio.CondicionesTranPubParada,
                distancia_estacion_cetram = predio.DistanciaTranPubEstacionCetram,
                condiciones_estacion_cetram = predio.CondicionesTranPubEstacionCetram,
                distancia_sitio_taxis = predio.DistanciaTranPubSitioTaxis,
                condiciones_sitio_taxis = predio.CondicionesTranPubSitioTaxis,
                otro_estructura_transporte = predio.TranPubOtro,
                distancia_otro_estructura_transporte = predio.DistanciaTranPubOtro,
                condiciones_otro_estructura_transporte = predio.CondicionesTranPubOtro,
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
                condiciones_predio_apto_desarrollo = predio.CondicionesPredioApto,
                porcentaje_superficie_utilizable = predio.PorcentajeSuperficieUtillizable,
                observaciones_adicionales = predio.ObservacionesAdicionales,
                nombre_realizo_recorrido = predio.NombreRealizoRecorrido
            });
            return result > 0;
        }
        public async Task<Predio> GetFormatoLevantamiento(int id)
        {
            var db = DbConnection();
            var sql = @"
                        SELECT fp.id Id, fp.fecha_recorrido FechaRecorrido, fp.domicilio Domicilio, fp.cve_edo CveEstado, fp.cve_mun CveMunicipio, fp.cve_loc CveLocalidad, fp.coord_n CoordN, fp.coord_o CoordO, fp.tipo_predio CveTipoPredio, fp.localizacion CveLocalizacion, fp.distancia_localizacion DistanciaLocalizacion, fp.acceso CveAcceso, fp.otra_caracteristica CveOtrasCaracteristicas, fp.otra_caracteristica_desc CaracPredioOtro, fp.electricidad CveElectricidad, fp.electricidad_distancia DistElectricidad, fp.drenaje CveDrenaje, fp.drenaje_distancia DistDrenaje, fp.agua_potable CveAguaPotable, fp.agua_potable_distancia DistAguaPotable, fp.niveles_construidos NivelesConstruidos, fp.materiales_naturales MaterialesNaturales, fp.materiales_industrializados MaterialesIndustrializados, fp.vialidad_conexion_predio TieneConexionVialidad, fp.tipo_vialidad_conexion_predio TipoVialidadConexion, fp.distancia_vialidad_conexion_predio DistanciaVialidadConexion, fp.condicion_vialidad_conexion_predio CondicionesVialidadConexion, fp.material_vialidad_conexion_predio MaterialesVialidadConexion, fp.acceso_predio TieneAccesoPredio, fp.tipo_acceso_predio TipoAccesoPredio, fp.distancia_acceso_predio DistanciaAccesoPredio, fp.condicion_acceso_predio CondicionesAccesoPredio, fp.material_acceso_predio MaterialesAccesoPredio, fp.distancia_transporte_publico_inmediato DistanciaTranPubAccesoInmediato, fp.condiciones_transporte_publico_inmediato CondicionesTranPubAccesoInmediato, fp.distancia_transporte_publico_comoda DistanciaTranPubComoda, fp.condiciones_transporte_publico_comoda CondicionesTranPubComoda, fp.distancia_transporte_publico_lejano DistanciaTranPubLejano, fp.condiciones_transporte_publico_lejano CondicionesTranPubLejano, fp.distancia_parada_transporte DistanciaTranPubParada, fp.condiciones_parada_transporte CondicionesTranPubParada, fp.distancia_estacion_cetram DistanciaTranPubEstacionCetram, fp.condiciones_estacion_cetram CondicionesTranPubEstacionCetram, fp.distancia_sitio_taxis DistanciaTranPubSitioTaxis, fp.condiciones_sitio_taxis CondicionesTranPubSitioTaxis, fp.otro_estructura_transporte TranPubOtro, fp.distancia_otro_estructura_transporte DistanciaTranPubOtro, fp.condiciones_otro_estructura_transporte CondicionesTranPubOtro, fp.nivel_equrban_educacion EqUrbEducacionNivel, fp.distancia_equrban_educacion EqUrbEducacionDistancia, fp.nivel_equrban_salud EqUrbSaludNivel, fp.distancia_equrban_salud EqUrbSaludDistancia, fp.nivel_equrban_recreacion EqUrbRecreacionNivel, fp.distancia_equrban_recreacion EqUrbRecreacionDistancia, fp.nivel_equrban_cultura EqUrbCulturaNivel, fp.distancia_equrban_cultura EqUrbCulturaDistancia, fp.nivel_equrban_servurbanos EqUrbServUrbanosNivel, fp.distancia_equrban_servurbanos EqUrbServUrbanosDistancia, fp.nivel_equrban_asistencia_social EqUrbAsistenciaSocialNivel, fp.distancia_equrban_asistencia_social EqUrbAsistenciaSocialDistancia, fp.otro_equrban EqUrbOtro, fp.nivel_equran_otro EqUrbOtroNivel, fp.distancia_equrban_otro EqUrbOtroDistancia, fp.nivel_servurbanos_educacion ServUrbEducacionNivel, fp.distancia_servurbanos_educacion ServUrbEducacionDistancia, fp.nivel_servurbanos_salud ServUrbSaludNivel, fp.distancia_servurbanos_salud ServUrbSaludDistancia, fp.nivel_servurbanos_recreacion ServUrbRecreacionNivel, fp.distancia_servurbanos_recreacion ServUrbRecreacionDistancia, fp.nivel_servurbanos_cultura ServUrbCulturaNivel, fp.distancia_servurbanos_cultura ServUrbCulturaDistancia, fp.otro_servurbanos ServUrbOtro, fp.nivel_servurbanos_otro ServUrbOtroNivel, fp.distancia_servurbanos_otro ServUrbOtroDistancia, fp.topografia_terreno CveTopografiaTerreno, fp.vegetacion_interior_est_arboreo EstratoArboreo, fp.vegetacion_interior_est_arbustivo EstratoArbustivo, fp.hidrografia CveHidrografia, fp.restricciones_federales CveRestriccionesFederales, fp.distancia_rios DistanciaRios, fp.distancia_corrientes_intermitentes DistanciaCorrientes, fp.distancia_cuerpos_agua DistanciaCuerposAgua, fp.distancia_lineas_alta_tension DistanciaLineasAltaTension, fp.distancia_vias_ferreas DistanciaViasFerreas, fp.distancia_carreteras DistanciaCarreteras, fp.distancia_ductos DistanciaDuctos, fp.distancia_barrancas DistanciaBarrancas, fp.caracteristicas_suelo CveCaracteristicasSuelo, fp.otras_observaciones CveOtrasObservaciones, fp.predio_apto_desarrollo_vivienda PredioAptoVivienda, fp.condiciones_predio_apto_desarrollo CondicionesPredioApto, fp.porcentaje_superficie_utilizable PorcentajeSuperficieUtillizable, fp.observaciones_adicionales ObservacionesAdicionales, fp.nombre_realizo_recorrido NombreRealizoRecorrido
                        FROM prod_predios.formato_predio fp
                        WHERE fp.id = @Id;";

            return await db.QueryFirstOrDefaultAsync<Predio>(sql, new { Id = id });
        }
        public async Task<IEnumerable<Predio>> GetPrediosAdquisicion()
        {
            var db = DbConnection();
            var sql = @"
                        SELECT fp.id Id, fp.fecha_recorrido FechaRecorrido, fp.domicilio Domicilio, ce.descripcion Estado, cm.descripcion Municipio, cl.descripcion Localidad, fp.nombre_realizo_recorrido NombreRealizoRecorrido, fp.predio_apto_desarrollo_vivienda PredioAptoVivienda, fp.condiciones_predio_apto_desarrollo CondicionesPredioApto
                        FROM prod_predios.formato_predio fp
                        JOIN prod_predios.c_entidad_federativa ce ON fp.cve_edo = ce.clave
                        JOIN prod_predios.c_municipio cm ON fp.cve_mun = cm.clave and cm.clave_entidad_federativa = fp.cve_edo
                        JOIN prod_predios.c_localidad cl ON fp.cve_loc = cl.clave and cl.clave_entidad_federativa = fp.cve_edo and cl.clave_municipio = fp.cve_mun;
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
        public async Task<Catalogo> GetFile(int idPredio, int idFile)
        {
            var db = DbConnection();
            var sql = @"
                        select nombre_archivo Descripcion from prod_predios.files_predios where idPredio = @IdPredio and idFile = @IdFile;
                       ";
            return await db.QueryFirstOrDefaultAsync<Catalogo>(sql, new { IdPredio = idPredio, IdFile = idFile });
        }
    }
}
