using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConaviWeb.Model.Sisevive
{
    public class Etiquetado
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; }
        public string Folio { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string Informe { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string Observacion { get; set; }
        public string Nombre { get; set; }
        public int IdAdminUser { get; set; }
        public string NombreArchInforme { get; set; }
        public int IdUserCarga { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public IFormFile ArchivoInforme { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public IFormFile ResponsivaCuvsXls { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public IFormFile ResponsivaCuvsPdf { get; set; }
        public string EmailPES { get; set; }
    }
}
