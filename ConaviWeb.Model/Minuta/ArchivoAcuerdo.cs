using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConaviWeb.Model.Minuta
{
    public class ArchivoAcuerdo
    {
        public int Id { get; set; }
        public int IdAcuerdo { get; set; }
        public string NombreArchivo { get; set; }
        public IFormFile Archivo { get; set; }
        public DateTime Fecha { get; set; }
        public string Usuario { get; set; }
        public string Estatus { get; set; }
    }
}
