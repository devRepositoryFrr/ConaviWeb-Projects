using Dapper;
using ConaviWeb.Model;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConaviWeb.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly MySQLConfiguration _connectionString;
        public UserRepository(MySQLConfiguration connectionString)
        {
            _connectionString = connectionString;
        }
        protected MySqlConnection DbConnection()
        {
            return new MySqlConnection(_connectionString.ConnectionString);
        }

        public async Task<IEnumerable<User>> GetAllUsers()
        {
            var db = DbConnection();

            var sql = @"
                        SELECT id Id, nombre Name, primer_apellido LName, segundo_apellido SLName, usuario SUser, id_rol Rol, cargo Position,
                                numero_empleado EmployeeNumber, rfc RFC, grado_academico Degree, fecha_alta CreateDate, integrador Integrador, 
                                id_sistema IdSystem, firmante Signer, activo Active
                        FROM qa_adms_conavi.usuario;";

            return await db.QueryAsync<User>(sql, new { });
        }

        public async Task<User> GetUserDetails(int id)
        {
            var db = DbConnection();

            var sql = @"
                         SELECT u.id Id, nombre Name, primer_apellido LName, segundo_apellido SLName, usuario SUser, id_rol Rol, cargo Position,
                                numero_empleado EmployeeNumber, rfc RFC, grado_academico Degree, fecha_alta CreateDate, integrador Integrador, 
                                id_sistema IdSystem, ca.descripcion Area, firmante Signer, activo Active, email Email
                        FROM qa_adms_conavi.usuario u
                        LEFT JOIN qa_adms_conavi.c_area ca ON ca.id = u.id_area
                        WHERE u.id = @Id";

            return await db.QueryFirstOrDefaultAsync<User>(sql, new { Id = id });
        }

    }
}
