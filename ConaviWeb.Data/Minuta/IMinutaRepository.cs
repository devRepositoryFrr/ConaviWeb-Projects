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
        public Task<IEnumerable<Catalogo>> GetReunion(int id);
        public Task<IEnumerable<Model.Minuta.ReunionResponse>> GetReuniones();
        public Task<bool> InsertReunion(Reunion reunion);
        public Task<int> InsertMinuta(Model.Minuta.Minuta minuta);
        public Task<bool> InsertParticipantes(IEnumerable<Participante> participantes);
        public Task<bool> InsertAcuerdo(Acuerdo acuerdo);
        public Task<bool> DeleteAcuerdo(int id);
        public Task<ReunionResponse> GetReunionDetail(int id);
    }
}
