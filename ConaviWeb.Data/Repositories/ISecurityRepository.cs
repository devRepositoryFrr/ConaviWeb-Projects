using ConaviWeb.Model;
using ConaviWeb.Model.Request;
using ConaviWeb.Model.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConaviWeb.Data.Repositories
{
    public interface ISecurityRepository
    {
        Task<UserResponse> GetLoginByCredentials(UserRequest login);
        Task<UserResponse> GetLoginByUserId(int userId);
        Task<IEnumerable<Module>> GetModules(int idRol, int idUser, int idSistema);
        Task<IEnumerable<Catalogo>> GetSistema(string nameSystem, int idSystem);
        Task<IEnumerable<Partition>> GetPartitions(int idSystem);
        Task<Partition> GetPartition(int idPartition);
        Task<bool> CreateUserSisevive(UserRequest userRequest);
    }
}
