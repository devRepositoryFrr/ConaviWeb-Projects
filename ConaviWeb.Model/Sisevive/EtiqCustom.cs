using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConaviWeb.Model.Sisevive
{
    public class EtiqCustom
    {

        public int Id { get; set; }
        public string Informe { get; set; }
        public string Color { get; set; }
        public string Observacion { get; set; }
        public IFormFile ArchivoInforme { get; set; }
        public string NombreArchInforme { get; set; }
        public int IdAdminUser { get; set; }

    }
}
