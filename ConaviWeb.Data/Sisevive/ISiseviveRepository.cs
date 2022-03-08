using ConaviWeb.Model.Sisevive;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConaviWeb.Data.Sisevive
{
    public interface ISiseviveRepository
    {
        //Evaluación
        Task<bool> InsertEvaluation(string folio, int idUser);
        Task<Evaluacion> GetInforme(string folio);
        Task<IEnumerable<Evaluacion>> GetEvaluaciones();
        Task<bool> UpdateEvaluacion(EvalCustom evaluacion);
        Task<Evaluacion> GetEvaluacionesDetail(int Id);

        //Etiquetado
        Task<bool> InsertEtiquetado(string folio, int idUser, int IdEvaluacion);
        Task<Etiquetado> GetInformeEtiquetado(string folio);
        Task<Evaluacion> GetFolio(string folio);
        Task<IEnumerable<Etiquetado>> GetEtiquetados();
        Task<bool> UpdateEtiquetado(EtiqCustom etiquetado);
        Task<Etiquetado> GetEtiquetadoDetail(int id);
    }
}
