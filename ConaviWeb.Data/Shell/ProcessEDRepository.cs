using ConaviWeb.Model.Shell;
using Dapper;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConaviWeb.Data.Shell
{
    public class ProcessEDRepository : IProcessEDRepository
    {

        private readonly MySQLConfiguration _connectionString;
        public ProcessEDRepository(MySQLConfiguration connectionString)
        {
            _connectionString = connectionString;
        }

        protected MySqlConnection DbConnection()
        {
            return new MySqlConnection(_connectionString.EDConnectionString);
        }
        
        public async Task<bool> InsertVoBo(string fileName, string path,DateTime dateProcess, int idUser,string ed)
        {
            var db = DbConnection();

            var sql = @"
                        CALL sp_insert_vobo(@FileName, @Path, @DateProcess, @IdUser, @ED);";

            var result = await db.ExecuteAsync(sql, new { fileName, path, dateProcess, idUser , ed});
            return result > 0;
        }
        public async Task<bool> UpdateVoBo(string fileName, string path, DateTime dateProcess, int idUser, string ed)
        {
            var db = DbConnection();

            var sql = @"
                        CALL sp_update_vobo(@FileName, @Path, @DateProcess, @IdUser, @ED);";

            var result = await db.ExecuteAsync(sql, new { fileName, path, dateProcess, idUser, ed });
            return result > 0;
        }

        public async Task<IEnumerable<ProcessED>> SelectVoBo(string type, string process)
        {
            var db = DbConnection();

            var sql = @"
                         select id ID, nombre_archivo FileName, ruta_archivo FilePath, 
                                fecha_vobo1 DateVoBo1, fecha_vobo2 DateVoBo2, fecha_procesado DateED, if(tipo_proceso = 'Encriptado', 'encrypt','decrypt') ProcessType 
                        from proceso_ed where ruta_archivo = @Process and tipo_proceso = @Type order by id desc";

            return await db.QueryAsync<ProcessED>(sql, new { Type = type, Process = process });
        }

        public async Task<bool> InsertED(string fileName, string path, DateTime dateProcess, int idUser, string ed)
        {
            var db = DbConnection();

            var sql = @"
                        CALL sp_insert_ed(@FileName, @Path, @DateProcess, @IdUser, @ED);";

            var result = await db.ExecuteAsync(sql, new { fileName, path, dateProcess, idUser, ed });
            return result > 0;
        }
    }
}
