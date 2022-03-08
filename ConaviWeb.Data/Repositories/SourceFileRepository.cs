using Dapper;
using ConaviWeb.Model;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConaviWeb.Data.Repositories
{
    public class SourceFileRepository : ISourceFileRepository
    {
        private readonly MySQLConfiguration _connectionString;
        public SourceFileRepository( MySQLConfiguration connectionString)
        {
            _connectionString = connectionString;
        }

        protected MySqlConnection DbConnection()
        {
            return new MySqlConnection(_connectionString.ConnectionString);
        }

        public async Task<IEnumerable<SourceFile>> GetAllSourceFile()
        {
            var db = DbConnection();

            var sql = @"
                        SELECT id Id, ruta_archivo FilePath, nombre_archivo FileName, fecha_carga DateUpload, hash_archivo Hash, id_usuario IdUser, numero_firma NuSign, id_estatus_proceso IdStatus
                        FROM archivo_origen";

            return await db.QueryAsync<SourceFile>(sql,new { });
        }

        public async Task<SourceFile> GetSourceFileDetails(int id)
        {
            var db = DbConnection();

            var sql = @"
                        SELECT id, ruta_archivo, nombre_archivo, fecha_carga, hash_archivo, id_usuario, numero_firma, id_estatus_proceso
                        FROM archivo_origen
                        WHERE id = @Id";

            return await db.QueryFirstOrDefaultAsync<SourceFile>(sql, new { Id = id });
        }

        public async Task<bool> InsertSourceFile(SourceFile sourceFile)
        {
            var db = DbConnection();

            var sql = @"
                        INSERT INTO archivo_origen (ruta_archivo, nombre_archivo, hash_archivo, id_usuario, id_particion)
                        VALUES (@FilePath, @FileName, @Hash, @IdUser, @IdPartition)";

            var result = await db.ExecuteAsync(sql, new { sourceFile.FilePath, sourceFile.FileName, sourceFile.Hash, sourceFile.IdUser, sourceFile.IdPartition});
            return result > 0;
        }

        public async Task<bool> UpdateSourceFile(SourceFile sourceFile)
        {
            var db = DbConnection();

            var sql = @"
                        UPDATE archivo_origen 
                        SET ruta_archivo = @FilePath, nombre_archivo = @FileName, fecha_carga = @DateUpload, hash_archivo = @Hash, id_usuario = @IdUser,
                        numero_firma = @NuSign, id_estatus_proceso = @IdStatus
                        WHERE id = @Id";

            var result = await db.ExecuteAsync(sql, new { sourceFile.FilePath, sourceFile.FileName, sourceFile.DateUpload, sourceFile.Hash, sourceFile.IdUser,
                                                          sourceFile.NuSign, sourceFile.IdStatus });
            return result > 0;
        }

        public async Task<bool> DeleteSourceFile(SourceFile sourceFile)
        {
            var db = DbConnection();

            var sql = @"
                        DELETE
                        FROM archivo_origen
                        WHERE id = @Id";

            var result = await db.ExecuteAsync(sql, new { Id = sourceFile.Id });
            return result > 0;
        }

        public async Task<bool> InsertPartition(string partition, User user)
        {
            var db = DbConnection();
            DateTime dateTime = DateTime.Now;
            partition = partition + "_" + dateTime.ToString("ddMMyyyy_HHmmss");
            var sql = @"
                        INSERT INTO c_particion (descripcion, id_usuario, id_sistema) VALUES (
                        @Partition, @IdUser, @IdSystem)";

            var result = await db.ExecuteAsync(sql, new
            {
                Partition = partition,
                IdUser = user.Id,
                IdSystem = user.IdSystem
            });
            return result > 0;
        }
        public async Task<bool> UpdateParition(Partition partition)
        {
            var db = DbConnection();

            var sql = @"
                        UPDATE c_particion 
                        SET ruta_particion = @PathPartition
                        WHERE id = @Id";

            var result = await db.ExecuteAsync(sql, new
            {
                partition.PathPartition,
                partition.Id
            });
            return result > 0;
        }
    }
}
