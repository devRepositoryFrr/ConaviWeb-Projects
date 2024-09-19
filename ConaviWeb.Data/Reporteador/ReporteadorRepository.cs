using ConaviWeb.Model.Reporteador;
using Dapper;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConaviWeb.Data.Reporteador
{
    public class ReporteadorRepository : IReporteadorRepository
    {
        private readonly MySQLConfiguration _connectionString;
        public ReporteadorRepository(MySQLConfiguration connectionString)
        {
            _connectionString = connectionString;
        }

        protected MySqlConnection DbConnection()
        {
            return new MySqlConnection(_connectionString.ConnectionString);
        }
        public async Task<PevC2sr> GetBeneficiario(string curp)
        {
            var db = DbConnection();

            var sql = @"
                    call prod_pev.sp_get_pevc2sr(@Curp);
                       ";

            return await db.QueryFirstOrDefaultAsync<PevC2sr>(sql, new { Curp = curp });
        }

        public async Task<IEnumerable<PevC2sr>> GetBeneficiarios()
        {
            var db = DbConnection();

            var sql = @"
                        call prod_pev.sp_get_list_pevc2sr();
                       ";

            return await db.QueryAsync<PevC2sr>(sql, new { });
        }
        public async Task<IEnumerable<PevCartaBB>> GetCartasBB(int id)
        {
            var db = DbConnection();

            var sql = @"
                        call prod_pev.sp_get_cartas_bienestar(@Id);
                       ";

            return await db.QueryAsync<PevCartaBB>(sql, new { Id = id });
        }
        public async Task<IEnumerable<PevCartaBA>> GetCartasBA(int id)
        {
            var db = DbConnection();

            var sql = @"
                        call prod_pev.sp_get_cartas_baz(@Id);
                       ";

            return await db.QueryAsync<PevCartaBA>(sql, new { Id = id });
        }
        public async Task<IEnumerable<PevCartaPMV>> GetCartasPMV(int id)
        {
            var db = DbConnection();

            var sql = @"
                        call prod_pev.sp_get_cartas_pmv(@Id);
                       ";

            return await db.QueryAsync<PevCartaPMV>(sql, new { Id = id });
        }
        public async Task<IEnumerable<PevCartaPMV>> GetCartasPMV24(int id)
        {
            var db = DbConnection();

            var sql = @"
                        call prod_pmv_2024.sp_get_cartas_pmv_24(@Id);
                       ";

            return await db.QueryAsync<PevCartaPMV>(sql, new { Id = id });
        }
        public async Task<PevC4> GetPMV24C4(string id)
        {
            var db = DbConnection();

            var sql = @"
                    call prod_pmv_2024.sp_get_pmv_pdf_conclusion(@Id);
                       ";

            return await db.QueryFirstOrDefaultAsync<PevC4>(sql, new { Id = id });
        }
        public async Task<PevSol> GetPMV24C2(string id)
        {
            var db = DbConnection();

            var sql = @"
                    call prod_pmv_2024.sp_get_pmv_pdf_solventa(@Id);
                       ";

            return await db.QueryFirstOrDefaultAsync<PevSol>(sql, new { Id = id });
        }
        public async Task<IEnumerable<string>> GetPMV24C2Ids(int id)
        {
            var db = DbConnection();

            var sql = @"
                        SELECT id_unico FROM prod_pmv_2024.pmv_solventa so where so.cve_bajal = 'A' and YEAR(uploaded_at) >= 2024 and MONTH(uploaded_at) = @Id;
                       ";

            return await db.QueryAsync<string>(sql, new { Id = id });
        }
    }
}
