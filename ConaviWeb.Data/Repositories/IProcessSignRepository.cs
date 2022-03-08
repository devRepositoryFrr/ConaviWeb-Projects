using ConaviWeb.Model;
using ConaviWeb.Model.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConaviWeb.Data.Repositories
{
    public interface IProcessSignRepository
    {
        Task<IEnumerable<FileResponse>> GetFiles(int idSystem);
        Task<IEnumerable<FileResponse>> GetSignedFiles(int idSystem);
        Task<IEnumerable<FileResponse>> GetSignedFilesCancel(int idSystem);
        Task<IEnumerable<FileResponse>> GetPartitionFiles(int idPartition);
        Task<IEnumerable<FileResponse>> GetPartitionFilesCancel(int idPartition);
        Task<IEnumerable<FileResponse>> GetPartitionSourceFiles(int idPartition);
        Task<IEnumerable<FileResponse>> GetExternalFiles(string integrador);
        Task<IEnumerable<FileResponse>> GetFilesForSign(int idSystem, string arrayFiles);
        Task<IEnumerable<FileResponse>> GetFilesForCancel(int idSystem, string arrayFiles);
        Task<bool> InsertSigningFile(SigningFile signingFile, User user, int idArchivoPadre, string currentXML, string XMLName, Partition partition);
        Task<bool> InsertCancelFile(SigningFile signingFile, User user, int idArchivoPadre, string currentXML, string XMLName, Partition partition);
    }
}
