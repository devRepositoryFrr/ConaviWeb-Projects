using ConaviWeb.Model.RH;
using Dapper;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConaviWeb.Data.RH
{
    public class NominaRepository:INominaRepository
    {
        private readonly MySQLConfiguration _connectionString;
        public NominaRepository(MySQLConfiguration connectionString)
        {
            _connectionString = connectionString;
        }

        protected MySqlConnection DbConnection()
        {
            return new MySqlConnection(_connectionString.ConnectionString);
        }
        public async Task<bool> InsertAceptacion(Aceptacion aceptacion)
        {
            var db = DbConnection();

            var sql = @"
                        INSERT INTO prod_rh.rn_aceptacion
                        (anio,
                        mes,
                        quincena,
                        aceptacion,
                        id_empleado)
                        VALUES (@Anio,
                                @Mes,
                                @Quincena,
                                @Aceptacion,
                                @Empleado
                                )";

            var result = await db.ExecuteAsync(sql, new
            {
                Anio = aceptacion.Anio,
                Mes = aceptacion.Mes,
                Quincena = aceptacion.Quincena,
                Aceptacion = aceptacion.Acepta,
                Empleado = aceptacion.NuEmpleado
            });
            return result > 0;
        }
    }
}
