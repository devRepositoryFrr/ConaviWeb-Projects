using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConaviWeb.Model;
using ConaviWeb.Model.PrediosAdquisicion;

namespace ConaviWeb.Data.Levantamientos
{
    public interface ILevantamientoRepository
    {
        Task<IEnumerable<Catalogo>> GetEstados();
        Task<IEnumerable<Catalogo>> GetMunicipios(string estado);
        Task<IEnumerable<Catalogo>> GetLocalidades(string estado, string cvemun);
        Task<bool> InsertFormatoLevantamiento(Predio predio);
        Task<Predio> GetFormatoLevantamiento(int id);
        Task<IEnumerable<Predio>> GetPrediosAdquisicion();
        Task<bool> DropPredio(int id);
        Task<bool> InsertFilePredio(string idPredio, string idFile, string filename, string extension);
        Task<Catalogo> GetFile(int idPredio, int idFile);
    }
}
