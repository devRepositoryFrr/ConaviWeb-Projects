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
    }
}
