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
        //public async Task<bool> UpdateFormatoLevantamiento(Predio predio)
        //{
        //    var db = DbConnection();
        //    var sql = @"
        //                UPDATE prod_predios.formato_predio
        //                (nombre, numero_legajos, numero_fojas, numero_partes, fecha_primero, fecha_ultimo, fecha_elaboracion, ubicacion, descripcion, observaciones, anios_resguardo, id_user, id_expediente, id_inventario_control, id_tipo_documental, id_tipo_soporte, tipo_soporte_documental)
        //                VALUES (@Nombre, @NumeroLegajos, @NumeroFojas, @NumeroPartes, @FechaPrimero, @FechaUltimo, @FechaElaboracion, @Ubicacion, @Descripcion, @Observaciones, @AniosResguardo, @IdUser, @IdExpediente, @IdInventario, @IdTipoDocumental, @IdTipoSoporte, @TipoSoporteDocumental);";

        //    var result = await db.ExecuteAsync(sql, new
        //    {
        //        //Nombre = predio.Nombre,
        //        //NumeroLegajos = predio.Legajos,
        //        //NumeroFojas = predio.Fojas,
        //        //NumeroPartes = predio.NoPartes,
        //        //FechaPrimero = predio.FechaPrimeroAntiguo,
        //        //FechaUltimo = predio.FechaUltimoReciente,
        //        //FechaElaboracion = predio.FechaElaboracion,
        //        //Ubicacion = predio.Ubicacion,
        //        //Descripcion = predio.Descripcion,
        //        //Observaciones = predio.Observaciones,
        //        //AniosResguardo = predio.AniosResguardo,
        //        //IdUser = predio.IdUser,
        //        //Idpredio = predio.Idpredio,
        //        //IdInventario = predio.IdInventario,
        //        //IdTipoDocumental = predio.IdTipoDocumental,
        //        //IdTipoSoporte = predio.IdTipoSoporte,
        //        //TipoSoporteDocumental = predio.IdTipoSoporteDocumental
        //    });
        //    return result > 0;
        //}
        //public async Task<IEnumerable<Catalogo>> GetAreas()
        //{
        //    var db = DbConnection();
        //    var sql = @"
        //                select id Id,descripcion Clave from prod_control_exp.cat_areas where estatus = 1 order by id;
        //               ";
        //    return await db.QueryAsync<Catalogo>(sql, new { });
        //}
        //public async Task<int> GetIdUserArea(string area)
        //{
        //    var db = DbConnection();
        //    var sql = @"
        //                select id from prod_control_exp.cat_areas where descripcion = @Area order by id;
        //               ";
        //    return await db.QueryFirstOrDefaultAsync<int>(sql, new { Area = area });
        //}

        //public async Task<IEnumerable<Catalogo>> GetTiposSoporte()
        //{
        //    var db = DbConnection();
        //    var sql = @"
        //                SELECT id Id, concat(clave,' - ',descripcion) Clave FROM prod_control_exp.cat_tipo_soporte WHERE activo = 1 ORDER BY id;
        //                ";
        //    return await db.QueryAsync<Catalogo>(sql, new { });
        //}
        //public async Task<IEnumerable<Catalogo>> GetTiposDocumentales()
        //{
        //    var db = DbConnection();
        //    var sql = @"
        //                SELECT id Id, CONCAT(id,'. ',descripcion) Clave FROM prod_control_exp.cat_tipo_documentales WHERE activo = 1 ORDER BY id;
        //                ";
        //    return await db.QueryAsync<Catalogo>(sql, new { });
        //}
        //public async Task<IEnumerable<SerieDocumental>> GetSeries()
        //{
        //    var db = DbConnection();
        //    var sql = @"
        //                select id Id, codigo Codigo, descripcion Descripcion,
        //                vig_doc_val_a VigDocA, vig_doc_val_l VigDocL, vig_doc_val_fc VigDocFC,
        //                vig_doc_pla_con_at PlazoConAT, vig_doc_pla_con_ac PlazoConAC, (vig_doc_pla_con_at + vig_doc_pla_con_ac) PlazoConTot,
        //                tec_sel_e TecSelE, tec_sel_c TecSelC, tec_sel_m TecSelM,
        //                observaciones Observaciones, estatus
        //                from prod_control_exp.cat_serie_documental
        //                order by id;
        //                ";
        //    return await db.QueryAsync<SerieDocumental>(sql, new { });
        //}
        //public async Task<SerieDocumental> GetSerieDocumental(int id)
        //{
        //    var db = DbConnection();

        //    var sql = @"
        //                select id Id, codigo Codigo, descripcion Descripcion,
        //                vig_doc_val_a VigDocA, vig_doc_val_l VigDocL, vig_doc_val_fc VigDocFC,
        //                vig_doc_pla_con_at PlazoConAT, vig_doc_pla_con_ac PlazoConAC, (vig_doc_pla_con_at + vig_doc_pla_con_ac) PlazoConTot,
        //                tec_sel_e TecSelE, tec_sel_c TecSelC, tec_sel_m TecSelM,
        //                observaciones Observaciones, estatus
        //                from prod_control_exp.cat_serie_documental
        //                where id = @Id";

        //    return await db.QueryFirstOrDefaultAsync<SerieDocumental>(sql, new { Id = id });
        //}
        //public async Task<bool> UpdateSerieDocCat(SerieDocumental serie)
        //{
        //    var db = DbConnection();
        //    var sql = @"";
        //    if (serie.Id == 0)
        //    {
        //        sql = @"
        //                INSERT INTO prod_control_exp.cat_serie_documental(codigo,descripcion,vig_doc_val_a,vig_doc_val_l,vig_doc_val_fc, vig_doc_pla_con_at, vig_doc_pla_con_ac, vig_doc_pla_con_tot,tec_sel_e, tec_sel_c, tec_sel_m, observaciones,id_seccion)
        //                VALUES(@Codigo,@Descripcion,@VigDocValA,@VigDocValL,@VigDocValFC,@VigDocPlaConAt,@VigDocPlaConAC,@VigDocPlaConAt + @VigDocPlaConAC,@TecSelE,@TecSelC,@TecSelM,@Observaciones,1);";
        //    }
        //    else
        //    {
        //        sql = @"
        //                UPDATE prod_control_exp.cat_serie_documental
        //                SET
        //                codigo = @Codigo,
        //                descripcion = @Descripcion,
        //                vig_doc_val_a = @VigDocValA,
        //                vig_doc_val_l = @VigDocValL, 
        //                vig_doc_val_fc = @VigDocValFC, 
        //                vig_doc_pla_con_at = @VigDocPlaConAt, 
        //                vig_doc_pla_con_ac = @VigDocPlaConAC, 
        //                vig_doc_pla_con_tot = @VigDocPlaConAt + @VigDocPlaConAC,
        //                tec_sel_e = @TecSelE, 
        //                tec_sel_c = @TecSelC, 
        //                tec_sel_m = @TecSelM, 
        //                observaciones = @Observaciones
        //                WHERE id = @IdSerieDoc;";
        //    }
        //    var result = await db.ExecuteAsync(sql, new
        //    {
        //        Codigo = serie.Codigo,
        //        Descripcion = serie.Descripcion,
        //        VigDocValA = serie.VigDocA,
        //        VigDocValL = serie.VigDocL,
        //        VigDocValFC = serie.VigDocFC,
        //        VigDocPlaConAt = serie.PlazoConAT,
        //        VigDocPlaConAC = serie.PlazoConAC,
        //        TecSelE = serie.TecSelE,
        //        TecSelC = serie.TecSelC,
        //        TecSelM = serie.TecSelM,
        //        Observaciones = serie.Observaciones,
        //        IdSerieDoc = serie.Id
        //    });
        //    return result > 0;
        //}
        //public async Task<bool> ActivarSerieDocCat(int id)
        //{
        //    var db = DbConnection();

        //    var sql = @"
        //                UPDATE prod_control_exp.cat_serie_documental
        //                SET
        //                estatus = 1
        //                WHERE id = @IdSerieDoc;";

        //    var result = await db.ExecuteAsync(sql, new
        //    {
        //        IdSerieDoc = id
        //    });
        //    return result > 0;
        //}
        //public async Task<bool> DesactivarSerieDocCat(int id)
        //{
        //    var db = DbConnection();

        //    var sql = @"
        //                UPDATE prod_control_exp.cat_serie_documental
        //                SET
        //                estatus = 2
        //                WHERE id = @IdSerieDoc;";

        //    var result = await db.ExecuteAsync(sql, new
        //    {
        //        IdSerieDoc = id
        //    });
        //    return result > 0;
        //}

        //public async Task<Inventario> GetInventarioTP(string puesto)
        //{
        //    var db = DbConnection();

        //    var sql = @"
        //                select itr.id Id, itr.fecha_elaboracion FechaElaboracion, itr.fecha_transferencia FechaTransferencia, itr.nombre_responsable_archivo_tramite NombreResponsableAT
        //                from prod_control_exp.inventario_transferencia itr
        //                join prod_control_exp.cat_puestos cp on itr.id_puesto = cp.id
        //                where cp.descripcion = @Puesto";

        //    return await db.QueryFirstOrDefaultAsync<Inventario>(sql, new { Puesto = puesto });
        //}
        //public async Task<bool> InsertInventarioTP(Inventario inventario)
        //{
        //    var db = DbConnection();

        //    //var sql = @"
        //    //            INSERT INTO prod_control_exp.inventario_transferencia
        //    //            (id_area, fecha_elaboracion, fecha_transferencia, nombre_responsable_archivo_tramite)
        //    //            VALUES (@IdArea, @FechaElaboracion, @FechaTransferencia, @NombreResponsable)
        //    //            ON DUPLICATE KEY UPDATE id_area = @IdArea, fecha_elaboracion = @FechaElaboracion, fecha_transferencia = @FechaTransferencia, nombre_responsable_archivo_tramite = @NombreResponsable;";
        //    var sql = @"
        //                UPDATE prod_control_exp.inventario_control SET fecha_transferencia = @FechaTransferencia WHERE id_puesto = @IdPuesto;";

        //    var result = await db.ExecuteAsync(sql, new
        //    {
        //        IdPuesto = inventario.IdPuesto,
        //        //FechaElaboracion = inventario.FechaElaboracion,
        //        //FechaTransferencia = inventario.FechaTransferencia?.ToString("yyyy-MM-dd"),
        //        FechaTransferencia = inventario.FechaTransferencia,
        //        //NombreResponsable = inventario.NombreResponsableAT
        //    });
        //    return result > 0;
        //}
        ////public async Task<bool> InsertExpedienteInventarioTP(Expediente expediente)
        ////{
        ////    var db = DbConnection();

        ////    var sql = @"
        ////                INSERT INTO prod_control_exp.expediente_transferencia
        ////                (id_expediente, nombre, periodo, anios_resguardo, numero_legajos, numero_fojas, observaciones, id_inventario, id_user)
        ////                VALUES (@IdExpediente, @Nombre, @Periodo, @AniosResguardo, @NumeroLegajos, @NumeroFojas, @Observaciones, @IdInventario, @IdUser);";

        ////    var result = await db.ExecuteAsync(sql, new
        ////    {
        ////        IdExpediente = expediente.IdExpediente,
        ////        Nombre = expediente.Nombre,
        ////        Periodo = expediente.Periodo,
        ////        AniosResguardo = expediente.AniosResguardo,
        ////        NumeroLegajos = expediente.Legajos,
        ////        NumeroFojas = expediente.Fojas,
        ////        Observaciones = expediente.Observaciones,
        ////        IdUser = expediente.IdUser,
        ////        IdInventario = expediente.IdInventario
        ////    });
        ////    return result > 0;
        ////}
        ////public async Task<bool> InsertCaratulaExpedienteTP(Caratula caratula)
        ////{
        ////    var db = DbConnection();

        ////    var sql = @"
        ////                INSERT INTO prod_control_exp.caratula
        ////                (`cant_doc_ori`,`cant_doc_copias`,`cant_cds`,`tec_sel_doc`,`publica`,`confidencial`,`reservada_sol_info`,`descripcion_asunto_expediente`,`fecha_clasificacion`,`periodo_reserva`,`fundamento_legal`,`ampliacion_periodo_reserva`,`fecha_desclasificacion`,`nombre_desclasifica`,`cargo_desclasifica`,`partes_reservando`,`id_user_captura`,`datos_topograficos`,`id_expediente_tp`)
        ////                VALUES (@DocOriginales, @DocCopias, @Cds, @TecnicasSeleccion, @Publica, @Confidencial, @Reservada, @DescripcionAsunto, @FechaClasificacion, @PeriodoReserva, @FundamentoLegal, @AmpliacionPeriodo, @FechaDesclasificacion, @NombreDesclasifica, @CargoDesclasifica, @PartesReservando, @IdUser, @DatosTopograficos, @IdExpediente)
        ////                ON DUPLICATE KEY UPDATE `cant_doc_ori` = @DocOriginales,`cant_doc_copias` = @DocCopias,`cant_cds` = @Cds,`tec_sel_doc` = @TecnicasSeleccion,`publica` = @Publica,`confidencial` = @Confidencial,`reservada_sol_info` = @Reservada,`descripcion_asunto_expediente` = @DescripcionAsunto,`fecha_clasificacion` = @FechaClasificacion,`periodo_reserva` = @PeriodoReserva,`fundamento_legal` = @FundamentoLegal,`ampliacion_periodo_reserva` = @AmpliacionPeriodo,`fecha_desclasificacion` = @FechaDesclasificacion,`nombre_desclasifica` = @NombreDesclasifica,`cargo_desclasifica` = @CargoDesclasifica,`partes_reservando` = @PartesReservando,`id_user_captura` = @IdUser,`datos_topograficos` = @DatosTopograficos;";

        ////    var result = await db.ExecuteAsync(sql, new
        ////    {
        ////        IdExpediente = caratula.IdExpediente,
        ////        //Nombre = caratula.Nombre,
        ////        //NumeroLegajos = caratula.Legajos,
        ////        //Fojas = caratula.Fojas,
        ////        DocOriginales = caratula.DocOriginales,
        ////        DocCopias = caratula.DocCopias,
        ////        Cds = caratula.Cds,
        ////        TecnicasSeleccion = caratula.TecnicasSeleccion,
        ////        Publica = caratula.Publica,
        ////        Confidencial = caratula.Confidencial,
        ////        Reservada = caratula.Reservada,
        ////        DescripcionAsunto = caratula.DescripcionAsunto,
        ////        FechaClasificacion = caratula.FechaClasificacion,
        ////        PeriodoReserva = caratula.PeriodoReserva,
        ////        FundamentoLegal = caratula.FundamentoLegal,
        ////        AmpliacionPeriodo = caratula.AmpliacionPeriodo,
        ////        FechaDesclasificacion = caratula.FechaDesclasificacion,
        ////        NombreDesclasifica = caratula.NombreDesclasifica,
        ////        CargoDesclasifica = caratula.CargoDesclasifica,
        ////        PartesReservando = caratula.PartesReservando,
        ////        DatosTopograficos = caratula.DatosTopograficos,
        ////        IdUser = caratula.IdUser,
        ////    });
        ////    return result > 0;
        ////}
        //public async Task<IEnumerable<Expediente>> GetExpedientesInventarioTP(int id, int id_inventario)
        //{
        //    var db = DbConnection();

        //    //var sql = @"
        //    //            select ROW_NUMBER() over(order by et.id) NoProg, et.id Id, et.id_expediente IdExpediente, csd.codigo Codigo, nombre Nombre, periodo Periodo, anios_resguardo AniosResguardo, numero_legajos Legajos, numero_fojas Fojas, et.observaciones Observaciones, et.fecha_registro FechaRegistro, et.id_inventario IdInventario
        //    //                ,if(et.id_user = @id, 'editable', 'noeditable') EsEditable, et.estatus Estatus
        //    //            from prod_control_exp.expediente_transferencia et
        //    //            join prod_control_exp.cat_serie_documental csd on et.id_expediente = csd.id
        //    //            where et.id_inventario = @IdInv
        //    //            order by et.id;";
        //    var sql = @"
        //                select ROW_NUMBER() over(order by ec.fecha_ultimo, ec.id) NoProg, ROW_NUMBER() over(partition by year(ec.fecha_ultimo) order by ec.fecha_ultimo, ec.id) Consecutivo, ec.id Id, ec.id_expediente IdExpediente, csd.codigo Codigo, ec.nombre Nombre, if(year(ec.fecha_primero)=year(ec.fecha_ultimo),year(ec.fecha_primero),concat(year(ec.fecha_primero),'-',year(ec.fecha_ultimo))) Periodo, ec.anios_resguardo AniosResguardo, ec.numero_legajos Legajos, ec.numero_fojas Fojas, ec.observaciones Observaciones, ec.fecha_registro FechaRegistro, ec.id_inventario_control IdInventario
        //                    ,if(ec.id_user = @id, 'editable', 'noeditable') EsEditable, ec.estatus Estatus
        //                from prod_control_exp.expediente_control ec
        //                join prod_control_exp.cat_serie_documental csd on ec.id_expediente = csd.id
        //                where ec.id_inventario_control = @IdInv and ec.migrado_tp = 1
        //                order by ec.fecha_ultimo, ec.id;";
        //    return await db.QueryAsync<Expediente>(sql, new { Id = id, IdInv = id_inventario });
        //}
        //public async Task<IEnumerable<Expediente>> GetExpedientesTPByIdInv(int id)
        //{
        //    var db = DbConnection();

        //    var sql = @"
        //                select ROW_NUMBER() over(order by ec.fecha_ultimo, ec.id) NoProg, ROW_NUMBER() over(partition by year(ec.fecha_ultimo) order by ec.fecha_ultimo, ec.id) Consecutivo, ec.id Id, ec.id_expediente IdExpediente, csd.codigo Codigo, ec.nombre Nombre, if(year(ec.fecha_primero)=year(ec.fecha_ultimo),year(ec.fecha_primero),concat(year(ec.fecha_primero),'-',year(ec.fecha_ultimo))) Periodo, ec.anios_resguardo AniosResguardo, ec.numero_legajos Legajos, ec.numero_fojas Fojas, ec.observaciones Observaciones
        //                    ,ec.fecha_registro FechaRegistro, ec.id_inventario_control IdInventario, ec.estatus Estatus
        //                from prod_control_exp.expediente_control ec
        //                join prod_control_exp.cat_serie_documental csd on ec.id_expediente = csd.id
        //                where ec.id_inventario_control = @Id and ec.migrado_tp = 1
        //                order by ec.fecha_ultimo, ec.id;";
        //    return await db.QueryAsync<Expediente>(sql, new { Id = id });
        //}
        ////public async Task<IEnumerable<ExpedienteBibliohemerografico>> GetExpedientesValidacionBiblio(int idArea)
        ////{
        ////    var db = DbConnection();

        ////    var sql = @"
        ////                select cons.NoProg NoProg, eb.id Id, eb.numero_ejemplar Ejemplar, eb.id_tipo_soporte IdTipoSoporte, ct.clave ClaveSoporte, eb.titulo_del_libro Titulo, eb.nombre_autor Autor, eb.tema Tema, eb.editorial Editorial, eb.anio Anio, eb.isbn_issn IsbnIssn, eb.numero_paginas Paginas, eb.numero_volumen Volumen, eb.id_inventario_bibliohemerografico IdInventario
        ////                from prod_control_exp.expediente_bibliohemerografico eb
        ////                join prod_control_exp.inventario_bibliohemerografico ib on eb.id_inventario_bibliohemerografico = ib.id
        ////                join prod_control_exp.cat_tipo_soporte ct on eb.id_tipo_soporte = ct.id
        ////                join (select ROW_NUMBER() over(order by ets.anio,ets.id) NoProg, ets.id from prod_control_exp.expediente_bibliohemerografico ets join prod_control_exp.inventario_bibliohemerografico ib on ets.id_inventario_bibliohemerografico = ib.id where ib.id_area = @IdArea) cons on eb.id = cons.id
        ////                where ib.id_area = @IdArea and eb.estatus = 2
        ////                order by eb.anio,eb.id;";
        ////    return await db.QueryAsync<ExpedienteBibliohemerografico>(sql, new { IdArea = idArea });
        ////}
        ////public async Task<Expediente> GetExpedienteTP(int id)
        ////{
        ////    var db = DbConnection();

        ////    var sql = @"
        ////                select cons.NoProg NoProg, et.id Id, cs.codigo Codigo, id_expediente IdExpediente, nombre Nombre, periodo Periodo, anios_resguardo AniosResguardo, numero_legajos Legajos, numero_fojas Fojas, et.observaciones Observaciones, et.fecha_registro FechaRegistro, id_inventario IdInventario
        ////                    ,cs.vig_doc_val_a VigDocValA, cs.vig_doc_val_l VigDocValL, cs.vig_doc_val_fc VigDocValFC, cs.vig_doc_pla_con_at VigDocPlaConAT, cs.vig_doc_pla_con_ac VigDocPlaConAC, cs.vig_doc_pla_con_tot VigDocPlaConTot, cs.tec_sel_e TecSelE, cs.tec_sel_c TecSelC, cs.tec_sel_m TecSelM
        ////                    ,ca.descripcion Area, et.estatus Estatus
        ////                from prod_control_exp.expediente_transferencia et
        ////                join prod_control_exp.cat_serie_documental cs on et.id_expediente = cs.id
        ////                join prod_control_exp.inventario_transferencia itf on et.id_inventario = itf.id
        ////                join prod_control_exp.cat_areas ca on itf.id_area = ca.id
        ////                join (select ROW_NUMBER() over(order by ets.id) NoProg, ets.id from prod_control_exp.expediente_transferencia ets where ets.id_inventario = (select id_inventario from expediente_transferencia where id = @Id)) cons on et.id = cons.id
        ////                where et.id = @Id";

        ////    return await db.QueryFirstOrDefaultAsync<Expediente>(sql, new { Id = id });
        ////}
        //public async Task<Caratula> GetCaratulaExpedienteTP(int id, int legajo)
        //{
        //    var db = DbConnection();
        //    //var sql = @"
        //    //            select cons.NoProg NoProg, et.id Id, cs.codigo Codigo, id_expediente IdExpediente, nombre Nombre, periodo Periodo, anios_resguardo AniosResguardo, numero_legajos Legajos, numero_fojas Fojas, et.observaciones Observaciones, et.fecha_registro FechaRegistro, id_inventario IdInventario
        //    //                ,cs.vig_doc_val_a VigDocValA, cs.vig_doc_val_l VigDocValL, cs.vig_doc_val_fc VigDocValFC, cs.vig_doc_pla_con_at VigDocPlaConAT, cs.vig_doc_pla_con_ac VigDocPlaConAC, cs.vig_doc_pla_con_tot VigDocPlaConTot, cs.tec_sel_e TecSelE, cs.tec_sel_c TecSelC, cs.tec_sel_m TecSelM
        //    //                ,ca.descripcion Area, et.estatus Estatus
        //    //                ,crt.fojas Fojas, crt.cant_doc_ori DocOriginales, crt.cant_doc_copias DocCopias, crt.cant_cds Cds, crt.tec_sel_doc TecnicasSeleccion, crt.publica Publica, crt.confidencial Confidencial, crt.reservada_sol_info Reservada, crt.descripcion_asunto_expediente DescripcionAsunto, crt.fecha_clasificacion FechaClasificacion, crt.periodo_reserva PeriodoReserva, crt.fundamento_legal FundamentoLegal, crt.ampliacion_periodo_reserva AmpliacionPeriodo, crt.fecha_desclasificacion FechaDesclasificacion, crt.nombre_desclasifica NombreDesclasifica, crt.cargo_desclasifica CargoDesclasifica, crt.partes_reservando PartesReservando, crt.datos_topograficos DatosTopograficos, crt.id_expediente_tp
        //    //            from prod_control_exp.expediente_transferencia et
        //    //            join prod_control_exp.cat_serie_documental cs on et.id_expediente = cs.id
        //    //            join prod_control_exp.inventario_transferencia itf on et.id_inventario = itf.id
        //    //            join prod_control_exp.cat_areas ca on itf.id_area = ca.id
        //    //            join (select ROW_NUMBER() over(order by ets.id) NoProg, ets.id from prod_control_exp.expediente_transferencia ets where ets.id_inventario = (select id_inventario from expediente_transferencia where id = @Id)) cons on et.id = cons.id
        //    //            left join prod_control_exp.caratula crt on et.id = crt.id_expediente_tp
        //    //            where et.id = @Id";
        //    var sql = @"
        //                select cons.NoProg, cons.Consecutivo, et.id Id, cs.codigo Codigo, id_expediente IdExpediente, nombre Nombre, if(year(et.fecha_primero)=year(et.fecha_ultimo),year(et.fecha_primero),concat(year(et.fecha_primero),'-',year(et.fecha_ultimo))) Periodo, anios_resguardo AniosResguardo, et.numero_legajos Legajos, if(et.numero_legajos>1,crt.fojas,ifnull(crt.fojas,et.numero_fojas)) Fojas, et.observaciones Observaciones, et.fecha_registro FechaRegistro, if(et.numero_legajos>1,date_format(crt.fecha_primero,'%Y/%m/%d'),ifnull(date_format(crt.fecha_primero,'%Y/%m/%d'),date_format(et.fecha_primero,'%Y/%m/%d'))) FechaPrimeroAntiguo, if(et.numero_legajos>1,date_format(crt.fecha_ultimo,'%Y/%m/%d'),ifnull(date_format(crt.fecha_ultimo,'%Y/%m/%d'),date_format(et.fecha_ultimo,'%Y/%m/%d'))) FechaUltimoReciente, et.id_inventario_control IdInventario, et.ubicacion DatosTopograficos, et.descripcion DescripcionAsunto, et.tipo_soporte_documental TipoSoporteDocumental
        //                    ,cs.vig_doc_val_a VigDocValA, cs.vig_doc_val_l VigDocValL, cs.vig_doc_val_fc VigDocValFC, cs.vig_doc_pla_con_at VigDocPlaConAT, cs.vig_doc_pla_con_ac VigDocPlaConAC, cs.vig_doc_pla_con_tot VigDocPlaConTot, cs.tec_sel_e TecSelE, cs.tec_sel_c TecSelC, cs.tec_sel_m TecSelM
        //                    ,cp.descripcion Puesto, et.estatus Estatus, ca.descripcion Area
        //                    ,crt.cant_doc_ori DocOriginales, crt.cant_doc_copias DocCopias, crt.cant_cds Cds, crt.tec_sel_doc TecnicasSeleccion, crt.publica Publica, crt.confidencial Confidencial, crt.reservada_sol_info Reservada, date_format(crt.fecha_clasificacion,'%Y/%m/%d') FechaClasificacion, crt.periodo_reserva PeriodoReserva, crt.fundamento_legal FundamentoLegal, crt.ampliacion_periodo_reserva AmpliacionPeriodo, date_format(crt.fecha_desclasificacion,'%Y/%m/%d') FechaDesclasificacion, crt.nombre_desclasifica NombreDesclasifica, crt.cargo_desclasifica CargoDesclasifica, crt.partes_reservando PartesReservando
        //                from prod_control_exp.expediente_control et
        //                join prod_control_exp.cat_serie_documental cs on et.id_expediente = cs.id
        //                join prod_control_exp.inventario_control itf on et.id_inventario_control = itf.id
        //                join prod_control_exp.cat_puestos cp on itf.id_puesto = cp.id
        //                join prod_control_exp.cat_areas ca on cp.id_area = ca.id
        //                join (select ROW_NUMBER() over(order by ets.fecha_ultimo, ets.id) NoProg, ROW_NUMBER() over(partition by year(ets.fecha_ultimo) order by ets.fecha_ultimo, ets.id) Consecutivo, ets.id from prod_control_exp.expediente_control ets where ets.id_inventario_control = (select id_inventario_control from prod_control_exp.expediente_control where id = @Id) AND ets.migrado_tp = 1 ORDER BY ets.fecha_ultimo, ets.id) cons on et.id = cons.id
        //                left join prod_control_exp.caratula crt on et.id = crt.id_expediente_control and crt.legajo = @Legajo
        //                where et.id = @Id";
        //    return await db.QueryFirstOrDefaultAsync<Caratula>(sql, new { Id = id, Legajo = legajo });
        //}
        ////public async Task<bool> DropExpediente(int id)
        ////{
        ////    var db = DbConnection();
        ////    var sql = @"
        ////                delete from prod_control_exp.expediente_transferencia where id = @Id;";
        ////    var result = await db.ExecuteAsync(sql, new { Id = id });
        ////    return result > 0;
        ////}
        //public async Task<int> sePuedeMigrarExpediente(int id, int tipo)
        //{
        //    var db = DbConnection();
        //    var campos = "";
        //    if (tipo == 1) //para inventario transferencia primaria al archivo de concentración
        //        campos = " and ec.anios_resguardo is not null and ec.numero_fojas is not null";
        //    else //para inventario de documentación no expedientable
        //        campos = " and ec.numero_partes is not null and ec.fecha_elaboracion is not null and ec.id_tipo_documental is not null and ec.id_tipo_soporte is not null";
        //    var sql = @"
        //                select id from prod_control_exp.expediente_control ec where id = @Id " + campos + ";";
        //    var result = await db.QueryFirstOrDefaultAsync<int>(sql, new { Id = id });
        //    if (result == 0)
        //    {
        //        sql = @"
        //                UPDATE prod_control_exp.expediente_control SET estatus = 5 WHERE id = @Id;";
        //        _ = db.ExecuteAsync(sql, new { Id = id });
        //    }
        //    return result;
        //}
        //public async Task<bool> MigrarExpedienteInvTP(int id)
        //{
        //    var db = DbConnection();
        //    var sql = @"
        //                call migrarExpedienteInvTP(@Id);";
        //    var result = await db.ExecuteAsync(sql, new { Id = id });
        //    return result > 0;
        //}
        ////public async Task<int> sePuedeMigrarExpedienteInvNE(int id)
        ////{
        ////    var db = DbConnection();
        ////    var sql = @"
        ////                select id from prod_control_exp.expediente_control ec where id = @Id and ec.numero_partes is not null and ec.fecha_elaboracion is not null and ec.id_tipo_documental is not null and ec.id_tipo_soporte is not null;";
        ////    var result = await db.QueryFirstOrDefaultAsync<int>(sql, new { Id = id });
        ////    if (result == 0)
        ////    {
        ////        sql = @"
        ////                UPDATE prod_control_exp.expediente_control SET estatus = 5 WHERE id = @Id;";
        ////        _ = db.ExecuteAsync(sql, new { Id = id });
        ////    }
        ////    return result;
        ////}
        //public async Task<bool> MigrarExpedienteInvNE(int id)
        //{
        //    var db = DbConnection();
        //    var sql = @"
        //                call migrarExpedienteInvNE(@Id);";
        //    var result = await db.ExecuteAsync(sql, new { Id = id });
        //    return result > 0;
        //}
        //public async Task<Inventario> GetInventarioControl(string puesto)
        //{
        //    var db = DbConnection();

        //    //var sql = @"
        //    //            select itr.id Id, itr.id_area IdArea, itr.responsable_archivo_tramite NombreResponsableAT, date_format(itr.fecha_elaboracion, '%Y/%m/%d') FechaElaboracion, date_format(itr.fecha_entrega, '%Y/%m/%d') FechaEntrega, date_format(itr.fecha_transferencia,'%Y/%m/%d') FechaTransferencia
        //    //            from prod_control_exp.inventario_control itr
        //    //            join prod_control_exp.cat_areas ca on itr.id_area = ca.id
        //    //            where ca.descripcion = @Area";
        //    var sql = @"
        //                select itr.id Id, itr.id_puesto IdPuesto, itr.responsable_archivo_tramite NombreResponsableAT, date_format(itr.fecha_elaboracion, '%Y/%m/%d') FechaElaboracion, date_format(itr.fecha_entrega, '%Y/%m/%d') FechaEntrega, date_format(itr.fecha_transferencia,'%Y/%m/%d') FechaTransferencia, itr.ubicacion Ubicacion, itr.peso_electronico PesoElectronico, itr.almacenamiento Almacenamiento
        //                from prod_control_exp.inventario_control itr
        //                join prod_control_exp.cat_puestos cp on itr.id_puesto = cp.id
        //                where cp.descripcion = @Puesto";

        //    return await db.QueryFirstOrDefaultAsync<Inventario>(sql, new { Puesto = puesto });
        //}
        //public async Task<Inventario> GetInventarioControlById(int id)
        //{
        //    var db = DbConnection();

        //    var sql = @"
        //                select itr.id Id, itr.id_puesto IdPuesto, ca.descripcion NombreUnidadAdministrativa, cp.descripcion NombrePuesto, itr.responsable_archivo_tramite NombreResponsableAT, date_format(itr.fecha_elaboracion,'%Y/%m/%d') FechaElaboracion, date_format(itr.fecha_entrega,'%Y/%m/%d') FechaEntrega, date_format(itr.fecha_transferencia,'%Y/%m/%d') FechaTransferencia, itr.ubicacion Ubicacion, itr.peso_electronico PesoElectronico, itr.almacenamiento Almacenamiento
        //                from prod_control_exp.inventario_control itr
        //                join prod_control_exp.cat_puestos cp on itr.id_puesto = cp.id
        //                join prod_control_exp.cat_areas ca on cp.id_area = ca.id
        //                where itr.id = @Id";

        //    return await db.QueryFirstOrDefaultAsync<Inventario>(sql, new { Id = id });
        //}
        //public async Task<bool> InsertInventarioControl(Inventario inventario)
        //{
        //    var db = DbConnection();

        //    var sql = @"
        //                INSERT INTO prod_control_exp.inventario_control
        //                (id_puesto, responsable_archivo_tramite, fecha_elaboracion, fecha_entrega, ubicacion, peso_electronico, almacenamiento)
        //                VALUES (@IdPuesto, @NombreResponsable, @FechaElaboracion, @FechaEntrega, @Ubicacion, @Peso, @Almacenamiento)
        //                ON DUPLICATE KEY UPDATE id_puesto = @IdPuesto, responsable_archivo_tramite = @NombreResponsable, fecha_elaboracion = @FechaElaboracion, fecha_entrega = @FechaEntrega, ubicacion = @Ubicacion, peso_electronico = @Peso, almacenamiento = @Almacenamiento;";

        //    var result = await db.ExecuteAsync(sql, new
        //    {
        //        IdPuesto = inventario.IdPuesto,
        //        NombreResponsable = inventario.NombreResponsableAT,
        //        //FechaElaboracion = inventario.FechaElaboracion.ToString("yyyy-MM-dd"),
        //        FechaElaboracion = inventario.FechaElaboracion,
        //        FechaEntrega = inventario.FechaEntrega,
        //        Ubicacion = inventario.Ubicacion,
        //        Peso = inventario.PesoElectronico,
        //        Almacenamiento = inventario.Almacenamiento
        //    });
        //    return result > 0;
        //}
        //public async Task<bool> InsertExpedienteInventarioControl(Expediente expediente)
        //{
        //    var db = DbConnection();
        //    //(id_expediente, nombre, numero_legajos, fecha_primero, fecha_ultimo, id_inventario_control, id_user)
        //    var sql = @"
        //                INSERT INTO prod_control_exp.expediente_control
        //                (nombre, numero_legajos, numero_fojas, numero_partes, fecha_primero, fecha_ultimo, fecha_elaboracion, ubicacion, descripcion, observaciones, anios_resguardo, id_user, id_expediente, id_inventario_control, id_tipo_documental, id_tipo_soporte, tipo_soporte_documental)
        //                VALUES (@Nombre, @NumeroLegajos, @NumeroFojas, @NumeroPartes, @FechaPrimero, @FechaUltimo, @FechaElaboracion, @Ubicacion, @Descripcion, @Observaciones, @AniosResguardo, @IdUser, @IdExpediente, @IdInventario, @IdTipoDocumental, @IdTipoSoporte, @TipoSoporteDocumental);";

        //    var result = await db.ExecuteAsync(sql, new
        //    {
        //        Nombre = expediente.Nombre,
        //        NumeroLegajos = expediente.Legajos,
        //        NumeroFojas = expediente.Fojas,
        //        NumeroPartes = expediente.NoPartes,
        //        //FechaPrimero = expediente.FechaPrimeroAntiguo.ToString("yyyy-MM-dd"),
        //        FechaPrimero = expediente.FechaPrimeroAntiguo,
        //        FechaUltimo = expediente.FechaUltimoReciente,
        //        FechaElaboracion = expediente.FechaElaboracion,
        //        Ubicacion = expediente.Ubicacion,
        //        Descripcion = expediente.Descripcion,
        //        Observaciones = expediente.Observaciones,
        //        AniosResguardo = expediente.AniosResguardo,
        //        IdUser = expediente.IdUser,
        //        IdExpediente = expediente.IdExpediente,
        //        IdInventario = expediente.IdInventario,
        //        IdTipoDocumental = expediente.IdTipoDocumental,
        //        IdTipoSoporte = expediente.IdTipoSoporte,
        //        TipoSoporteDocumental = expediente.IdTipoSoporteDocumental
        //    });
        //    return result > 0;
        //}
        //public async Task<bool> UpdateExpedienteInventarioControl(Expediente expediente)
        //{
        //    var db = DbConnection();
        //    var sql = @"
        //                UPDATE prod_control_exp.expediente_control set nombre = @Nombre, numero_legajos = @NumeroLegajos, numero_fojas = @NumeroFojas, numero_partes = @NumeroPartes, fecha_primero = @FechaPrimero, fecha_ultimo = @FechaUltimo, fecha_elaboracion = @FechaElaboracion, ubicacion = @Ubicacion, descripcion = @Descripcion, observaciones = @Observaciones, anios_resguardo = @AniosResguardo, id_user = @IdUser, id_expediente = @IdExpediente, id_inventario_control = @IdInventario, id_tipo_documental = @IdTipoDocumental, id_tipo_soporte = @IdTipoSoporte, tipo_soporte_documental = @TipoSoporteDocumental
        //                WHERE id = @Id;";

        //    var result = await db.ExecuteAsync(sql, new
        //    {
        //        Id = expediente.Id,
        //        Nombre = expediente.Nombre,
        //        NumeroLegajos = expediente.Legajos,
        //        NumeroFojas = expediente.Fojas,
        //        NumeroPartes = expediente.NoPartes,
        //        //FechaPrimero = expediente.FechaPrimeroAntiguo.ToString("yyyy-MM-dd"),
        //        FechaPrimero = expediente.FechaPrimeroAntiguo,
        //        FechaUltimo = expediente.FechaUltimoReciente,
        //        FechaElaboracion = expediente.FechaElaboracion,
        //        Ubicacion = expediente.Ubicacion,
        //        Descripcion = expediente.Descripcion,
        //        Observaciones = expediente.Observaciones,
        //        AniosResguardo = expediente.AniosResguardo,
        //        IdUser = expediente.IdUser,
        //        IdExpediente = expediente.IdExpediente,
        //        IdInventario = expediente.IdInventario,
        //        IdTipoDocumental = expediente.IdTipoDocumental,
        //        IdTipoSoporte = expediente.IdTipoSoporte,
        //        TipoSoporteDocumental = expediente.IdTipoSoporteDocumental
        //    });
        //    return result > 0;
        //}
        //public async Task<bool> InsertCaratulaExpedienteIC(Caratula caratula)
        //{
        //    var db = DbConnection();
        //    var sql = @"
        //                INSERT INTO prod_control_exp.caratula
        //                (`cant_doc_ori`,`cant_doc_copias`,`cant_cds`,`tec_sel_doc`,`publica`,`confidencial`,`reservada_sol_info`,`fecha_clasificacion`,`periodo_reserva`,`fundamento_legal`,`ampliacion_periodo_reserva`,`fecha_desclasificacion`,`nombre_desclasifica`,`cargo_desclasifica`,`partes_reservando`,`id_user_captura`,`id_expediente_control`,`fecha_primero`,`fecha_ultimo`,`fojas`,`legajo`)
        //                VALUES (@DocOriginales, @DocCopias, @Cds, @TecnicasSeleccion, @Publica, @Confidencial, @Reservada, @FechaClasificacion, @PeriodoReserva, @FundamentoLegal, @AmpliacionPeriodo, @FechaDesclasificacion, @NombreDesclasifica, @CargoDesclasifica, @PartesReservando, @IdUser, @IdExpediente, @FechaPrimero, @FechaUltimo, @Fojas, @Legajo)
        //                ON DUPLICATE KEY UPDATE `cant_doc_ori` = @DocOriginales,`cant_doc_copias` = @DocCopias,`cant_cds` = @Cds,`tec_sel_doc` = @TecnicasSeleccion,`publica` = @Publica,`confidencial` = @Confidencial,`reservada_sol_info` = @Reservada,`fecha_clasificacion` = @FechaClasificacion,`periodo_reserva` = @PeriodoReserva,`fundamento_legal` = @FundamentoLegal,`ampliacion_periodo_reserva` = @AmpliacionPeriodo,`fecha_desclasificacion` = @FechaDesclasificacion,`nombre_desclasifica` = @NombreDesclasifica,`cargo_desclasifica` = @CargoDesclasifica,`partes_reservando` = @PartesReservando,`id_user_captura` = @IdUser, `fecha_primero` = @FechaPrimero, `fecha_ultimo` = @FechaUltimo, `fojas` = @Fojas, `legajo` = @Legajo;";

        //    var result = await db.ExecuteAsync(sql, new
        //    {
        //        IdExpediente = caratula.IdExpediente,
        //        DocOriginales = caratula.DocOriginales,
        //        DocCopias = caratula.DocCopias,
        //        Cds = caratula.Cds,
        //        TecnicasSeleccion = caratula.TecnicasSeleccion,
        //        Publica = caratula.Publica,
        //        Confidencial = caratula.Confidencial,
        //        Reservada = caratula.Reservada,
        //        //DescripcionAsunto = caratula.DescripcionAsunto,
        //        FechaClasificacion = caratula.FechaClasificacion,
        //        PeriodoReserva = caratula.PeriodoReserva,
        //        FundamentoLegal = caratula.FundamentoLegal,
        //        AmpliacionPeriodo = caratula.AmpliacionPeriodo,
        //        FechaDesclasificacion = caratula.FechaDesclasificacion,
        //        NombreDesclasifica = caratula.NombreDesclasifica,
        //        CargoDesclasifica = caratula.CargoDesclasifica,
        //        PartesReservando = caratula.PartesReservando,
        //        //DatosTopograficos = caratula.DatosTopograficos,
        //        FechaPrimero = caratula.FechaPrimeroAntiguo,
        //        FechaUltimo = caratula.FechaUltimoReciente,
        //        Fojas = caratula.Fojas,
        //        Legajo = caratula.Legajos,
        //        IdUser = caratula.IdUser,
        //    });
        //    return result > 0;
        //}
        //public async Task<IEnumerable<Expediente>> GetExpedientesInventarioControl(int id, int id_inventario)
        //{
        //    var db = DbConnection();
        //    var sql = @"
        //                select ROW_NUMBER() over(order by ec.fecha_ultimo, ec.id) NoProg, ROW_NUMBER() over(partition by year(ec.fecha_ultimo) order by ec.fecha_ultimo, ec.id) Consecutivo, ec.id Id, ec.id_expediente IdExpediente, if(year(ec.fecha_primero)=year(ec.fecha_ultimo),year(ec.fecha_primero),concat(year(ec.fecha_primero),'-',year(ec.fecha_ultimo))) Periodo, ec.nombre Nombre, ec.numero_legajos Legajos, ec.numero_fojas Fojas, ec.observaciones Observaciones, ec.fecha_primero FechaPrimeroAntiguo, ec.fecha_ultimo FechaUltimoReciente, ec.id_inventario_control IdInventario, ec.obs_revalidacion ObservacionesRevalidacion, ec.ubicacion Ubicacion, ec.descripcion Descripcion, ec.tipo_soporte_documental TipoSoporteDocumental
        //                    ,if(ec.id_user = @Id, 'editable', 'noeditable') EsEditable, ec.estatus Estatus
        //                    -- ,if(ec.migrado_tp = 1, ec.migrado_tp, if(ec.anios_resguardo is not null and ec.numero_fojas is not null, 0, 1)) MigradoTP
        //                    ,ec.migrado_tp MigradoTP, ec.migrado_ne MigradoNE
        //                    -- ,if(ec.migrado_ne = 1, ec.migrado_ne, if(ec.numero_partes is not null and ec.fecha_elaboracion is not null and ec.id_tipo_documental is not null and ec.id_tipo_soporte is not null, 0, 1)) MigradoNE
        //                    ,csd.codigo Codigo, csd.vig_doc_val_a VigDocValA, csd.vig_doc_val_l VigDocValL, csd.vig_doc_val_fc VigDocValFC, csd.vig_doc_pla_con_at VigDocPlaConAT, csd.vig_doc_pla_con_ac VigDocPlaConAC, csd.vig_doc_pla_con_tot VigDocPlaConTot
        //                    ,cds.no_cds CDs
        //                from prod_control_exp.expediente_control ec
        //                join prod_control_exp.cat_serie_documental csd on ec.id_expediente = csd.id
        //                left join (select coalesce(sum(cant_cds),0) no_cds, id_expediente_control from prod_control_exp.caratula group by id_expediente_control) cds on ec.id = cds.id_expediente_control
        //                where ec.id_inventario_control = @IdInv AND ec.migrado_tp = 0 AND ec.migrado_ne = 0
        //                order by ec.fecha_ultimo, ec.id;";
        //    return await db.QueryAsync<Expediente>(sql, new { Id = id, IdInv = id_inventario });
        //}
        //public async Task<IEnumerable<Expediente>> GetExpedientesInventarioControlByIdInv(int id)
        //{
        //    var db = DbConnection();
        //    var sql = @"
        //                select ROW_NUMBER() over(order by ec.fecha_ultimo, ec.id) NoProg, ROW_NUMBER() over(partition by year(ec.fecha_ultimo) order by ec.fecha_ultimo, ec.id) Consecutivo, ec.id Id, ec.id_expediente IdExpediente, csd.codigo Codigo, if(year(ec.fecha_primero)=year(ec.fecha_ultimo),year(ec.fecha_primero),concat(year(ec.fecha_primero),'-',year(ec.fecha_ultimo))) Periodo, ec.nombre Nombre, ec.numero_legajos Legajos, ec.fecha_primero FechaPrimeroAntiguo
        //                    ,ec.fecha_ultimo FechaUltimoReciente, ec.id_inventario_control IdInventario, ec.obs_revalidacion ObservacionesRevalidacion, ec.estatus Estatus
        //                    ,if(ec.migrado_tp = 1, ec.migrado_tp, if(ec.anios_resguardo is not null and ec.numero_fojas is not null, 0, 1)) MigradoTP
        //                    ,if(ec.migrado_ne = 1, ec.migrado_ne, if(ec.numero_partes is not null and ec.fecha_elaboracion is not null and ec.id_tipo_documental is not null and ec.id_tipo_soporte is not null, 0, 1)) MigradoNE
        //                from prod_control_exp.expediente_control ec
        //                join prod_control_exp.cat_serie_documental csd on ec.id_expediente = csd.id
        //                where ec.id_inventario_control = @IdInv AND ec.migrado_tp = 0 AND ec.migrado_ne = 0
        //                order by ec.fecha_ultimo, ec.id;";
        //    return await db.QueryAsync<Expediente>(sql, new { IdInv = id });
        //}
        //public async Task<IEnumerable<Expediente>> GetExpedientesValidacionInventarioControl(int idPuesto)
        //{
        //    var db = DbConnection();
        //    var sql = @"
        //                select ROW_NUMBER() over(order by ec.fecha_ultimo, ec.id) NoProg, cons.Consecutivo, ec.id Id, ec.id_expediente IdExpediente, csd.codigo Codigo, if(year(ec.fecha_primero)=year(ec.fecha_ultimo),year(ec.fecha_primero),concat(year(ec.fecha_primero),'-',year(ec.fecha_ultimo))) Periodo, ec.nombre Nombre, ec.numero_legajos Legajos, ec.fecha_primero FechaPrimeroAntiguo, ec.fecha_ultimo FechaUltimoReciente, ec.id_inventario_control IdInventario, ec.estatus Estatus
        //                from prod_control_exp.expediente_control ec
        //                join prod_control_exp.inventario_control ic on ec.id_inventario_control = ic.id
        //                join prod_control_exp.cat_serie_documental csd on ec.id_expediente = csd.id
        //                join (select ROW_NUMBER() over(partition by year(ets.fecha_ultimo) order by ets.fecha_ultimo, ets.id) Consecutivo, ets.id from prod_control_exp.expediente_control ets join prod_control_exp.inventario_control ic2 on ets.id_inventario_control = ic2.id and ic2.id_puesto = @IdPuesto WHERE ets.migrado_tp = 0 AND ets.migrado_ne = 0 ORDER BY ets.fecha_ultimo, ets.id) cons on ec.id = cons.id
        //                where ic.id_puesto = @IdPuesto and ec.estatus = 2 AND ec.migrado_tp = 0 and ec.migrado_ne = 0
        //                order by ec.fecha_ultimo, ec.id;";
        //    return await db.QueryAsync<Expediente>(sql, new { IdPuesto = idPuesto });
        //}
        //public async Task<Expediente> GetExpedienteControl(int id)
        //{
        //    var db = DbConnection();

        //    var sql = @"
        //                select cons.NoProg, cons.Consecutivo, ec.id Id, cs.codigo Codigo, if(year(ec.fecha_primero)=year(ec.fecha_ultimo),year(ec.fecha_primero),concat(year(ec.fecha_primero),'-',year(ec.fecha_ultimo))) Periodo, ec.id_expediente IdExpediente, nombre Nombre, ec.descripcion Descripcion, ec.observaciones Observaciones, ec.numero_fojas Fojas, ec.numero_legajos Legajos, date_format(ec.fecha_primero,'%Y/%m/%d') FechaPrimeroAntiguo, date_format(ec.fecha_ultimo,'%Y/%m/%d') FechaUltimoReciente, ec.anios_resguardo AniosResguardo, ec.id_tipo_documental IdTipoDocumental, ec.id_tipo_soporte IdTipoSoporte, ec.numero_partes NoPartes, date_format(ec.fecha_elaboracion,'%Y/%m/%d') FechaElaboracion, id_inventario_control IdInventario, ec.tipo_soporte_documental TipoSoporteDocumental, ec.ubicacion Ubicacion
        //                    ,cs.vig_doc_val_a VigDocValA, cs.vig_doc_val_l VigDocValL, cs.vig_doc_val_fc VigDocValFC, cs.vig_doc_pla_con_at VigDocPlaConAT, cs.vig_doc_pla_con_ac VigDocPlaConAC, cs.vig_doc_pla_con_tot VigDocPlaConTot, cs.tec_sel_e TecSelE, cs.tec_sel_c TecSelC, cs.tec_sel_m TecSelM
        //                    ,cp.descripcion Area, ec.estatus Estatus
        //                from prod_control_exp.expediente_control ec
        //                join prod_control_exp.cat_serie_documental cs on ec.id_expediente = cs.id
        //                join prod_control_exp.inventario_control itf on ec.id_inventario_control = itf.id
        //                join prod_control_exp.cat_puestos cp on itf.id_puesto = cp.id
        //                join (select ROW_NUMBER() over(order by ets.fecha_ultimo, ets.id) NoProg, ROW_NUMBER() over(partition by year(ets.fecha_ultimo) order by ets.fecha_ultimo, ets.id) Consecutivo, ets.id from prod_control_exp.expediente_control ets WHERE ets.id_inventario_control = (select id_inventario_control from prod_control_exp.expediente_control where id = @Id) AND ets.migrado_tp = 0 AND ets.migrado_ne = 0 ORDER BY ets.fecha_ultimo, ets.id) cons on ec.id = cons.id
        //                where ec.id = @Id";

        //    return await db.QueryFirstOrDefaultAsync<Expediente>(sql, new { Id = id });
        //}
        //public async Task<Caratula> GetCaratulaExpedienteControl(int id, int legajo, int idUser)
        //{
        //    var db = DbConnection();
        //    // crt.descripcion_asunto_expediente DescripcionAsunto,
        //    var sql = @"
        //                select cons.NoProg, cons.Consecutivo, ec.id Id, cs.codigo Codigo, if(year(ec.fecha_primero)=year(ec.fecha_ultimo),year(ec.fecha_primero),concat(year(ec.fecha_primero),'-',year(ec.fecha_ultimo))) Periodo, ec.id_expediente IdExpediente, ec.nombre Nombre, if(ec.numero_legajos>1,crt.fojas,ifnull(crt.fojas,ec.numero_fojas)) Fojas, ec.numero_legajos Legajos, if(ec.numero_legajos>1,date_format(crt.fecha_primero,'%Y/%m/%d'),ifnull(date_format(crt.fecha_primero,'%Y/%m/%d'),date_format(ec.fecha_primero,'%Y/%m/%d'))) FechaPrimeroAntiguo, if(ec.numero_legajos>1,date_format(crt.fecha_ultimo,'%Y/%m/%d'),ifnull(date_format(crt.fecha_ultimo,'%Y/%m/%d'),date_format(ec.fecha_ultimo,'%Y/%m/%d'))) FechaUltimoReciente, ec.id_inventario_control IdInventario, ec.estatus Estatus, ec.descripcion DescripcionAsunto, ec.observaciones Observaciones, ec.tipo_soporte_documental TipoSoporteDocumental, ec.ubicacion DatosTopograficos
        //                    ,if(ec.id_user = @IdUser, 'editable', 'noeditable') EsEditable
        //                    ,cs.vig_doc_val_a VigDocValA, cs.vig_doc_val_l VigDocValL, cs.vig_doc_val_fc VigDocValFC, cs.vig_doc_pla_con_at VigDocPlaConAT, cs.vig_doc_pla_con_ac VigDocPlaConAC, cs.vig_doc_pla_con_tot VigDocPlaConTot, cs.tec_sel_e TecSelE, cs.tec_sel_c TecSelC, cs.tec_sel_m TecSelM
        //                    ,cp.descripcion Puesto
        //                    ,ca.descripcion Area
        //                    ,crt.cant_doc_ori DocOriginales, crt.cant_doc_copias DocCopias, crt.cant_cds Cds, crt.tec_sel_doc TecnicasSeleccion, crt.publica Publica, crt.confidencial Confidencial, crt.reservada_sol_info Reservada, date_format(crt.fecha_clasificacion,'%Y/%m/%d') FechaClasificacion, crt.periodo_reserva PeriodoReserva, crt.fundamento_legal FundamentoLegal, crt.ampliacion_periodo_reserva AmpliacionPeriodo, date_format(crt.fecha_desclasificacion,'%Y/%m/%d') FechaDesclasificacion, crt.nombre_desclasifica NombreDesclasifica, crt.cargo_desclasifica CargoDesclasifica, crt.partes_reservando PartesReservando, crt.id_expediente_control
        //                    ,concat(u.nombre,' ',u.primer_apellido,' ',u.segundo_apellido) UserName
        //                from prod_control_exp.expediente_control ec
        //                join prod_control_exp.cat_serie_documental cs on ec.id_expediente = cs.id
        //                join prod_control_exp.inventario_control itf on ec.id_inventario_control = itf.id
        //                join prod_control_exp.cat_puestos cp on itf.id_puesto = cp.id
        //                join prod_control_exp.cat_areas ca on cp.id_area = ca.id
        //                join (select ROW_NUMBER() over(order by ets.fecha_ultimo, ets.id) NoProg, ROW_NUMBER() over(partition by year(ets.fecha_ultimo) order by ets.fecha_ultimo, ets.id) Consecutivo, ets.id from prod_control_exp.expediente_control ets where ets.id_inventario_control = (select id_inventario_control from prod_control_exp.expediente_control where id = @Id) AND ets.migrado_tp = 0 AND ets.migrado_ne = 0 ORDER BY ets.fecha_ultimo, ets.id) cons on ec.id = cons.id
        //                left join prod_control_exp.caratula crt on ec.id = crt.id_expediente_control and legajo = @Legajo
        //                left join qa_adms_conavi.usuario u on crt.id_user_captura = u.id
        //                where ec.id = @Id";

        //    return await db.QueryFirstOrDefaultAsync<Caratula>(sql, new { Id = id, Legajo = legajo, IdUser = idUser });
        //}
        //public async Task<bool> DropExpedienteControl(int id)
        //{
        //    var db = DbConnection();
        //    var sql = @"
        //                delete ec,cc from prod_control_exp.expediente_control ec left join prod_control_exp.caratula cc on ec.id = cc.id_expediente_control where ec.id = @Id;";
        //    var result = await db.ExecuteAsync(sql, new { Id = id });
        //    return result > 0;
        //}
        //public async Task<bool> SendValExpedienteControl(int id)
        //{
        //    var db = DbConnection();
        //    var sql = @"
        //                update prod_control_exp.expediente_control set estatus = 2 where id = @Id;";
        //    var result = await db.ExecuteAsync(sql, new { Id = id });
        //    return result > 0;
        //}
        //public async Task<bool> VoBoExpedienteControl(int id)
        //{
        //    var db = DbConnection();
        //    var sql = @"
        //                update prod_control_exp.expediente_control set estatus = 3 where id = @Id;";
        //    var result = await db.ExecuteAsync(sql, new { Id = id });
        //    return result > 0;
        //}
        //public async Task<bool> RevalidacionExpedienteControl(int id, string obs)
        //{
        //    var db = DbConnection();
        //    var sql = @"
        //                update prod_control_exp.expediente_control set estatus = 4, obs_revalidacion = @Obs where id = @Id;";
        //    var result = await db.ExecuteAsync(sql, new { Id = id, Obs = obs });
        //    return result > 0;
        //}
        //public async Task<Inventario> GetInventarioBibliohemerografico(string puesto)
        //{
        //    var db = DbConnection();

        //    var sql = @"
        //                select itr.id Id, itr.id_puesto IdPuesto, itr.nombre_responsable NombreResponsableAT, date_format(itr.fecha_transferencia,'%Y/%m/%d') FechaTransferencia, date_format(itr.fecha_elaboracion,'%Y/%m/%d') FechaElaboracion
        //                from prod_control_exp.inventario_bibliohemerografico itr
        //                join prod_control_exp.cat_puestos cp on itr.id_puesto = cp.id
        //                where cp.descripcion = @Puesto";

        //    return await db.QueryFirstOrDefaultAsync<Inventario>(sql, new { Puesto = puesto });
        //}
        //public async Task<Inventario> GetInventarioBiblioById(int id)
        //{
        //    var db = DbConnection();

        //    var sql = @"
        //                select itr.id Id, itr.id_puesto IdPuesto, cp.descripcion NombreUnidadAdministrativa, itr.nombre_responsable NombreResponsableAT, date_format(itr.fecha_transferencia,'%Y/%m/%d') FechaTransferencia, date_format(itr.fecha_elaboracion,'%Y/%m/%d') FechaElaboracion
        //                from prod_control_exp.inventario_bibliohemerografico itr
        //                join prod_control_exp.cat_puestos cp on itr.id_puesto = cp.id
        //                where itr.id = @Id";

        //    return await db.QueryFirstOrDefaultAsync<Inventario>(sql, new { Id = id });
        //}
        //public async Task<bool> InsertInventarioBibliohemerografico(Inventario inventario)
        //{
        //    var db = DbConnection();

        //    var sql = @"
        //                INSERT INTO prod_control_exp.inventario_bibliohemerografico
        //                (id_puesto, nombre_responsable, fecha_transferencia, fecha_elaboracion)
        //                VALUES (@IdPuesto, @NombreResponsable, @FechaTransferencia, @FechaElaboracion)
        //                ON DUPLICATE KEY UPDATE nombre_responsable = @NombreResponsable, fecha_transferencia = @FechaTransferencia, fecha_elaboracion = @FechaElaboracion;";

        //    var result = await db.ExecuteAsync(sql, new
        //    {
        //        IdPuesto = inventario.IdPuesto,
        //        NombreResponsable = inventario.NombreResponsableAT,
        //        //FechaTransferencia = inventario.FechaTransferencia?.ToString("yyyy-MM-dd"),
        //        FechaTransferencia = inventario.FechaTransferencia,
        //        FechaElaboracion = inventario.FechaElaboracion
        //    });
        //    return result > 0;
        //}
        //public async Task<bool> InsertExpedienteBibliohemerografico(ExpedienteBibliohemerografico expediente)
        //{

        //    var db = DbConnection();

        //    var sql = @"
        //                INSERT INTO prod_control_exp.expediente_bibliohemerografico
        //                (numero_ejemplar, id_tipo_soporte, titulo_del_libro, nombre_autor, tema, editorial, anio, isbn_issn, numero_paginas, numero_volumen, id_inventario_bibliohemerografico, id_user)
        //                VALUES (@Ejemplar, @IdSoporte, @Titulo, @Autor, @Tema, @Editorial, @Anio, @Isbn, @Paginas, @Volumen, @IdInventario, @IdUser);";

        //    var result = await db.ExecuteAsync(sql, new
        //    {
        //        Ejemplar = expediente.Ejemplar,
        //        IdSoporte = expediente.IdTipoSoporte,
        //        Titulo = expediente.Titulo,
        //        Autor = expediente.Autor,
        //        Tema = expediente.Tema,
        //        Editorial = expediente.Editorial,
        //        Anio = expediente.Anio,
        //        Isbn = expediente.IsbnIssn,
        //        Paginas = expediente.Paginas,
        //        Volumen = expediente.Volumen,
        //        IdInventario = expediente.IdInventario,
        //        IdUser = expediente.IdUser
        //    });
        //    return result > 0;
        //}
        //public async Task<bool> UpdateExpedienteBibliohemerografico(ExpedienteBibliohemerografico expediente)
        //{

        //    var db = DbConnection();

        //    var sql = @"
        //                UPDATE prod_control_exp.expediente_bibliohemerografico SET numero_ejemplar = @Ejemplar, id_tipo_soporte = @IdSoporte, titulo_del_libro = @Titulo, nombre_autor = @Autor, tema = @Tema, editorial = @Editorial, anio = @Anio, isbn_issn = @Isbn, numero_paginas = @Paginas, numero_volumen = @Volumen, id_user = @IdUser
        //                WHERE id = @Id;";

        //    var result = await db.ExecuteAsync(sql, new
        //    {
        //        Id = expediente.Id,
        //        Ejemplar = expediente.Ejemplar,
        //        IdSoporte = expediente.IdTipoSoporte,
        //        Titulo = expediente.Titulo,
        //        Autor = expediente.Autor,
        //        Tema = expediente.Tema,
        //        Editorial = expediente.Editorial,
        //        Anio = expediente.Anio,
        //        Isbn = expediente.IsbnIssn,
        //        Paginas = expediente.Paginas,
        //        Volumen = expediente.Volumen,
        //        //IdInventario = expediente.IdInventario,
        //        IdUser = expediente.IdUser
        //    });
        //    return result > 0;
        //}
        //public async Task<IEnumerable<ExpedienteBibliohemerografico>> GetExpedientesBibliohemerograficos(int id, int id_inventario)
        //{
        //    var db = DbConnection();
        //    var sql = @"
        //                select ROW_NUMBER() over(order by eb.anio,eb.id) NoProg, ROW_NUMBER() over(partition by eb.anio order by eb.anio,eb.id) Consecutivo, eb.id Id, eb.numero_ejemplar Ejemplar, eb.id_tipo_soporte IdTipoSoporte
        //                    ,cts.clave ClaveSoporte, cts.descripcion Soporte,eb.titulo_del_libro Titulo, eb.nombre_autor Autor, eb.tema Tema
        //                    ,eb.editorial Editorial, eb.anio Anio,eb.isbn_issn IsbnIssn,eb.numero_paginas Paginas, eb.numero_volumen Volumen
        //                    ,eb.fecha_registro FechaRegistro,eb.id_inventario_bibliohemerografico IdInventario, if(eb.id_user = @Id, 'editable', 'noeditable') EsEditable
        //                    ,eb.estatus Estatus
        //                from prod_control_exp.expediente_bibliohemerografico eb
        //                join prod_control_exp.cat_tipo_soporte cts on eb.id_tipo_soporte = cts.id
        //                where eb.id_inventario_bibliohemerografico = @IdInv
        //                order by eb.anio,eb.id;";
        //    return await db.QueryAsync<ExpedienteBibliohemerografico>(sql, new { Id = id, IdInv = id_inventario });
        //}
        //public async Task<IEnumerable<ExpedienteBibliohemerografico>> GetExpedientesBibliohemerograficosByIdInv(int id)
        //{
        //    var db = DbConnection();
        //    var sql = @"
        //                select ROW_NUMBER() over(order by eb.anio,eb.id) NoProg, ROW_NUMBER() over(partition by eb.anio order by eb.anio,eb.id) Consecutivo, eb.id Id, eb.numero_ejemplar Ejemplar, eb.id_tipo_soporte IdTipoSoporte
        //                    ,cts.clave ClaveSoporte, cts.descripcion Soporte,eb.titulo_del_libro Titulo, eb.nombre_autor Autor, eb.tema Tema
        //                    ,eb.editorial Editorial, eb.anio Anio,eb.isbn_issn IsbnIssn,eb.numero_paginas Paginas, eb.numero_volumen Volumen
        //                    ,eb.fecha_registro FechaRegistro,eb.id_inventario_bibliohemerografico IdInventario
        //                    ,eb.estatus Estatus
        //                from prod_control_exp.expediente_bibliohemerografico eb
        //                join prod_control_exp.cat_tipo_soporte cts on eb.id_tipo_soporte = cts.id
        //                where eb.id_inventario_bibliohemerografico = @IdInv
        //                order by eb.anio,eb.id;";
        //    return await db.QueryAsync<ExpedienteBibliohemerografico>(sql, new { IdInv = id });
        //}
        //public async Task<ExpedienteBibliohemerografico> GetExpedienteBibliohemerografico(int id)
        //{
        //    var db = DbConnection();
        //    var sql = @"
        //                select cons.NoProg, cons.Consecutivo, eb.id Id, eb.numero_ejemplar Ejemplar, eb.id_tipo_soporte IdTipoSoporte, eb.titulo_del_libro Titulo, eb.nombre_autor Autor
        //                    ,eb.tema Tema, eb.editorial Editorial, eb.anio Anio,eb.isbn_issn IsbnIssn,eb.numero_paginas Paginas, eb.numero_volumen Volumen
        //                    ,eb.fecha_registro FechaRegistro,eb.id_inventario_bibliohemerografico IdInventario, if(eb.id_user = @Id, 'editable', 'noeditable') EsEditable
        //                    ,eb.estatus Estatus, cts.clave ClaveSoporte, cts.descripcion Soporte
        //                from prod_control_exp.expediente_bibliohemerografico eb
        //                join prod_control_exp.cat_tipo_soporte cts on eb.id_tipo_soporte = cts.id
        //                join (select ROW_NUMBER() over(order by eb.anio, eb.id) NoProg, ROW_NUMBER() over(partition by eb.anio order by eb.anio, eb.id) Consecutivo, eb.id from prod_control_exp.expediente_bibliohemerografico eb where eb.id_inventario_bibliohemerografico = (select id_inventario_bibliohemerografico from prod_control_exp.expediente_bibliohemerografico where id = @Id) ORDER BY eb.anio, eb.id) cons on eb.id = cons.id
        //                where eb.id = @Id;";
        //    return await db.QueryFirstOrDefaultAsync<ExpedienteBibliohemerografico>(sql, new { Id = id });
        //}
        ////public async Task<bool> SendValExpedienteBiblio(int id)
        ////{
        ////    var db = DbConnection();
        ////    var sql = @"
        ////                update prod_control_exp.expediente_bibliohemerografico set estatus = 2 where id = @Id;";
        ////    var result = await db.ExecuteAsync(sql, new { Id = id });
        ////    return result > 0;
        ////}

        //public async Task<bool> DropExpedienteBibliohemerografico(int id)
        //{
        //    var db = DbConnection();
        //    var sql = @"
        //                delete from prod_control_exp.expediente_bibliohemerografico where id = @Id;";
        //    var result = await db.ExecuteAsync(sql, new { Id = id });
        //    return result > 0;
        //}
        ////public async Task<bool> VoBoExpedienteBiblio(int id)
        ////{
        ////    var db = DbConnection();
        ////    var sql = @"
        ////                update prod_control_exp.expediente_bibliohemerografico set estatus = 3 where id = @Id;";
        ////    var result = await db.ExecuteAsync(sql, new { Id = id });
        ////    return result > 0;
        ////}
        ////public async Task<bool> RevalidacionExpedienteBiblio(int id)
        ////{
        ////    var db = DbConnection();
        ////    var sql = @"
        ////                update prod_control_exp.expediente_bibliohemerografico set estatus = 4 where id = @Id;";
        ////    var result = await db.ExecuteAsync(sql, new { Id = id });
        ////    return result > 0;
        ////}
        ////public async Task<Inventario> GetInventarioNoExpedientable(string area)
        ////{
        ////    var db = DbConnection();

        ////    var sql = @"
        ////                select itr.id Id, itr.id_area IdArea, itr.nombre_responsable NombreResponsableAT, itr.fecha_elaboracion FechaElaboracion, itr.fecha_transferencia FechaTransferencia
        ////                from prod_control_exp.inventario_noexpedientable itr
        ////                join prod_control_exp.cat_areas ca on itr.id_area = ca.id
        ////                where ca.descripcion = @Area";

        ////    return await db.QueryFirstOrDefaultAsync<Inventario>(sql, new { Area = area });
        ////}
        //public async Task<bool> InsertInventarioNoExpedientable(Inventario inventario)
        //{
        //    var db = DbConnection();

        //    //var sql = @"
        //    //            INSERT INTO prod_control_exp.inventario_noexpedientable
        //    //            (id_area, nombre_responsable, fecha_elaboracion, fecha_transferencia)
        //    //            VALUES (@IdArea, @NombreResponsable, @FechaElaboracion, @FechaTransferencia)
        //    //            ON DUPLICATE KEY UPDATE nombre_responsable = @NombreResponsable, fecha_elaboracion = @FechaElaboracion, fecha_transferencia = @FechaTransferencia;";
        //    var sql = @"
        //                UPDATE prod_control_exp.inventario_control SET fecha_transferencia = @FechaTransferencia WHERE id_puesto = @IdPuesto;";

        //    var result = await db.ExecuteAsync(sql, new
        //    {
        //        IdPuesto = inventario.IdPuesto,
        //        //NombreResponsable = inventario.NombreResponsableAT,
        //        //FechaElaboracion = inventario.FechaElaboracion,
        //        //FechaTransferencia = inventario.FechaTransferencia?.ToString("yyyy-MM-dd"),
        //        FechaTransferencia = inventario.FechaTransferencia,
        //    });
        //    return result > 0;
        //}
        ////public async Task<bool> InsertExpedienteNoExpedientable(ExpedienteNoExpedientable expediente)
        ////{
        ////    var db = DbConnection();

        ////    var sql = @"
        ////                INSERT INTO prod_control_exp.expediente_no_expedientable
        ////                (id_tipo_documental, id_tipo_soporte, id_clave_interna, numero_partes, fecha_elaboracion, titulo_expediente, observaciones, id_inventario_no_expedientable, id_user)
        ////                VALUES (@IdTipoDocumental, @IdSoporte, @IdClaveInterna, @Partes, @FechaElaboracion, @Titulo, @Observaciones, @IdInventario, @IdUser);";

        ////    var result = await db.ExecuteAsync(sql, new
        ////    {
        ////        IdTipoDocumental = expediente.IdTipoDocumental,
        ////        IdSoporte = expediente.IdTipoSoporte,
        ////        IdClaveInterna = expediente.IdClaveInterna,
        ////        Partes = expediente.Partes,
        ////        FechaElaboracion = expediente.FechaElaboracion,
        ////        Titulo = expediente.Titulo,
        ////        Observaciones = expediente.Observaciones,
        ////        IdInventario = expediente.IdInventario,
        ////        IdUser = expediente.IdUser
        ////    });
        ////    return result > 0;
        ////}
        //public async Task<IEnumerable<Expediente>> GetExpedientesNoExpedientables(int id, int id_inventario)
        //{
        //    var db = DbConnection();
        //    //var sql = @"
        //    //            select ROW_NUMBER() over(order by en.id) NoProg, en.id Id, cts.clave Clave, en.id_tipo_soporte IdTipoSoporte, en.id_tipo_documental IdTipoDocumental
        //    //                ,cts.descripcion Soporte,en.id_clave_interna IdClaveInterna, csd.codigo ClaveInterna, en.titulo_expediente Titulo, en.numero_partes Partes
        //    //                ,en.observaciones Observaciones, en.fecha_elaboracion FechaElaboracion, en.fecha_registro FechaRegistro
        //    //                ,en.id_inventario_no_expedientable IdInventario,if(en.id_user = @Id, 'editable', 'noeditable') EsEditable
        //    //            from prod_control_exp.expediente_no_expedientable en
        //    //            join prod_control_exp.cat_serie_documental csd on en.id_clave_interna = csd.id
        //    //            left join prod_control_exp.cat_tipo_soporte cts on en.id_tipo_soporte = cts.id
        //    //            where en.id_inventario_no_expedientable = @IdInv
        //    //            order by en.id;";
        //    var sql = @"
        //                select ROW_NUMBER() over(order by ec.fecha_ultimo, ec.id) NoProg, ROW_NUMBER() over(partition by year(ec.fecha_ultimo) order by ec.fecha_ultimo, ec.id) Consecutivo, ec.id Id, cts.clave Codigo, ec.id_tipo_soporte IdTipoSoporte, ec.id_tipo_documental IdTipoDocumental
        //                 ,cts.descripcion Soporte, ec.id_expediente IdClaveInterna, csd.codigo Clave, ec.nombre Nombre, ec.numero_partes NoPartes
        //                 ,ec.observaciones Observaciones, ec.fecha_elaboracion FechaElaboracion, ec.fecha_registro FechaRegistro
        //                 ,ec.id_inventario_control IdInventario, if(ec.id_user = @Id, 'editable', 'noeditable') EsEditable
        //                    ,ec.estatus Estatus
        //                    ,if(year(ec.fecha_primero)=year(ec.fecha_ultimo),year(ec.fecha_primero),concat(year(ec.fecha_primero),'-',year(ec.fecha_ultimo))) Periodo
        //                from prod_control_exp.expediente_control ec
        //                join prod_control_exp.cat_serie_documental csd on ec.id_expediente = csd.id
        //                left join prod_control_exp.cat_tipo_soporte cts on ec.id_tipo_soporte = cts.id
        //                where ec.id_inventario_control = @IdInv and ec.migrado_ne = 1
        //                order by ec.fecha_ultimo, ec.id;";
        //    return await db.QueryAsync<Expediente>(sql, new { Id = id, IdInv = id_inventario });
        //}
        //public async Task<IEnumerable<Expediente>> GetExpedientesNoExpedientablesByIdInv(int id)
        //{
        //    var db = DbConnection();
        //    var sql = @"
        //                select ROW_NUMBER() over(order by ec.fecha_ultimo, ec.id) NoProg, ROW_NUMBER() over(partition by year(ec.fecha_ultimo) order by ec.fecha_ultimo, ec.id) Consecutivo, ec.id Id, cts.clave Codigo, ec.id_tipo_soporte IdTipoSoporte, ec.id_tipo_documental IdTipoDocumental
        //                 ,cts.descripcion Soporte, ec.id_expediente IdClaveInterna, csd.codigo Clave, ec.nombre Nombre, ec.numero_partes NoPartes
        //                 ,ec.observaciones Observaciones, ec.fecha_elaboracion FechaElaboracion, ec.fecha_registro FechaRegistro, ec.id_inventario_control IdInventario
        //                    ,ec.estatus Estatus
        //                    ,if(year(ec.fecha_primero)=year(ec.fecha_ultimo),year(ec.fecha_primero),concat(year(ec.fecha_primero),'-',year(ec.fecha_ultimo))) Periodo
        //                from prod_control_exp.expediente_control ec
        //                join prod_control_exp.cat_serie_documental csd on ec.id_expediente = csd.id
        //                left join prod_control_exp.cat_tipo_soporte cts on ec.id_tipo_soporte = cts.id
        //                where ec.id_inventario_control = @IdInv and ec.migrado_ne = 1
        //                order by ec.fecha_ultimo, ec.id;";
        //    return await db.QueryAsync<Expediente>(sql, new { IdInv = id });
        //}
        //public async Task<Expediente> GetNoExpedientable(int id)
        //{
        //    var db = DbConnection();

        //    var sql = @"
        //                select cons.NoProg, cons.Consecutivo, ec.id Id, cs.codigo Codigo, year(ec.fecha_elaboracion) Periodo, ec.id IdExpediente, ec.titulo_expediente Nombre, date_format(ec.fecha_elaboracion,'%Y/%m/%d') FechaElaboracion, ec.id_inventario_no_expedientable IdInventario
        //                 ,cs.vig_doc_val_a VigDocValA, cs.vig_doc_val_l VigDocValL, cs.vig_doc_val_fc VigDocValFC, cs.vig_doc_pla_con_at VigDocPlaConAT, cs.vig_doc_pla_con_ac VigDocPlaConAC, cs.vig_doc_pla_con_tot VigDocPlaConTot, cs.tec_sel_e TecSelE, cs.tec_sel_c TecSelC, cs.tec_sel_m TecSelM
        //                 ,cp.descripcion Area
        //                from prod_control_exp.expediente_no_expedientable ec
        //                join prod_control_exp.cat_serie_documental cs on ec.id = cs.id
        //                join prod_control_exp.inventario_noexpedientable itf on ec.id_inventario_no_expedientable = itf.id
        //                join prod_control_exp.cat_puestos cp on itf.id_puesto = cp.id
        //                join (select ROW_NUMBER() over(order by ets.fecha_ultimo, ets.id) NoProg, ROW_NUMBER() over(partition by year(ets.fecha_ultimo) order by ets.fecha_ultimo, ets.id) Consecutivo, ets.id from prod_control_exp.expediente_no_expedientable ets where ets.id_inventario_no_expedientable = (select id_inventario_no_expedientable from expediente_no_expedientable where id = @Id) ORDER BY ets.fecha_ultimo, ets.id) cons on ec.id = cons.id
        //                where ec.id = @Id";

        //    return await db.QueryFirstOrDefaultAsync<Expediente>(sql, new { Id = id });
        //}
        //public async Task<Caratula> GetCaratulaNoExpedientable(int id, int legajo)
        //{
        //    var db = DbConnection();

        //    //var sql = @"
        //    //            select cons.NoProg NoProg, ec.id Id, cs.codigo Codigo, year(ec.fecha_elaboracion) Periodo, ec.id IdExpediente, ec.titulo_expediente Nombre, ec.fecha_elaboracion FechaElaboracion, ec.id_inventario_no_expedientable IdInventario
        //    //             ,cs.vig_doc_val_a VigDocValA, cs.vig_doc_val_l VigDocValL, cs.vig_doc_val_fc VigDocValFC, cs.vig_doc_pla_con_at VigDocPlaConAT, cs.vig_doc_pla_con_ac VigDocPlaConAC, cs.vig_doc_pla_con_tot VigDocPlaConTot, cs.tec_sel_e TecSelE, cs.tec_sel_c TecSelC, cs.tec_sel_m TecSelM
        //    //             ,ca.descripcion Area
        //    //                ,crt.fojas Fojas, crt.cant_doc_ori DocOriginales, crt.cant_doc_copias DocCopias, crt.cant_cds Cds, crt.tec_sel_doc TecnicasSeleccion, crt.publica Publica, crt.confidencial Confidencial, crt.reservada_sol_info Reservada, crt.descripcion_asunto_expediente DescripcionAsunto, crt.fecha_clasificacion FechaClasificacion, crt.periodo_reserva PeriodoReserva, crt.fundamento_legal FundamentoLegal, crt.ampliacion_periodo_reserva AmpliacionPeriodo, crt.fecha_desclasificacion FechaDesclasificacion, crt.nombre_desclasifica NombreDesclasifica, crt.cargo_desclasifica CargoDesclasifica, crt.partes_reservando PartesReservando, crt.datos_topograficos DatosTopograficos, crt.id_expediente_noexp
        //    //            from prod_control_exp.expediente_no_expedientable ec
        //    //            join prod_control_exp.cat_serie_documental cs on ec.id = cs.id
        //    //            join prod_control_exp.inventario_noexpedientable itf on ec.id_inventario_no_expedientable = itf.id
        //    //            join prod_control_exp.cat_areas ca on itf.id_area = ca.id
        //    //            join (select ROW_NUMBER() over(order by ets.id) NoProg, ets.id from prod_control_exp.expediente_no_expedientable ets where ets.id_inventario_no_expedientable = (select id_inventario_no_expedientable from expediente_no_expedientable where id = @Id)) cons on ec.id = cons.id
        //    //            left join prod_control_exp.caratula crt on ec.id = crt.id_expediente_noexp
        //    //            where ec.id = @Id";
        //    var sql = @"
        //                select cons.NoProg, cons.Consecutivo, ec.id Id, cs.codigo Codigo, ec.id_expediente IdExpediente, ec.nombre Nombre, if(year(ec.fecha_primero)=year(ec.fecha_ultimo),year(ec.fecha_primero),concat(year(ec.fecha_primero),'-',year(ec.fecha_ultimo))) Periodo, ec.anios_resguardo AniosResguardo, ec.numero_legajos Legajos, if(ec.numero_legajos>1,crt.fojas,ifnull(crt.fojas,ec.numero_fojas)) Fojas, ec.observaciones Observaciones, ec.fecha_registro FechaRegistro, if(ec.numero_legajos>1,date_format(crt.fecha_primero,'%Y/%m/%d'),ifnull(date_format(crt.fecha_primero,'%Y/%m/%d'),date_format(ec.fecha_primero,'%Y/%m/%d'))) FechaPrimeroAntiguo, if(ec.numero_legajos>1,date_format(crt.fecha_ultimo,'%Y/%m/%d'),ifnull(date_format(crt.fecha_ultimo,'%Y/%m/%d'),date_format(ec.fecha_ultimo,'%Y/%m/%d'))) FechaUltimoReciente, ec.id_inventario_control IdInventario, ec.ubicacion DatosTopograficos, ec.descripcion DescripcionAsunto, ec.tipo_soporte_documental TipoSoporteDocumental
        //                 ,cs.vig_doc_val_a VigDocValA, cs.vig_doc_val_l VigDocValL, cs.vig_doc_val_fc VigDocValFC, cs.vig_doc_pla_con_at VigDocPlaConAT, cs.vig_doc_pla_con_ac VigDocPlaConAC, cs.vig_doc_pla_con_tot VigDocPlaConTot, cs.tec_sel_e TecSelE, cs.tec_sel_c TecSelC, cs.tec_sel_m TecSelM
        //                 ,cp.descripcion Puesto, ec.estatus Estatus, ca.descripcion Area
        //                    ,crt.cant_doc_ori DocOriginales, crt.cant_doc_copias DocCopias, crt.cant_cds Cds, crt.tec_sel_doc TecnicasSeleccion, crt.publica Publica, crt.confidencial Confidencial, crt.reservada_sol_info Reservada, date_format(crt.fecha_clasificacion,'%Y/%m/%d') FechaClasificacion, crt.periodo_reserva PeriodoReserva, crt.fundamento_legal FundamentoLegal, crt.ampliacion_periodo_reserva AmpliacionPeriodo, date_format(crt.fecha_desclasificacion,'%Y/%m/%d') FechaDesclasificacion, crt.nombre_desclasifica NombreDesclasifica, crt.cargo_desclasifica CargoDesclasifica, crt.partes_reservando PartesReservando
        //                from prod_control_exp.expediente_control ec
        //                join prod_control_exp.cat_serie_documental cs on ec.id_expediente = cs.id
        //                join prod_control_exp.inventario_control itf on ec.id_inventario_control = itf.id
        //                join prod_control_exp.cat_puestos cp on itf.id_puesto = cp.id
        //                join prod_control_exp.cat_areas ca on cp.id_area = ca.id
        //                join (select ROW_NUMBER() over(order by ets.fecha_ultimo, ets.id) NoProg, ROW_NUMBER() over(partition by year(ets.fecha_ultimo) order by ets.fecha_ultimo, ets.id) Consecutivo, ets.id from prod_control_exp.expediente_control ets where ets.id_inventario_control = (select id_inventario_control from prod_control_exp.expediente_control where id = @Id) AND ets.migrado_ne = 1 ORDER BY ets.fecha_ultimo, ets.id) cons on ec.id = cons.id
        //                left join prod_control_exp.caratula crt on ec.id = crt.id_expediente_control and crt.legajo = @Legajo
        //                where ec.id = @Id";

        //    return await db.QueryFirstOrDefaultAsync<Caratula>(sql, new { Id = id, Legajo = legajo });
        //}
        //public async Task<bool> DropExpedienteNoExpedientable(int id)
        //{
        //    var db = DbConnection();
        //    var sql = @"
        //                delete from prod_control_exp.expediente_no_expedientable where id = @Id;";
        //    var result = await db.ExecuteAsync(sql, new { Id = id });
        //    return result > 0;
        //}
        //public async Task<IEnumerable<Area>> GetAreasLista()
        //{
        //    var db = DbConnection();
        //    var sql = @"
        //                select id Id, descripcion Descripcion, estatus
        //                from prod_control_exp.cat_areas
        //                order by id;
        //                ";
        //    return await db.QueryAsync<Area>(sql, new { });
        //}
        //public async Task<bool> UpdateArea(Area area)
        //{
        //    var db = DbConnection();
        //    var sql = @"";
        //    if (area.Id == 0)
        //    {
        //        sql = @"
        //                INSERT INTO prod_control_exp.cat_areas (descripcion) VALUES(@Descripcion);";
        //    }
        //    else
        //    {
        //        sql = @"
        //                UPDATE prod_control_exp.cat_areas SET descripcion = @Descripcion WHERE id = @IdArea;";
        //    }

        //    var result = await db.ExecuteAsync(sql, new
        //    {
        //        Descripcion = area.Descripcion,
        //        IdArea = area.Id
        //    });
        //    return result > 0;
        //}
        //public async Task<Area> GetArea(int id)
        //{
        //    var db = DbConnection();

        //    var sql = @"
        //                select id Id, descripcion Descripcion, estatus
        //                from prod_control_exp.cat_areas
        //                where id = @Id";

        //    return await db.QueryFirstOrDefaultAsync<Area>(sql, new { Id = id });
        //}
        //public async Task<IEnumerable<Area>> GetAreaUser(int id)
        //{
        //    var db = DbConnection();

        //    var sql = @"
        //                select id Id, descripcion Descripcion, estatus
        //                from prod_control_exp.cat_areas
        //                where id = @Id";

        //    return await db.QueryAsync<Area>(sql, new { Id = id });
        //}
        //public async Task<bool> ActivarArea(int id)
        //{
        //    var db = DbConnection();

        //    var sql = @"
        //                UPDATE prod_control_exp.cat_areas
        //                SET
        //                estatus = 1
        //                WHERE id = @IdArea;";

        //    var result = await db.ExecuteAsync(sql, new
        //    {
        //        IdArea = id
        //    });
        //    return result > 0;
        //}
        //public async Task<bool> DesactivarArea(int id)
        //{
        //    var db = DbConnection();

        //    var sql = @"
        //                UPDATE prod_control_exp.cat_areas
        //                SET
        //                estatus = 2
        //                WHERE id = @IdArea;";

        //    var result = await db.ExecuteAsync(sql, new
        //    {
        //        IdArea = id
        //    });
        //    return result > 0;
        //}
        //public async Task<IEnumerable<Area>> GetPuestosLista()
        //{
        //    var db = DbConnection();
        //    var sql = @"
        //                select cp.id IdPuesto, cp.descripcion Puesto, cp.estatus, cp.id_area Id, ca.descripcion Descripcion
        //                from prod_control_exp.cat_puestos cp
        //                join prod_control_exp.cat_areas ca on cp.id_area = ca.id
        //                order by cp.id;";
        //    return await db.QueryAsync<Area>(sql, new { });
        //}
        //public async Task<IEnumerable<Area>> GetPuestosListaValidacion()
        //{
        //    var db = DbConnection();
        //    var sql = @"
        //                select distinct cp.id IdPuesto, cp.descripcion Puesto, cp.estatus
        //                from prod_control_exp.inventario_control ic
        //                join prod_control_exp.expediente_control ec on ic.id = ec.id_inventario_control and ec.estatus = 2
        //                join prod_control_exp.cat_puestos cp on ic.id_puesto = cp.id
        //                order by cp.id;";
        //    return await db.QueryAsync<Area>(sql, new { });
        //}
        //public async Task<bool> UpdatePuesto(Area area)
        //{
        //    var db = DbConnection();
        //    var sql = @"";
        //    if (area.Id == 0)
        //    {
        //        sql = @"
        //                INSERT INTO prod_control_exp.cat_puestos (descripcion, id_area) VALUES(@Puesto, @IdArea);";
        //    }
        //    else
        //    {
        //        sql = @"
        //                UPDATE prod_control_exp.cat_puestos SET descripcion = @Puesto, id_area = @IdArea WHERE id = @IdPuesto;";
        //    }

        //    var result = await db.ExecuteAsync(sql, new
        //    {
        //        Puesto = area.Puesto,
        //        IdPuesto = area.IdPuesto,
        //        IdArea = area.Id
        //    });
        //    return result > 0;
        //}
        //public async Task<int> GetIdUserPuesto(string puesto)
        //{
        //    var db = DbConnection();
        //    var sql = @"
        //                select id from prod_control_exp.cat_puestos where descripcion = @Puesto order by id;
        //               ";
        //    return await db.QueryFirstOrDefaultAsync<int>(sql, new { Puesto = puesto });
        //}
        //public async Task<IEnumerable<Area>> GetPuestoUser(int id)
        //{
        //    var db = DbConnection();

        //    var sql = @"
        //                select id IdPuesto, descripcion Puesto, estatus
        //                from prod_control_exp.cat_puestos
        //                where id = @Id";

        //    return await db.QueryAsync<Area>(sql, new { Id = id });
        //}
        //public async Task<Area> GetPuesto(int id)
        //{
        //    var db = DbConnection();

        //    var sql = @"
        //                select id IdPuesto, descripcion Puesto, estatus, id_area Id
        //                from prod_control_exp.cat_puestos
        //                where id = @Id";

        //    return await db.QueryFirstOrDefaultAsync<Area>(sql, new { Id = id });
        //}
        //public async Task<bool> ActivarPuesto(int id)
        //{
        //    var db = DbConnection();

        //    var sql = @"
        //                UPDATE prod_control_exp.cat_puestos
        //                SET
        //                estatus = 1
        //                WHERE id = @IdPuesto;";

        //    var result = await db.ExecuteAsync(sql, new
        //    {
        //        IdPuesto = id
        //    });
        //    return result > 0;
        //}
        //public async Task<bool> DesactivarPuesto(int id)
        //{
        //    var db = DbConnection();

        //    var sql = @"
        //                UPDATE prod_control_exp.cat_puestos
        //                SET
        //                estatus = 2
        //                WHERE id = @IdPuesto;";

        //    var result = await db.ExecuteAsync(sql, new
        //    {
        //        IdPuesto = id
        //    });
        //    return result > 0;
        //}
        //public async Task<IEnumerable<User>> GetUsuariosLista()
        //{
        //    var db = DbConnection();
        //    var sql = @"
        //                select u.id Id, concat(u.nombre, ' ', u.primer_apellido, ' ', u.segundo_apellido) Name, u.usuario SUser, ca.descripcion Signer, u.cargo Position, u.numero_empleado EmployeeNumber, u.rfc RFC, u.activo Active
        //                from qa_adms_conavi.usuario u
        //                join qa_adms_conavi.c_area ca on u.id_area = ca.id
        //                where u.id_rol in (15,16) and u.id <> 212
        //                order by u.id;";
        //    return await db.QueryAsync<User>(sql, new { });
        //}
        //public async Task<bool> UpdateUsuario(User usuario)
        //{
        //    var db = DbConnection();
        //    var sql = @"";
        //    if (usuario.Id == 0)
        //    {
        //        sql = @"
        //                INSERT INTO qa_adms_conavi.usuario (nombre, primer_apellido, segundo_apellido, usuario, password, id_rol, cargo, numero_empleado, rfc, grado_academico, id_area, email, update_pass) VALUES(upper(@Nombre), upper(@PApellido), upper(@SApellido), @UserName, sha2(@Password,256), if(@Rol = 1, 15, 16), @Cargo, @NumEmpleado, upper(@RFC), upper(@GradoAcademico), @IdArea, lower(@Email), b'0');";
        //    }
        //    else
        //    {
        //        sql = @"
        //                UPDATE qa_adms_conavi.usuario SET nombre = upper(@Nombre), primer_apellido = upper(@PApellido), segundo_apellido = upper(@SApellido), id_area = @IdArea, usuario = @UserName, cargo = @Cargo, numero_empleado = @NumEmpleado, rfc = upper(@RFC), grado_academico = upper(@GradoAcademico), email = lower(@Email), id_rol = if(@Rol = 1, 15, 16)";
        //        if (!String.IsNullOrEmpty(usuario.Password))
        //        {
        //            sql += @", password = sha2(@Password,256)";
        //        }
        //        sql += @"
        //                WHERE id = @IdUsuario;";
        //    }

        //    var result = await db.ExecuteAsync(sql, new
        //    {
        //        Nombre = usuario.Name,
        //        PApellido = usuario.LName,
        //        SApellido = usuario.SLName,
        //        IdArea = usuario.IdSystem,
        //        UserName = usuario.SUser,
        //        Password = usuario.Password,
        //        Cargo = usuario.Position,
        //        NumEmpleado = usuario.EmployeeNumber,
        //        RFC = usuario.RFC,
        //        GradoAcademico = usuario.Degree,
        //        Email = usuario.Email,
        //        Rol = usuario.Rol,
        //        IdUsuario = usuario.Id
        //    });
        //    return result > 0;
        //}
        //public async Task<User> GetUsuario(int id)
        //{
        //    var db = DbConnection();

        //    var sql = @"
        //                select id Id, nombre Name, primer_apellido LName, segundo_apellido SLName, usuario SUser, if(id_rol=15,1,2) Rol, cargo Position, id_area IdSystem, numero_empleado EmployeeNumber, rfc RFC, grado_academico Degree, email Email
        //                from qa_adms_conavi.usuario
        //                where id = @Id";

        //    return await db.QueryFirstOrDefaultAsync<User>(sql, new { Id = id });
        //}
        //public async Task<bool> ActivarUsuario(int id)
        //{
        //    var db = DbConnection();

        //    var sql = @"
        //                UPDATE qa_adms_conavi.usuario
        //                SET
        //                activo = 1
        //                WHERE id = @IdUsuario;";

        //    var result = await db.ExecuteAsync(sql, new
        //    {
        //        IdUsuario = id
        //    });
        //    return result > 0;
        //}
        //public async Task<bool> DesactivarUsuario(int id)
        //{
        //    var db = DbConnection();

        //    var sql = @"
        //                UPDATE qa_adms_conavi.usuario
        //                SET
        //                activo = 2
        //                WHERE id = @IdUsuario;";

        //    var result = await db.ExecuteAsync(sql, new
        //    {
        //        IdUsuario = id
        //    });
        //    return result > 0;
        //}
    }
}
