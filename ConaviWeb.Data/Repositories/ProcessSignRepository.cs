using Dapper;
using ConaviWeb.Model;
using ConaviWeb.Model.Response;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConaviWeb.Data.Repositories
{
    public class ProcessSignRepository : IProcessSignRepository
    {
        private readonly MySQLConfiguration _connectionString;
        public ProcessSignRepository(MySQLConfiguration connectionString)
        {
            _connectionString = connectionString;
        }

        protected MySqlConnection DbConnection()
        {
            return new MySqlConnection(_connectionString.ConnectionString);
        }
        public async Task<IEnumerable<FileResponse>> GetFiles(int idSystem)
        {
            var db = DbConnection();

            var sql = @"
                        CALL sp_get_files(@IdSystem)";

            return await db.QueryAsync<FileResponse>(sql, new { IdSystem = idSystem });
        }
        public async Task<IEnumerable<FileResponse>> GetSignedFiles(int idSystem)
        {
            var db = DbConnection();

            var sql = @"
                        CALL sp_get_signed_files(@IdSystem)";

            return await db.QueryAsync<FileResponse>(sql, new { IdSystem = idSystem });
        }
        public async Task<IEnumerable<FileResponse>> GetSignedFilesCancel(int idSystem)
        {
            var db = DbConnection();

            var sql = @"
                        CALL sp_get_signed_files_cancel(@IdSystem)";

            return await db.QueryAsync<FileResponse>(sql, new { IdSystem = idSystem });
        }
        public async Task<IEnumerable<FileResponse>> GetPartitionFiles(int idPartition)
        {
            var db = DbConnection();

            var sql = @"
                        CALL sp_get_partition_files(@IdPartition)";

            return await db.QueryAsync<FileResponse>(sql, new { IdPartition = idPartition });
        }
        public async Task<IEnumerable<FileResponse>> GetPartitionFilesCancel(int idPartition)
        {
            var db = DbConnection();

            var sql = @"
                        CALL sp_get_partition_files_cancel(@IdPartition)";

            return await db.QueryAsync<FileResponse>(sql, new { IdPartition = idPartition });
        }

        public async Task<IEnumerable<FileResponse>> GetPartitionSourceFiles(int idPartition)
        {
            var db = DbConnection();

            var sql = @"
                        CALL sp_get_partition_source_files(@IdPartition)";

            return await db.QueryAsync<FileResponse>(sql, new { IdPartition = idPartition });
        }
        public async Task<IEnumerable<FileResponse>> GetExternalFiles(string integrador)
        {
            var db = DbConnection();

            var sql = @"
                        SELECT ao.id AS Id, ao.ruta_archivo AS FilePath, ao.nombre_archivo AS FileName, ao.fecha_carga AS DateUpload, cep.descripcion AS Status, id_partition as IdPartition
                        FROM archivo_origen ao
                        JOIN c_estatus_proceso cep ON cep.id = ao.id_estatus_proceso
                        WHERE ao.id_estatus_proceso = 1
                        AND ao.nombre_archivo = concat(@Integrador,'.pdf')";

            return await db.QueryAsync<FileResponse>(sql, new { Integrador = integrador });
        }
        public async Task<IEnumerable<FileResponse>> GetFilesForSign(int idSystem, string arrayFiles)
        {
            var db = DbConnection();

            var sql = @"
                        CALL sp_get_files_for_sign(@IdSystem, @ArrayFiles)";

            return await db.QueryAsync<FileResponse>(sql, new { IdSystem = idSystem , ArrayFiles = arrayFiles });
        }

        public async Task<IEnumerable<FileResponse>> GetFilesForCancel(int iSystem, string arrayFiles)
        {
            var db = DbConnection();

            var sql = @"
                        CALL sp_get_files_for_cancel(@IdSystem, @ArrayFiles)";

            return await db.QueryAsync<FileResponse>(sql, new { IdSystem = iSystem, ArrayFiles = arrayFiles });
        }

        public async Task<bool> InsertSigningFile(SigningFile signingFile , User user, int idArchivoPadre, string currentXML, string XMLName, Partition partition)
        {
            var db = DbConnection();

            var sql = @"
                        CALL sp_inserta_archivo_firmado(
                        @Path,@FileName,@SignerDate,@Folio,@OriginalString,@SignatureStamp,@Series,@Algorithm,@Hash,@IdUser,@IdSourceFile,@IdSystem,@PathXML, @NameXML,@IdPartition)";

            var result = await db.ExecuteAsync(sql, new
            {
                Path = signingFile.FilePath,
                FileName = signingFile.FileName,
                SignerDate = signingFile.SignatureDate.ToString("yyyy-MM-dd HH:mm:ss"),
                Folio = signingFile.Folio,
                OriginalString = signingFile.OriginalString,
                SignatureStamp = signingFile.SignatureStamp,
                Series = signingFile.CertSeries,
                Algorithm = signingFile.Algorithm,
                Hash = signingFile.Hash,
                IdUser = user.Id,
                IdSourceFile = idArchivoPadre,
                IdSystem = user.IdSystem,
                PathXML = currentXML,
                NameXML = XMLName,
                IdPartition = partition.Id
            });
            return result > 0;
        }

        public async Task<bool> InsertCancelFile(SigningFile signingFile, User user, int idArchivoPadre, string currentXML, string XMLName, Partition partition)
        {
            var db = DbConnection();

            var sql = @"
                        CALL sp_inserta_archivo_cancelado(
                        @Path,@FileName,@SignerDate,@Folio,@OriginalString,@SignatureStamp,@Series,@Algorithm,@Hash,@IdUser,@IdSourceFile,@IdSystem,@PathXML, @NameXML,@IdPartition)";

            var result = await db.ExecuteAsync(sql, new
            {
                Path = signingFile.FilePath,
                FileName = signingFile.FileName,
                SignerDate = signingFile.SignatureDate.ToString("yyyy-MM-dd HH:mm:ss"),
                Folio = signingFile.Folio,
                OriginalString = signingFile.OriginalString,
                SignatureStamp = signingFile.SignatureStamp,
                Series = signingFile.CertSeries,
                Algorithm = signingFile.Algorithm,
                Hash = signingFile.Hash,
                IdUser = user.Id,
                IdSourceFile = idArchivoPadre,
                IdSystem = user.IdSystem,
                PathXML = currentXML,
                NameXML = XMLName,
                IdPartition = partition.Id
            });
            return result > 0;
        }
    }
}
