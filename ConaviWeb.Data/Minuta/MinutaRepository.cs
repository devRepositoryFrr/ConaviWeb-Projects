using ConaviWeb.Model;
using ConaviWeb.Model.Minuta;
using ConaviWeb.Model.RH;
using Dapper;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace ConaviWeb.Data.Minuta
{
    public class MinutaRepository : IMinutaRepository
    {
        private readonly MySQLConfiguration _connectionString;
        public MinutaRepository(MySQLConfiguration connectionString)
        {
            _connectionString = connectionString;
        }

        protected MySqlConnection DbConnection()
        {
            return new MySqlConnection(_connectionString.ConnectionString);
        }
        public async Task<int> InsertMinuta(Model.Minuta.Minuta minuta)
        {
            var db = DbConnection();

            var sql = @"
                        CALL sedatu.sp_inserta_minuta(@Folio, @Tema, @Asunto, @Contexto, @Descripcion);";

            var result = await db.QueryAsync<int>(sql, new { minuta.Folio, minuta.Tema, minuta.Asunto, minuta.Contexto, minuta.Descripcion});
            return result.FirstOrDefault();

        }

        public async Task<bool> InsertParticipantes(IEnumerable<Participante> participantes)
        {
            var db = DbConnection();
            var sql = @"
                        INSERT INTO sedatu.participante (id_minuta, area, nombre, email) VALUES(@IdMinuta, @Direccion, @Nombre, @Email);";
            var result = await db.ExecuteAsync(sql, participantes);
            return result > 0;
        }

        public async Task<bool> InsertAcuerdos(IEnumerable<Acuerdo> acuerdos)
        {
            var db = DbConnection();
            var sql = @"
                        INSERT INTO sedatu.acuerdo (id_minuta, email, dacuerdo, fecha_termino) VALUES(@IdMinuta, @Responsable, @Descripcion, @Fecha_termino);";
            var result = await db.ExecuteAsync(sql, acuerdos);
            return result > 0;
        }

        public async Task<IEnumerable<Model.Minuta.Minuta>> GetMinutas()
        {
            var lookup = new Dictionary<int, Model.Minuta.Minuta>();
            var db = DbConnection();
            var sql = @"
                            select m.id, m.folio, m.tema, m.asunto, m.contexto, m.descripcion, 
                                    p.id_minuta, p.area, p.nombre, p.email, 
                                    a.id_minuta, a.responsable, a.dacuerdo, a.fecha_termino
                                from sedatu.minuta m 
                                join sedatu.participante p on p.id_minuta = m.id
                                join sedatu.acuerdo a on a.responsable = p.email and a.id_minuta = m.id;
                           ";
            _ = await db.QueryAsync<Model.Minuta.Minuta, Participante, Acuerdo, Model.Minuta.Minuta>(sql, (m, p, a) =>
            {
                if (!lookup.TryGetValue(m.Id, out var mEntry))
                {
                    mEntry = m;
                    mEntry.Participantes ??= new List<Participante>();
                    mEntry.Acuerdos ??= new List<Acuerdo>();
                    lookup.Add(m.Id, mEntry);
                }

                mEntry.Participantes.Add(p);
                mEntry.Acuerdos.Add(a);
                return mEntry;

            }, splitOn: "id_minuta");

            return lookup.Values.AsList();
        }

    }
}
