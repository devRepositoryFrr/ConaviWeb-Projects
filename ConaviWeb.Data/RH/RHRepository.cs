using ConaviWeb.Model.RH;
using Dapper;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConaviWeb.Data.RH
{
    public class RHRepository : IRHRepository
    {
        private readonly MySQLConfiguration _connectionString;
        public RHRepository(MySQLConfiguration connectionString)
        {
            _connectionString = connectionString;
        }

        protected MySqlConnection DbConnection()
        {
            return new MySqlConnection(_connectionString.ConnectionString);
        }
        
        public async Task<bool> InsertViaticos(Viaticos viaticos)
        {
            var db = DbConnection();

            var sql = @"
                        INSERT INTO prod_rh.solicitud_viaticos (id_usuario,
                                descripcion_comision,
                                objetivo,
                                observaciones,
                                lugares_asignados_comision,
                                medio_transporte,
                                periodo_comision_i,
                                periodo_comision_f,
                                dias_duracion,
                                cuota_diaria,
                                importe_viaticos,
                                num_casetas,
                                dotacion_combustible,
                                importe_gastos,
                                total_peajes,
                                fecha_salida,
                                fecha_regreso,
                                horario_estimado_s,
                                horario_estimado_r,
                                linea_aerea,
                                ruta_i,
                                vuelo_i,
                                sale_i,
                                llega_i,
                                ruta_f,
                                vuelo_f,
                                sale_f,
                                llega_f)
                        VALUES (@IdUsuario,
                                @Descripcion_comision,
                                @Objetivo,
                                @Observaciones,
                                @Lugares_asignados_comision,
                                @Medio_transporte,
                                @Periodo_comision_i,
                                @Periodo_comision_f,
                                @Dias_duracion,
                                @Cuota_diaria,
                                @Importe_viaticos,
                                @Num_casetas,
                                @Dotacion_combustible,
                                @Importe_gastos,
                                @Total_peajes,
                                @Fecha_salida,
                                @Fecha_regreso,
                                @Horario_salida,
                                @Horario_regreso,
                                @Linea_aerea,
                                @Ruta_i,
                                @Vuelo_i,
                                @Sale_i,
                                @Llega_i,
                                @Ruta_f,
                                @Vuelo_f,
                                @Sale_f,
                                @Llega_f)";

            var result = await db.ExecuteAsync(sql, new
            {
                viaticos.IdUsuario,
                viaticos.Descripcion_comision,
                viaticos.Objetivo,
                viaticos.Observaciones,
                viaticos.Lugares_asignados_comision,
                viaticos.Medio_transporte,
                viaticos.Periodo_comision_i,
                viaticos.Periodo_comision_f,
                viaticos.Dias_duracion,
                viaticos.Cuota_diaria,
                viaticos.Importe_viaticos,
                viaticos.Num_casetas,
                viaticos.Dotacion_combustible,
                viaticos.Importe_gastos,
                viaticos.Total_peajes,
                viaticos.Fecha_salida,
                viaticos.Fecha_regreso,
                viaticos.Horario_salida,
                viaticos.Horario_regreso,
                viaticos.Linea_aerea,
                viaticos.Ruta_i,
                viaticos.Vuelo_i,
                viaticos.Sale_i,
                viaticos.Llega_i,
                viaticos.Ruta_f,
                viaticos.Vuelo_f,
                viaticos.Sale_f,
                viaticos.Llega_f
            });
            return result > 0;
        }

        public async Task<IEnumerable<Viaticos>> GetSolicitudes()
        {
            var db = DbConnection();

            var sql = @"
                        SELECT 
                            id AS Id,
                            nombre AS Nombre,
                            puesto AS Puesto,
                            area_adscripcion AS Area_adscripcion,
                            descripcion_comision AS Descripcion_comision,
                            unidad AS Unidad,
                            cantidad AS Cantidad,
                            monto_total AS Monto_total,
                            objetivo AS Objetivo,
                            observaciones AS Observaciones,
                            lugares_asignados_comision AS Lugares_asignados_comision,
                            medio_transporte AS Medio_transporte,
                            periodo_comision_i AS Periodo_comision_i,
                            periodo_comision_f AS Periodo_comision_f,
                            dias_duracion AS Dias_duracion,
                            cuota_diaria AS Cuota_diaria,
                            importe_viaticos AS Importe_viaticos,
                            num_casetas AS Num_casetas,
                            dotacion_combustible AS Dotacion_combustible,
                            importe_gastos AS Importe_gastos,
                            total_peajes AS Total_peajes,
                            fecha_salida AS Fecha_salida,
                            fecha_regreso AS Fecha_regreso,
                            linea_aerea AS Linea_aerea,
                            ruta_i AS Ruta_i,
                            vuelo_i AS Vuelo_i,
                            sale_i AS Sale_i,
                            llega_i AS Llega_i,
                            ruta_f AS Ruta_f,
                            vuelo_f AS Vuelo_f,
                            sale_f AS Sale_f,
                            llega_f AS Llega_f
                        FROM prod_rh.solicitud_viaticos;";

            return await db.QueryAsync<Viaticos>(sql, new { });
        }
    }
}
