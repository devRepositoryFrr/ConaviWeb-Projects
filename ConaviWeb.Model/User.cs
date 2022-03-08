using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConaviWeb.Model
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string LName { get; set; }
        public string SLName { get; set; }
        public string SUser { get; set; }
        public string Password { get; set; }
        public RolType Rol { get; set; }
        public string Position { get; set; }
        public int EmployeeNumber { get; set; }
        public string RFC { get; set; }
        public string Degree { get; set; }
        public DateTime CreateDate { get; set; }
        public string Integrador { get; set; }
        public AreaType Area { get; set; }
        public int IdSystem { get; set; }
        public string Signer { get; set; }
        public string Active { get; set; }
        public string Email { get; set; }
    }

    public enum RolType
    {
        Administrador = 1,
        Operador = 2,
        OperShell1 = 3,
        OperShell2 = 4,
        VoBoShell = 5
    }
public enum AreaType
{
    CONAVI = 1,
    EXTERNO = 2
}
}
