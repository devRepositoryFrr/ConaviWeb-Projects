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
    }
}
