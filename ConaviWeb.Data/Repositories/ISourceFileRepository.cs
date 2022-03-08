using ConaviWeb.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConaviWeb.Data.Repositories
{
    public interface ISourceFileRepository
    {
        Task<IEnumerable<SourceFile>> GetAllSourceFile();
        Task<SourceFile> GetSourceFileDetails(int id);
        Task<bool> InsertSourceFile(SourceFile sourceFile);
        Task<bool> UpdateSourceFile(SourceFile sourceFile);
        Task<bool> DeleteSourceFile(SourceFile sourceFile);
        Task<bool> InsertPartition(string partition, User user);
        Task<bool> UpdateParition(Partition partition);
    }
}
