using ConaviWeb.Model;
using ConaviWeb.Model.Minuta;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConaviWeb.Data.Minuta
{
    public interface IMinutaRepository
    {
        public Task<IEnumerable<Catalogo>> GetSector();
        public Task<IEnumerable<Catalogo>> GetEntidad();
        public Task<IEnumerable<Catalogo>> GetMunicipio(string clave);
        public Task<IEnumerable<Catalogo>> GetResponsable();
        public Task<IEnumerable<Catalogo>> GetGestion();
        public Task<IEnumerable<Catalogo>> GetMesesAcu();
        public Task<IEnumerable<Catalogo>> GetMesesReu();
        public Task<IEnumerable<Catalogo>> GetEstatus();
        public Task<IEnumerable<Catalogo>> GetReunion(int id);
        public Task<IEnumerable<Catalogo>> GetReunion();
        public Task<IEnumerable<Catalogo>> GetTemas();
        public Task<IEnumerable<Model.Minuta.ReunionResponse>> GetReuniones();
        public Task<IEnumerable<Model.Minuta.AcuerdoResponse>> GetAcuerdos();
        public Task<ReunionTitular> GetRTitular(int id);
        public Task<bool> InsertReunion(Reunion reunion);
        public Task<bool> InsertReunion(ReunionTitular reunion);
        public Task<bool> InsertMinuta(Model.Minuta.Minuta minuta);
        public Task<bool> InsertParticipantes(IEnumerable<Participante> participantes);
        public Task<bool> InsertAcuerdo(Acuerdo acuerdo);
        public Task<bool> DeleteAcuerdo(int id);
        public Task<ReunionResponse> GetReunionDetail(int id);
        public Task<AcuerdoResponse> GetAcuerdoDetail(int id);
        public Task<Model.Minuta.Minuta> GetMinuta(int id);
        public Task<IEnumerable<Model.Minuta.AcuerdoResponse>> GetAcuerdos(int id);
        public Task<IEnumerable<ReunionIndicadores>> GetIndReunion(int id);
        public Task<IEnumerable<ReunionIndicadores>> GetIndReunionMes(int id, string clave);
        public Task<IEnumerable<AcuerdoIndicadores>> GetIndAcuerdo(int id, string clave);
        public Task<IEnumerable<AcuerdoIndicadores>> GetIndAcuerdoMes(int id, string clave);
        
    }
}
