using System;
using ConaviWeb.Model;
using ConaviWeb.Model.Expedientes;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConaviWeb.Data.Expedientes
{
    public interface IExpedienteRepository
    {
        Task<int> GetIdUserArea(string area);
        Task<Inventario> GetInventarioTP(string area);
        Task<Inventario> GetInventarioControl(string area);
        Task<int> GetIdInventarioBibliohemerografico(string area);
        Task<Inventario> GetInventarioNoExpedientable(string area);
        Task<IEnumerable<Catalogo>> GetCodigosExp();
        Task<IEnumerable<Catalogo>> GetTiposSoporte();
        Task<IEnumerable<Catalogo>> GetTiposDocumentales();
        Task<IEnumerable<SerieDocumental>> GetSeries();
        Task<SerieDocumental> GetSerieDocumental(int id);
        Task<bool> UpdateSerieDocCat(SerieDocumental serie);
        Task<IEnumerable<Catalogo>> GetAreas();
        Task<bool> InsertInventarioTP(Inventario inventario);
        Task<bool> InsertExpedienteInventarioTP(Expediente expediente);
        Task<bool> InsertCaratulaExpedienteTP(Caratula caratula);
        Task<IEnumerable<Expediente>> GetExpedientesInventarioTP(int id, int id_inventario);
        Task<IEnumerable<Expediente>> GetExpedientesValidacionTP(int idArea);
        Task<Expediente> GetExpedienteTP(int id);
        Task<Caratula> GetCaratulaExpedienteTP(int id);
        Task<bool> DropExpediente(int id);
        Task<bool> SendValExpediente(int id);
        Task<bool> VoBoExpediente(int id);
        Task<bool> RevalidacionExpediente(int id);
        Task<bool> InsertInventarioControl(Inventario inventario);
        Task<bool> InsertExpedienteInventarioControl(Expediente expediente);
        Task<bool> InsertCaratulaExpedienteIC(Caratula caratula);
        Task<IEnumerable<Expediente>> GetExpedientesInventarioControl(int id, int id_inventario);
        Task<IEnumerable<Expediente>> GetExpedientesValidacionInventarioControl(int id_area);
        Task<Expediente> GetExpedienteControl(int id);
        Task<Caratula> GetCaratulaExpedienteControl(int id);
        Task<bool> DropExpedienteControl(int id);
        Task<bool> SendValExpedienteControl(int id);
        Task<bool> VoBoExpedienteControl(int id);
        Task<bool> RevalidacionExpedienteControl(int id);
        Task<bool> MigrarExpedienteInvTP(int id);
        Task<bool> MigrarExpedienteInvNE(int id);
        Task<bool> InsertInventarioBibliohemerografico(Inventario inventario);
        Task<bool> InsertExpedienteBibliohemerografico(ExpedienteBibliohemerografico expediente);
        Task<IEnumerable<ExpedienteBibliohemerografico>> GetExpedientesBibliohemerograficos(int id, int id_inventario);
        Task<bool> DropExpedienteBibliohemerografico(int id);
        Task<bool> InsertInventarioNoExpedientable(Inventario inventario);
        Task<bool> InsertExpedienteNoExpedientable(ExpedienteNoExpedientable expediente);
        Task<bool> InsertCaratulaNoExpedientable(Caratula caratula);
        Task<IEnumerable<ExpedienteNoExpedientable>> GetExpedientesNoExpedientables(int id, int id_inventario);
        Task<Expediente> GetNoExpedientable(int id);
        Task<Caratula> GetCaratulaNoExpedientable(int id);
        Task<bool> DropExpedienteNoExpedientable(int id);
    }
}