using ConaviWeb.Model;
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
                                folio,
                                descripcion_comision,
                                objetivo,
                                observaciones,
                                clave_estado,
                                municipio,
                                medio_transporte,
                                periodo_comision_i,
                                periodo_comision_f,
                                dias_duracion,
                                cuota_diaria,
                                importe_viaticos,
                                num_casetas,
                                obs_aereo,
                                importe_gastos,
                                total_peajes,
                                img_traza_ruta,
                                destino_1,
                                fecha_1,
                                horario_estimado_1,
                                destino_2,
                                fecha_2,
                                horario_estimado_2,
                                destino_3,
                                fecha_3,
                                horario_estimado_3,
                                destino_4,
                                fecha_4,
                                horario_estimado_4,
                                linea_aerea,
                                ruta_i,
                                vuelo_i,
                                sale_i,
                                llega_i,
                                ruta_f,
                                vuelo_f,
                                sale_f,
                                llega_f,
                                total_viaticos)
                        VALUES (@IdUsuario,
                                @Folio,
                                @Descripcion_comision,
                                @Objetivo,
                                @Observaciones,
                                @Clave_estado,
                                @Municipio,
                                @Medio_transporte,
                                @Periodo_comision_i,
                                @Periodo_comision_f,
                                @Dias_duracion,
                                @Cuota_diaria,
                                @Importe_viaticos,
                                @Num_casetas,
                                @Obs_aereo,
                                @Importe_gastos,
                                @Total_peajes,
                                @Traza_ruta,
                                @Destino_1,
                                @Fecha_1,
                                @Horario_1,
                                @Destino_2,                                
                                @Fecha_2,
                                @Horario_2,
                                @Destino_3,
                                @Fecha_3,
                                @Horario_3,
                                @Destino_4,
                                @Fecha_4,
                                @Horario_4,
                                @Linea_aerea,
                                @Ruta_i,
                                @Vuelo_i,
                                @Sale_i,
                                @Llega_i,
                                @Ruta_f,
                                @Vuelo_f,
                                @Sale_f,
                                @Llega_f,
                                @TotalViaticos)";

            var result = await db.ExecuteAsync(sql, new
            {
                viaticos.IdUsuario,
                viaticos.Folio,
                viaticos.Descripcion_comision,
                viaticos.Objetivo,
                viaticos.Observaciones,
                viaticos.Clave_estado,
                viaticos.Municipio,
                viaticos.Medio_transporte,
                viaticos.Periodo_comision_i,
                viaticos.Periodo_comision_f,
                viaticos.Dias_duracion,
                viaticos.Cuota_diaria,
                viaticos.Importe_viaticos,
                viaticos.Num_casetas,
                viaticos.Obs_aereo,
                viaticos.Importe_gastos,
                viaticos.Total_peajes,
                viaticos.Traza_ruta,
                viaticos.Destino_1,
                viaticos.Fecha_1,
                viaticos.Horario_1,
                viaticos.Destino_2,
                viaticos.Fecha_2,
                viaticos.Horario_2,
                viaticos.Destino_3,
                viaticos.Fecha_3,
                viaticos.Horario_3,
                viaticos.Destino_4,
                viaticos.Fecha_4,
                viaticos.Horario_4,
                viaticos.Linea_aerea,
                viaticos.Ruta_i,
                viaticos.Vuelo_i,
                viaticos.Sale_i,
                viaticos.Llega_i,
                viaticos.Ruta_f,
                viaticos.Vuelo_f,
                viaticos.Sale_f,
                viaticos.Llega_f,
                viaticos.TotalViaticos
            });
            return result > 0;
        }

        public async Task<IEnumerable<Viaticos>> GetSolicitudes()
        {
            var db = DbConnection();

            var sql = @"
                       SELECT sv.id AS Id,
                                sv.id_usuario AS IdUsuario,
                            folio AS Folio,
                            date_format(fecha_registro, '%d/%m/%Y') AS FechaSol,
                            concat(u.nombre, ' ', u.primer_apellido, ' ', u.segundo_apellido)  AS Nombre,
                            u.cargo AS Puesto ,
                            u.id AS IdUsuario,
                            ca.descripcion  AS Area_adscripcion ,
                            descripcion_comision AS Descripcion_comision ,
                            objetivo AS Objetivo ,
                            observaciones AS Observaciones ,
                            concat(cef.descripcion,' - ',municipio) AS Lugares_asignados_comision,
                            medio_transporte AS Medio_transporte ,
                            periodo_comision_i AS Periodo_comision_i ,
                            periodo_comision_f AS Periodo_comision_f ,
                            dias_duracion AS Dias_duracion ,
                            cuota_diaria AS Cuota_diaria ,
                            importe_viaticos AS Importe_viaticos ,
                            num_casetas AS Num_casetas ,
                            dotacion_combustible AS Dotacion_combustible ,
                            importe_gastos AS Importe_gastos ,
                            total_peajes AS Total_peajes ,
                            img_traza_ruta AS Traza_ruta ,
                            destino_1 AS Destino_1,
                            fecha_1 AS Fecha_1,
                            horario_estimado_1 AS Horario_1,
                            destino_2 AS Destino_2,
                            fecha_2 AS Fecha_2,
                            horario_estimado_2 AS Horario_2,
                            destino_3 AS Destino_3,
                            fecha_3 AS Fecha_3,
                            horario_estimado_3 AS Horario_3,
                            destino_4 AS Destino_4,
                            fecha_4 AS Fecha_4,
                            horario_estimado_4 AS Horario_4,
                            linea_aerea AS Linea_aerea ,
                            ruta_i AS Ruta_i ,
                            vuelo_i AS Vuelo_i ,
                            sale_i AS Sale_i ,
                            llega_i AS Llega_i ,
                            ruta_f AS Ruta_f ,
                            vuelo_f AS Vuelo_f ,
                            sale_f AS Sale_f ,
                            llega_f AS Llega_f,
                            ruta_j AS Ruta_j ,
                            vuelo_j AS Vuelo_j ,
                            sale_j AS Sale_j ,
                            llega_j AS Llega_j,
                            ruta_k AS Ruta_k ,
                            vuelo_k AS Vuelo_k ,
                            sale_k AS Sale_k ,
                            llega_k AS Llega_k,
                            estatus AS Estatus,
                            obs_cancela AS ObsCan,
                            archivo_pago AS Archivo_pago,
                            (select af.nombre_archivo_firma from prod_web_efirma.archivo_origen ao
                            join prod_web_efirma.archivo_firma af on af.id_archivo_padre = ao.id
                            where ao.nombre_archivo = concat(folio, '.pdf')

                            ORDER BY af.id DESC LIMIT 1) AS Archivo_firma
                        FROM prod_rh.solicitud_viaticos sv
                        JOIN qa_adms_conavi.usuario u ON u.id = sv.id_usuario
                        JOIN qa_adms_conavi.c_area ca ON ca.id = u.id_area
                        JOIN prod_catalogos.c_entidad_federativa cef on cef.clave = sv.clave_estado order by sv.id desc; ";

            return await db.QueryAsync<Viaticos>(sql, new { });
        }
        public async Task<IEnumerable<Viaticos>> GetSolicitudes(int estatus)
        {
            var db = DbConnection();

            var sql = @"
                       SELECT sv.id AS Id,
                            sv.id_usuario AS IdUsuario,
                            folio AS Folio,
                            fecha_registro AS FechaSol,
                            concat(u.nombre, ' ', u.primer_apellido, ' ', u.segundo_apellido)  AS Nombre,
                            u.cargo AS Puesto ,
                            ca.descripcion  AS Area_adscripcion ,
                            descripcion_comision AS Descripcion_comision ,
                            objetivo AS Objetivo ,
                            observaciones AS Observaciones ,
                            concat(cef.descripcion,' - ',municipio) AS Lugares_asignados_comision,
                            medio_transporte AS Medio_transporte ,
                            periodo_comision_i AS Periodo_comision_i ,
                            periodo_comision_f AS Periodo_comision_f ,
                            dias_duracion AS Dias_duracion ,
                            cuota_diaria AS Cuota_diaria ,
                            importe_viaticos AS Importe_viaticos ,
                            num_casetas AS Num_casetas ,
                            dotacion_combustible AS Dotacion_combustible ,
                            importe_gastos AS Importe_gastos ,
                            total_peajes AS Total_peajes ,
                            fecha_salida AS Fecha_salida ,
                            fecha_regreso AS Fecha_regreso ,
                            horario_estimado_s AS Horario_salida ,
                            horario_estimado_r AS Horario_regreso ,
                            linea_aerea AS Linea_aerea ,
                            ruta_i AS Ruta_i ,
                            vuelo_i AS Vuelo_i ,
                            sale_i AS Sale_i ,
                            llega_i AS Llega_i ,
                            ruta_f AS Ruta_f ,
                            vuelo_f AS Vuelo_f ,
                            sale_f AS Sale_f ,
                            llega_f AS Llega_f,
                            estatus AS Estatus,
                            archivo_pago AS Archivo_pago,
                            (select af.nombre_archivo_firma from prod_web_efirma.archivo_origen ao
							join prod_web_efirma.archivo_firma af on af.id_archivo_padre = ao.id
							where ao.nombre_archivo = concat(folio,'.pdf')
							ORDER BY af.id DESC LIMIT 1) AS Archivo_firma
                        FROM prod_rh.solicitud_viaticos sv
                        JOIN qa_adms_conavi.usuario u ON u.id = sv.id_usuario
                        JOIN qa_adms_conavi.c_area ca ON ca.id = u.id_area
                        JOIN prod_catalogos.c_entidad_federativa cef on cef.clave = sv.clave_estado
                        WHERE sv.estatus = @Estatus;";

            return await db.QueryAsync<Viaticos>(sql, new { Estatus = estatus });
        }
        public async Task<IEnumerable<Viaticos>> GetSolicitudesUser(int idUser)
        {
            var db = DbConnection();

            var sql = @"
                        SELECT sv.id AS Id,
                            folio AS Folio,
                            date_format(fecha_registro, '%d/%m/%Y') AS FechaSol,
                            concat(u.nombre, ' ', u.primer_apellido, ' ', u.segundo_apellido)  AS Nombre,
                            estatus AS Estatus,
                            (select af.nombre_archivo_firma from prod_web_efirma.archivo_origen ao
							join prod_web_efirma.archivo_firma af on af.id_archivo_padre = ao.id
							where ao.nombre_archivo = concat(folio,'.pdf')
							ORDER BY af.id DESC LIMIT 1) AS Archivo_firma
                        FROM prod_rh.solicitud_viaticos sv
                        JOIN qa_adms_conavi.usuario u ON u.id = sv.id_usuario
                        WHERE sv.id_usuario = @IdUser ORDER BY sv.id desc;";

            return await db.QueryAsync<Viaticos>(sql, new { IdUser = idUser });
        }
        public async Task<IEnumerable<Viaticos>> GetSolicitudesUser(int idUser, int idEstatus)
        {
            var db = DbConnection();

            var sql = @"
                        SELECT sv.id AS Id,
                            sv.id_usuario AS IdUsuario,
                            folio AS Folio,
                            fecha_registro AS FechaSol,
                            concat(u.nombre, ' ', u.primer_apellido, ' ', u.segundo_apellido)  AS Nombre,
                            u.cargo AS Puesto ,
                            ca.descripcion  AS Area_adscripcion ,
                            descripcion_comision AS Descripcion_comision ,
                            objetivo AS Objetivo ,
                            observaciones AS Observaciones ,
                            concat(cef.descripcion,' - ',municipio) AS Lugares_asignados_comision,
                            medio_transporte AS Medio_transporte ,
                            periodo_comision_i AS Periodo_comision_i ,
                            periodo_comision_f AS Periodo_comision_f ,
                            dias_duracion AS Dias_duracion ,
                            cuota_diaria AS Cuota_diaria ,
                            importe_viaticos AS Importe_viaticos ,
                            num_casetas AS Num_casetas ,
                            dotacion_combustible AS Dotacion_combustible ,
                            importe_gastos AS Importe_gastos ,
                            total_peajes AS Total_peajes ,                           
                            linea_aerea AS Linea_aerea ,
                            ruta_i AS Ruta_i ,
                            vuelo_i AS Vuelo_i ,
                            sale_i AS Sale_i ,
                            llega_i AS Llega_i ,
                            ruta_f AS Ruta_f ,
                            vuelo_f AS Vuelo_f ,
                            sale_f AS Sale_f ,
                            llega_f AS Llega_f,
                            estatus AS Estatus,
                            archivo_pago AS Archivo_pago,
                            (select af.nombre_archivo_firma from prod_web_efirma.archivo_origen ao
							join prod_web_efirma.archivo_firma af on af.id_archivo_padre = ao.id
							where ao.nombre_archivo = concat(folio,'.pdf')
							ORDER BY af.id DESC LIMIT 1) AS Archivo_firma
                        FROM prod_rh.solicitud_viaticos sv
                        JOIN qa_adms_conavi.usuario u ON u.id = sv.id_usuario
                        JOIN qa_adms_conavi.c_area ca ON ca.id = u.id_area
                        JOIN prod_catalogos.c_entidad_federativa cef on cef.clave = sv.clave_estado
                        WHERE sv.estatus = @Estatus
                        AND sv.id_usuario = @IdUser;";

            return await db.QueryAsync<Viaticos>(sql, new {Estatus = idEstatus, IdUser = idUser });
        }
        public async Task<bool> UpdateViaticos(Viaticos viaticos)
        {
            var db = DbConnection();

            var sql = @"
                        UPDATE prod_rh.solicitud_viaticos
                            SET linea_aerea = @Linea_aerea, 
                            ruta_i = @Ruta_i, 
                            vuelo_i = @Vuelo_i, 
                            sale_i = @Sale_i, 
                            llega_i = @Llega_i, 
                            ruta_f = @Ruta_f, 
                            vuelo_f = @Vuelo_f, 
                            sale_f = @Sale_f, 
                            llega_f = @Llega_f,
                            ruta_j = @Ruta_j, 
                            vuelo_j = @Vuelo_j, 
                            sale_j = @Sale_j, 
                            llega_j = @Llega_j, 
                            ruta_k = @Ruta_k, 
                            vuelo_k = @Vuelo_k, 
                            sale_k = @Sale_k, 
                            llega_k = @Llega_k
                        WHERE id = @Id;";

            var result = await db.ExecuteAsync(sql, new
            {
                viaticos.Id,
                viaticos.Linea_aerea,
                viaticos.Ruta_i,
                viaticos.Vuelo_i,
                viaticos.Sale_i,
                viaticos.Llega_i,
                viaticos.Ruta_f,
                viaticos.Vuelo_f,
                viaticos.Sale_f,
                viaticos.Llega_f,
                viaticos.Ruta_j,
                viaticos.Vuelo_j,
                viaticos.Sale_j,
                viaticos.Llega_j,
                viaticos.Ruta_k,
                viaticos.Vuelo_k,
                viaticos.Sale_k,
                viaticos.Llega_k
            });
            return result > 0;
        }
        public async Task<bool> UpdateEstatus(int id, int estatus)
        {
            var db = DbConnection();

            var sql = @"
                        UPDATE prod_rh.solicitud_viaticos
                            SET estatus = @Estatus
                        WHERE id = @Id;";

            var result = await db.ExecuteAsync(sql, new
            {
                id,
                estatus,
                
            });
            return result > 0;
        }

        public async Task<Viaticos> GetSolicitud(int id)
        {
            var db = DbConnection();

            var sql = @"
                         SELECT sv.id AS Id,
                            folio AS Folio,
                            fecha_registro AS FechaSol,
                            concat(u.nombre, ' ', u.primer_apellido, ' ', u.segundo_apellido)  AS Nombre,
                            u.cargo AS Puesto ,
                            ifnull(u.rfc,'') AS RFC,
                            ifnull(u.clave_nivel,'') AS CvNivel,
                            ca.descripcion  AS Area_adscripcion ,
                            descripcion_comision AS Descripcion_comision ,
                            objetivo AS Objetivo ,
                            ifnull(observaciones,'') AS Observaciones ,
                            concat(cef.descripcion,' - ',municipio) AS Lugares_asignados_comision,
                            medio_transporte AS Medio_transporte ,
                            periodo_comision_i AS Periodo_comision_i ,
                            ifnull(periodo_comision_f,'Indeterminado') AS Periodo_comision_f ,
                            ifnull(dias_duracion,'') AS Dias_duracion ,
                            ifnull(cuota_diaria,'') AS Cuota_diaria ,
                            ifnull(importe_viaticos,'') AS Importe_viaticos ,
                            ifnull(num_casetas,'') AS Num_casetas ,
                            ifnull(dotacion_combustible,'') AS Dotacion_combustible ,
							ifnull(importe_gastos,'') AS Importe_gastos ,
							ifnull(total_peajes,'') AS Total_peajes,
                            ifnull(fecha_1,'') AS Fecha_1,
							ifnull(horario_estimado_1,'') AS Horario_1,
							ifnull(fecha_2,'') AS Fecha_2,
							ifnull(horario_estimado_2,'') AS Horario_2,
							ifnull(fecha_3,'') AS Fecha_3,
							ifnull(horario_estimado_3,'') AS Horario_3,
							ifnull(fecha_4,'') AS Fecha_4,
							ifnull(horario_estimado_4,'') AS Horario_4,
							ifnull(linea_aerea,'') AS Linea_aerea ,
							ifnull(ruta_i,'') AS Ruta_i ,
							ifnull(vuelo_i,'') AS Vuelo_i ,
							ifnull(sale_i,'') AS Sale_i ,
							ifnull(llega_i,'') AS Llega_i ,
							ifnull(ruta_f,'') AS Ruta_f ,
							ifnull(vuelo_f,'') AS Vuelo_f ,
							ifnull(sale_f,'') AS Sale_f ,
							ifnull(llega_f,'') AS Llega_f,
                            ifnull(ruta_j,'') AS Ruta_j ,
							ifnull(vuelo_j,'') AS Vuelo_j ,
							ifnull(sale_j,'') AS Sale_j ,
							ifnull(llega_j,'') AS Llega_j,
                            ifnull(ruta_k,'') AS Ruta_k ,
							ifnull(vuelo_k,'') AS Vuelo_k ,
							ifnull(sale_k,'') AS Sale_k ,
							ifnull(llega_k,'') AS Llega_k,
                            total_viaticos AS TotalViaticos
                        FROM prod_rh.solicitud_viaticos sv
                        JOIN qa_adms_conavi.usuario u ON u.id = sv.id_usuario
                        JOIN qa_adms_conavi.c_area ca ON ca.id = u.id_area
                        JOIN prod_catalogos.c_entidad_federativa cef on cef.clave = sv.clave_estado
                        WHERE sv.id = @Id";

            return await db.QueryFirstOrDefaultAsync<Viaticos>(sql, new { Id = id });
        }

        public async Task<IEnumerable<Catalogo>> GetEntidades()
        {
            var db = DbConnection();

            var sql = @"
                            SELECT 
                                clave AS Clave , 
                                descripcion AS Descripcion 
                            FROM prod_catalogos.c_entidad_federativa;
                         ";

            return await db.QueryAsync<Catalogo>(sql, new { });
        }

        public async Task<bool> UpdateEstatus(int id, int estatus, string obs)
        {
            var db = DbConnection();

            var sql = @"
                        UPDATE prod_rh.solicitud_viaticos
                            SET estatus = @Estatus,
                                obs_cancela = @Obs
                        WHERE id = @Id;";

            var result = await db.ExecuteAsync(sql, new
            {
                id,
                estatus,
                obs
            });
            return result > 0;
        }
        public async Task<bool> UpdateEstatus(int id, string path, int estatus)
        {
            var db = DbConnection();

            var sql = @"
                        UPDATE prod_rh.solicitud_viaticos
                            SET estatus = @Estatus,
                                archivo_pago = @Path
                        WHERE id = @Id;";

            var result = await db.ExecuteAsync(sql, new
            {
                id,
                estatus,
                path
            });
            return result > 0;
        }


        public async Task<bool> InsertComprobacion(CFDI cfdi)
        {
            var db = DbConnection();

            var sql = @"
                        INSERT INTO prod_rh.comprobacion_viaticos
                                (rfc_emisor,
                                rfc_receptor,
                                total,
                                uuid,
                                codigo_estatus,
                                es_cancelable,
                                estado,
                                estatus_canceladion,
                                validacion_efos,
                                folio)
                        VALUES (@RFCEmisor,
                                @RFCReceptor,
                                @TOTAL,
                                @UUID,
                                @codigoEstatus,
                                @esCancelable,
                                @estado,
                                @estatusCancelacion,
                                @validacionEFOS,
                                @FOLIO)";

            var result = await db.ExecuteAsync(sql, new
            {
                cfdi.RFCEmisor,
                cfdi.RFCReceptor,
                cfdi.TOTAL,
                cfdi.UUID,
                cfdi.codigoEstatus,
                cfdi.esCancelable,
                cfdi.estado,
                cfdi.estatusCancelacion,
                cfdi.validacionEFOS,
                cfdi.FOLIO
            });
            return result > 0;
        }
        public async Task<IEnumerable<CFDI>> GetComprobaciones(string folio)
        {
            var db = DbConnection();

            var sql = @"
                            SELECT id AS Id,
                                    rfc_emisor AS RFCEmisor,
                                    rfc_receptor AS RFCReceptor,
                                    total AS TOTAL,
                                    uuid AS UUID,
                                    codigo_estatus AS codigoEstatus,
                                    es_cancelable AS esCancelable,
                                    estado,
                                    estatus_cancelacion AS esCancelable,
                                    validacion_efos AS validacionEFOS,
                                    folio AS FOLIO
                                FROM prod_rh.comprobacion_viaticos
                                WHERE folio = @Folio;
                         ";

            return await db.QueryAsync<CFDI>(sql, new { folio });
        }
        public async Task<string> GetDataMail(int id_user)
        {
            var db = DbConnection();

            var sql = @"CALL prod_rh.sp_get_firma_mail(@Id_user)";

            return await db.QueryFirstOrDefaultAsync<string>(sql, new { id_user });
        }
    }
}
