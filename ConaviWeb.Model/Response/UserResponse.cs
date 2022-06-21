using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ConaviWeb.Model.Response
{
    public class UserResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string SUser { get; set; }
        public string NuEmpleado { get; set; }
        public string Cargo { get; set; }
        public string Area { get; set; }
        public RolType Rol { get; set; }
        public int Sistema { get; set; }
        public IEnumerable<Module> Modules { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public string Controller { get; set; }
        public string CvNivel { get; set; }
        public int UpdatePass { get; set; }
    }
}
