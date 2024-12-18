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
        public async Task<IEnumerable<Catalogo>> GetSector()
        {
            var db = DbConnection();

            var sql = @"
                            SELECT 
                                id Clave, 
                                descripcion AS Descripcion 
                            FROM sedatu.c_sector;
                         ";

            return await db.QueryAsync<Catalogo>(sql);
        }
        public async Task<IEnumerable<Catalogo>> GetEntidad()
        {
            var db = DbConnection();

            var sql = @"
                            SELECT 
                                clave Clave, 
                                descripcion AS Descripcion 
                            FROM sedatu.c_entidad;
                         ";

            return await db.QueryAsync<Catalogo>(sql);
        }
        public async Task<IEnumerable<Catalogo>> GetMunicipio(string clave)
        {
            var db = DbConnection();

            var sql = @"
                            SELECT 
                                cv_mun Clave, 
                                descripcion AS Descripcion 
                            FROM sedatu.c_municipio WHERE cv_ef = @Clave;
                         ";

            return await db.QueryAsync<Catalogo>(sql ,new { Clave = clave});
        }
        public async Task<IEnumerable<Catalogo>> GetResponsable()
        {
            var db = DbConnection();

            var sql = @"
                            SELECT 
                                id Clave, 
                                nombre AS Descripcion 
                            FROM sedatu.c_personal;
                         ";

            return await db.QueryAsync<Catalogo>(sql);
        }
        public async Task<IEnumerable<Catalogo>> GetGestion()
        {
            var db = DbConnection();

            var sql = @"
                            SELECT 
                                id Clave, 
                                descripcion AS Descripcion 
                            FROM sedatu.c_gestion;
                         ";

            return await db.QueryAsync<Catalogo>(sql);
        }
        public async Task<IEnumerable<Catalogo>> GetEstatus()
        {
            var db = DbConnection();

            var sql = @"
                            SELECT 
                                id Clave, 
                                descripcion AS Descripcion 
                            FROM sedatu.c_estatus;
                         ";

            return await db.QueryAsync<Catalogo>(sql);
        }
        public async Task<IEnumerable<Catalogo>> GetReunion(int id)
        {
            var db = DbConnection();

            var sql = @"
                            SELECT 
                                id Clave, 
                                asunto AS Descripcion 
                            FROM sedatu.reunion
                            WHERE id = @Id;
                         ";

            return await db.QueryAsync<Catalogo>(sql, new { Id = id});
        }
        public async Task<IEnumerable<Catalogo>> GetReunion()
        {
            var db = DbConnection();

            var sql = @"
                            SELECT 
                                id Clave, 
                                CONCAT_WS(' - ',id,asunto) AS Descripcion 
                            FROM sedatu.reunion;
                         ";

            return await db.QueryAsync<Catalogo>(sql);
        }
        public async Task<ReunionResponse> GetReunionDetail(int id)
        {
            var reunion = new ReunionResponse();
            var db = DbConnection();
            var results = await db.QueryMultipleAsync(
                @"
                            select r.id id,cs.descripcion sector,ce.descripcion EntidadFed,cm.descripcion Municipio,DATE_FORMAT(fch_solicitud,'%d/%m/%Y') FechaSolicitud,DATE_FORMAT(fch_atencion,'%d/%m/%Y') FechaAtencion,asunto,nombre_sol nombreSol,cargo_sol cargoSol,dependencia_sol dependenciaSol,r.nombre_cont nombreCont,r.telefono_cont telefonoCont,r.email_cont emailCont,antecedentes,observaciones,cp.nombre responsable ,cg.descripcion gestion,e.descripcion estatus
                            from sedatu.reunion r
                            join sedatu.c_sector cs on cs.id = r.id_sector
                            join sedatu.c_entidad ce on ce.clave = r.cv_ef
                            join sedatu.c_municipio cm on cm.cv_ef = r.cv_ef and cm.cv_mun = r.cv_mun
                            left join sedatu.c_personal cp on cp.id = r.id_responsable
                            join sedatu.c_gestion cg on cg.id = r.id_gestion
                            join sedatu.c_estatus e on e.id = r.estatus
                            WHERE r.id = @Id;
                            select a.id,a.id_reunion IdReunion,a.responsable,DATE_FORMAT(a.fecha_termino,'%d/%m/%Y') FechaTermino,a.descripcion,cg.descripcion gestion,e.descripcion estatus from sedatu.acuerdo a 
                            join sedatu.reunion r on r.id = a.id_reunion 
                            join sedatu.c_gestion cg on cg.id = a.id_gestion
                            join sedatu.c_estatus e on e.id = a.estatus
                            WHERE a.id_reunion = @Id AND cv_bajal = 1;
                         ", new { Id = id});
            reunion = await results.ReadFirstOrDefaultAsync<ReunionResponse>();
            reunion.Acuerdos = (List<AcuerdoResponse>)await results.ReadAsync<AcuerdoResponse>();
                        
            return reunion;
        }
        public async Task<AcuerdoResponse> GetAcuerdoDetail(int id)
        {
            var reunion = new AcuerdoResponse();
            var db = DbConnection();
            var results = await db.QueryMultipleAsync(
                @"
                            select a.id,a.id_reunion IdReunion,a.responsable,DATE_FORMAT(a.fecha_termino,'%d/%m/%Y') FechaTermino,a.descripcion,cg.descripcion gestion,e.descripcion estatus from sedatu.acuerdo a 
                            join sedatu.reunion r on r.id = a.id_reunion 
                            join sedatu.c_gestion cg on cg.id = a.id_gestion
                            join sedatu.c_estatus e on e.id = a.estatus
                            WHERE a.id = @Id AND cv_bajal = 1;
                         ", new { Id = id });
            reunion = await results.ReadFirstOrDefaultAsync<AcuerdoResponse>();

            return reunion;
        }
        public async Task<Model.Minuta.Minuta> GetMinuta(int id)
        {
            var db = DbConnection();

            var sql = @"
                            select id, id_reunion, tema, asunto, interno, externo, contexto, descripcion, estatus, created_at FchCreate
                            from sedatu.minuta
                            where id_reunion = @Id;
                         ";

            return await db.QueryFirstOrDefaultAsync<Model.Minuta.Minuta>(sql, new { Id = id });
        }
        public async Task<IEnumerable<AcuerdoResponse>> GetAcuerdos(int id)
        {
            var lookup = new Dictionary<int, Model.Minuta.AcuerdoResponse>();
            var db = DbConnection();
            var sql = @"
                            select a.id,id_reunion idReunion,responsable ,DATE_FORMAT(a.fecha_termino,'%d/%m/%Y') fechaTermino ,a.descripcion,cg.descripcion gestion ,'' area,e.descripcion estatus
                            from sedatu.acuerdo a
                            join sedatu.c_gestion cg on cg.id = a.id_gestion 
                            join sedatu.c_estatus e on e.id = a.estatus
                            where id_reunion  = @Id;
                           ";
            return await db.QueryAsync<AcuerdoResponse>(sql, new { Id = id });
        }

        public async Task<bool> InsertReunion(Reunion reunion)
        {
            var db = DbConnection();

            var sql = @"
                        CALL sedatu.sp_inserta_reunion(@Sector,@EntidadFed,@Municipio,@Asunto,@FechaSolicitud,@NombreSol,@CargoSol,@DependenciaSol,@NombreCont,@TelefonoCont,@EmailCont,@Antecedentes,@Responsable,@FechaAtencion,@Observaciones,@Gestion,@IdEstatus);";

            var result = await db.ExecuteAsync(sql, new {
                reunion.Sector,
                reunion.EntidadFed,
                reunion.Municipio,
                reunion.Asunto,
                reunion.FechaSolicitud,
                reunion.NombreSol,
                reunion.CargoSol,
                reunion.DependenciaSol,
                reunion.NombreCont,
                reunion.TelefonoCont,
                reunion.EmailCont,
                reunion.Antecedentes,
                reunion.Responsable,
                reunion.FechaAtencion,
                reunion.Observaciones,
                reunion.Gestion,
                reunion.IdEstatus
            });
            return result > 0;

        }
        public async Task<bool> InsertMinuta(Model.Minuta.Minuta minuta)
        {
            var db = DbConnection();
            var sql = @"
                        INSERT INTO sedatu.minuta (id_reunion, tema, asunto, interno, externo, contexto, descripcion) VALUES(@IdReunion, @Tema, @Asunto, @Interno, @Externo, @Contexto, @Descripcion );";
            var result = await db.ExecuteAsync(sql, minuta);
            return result > 0;
        }
        public async Task<bool> InsertParticipantes(IEnumerable<Participante> participantes)
        {
            var db = DbConnection();
            var sql = @"
                        INSERT INTO sedatu.participante (id_minuta, area, nombre, email) VALUES(@IdMinuta, @Direccion, @Nombre, @Email);";
            var result = await db.ExecuteAsync(sql, participantes);
            return result > 0;
        }
        public async Task<bool> InsertAcuerdo(Acuerdo acuerdo)
        {
            var db = DbConnection();
            var sql = @"
                        INSERT INTO sedatu.acuerdo (id_reunion, responsable, fecha_termino, descripcion, id_gestion, estatus) VALUES(@IdReunion, @IdResponsable, @FechaTermino, @Descripcion, @IdGestion, @IdEstatus);";
            var result = await db.ExecuteAsync(sql, acuerdo);
            return result > 0;
        }
        public async Task<IEnumerable<ReunionResponse>> GetReuniones()
        {
            var lookup = new Dictionary<int, Model.Minuta.ReunionResponse>();
            var db = DbConnection();
            var sql = @"
                            select r.id id, cs.descripcion sector, ce.descripcion entidadFed, cm.descripcion municipio, r.asunto, r.nombre_sol nombreSol, e.descripcion estatus
                            from sedatu.reunion r 
                            join sedatu.c_sector cs on cs.id = r.id_sector 
                            join sedatu.c_entidad ce on ce.clave = r.cv_ef 
                            join sedatu.c_municipio cm on cm.cv_ef = r.cv_ef and cm.cv_mun = r.cv_mun
                            join sedatu.c_estatus e on e.id = r.estatus;
                           ";
            return await db.QueryAsync<ReunionResponse>(sql);
        }
        public async Task<IEnumerable<AcuerdoResponse>> GetAcuerdos()
        {
            var lookup = new Dictionary<int, Model.Minuta.AcuerdoResponse>();
            var db = DbConnection();
            var sql = @"
                            select a.id,id_reunion idReunion,responsable ,DATE_FORMAT(a.fecha_termino,'%d/%m/%Y') fechaTermino ,a.descripcion,cg.descripcion gestion ,'' area,e.descripcion estatus
                            from sedatu.acuerdo a
                            join sedatu.c_gestion cg on cg.id = a.id_gestion 
                            join sedatu.c_estatus e on e.id = a.estatus;
                           ";
            return await db.QueryAsync<AcuerdoResponse>(sql);
        }
        public async Task<IEnumerable<ReunionIndicadores>> GetIndReunion(int id)
        {
            var sql = @"
                            select COUNT(r.id) reuniones, e.descripcion estatus from sedatu.reunion r
                            join sedatu.c_estatus e on e.id = r.estatus 
                            group by r.estatus;
                           ";
            var db = DbConnection();
            if(id != 0) { 
                sql = @"
                            select COUNT(r.id) reuniones, e.descripcion estatus from sedatu.reunion r
                            join sedatu.c_estatus e on e.id = r.estatus 
                            where r.id_gestion = @Id
                            group by r.estatus;
                           ";
            }
            return await db.QueryAsync<ReunionIndicadores>(sql,new { Id = id});
        }
        public async Task<IEnumerable<AcuerdoIndicadores>> GetIndAcuerdo(int id)
        {
            var sql = @"
                            select COUNT(a.id) acuerdos, e.descripcion estatus from sedatu.acuerdo a
                            join sedatu.c_estatus e on e.id = a.estatus
                            group by a.estatus;
                           ";
            var db = DbConnection();
            if (id != 0)
            {
                sql = @"
                            select COUNT(a.id) acuerdos, e.descripcion estatus from sedatu.acuerdo a
                            join sedatu.c_estatus e on e.id = a.estatus
                            where a.id_gestion = @Id
                            group by a.estatus;
                           ";
            }
            return await db.QueryAsync<AcuerdoIndicadores>(sql, new { Id = id });
        }
        public async Task<bool> DeleteAcuerdo(int id)
        {
            var db = DbConnection();

            var sql = @"
                        update sedatu.acuerdo set cv_bajal = 2 where id = @Id ;
                       ";

            var result = await db.ExecuteAsync(sql,new { Id = id});
            return result > 0;

        }
        //public async Task<IEnumerable<Model.Minuta.ReunionResponse>> GetReuniones()
        //{
        //    var lookup = new Dictionary<int, Model.Minuta.ReunionResponse>();
        //    var db = DbConnection();
        //    var sql = @"
        //                    select r.id id, cs.descripcion sector, ce.descripcion entidadFed, cm.descripcion municipio, r.asunto, r.solicitante, p.id_minuta, p.area, p.nombre, p.email, a.id_minuta, a.responsable, a.dacuerdo, a.fecha_termino
        //                    from sedatu.reunion r 
        //                    join sedatu.c_sector cs on cs.id = r.id_sector 
        //                    join sedatu.c_entidad ce on ce.clave = r.cv_ef 
        //                    join sedatu.c_municipio cm on cm.cv_ef = r.cv_ef and cm.cv_mun = r.cv_mun 
        //                    left join sedatu.participante p on p.id_minuta = r.id
        //                    left join sedatu.acuerdo a on a.responsable = p.email and a.id_minuta = r.id;
        //                   ";
        //    _ = await db.QueryAsync<Model.Minuta.ReunionResponse, Participante, Acuerdo, Model.Minuta.ReunionResponse>(sql, (r, p, a) =>
        //    {
        //        if (!lookup.TryGetValue(r.Id, out var mEntry))
        //        {
        //            mEntry = r;
        //            mEntry.Participantes ??= new List<Participante>();
        //            mEntry.Acuerdos ??= new List<Acuerdo>();
        //            lookup.Add(r.Id, mEntry);
        //        }

        //        mEntry.Participantes.Add(p);
        //        mEntry.Acuerdos.Add(a);
        //        return mEntry;

        //    }, splitOn: "id_minuta");

        //    return lookup.Values.AsList();
        //}

    }
}
