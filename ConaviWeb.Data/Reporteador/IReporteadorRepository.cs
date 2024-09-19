using ConaviWeb.Model.Reporteador;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConaviWeb.Data.Reporteador
{
    public interface IReporteadorRepository
    {
        Task<IEnumerable<PevC2sr>> GetBeneficiarios();
        Task<PevC2sr> GetBeneficiario(string curp);
        Task<IEnumerable<PevCartaBB>> GetCartasBB(int id);
        Task<IEnumerable<PevCartaBA>> GetCartasBA(int id);
        Task<IEnumerable<PevCartaPMV>> GetCartasPMV(int id);
        Task<IEnumerable<PevCartaPMV>> GetCartasPMV24(int id);
        Task<PevC4> GetPMV24C4(string id);
        Task<PevSol> GetPMV24C2(string id);
        Task<IEnumerable<string>> GetPMV24C2Ids(int id);
    }
}
