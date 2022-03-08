using ConaviWeb.Model.Shell;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConaviWeb.Data.Shell
{
    public interface IProcessEDRepository
    {
        Task<bool> InsertVoBo(string fileName, string path, DateTime dateProcess, int idUser, string ed);
        Task<bool> UpdateVoBo(string fileName, string path, DateTime dateProcess, int idUser, string ed);
        Task<IEnumerable<ProcessED>> SelectVoBo(string type, string process);
        Task<bool> InsertED(string fileName, string path, DateTime dateProcess, int idUser, string ed);


    }
}
