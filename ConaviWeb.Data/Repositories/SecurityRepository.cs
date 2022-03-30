using Dapper;
using ConaviWeb.Model;
using ConaviWeb.Model.Request;
using ConaviWeb.Model.Response;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace ConaviWeb.Data.Repositories
{
    public class SecurityRepository:ISecurityRepository
    {
        private readonly MySQLConfiguration _connectionString;
        public SecurityRepository(MySQLConfiguration connectionString)
        {
            _connectionString = connectionString;
        }

        protected MySqlConnection DbConnection()
        {
            return new MySqlConnection(_connectionString.UserConnectionString);
        }
        public async Task<UserResponse> GetLoginByCredentials(UserRequest login)
        {
            try
            {

            
            var db = DbConnection();

            var sql = @"
                        SELECT u.id AS Id, concat(nombre,' ',primer_apellido,' ',segundo_apellido) Name, usuario AS SUser, id_rol AS Rol, id_sistema AS Sistema,
                        (select descripcion from c_controlador where id = (
                        select id_controlador from sistema_rol_controlador where id_sistema = u.id_sistema and id_rol = u.id_rol)) Controller,
                        u.cargo Cargo, u.numero_empleado NuEmpleado, ca.descripcion Area, clave_nivel CvNivel
                        FROM usuario u 
                        LEFT JOIN c_area ca ON ca.id = u.id_area
                        WHERE usuario = @SUser AND password = @Password AND activo = 1";

            return await db.QueryFirstOrDefaultAsync<UserResponse>(sql, new { login.SUser, login.Password });
            }
            catch (System.Exception e)
            {

                throw;
            }
        }
        public async Task<UserResponse> GetLoginByUserId(int userId)
        {
            var db = DbConnection();

            var sql = @"
                        SELECT id AS Id, usuario AS SUser, id_rol AS Rol
                        FROM usuario WHERE id = @UserId AND activo = 1";

            return await db.QueryFirstOrDefaultAsync<UserResponse>(sql, new { UserId = userId });
        }
        public async Task<IEnumerable<Module>> GetModules(int idRol, int idSistema)
        {
            var db = DbConnection();

            var sql = @"
                        SELECT id Id, descripcion Text, url Url, ico Ico
                        FROM sistema_rol_modulo rm
                        JOIN c_modulo cm ON rm.id_modulo = cm.id
                        WHERE id_sistema = @IdSistema AND rm.id_rol = @IdRol";

            return await db.QueryAsync<Module>(sql, new { IdSistema = idSistema ,IdRol = idRol });
        }

        public async Task<IEnumerable<Partition>> GetPartitions(int idSystem)
        {
            var db = DbConnection();

            var sql = @"
                        SELECT id Id, descripcion Text
                        FROM c_particion
                        where id_sistema = @IdSystem";

            return await db.QueryAsync<Partition>(sql, new { IdSystem = idSystem });
        }

        public async Task<Partition> GetPartition(int idPartition)
        {
            var db = DbConnection();

            var sql = @"
                        SELECT id Id, descripcion Text, ruta_particion as PathPartition
                        FROM c_particion
                        where id = @IdPartition";

            return await db.QueryFirstOrDefaultAsync<Partition>(sql, new { IdPartition = idPartition });
        }

        public async Task<bool> CreateUserSisevive(UserRequest userRequest)
        {
            var db = DbConnection();

            var sql = @"
                        CALL sp_create_user_sisevive(@Name, @LName, @SLName, @SUser, @Password, @Email);";

            var result = await db.ExecuteAsync(sql, new { 
                userRequest.SUser, userRequest.Name, userRequest.LName, userRequest.SLName, userRequest.Password, userRequest.Email });
            return result > 0;
        }
    }
}
