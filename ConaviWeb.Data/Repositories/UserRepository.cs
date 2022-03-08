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
                        FROM prod_usuario.usuario;";

            return await db.QueryAsync<User>(sql, new { });
        }

        public async Task<User> GetUserDetails(int id)
        {
            var db = DbConnection();

            var sql = @"
                         SELECT id Id, nombre Name, primer_apellido LName, segundo_apellido SLName, usuario SUser, id_rol Rol, cargo Position,
                                numero_empleado EmployeeNumber, rfc RFC, grado_academico Degree, fecha_alta CreateDate, integrador Integrador, 
                                id_sistema IdSystem, area Area, firmante Signer, activo Active, email Email
                        FROM prod_usuario.usuario
                        WHERE id = @Id";

            return await db.QueryFirstOrDefaultAsync<User>(sql, new { Id = id });
        }

        public async Task<bool> InsertUser(User user)
        {
            var db = DbConnection();

            var sql = @"
                        ";

            var result = await db.ExecuteAsync(sql, new {  });
            return result > 0;
        }

        public async Task<bool> UpdateUser(User user)
        {
            var db = DbConnection();

            var sql = @"
                        ";

            var result = await db.ExecuteAsync(sql, new
            {
                
            });
            return result > 0;
        }

        public async Task<bool> DeleteUser(User user)
        {
            var db = DbConnection();

            var sql = @"
                        ";

            var result = await db.ExecuteAsync(sql, new { });
            return result > 0;
        }
    }
}
