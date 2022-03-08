using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConaviWeb.Model.Sisevive
{
    public class Evaluacion
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; }
        public string Folio { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string Informe { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string Letra { get; set; }
        public string Color { get; set; }
        public string NombreArchInforme{ get; set; }
        public int IdAdminUser { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string Observacion { get; set; }
        public string Nombre { get; set; }
        public int IdUserCarga { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public IFormFile ArchivoInforme { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public IFormFile DatoGeneralXls { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public IFormFile PlanoSembradoXls { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public IFormFile PlanoSembradoPdf { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public IFormFile PlanoArquitectonicoXls { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public IFormFile PlanoArquitectonicoPdf { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public IFormFile MaterialesXls { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public IFormFile MaterialesPdf { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public IFormFile DeeviXls { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public IFormFile SaaviXls { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public IFormFile IdgXls { get; set; }
        public string EmailPES { get; set; }

    }
}
