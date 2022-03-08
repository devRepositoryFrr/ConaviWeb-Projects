using ConaviWeb.Model.Sisevive;
using Dapper;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConaviWeb.Data.Sisevive
{
    public class SiseviveRepository : ISiseviveRepository
    {
        private readonly MySQLConfiguration _connectionString;
        public SiseviveRepository(MySQLConfiguration connectionString)
        {
            _connectionString = connectionString;
        }

        protected MySqlConnection DbConnection()
        {
            return new MySqlConnection(_connectionString.ConnectionString);
        }
        //Evaluación
        public async Task<bool> InsertEvaluation(string folio, int idUser)
        {
            var db = DbConnection();

            var sql = @"
                        INSERT INTO evaluacion_vivienda (folio, id_usuario_carga)
                        VALUES (@Folio, @IdUser)";

            var result = await db.ExecuteAsync(sql, new { folio, idUser });
            return result > 0;
        }
        public async Task<Evaluacion> GetInforme(string folio)
        {
            var db = DbConnection();

            var sql = @"
                         SELECT ev.id Id, ev.folio Folio, ci.descripcion Informe, cc.clave Letra, cc.descripcion Color, ev.observaciones Observacion,
                                id_usuario_carga IdUserCarga, archivo_informe NombreArchInforme
                        FROM evaluacion_vivienda ev
                        LEFT JOIN c_informe ci ON ci.id = ev.id_informe
                        LEFT JOIN c_calificacion cc ON cc.id = ev.id_calificacion
                        WHERE ev.folio = @Folio";

            return await db.QueryFirstOrDefaultAsync<Evaluacion>(sql, new { Folio = folio });
        }
        public async Task<Evaluacion> GetEvaluacionesDetail(int id)
        {
            var db = DbConnection();

            var sql = @"
                        SELECT ev.id Id, ev.folio Folio, ev.fecha_carga FechaCarga, ci.descripcion Informe , 
                        cc.clave Letra, ev.observaciones Observacion, concat(u.nombre,' ',u.primer_apellido,' ',u.segundo_apellido) Nombre,
                        id_usuario_carga IdUserCarga, u.email as EmailPES
                        FROM evaluacion_vivienda ev
                        LEFT JOIN c_informe ci ON ci.id = ev.id_informe
                        LEFT JOIN c_calificacion cc ON cc.id = ev.id_calificacion
                        JOIN prod_usuario.usuario u ON u.id = ev.id_usuario_carga
                        WHERE ev.id = @Id";

            return await db.QueryFirstOrDefaultAsync<Evaluacion>(sql, new { Id = id });
        }
        public async Task<IEnumerable<Evaluacion>> GetEvaluaciones()
        {
            var db = DbConnection();

            var sql = @"
                        SELECT ev.id Id, ev.folio Folio, ev.fecha_carga Fecha, ci.descripcion Informe , 
                        cc.clave Letra, ev.observaciones Observacion, concat(u.nombre,' ',u.primer_apellido,' ',u.segundo_apellido) Nombre,
                        id_usuario_carga IdUserCarga, archivo_informe NombreArchInforme
                        FROM evaluacion_vivienda ev
                        LEFT JOIN c_informe ci ON ci.id = ev.id_informe
                        LEFT JOIN c_calificacion cc ON cc.id = ev.id_calificacion
                        JOIN prod_usuario.usuario u ON u.id = ev.id_usuario_carga";

            return await db.QueryAsync<Evaluacion>(sql, new { });
        }
        public async Task<bool> UpdateEvaluacion(EvalCustom evaluacion)
        {
            var db = DbConnection();

            var sql = @"
                        UPDATE evaluacion_vivienda
                        SET
                        id_informe = @IdInforme,
                        id_calificacion = @IdCalificacion,
                        observaciones = @Observacion,
                        archivo_informe = @ArchivoInforme,
                        id_usuario_calificador = @IdUser
                        WHERE id = @IdEvaluacion;";

            var result = await db.ExecuteAsync(sql, new
            {
                IdInforme = evaluacion.Informe,
                IdCalificacion = evaluacion.Letra,
                Observacion = evaluacion.Observacion,
                ArchivoInforme = evaluacion.NombreArchInforme,
                IdUser = evaluacion.IdAdminUser,
                IdEvaluacion = evaluacion.Id
            });
            return result > 0;
        }
        public async Task<Evaluacion> GetFolio(string folio)
        {
            var db = DbConnection();

            var sql = @"
                         SELECT id Id, folio Folio
                        FROM evaluacion_vivienda
                        WHERE folio = @Folio
                        AND id_informe = 3";

            return await db.QueryFirstOrDefaultAsync<Evaluacion>(sql, new { Folio = folio });
        }

        //Etiquetado
        public async Task<bool> InsertEtiquetado(string folio, int idUser, int idEvaluacion)
        {
            var db = DbConnection();

            var sql = @"
                        INSERT INTO etiquetado_vivienda (folio, id_usuario_carga, id_evaluacion)
                        VALUES (@Folio, @IdUser, @IdEvaluacion)";

            var result = await db.ExecuteAsync(sql, new { folio, idUser, idEvaluacion });
            return result > 0;
        }
        public async Task<Etiquetado> GetInformeEtiquetado(string folio)
        {
            var db = DbConnection();

            var sql = @"
                         SELECT et.id Id, et.folio Folio, ci.descripcion Informe, et.observaciones Observacion,
                                id_usuario_carga IdUserCarga, archivo_informe NombreArchInforme
                        FROM etiquetado_vivienda et
                        LEFT JOIN c_informe ci ON ci.id = et.id_informe
                        WHERE et.folio = @Folio";

            return await db.QueryFirstOrDefaultAsync<Etiquetado>(sql, new { Folio = folio });
        }
        public async Task<IEnumerable<Etiquetado>> GetEtiquetados()
        {
            var db = DbConnection();

            var sql = @"
                        SELECT et.id Id, et.folio Folio, et.fecha_carga Fecha, ci.descripcion Informe , 
                        et.observaciones Observacion, concat(u.nombre,' ',u.primer_apellido,' ',u.segundo_apellido) Nombre,
                        id_usuario_carga IdUserCarga, archivo_informe NombreArchInforme
                        FROM etiquetado_vivienda et
                        JOIN c_informe ci ON ci.id = et.id_informe
                        JOIN prod_usuario.usuario u ON u.id = et.id_usuario_carga;";

            return await db.QueryAsync<Etiquetado>(sql, new { });
        }
        public async Task<Etiquetado> GetEtiquetadoDetail(int id)
        {
            var db = DbConnection();

            var sql = @"
                        SELECT et.id Id, et.folio Folio, et.fecha_carga Fecha, ci.descripcion Informe , 
                        et.observaciones Observacion, concat(u.nombre,' ',u.primer_apellido,' ',u.segundo_apellido) Nombre,
                        id_usuario_carga IdUserCarga, u.email as EmailPES, archivo_informe NombreArchInforme
                        FROM etiquetado_vivienda et
                        JOIN c_informe ci ON ci.id = et.id_informe
                        JOIN prod_usuario.usuario u ON u.id = et.id_usuario_carga
                        WHERE et.id = @Id";

            return await db.QueryFirstOrDefaultAsync<Etiquetado>(sql, new { Id = id });
        }
        public async Task<bool> UpdateEtiquetado(EtiqCustom etiquetado)
        {
            var db = DbConnection();

            var sql = @"
                        UPDATE etiquetado_vivienda
                        SET
                        id_informe = @IdInforme,
                        observaciones = @Observacion,
                        archivo_informe = @ArchivoInforme,
                        id_usuario_calificador = @IdUser
                        WHERE id = @IdEtiquetado;";

            var result = await db.ExecuteAsync(sql, new
            {
                IdInforme = etiquetado.Informe,
                Observacion = etiquetado.Observacion,
                ArchivoInforme = etiquetado.NombreArchInforme,
                IdUser = etiquetado.IdAdminUser,
                IdEtiquetado = etiquetado.Id
            });
            return result > 0;
        }

        
    }
}
