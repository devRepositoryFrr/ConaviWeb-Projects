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
        Task<bool> UpdateViaticos(Viaticos viaticos);
        Task<bool> UpdateEstatus(int id, int estatus);
        Task<Viaticos> GetSolicitud(int id);
        Task<IEnumerable<Catalogo>> GetEntidades();

    }
}
