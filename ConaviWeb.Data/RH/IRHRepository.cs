using ConaviWeb.Model;
using ConaviWeb.Model.RH;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConaviWeb.Data.RH
{
    public interface IRHRepository
    {
        Task<bool> InsertViaticos(Viaticos viaticos);
        Task<IEnumerable<Viaticos>> GetSolicitudes();
        Task<IEnumerable<Viaticos>> GetSolicitudes(int estatus);
        Task<IEnumerable<Viaticos>> GetSolicitudesUser(int idUser);
        Task<IEnumerable<Viaticos>> GetSolicitudesUser(int idUser, int idEstatus);
        Task<bool> UpdateViaticos(Viaticos viaticos);
        Task<bool> UpdateEstatus(int id, int estatus);
        Task<bool> UpdateEstatus(int id, int estatus, string obs);
        Task<bool> UpdateEstatus(int id, string path, int estatus);
        Task<Viaticos> GetSolicitud(int id);
        Task<IEnumerable<Catalogo>> GetEntidades();
        Task<bool> InsertComprobacion(CFDI cfdi);
        Task<IEnumerable<CFDI>> GetComprobaciones(string folio);
        Task<string> GetDataMail(int id_user);
    }
}
